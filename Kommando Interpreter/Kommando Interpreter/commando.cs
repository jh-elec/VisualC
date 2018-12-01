using System;
using System.Windows.Forms;
using System.IO.Ports;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Text;


public class Cmd
{
    /*  Event Delegate
        */
    public delegate void EventDelegate( Commando_Struct Parsed );

    public event EventDelegate CommandoFrameEvent;

    public void CommandoFrameEventFnc( Commando_Struct Parsed )
    {
        // Prüft ob das Event überhaupt einen Abonnenten hat.
        CommandoFrameEvent?.Invoke( Parsed );    
    }


    private Control SyncCtrl;
    public Control SyncForm
    {
        get { return SyncCtrl; }
        set { SyncCtrl = value; }
    }

    private byte SlaveCRC;
    private byte MasterCRC;

    private uint GoodFrameIncomming = 0;
    private uint BadFrameIncomming = 0;

    public uint GoodFrameCount
    {
        get{ return GoodFrameIncomming; }
    }

    public uint BadFrameCount
    {
        get { return BadFrameIncomming; }
    }

    private byte CmdCrc8CCITTUpdate(byte inCrc, byte inData)
    {
        Byte i = 0;
        Byte data = 0;

        data = Convert.ToByte(inCrc ^ inData);

        for (i = 0; i < 8; i++)
        {
            if ((data & 0x80) != 0)
            {
                data <<= 1;
                data ^= 0x07;
            }
            else
            {
                data <<= 1;
            }
        }

        return data;
    }

    private byte CmdCrc8StrCCITT(string str)
    {
        Byte crc = 0;
        Byte x = 0;

        for (x = 0; x < str.Length; x++)
        {
            crc = CmdCrc8CCITTUpdate(crc, Convert.ToByte(str[x]));
        }

        return crc;
    }

    public enum Data_Type_Enum
    {
        DATA_TYP_UINT8,
        DATA_TYP_UINT16,
        DATA_TYP_UINT32,
        DATA_TYP_FLOAT,
        DATA_TYP_STRING,

        DATA_TYP_INT8,
        DATA_TYP_INT16,
        DATA_TYP_INT32,

        __DATA_TYP_MAX_INDEX__
    };

    public enum Communication_Header_Enum
    {
        CMD_HEADER_LENGHT,      // Länge des ganzen Streams
        CMD_HEADER_DATA_TYP,    // (u)char , (u)int8 , (u)int16 , (u)int32
        CMD_HEADER_ID,          // Stream ID
        CMD_HEADER_EXITCODE,    // Exitkode aus Funktionen
        CMD_HEADER_CRC,         // Checksumme von der Message

        __CMD_HEADER_ENTRYS__
    };

    public enum Cmd_Basic_Id_Enum
    {
        ID_PING = 0, // Darauf sollte die Firmware ein Lebenszeichen zurückliefern
        ID_VERSION,
        ID_PROJECT_NAME,

        /*...*/

        ID_APPLICATION = 255 // Für irgendwelche System spezifschen Meldungen
    };

    public struct Commando_Struct
    {
        public byte DataLength;
        public byte DataType;
        public byte MessageID;
        public byte Exitcode;

        public byte[] Data;

        public Commando_Struct( int Size )
        {
            DataLength  = 0;
            DataType    = 0;
            MessageID   = 0;
            Exitcode    = 0;

            Data = new byte[Size];
        }
    }

    public Commando_Struct CommandoParsed;

    public int Parse(byte[] buffer, ref Commando_Struct Commando)
    {
        if (buffer.Length < (byte)Communication_Header_Enum.__CMD_HEADER_ENTRYS__)
        {
            return -1;
        }

        Commando.Data = new byte[buffer[(byte)Cmd.Communication_Header_Enum.CMD_HEADER_LENGHT]];

        try
        {
            Commando.DataLength = buffer[(byte)Communication_Header_Enum.CMD_HEADER_LENGHT];
            Commando.DataType   = buffer[(byte)Communication_Header_Enum.CMD_HEADER_DATA_TYP];
            Commando.MessageID  = buffer[(byte)Communication_Header_Enum.CMD_HEADER_ID];
            Commando.Exitcode   = buffer[(byte)Communication_Header_Enum.CMD_HEADER_EXITCODE];
            SlaveCRC            = buffer[(byte)Communication_Header_Enum.CMD_HEADER_CRC];
        }
        catch
        {
            return -1;
        }


        /*  Temporärer Speicher für die Checksummenbildung
        */
        byte[] Frame_ = new byte[]
        {
            Commando.DataLength,
            Commando.DataType,
            Commando.MessageID,
            Commando.Exitcode,
            0, // CRC ( Muss für die Bildung "0" sein! )
        };


        /*	Checksumme vom Header bilden
        */
        MasterCRC = 0;
        for (int x = 0; x < (byte)Cmd.Communication_Header_Enum.__CMD_HEADER_ENTRYS__; x++)
        {
            MasterCRC = CmdCrc8CCITTUpdate(MasterCRC, Frame_[x]);
        }


        /*	Checksumme von Nutzdaten bilden
        */
        int DataLength = (int)Commando.DataLength - (byte)Communication_Header_Enum.__CMD_HEADER_ENTRYS__;

        if (DataLength > 0)
        {
            for (int x = 0; x < DataLength; x++)
            {
                try
                {
                    MasterCRC = CmdCrc8CCITTUpdate(MasterCRC, buffer[(byte)Communication_Header_Enum.__CMD_HEADER_ENTRYS__ + x]);
                    Commando.Data[x] = buffer[(byte)Communication_Header_Enum.__CMD_HEADER_ENTRYS__ + x];
                }
                catch
                {
                    return -4;
                }
            }
        }

        Commando.DataLength = (byte)DataLength;

        if (MasterCRC == SlaveCRC)
        {
            GoodFrameIncomming++;

            if ( SyncForm != null && SyncForm.InvokeRequired )
            {
                SyncForm.Invoke(new EventDelegate(this.CommandoFrameEventFnc), CommandoParsed);
            }
        }
        else
        {
            BadFrameIncomming++;

            return -5;
        }

        return 0;
    }

    public byte[] BuildHeader ( Commando_Struct send )
    {
        byte[] Frame = new byte[(byte)Communication_Header_Enum.__CMD_HEADER_ENTRYS__ + send.DataLength];

        Frame[(byte)Communication_Header_Enum.CMD_HEADER_LENGHT]        = (byte)(Communication_Header_Enum.__CMD_HEADER_ENTRYS__ + send.DataLength);
        Frame[(byte)Communication_Header_Enum.CMD_HEADER_DATA_TYP]      = send.DataType;
        Frame[(byte)Communication_Header_Enum.CMD_HEADER_ID]            = send.MessageID;
        Frame[(byte)Communication_Header_Enum.CMD_HEADER_EXITCODE]      = send.Exitcode;
        Frame[(byte)Communication_Header_Enum.CMD_HEADER_CRC]           = 0;

        byte FrameMasterCRC = 0;

        for ( int x = 0; x < (byte)Communication_Header_Enum.__CMD_HEADER_ENTRYS__; x++)
        {
            FrameMasterCRC = CmdCrc8CCITTUpdate(FrameMasterCRC, Frame[x]);
        }

        if ( send.DataLength > 0 )
        {
            for ( int x = 0; x < send.DataLength; x++ )
            {
                FrameMasterCRC = CmdCrc8CCITTUpdate(FrameMasterCRC, send.Data[x]);
                Frame[(byte)Communication_Header_Enum.__CMD_HEADER_ENTRYS__ + x] = send.Data[x];
            }
        }

        Frame[(byte)Communication_Header_Enum.CMD_HEADER_CRC] = FrameMasterCRC;

        return Frame;
    }
  
    public string ConvertByteToSignedByte( byte[] buffer , uint index , uint length , string delimiter)
    {
        string convert = null;

        for ( uint x = 0; x < length; x += sizeof(byte) )
        {
            if ( x < (length - sizeof(byte)) )
            {
                convert += ((sbyte)buffer[index + x]).ToString() + " , ";
            }
            else
            {
                convert += ((sbyte)buffer[index + x]).ToString();
            }  
        }
        return convert;
    }

    public string ConvertInt16ToInt16( byte[] buffer , uint index , uint length, string delimiter)
    {
        string convert = null;

        for (uint x = 0; x < length; x += sizeof(Int16))
        {
            Int16 tmp = BitConverter.ToInt16(buffer, (int)(index + x));

            if (x < (length - sizeof(Int16)))
            {
                convert += tmp.ToString() + " , ";
            }
            else
            {
                convert += tmp.ToString();
            }
        }
        return convert;
    }

    public string ConvertInt32ToInt32(byte[] buffer , uint index , uint length, string delimiter)
    {
        string convert = null;

        for (uint x = 0; x < length; x += sizeof(Int32))
        {
            Int32 tmp = BitConverter.ToInt32(buffer, (int)(index + x));

            if (x < (length - sizeof(Int32)))
            {
                convert += tmp.ToString() + " , ";
            }
            else
            {
                convert += tmp.ToString();
            }
        }
        return convert;
    }

    public string ConvertByte( byte[] buffer , uint index , uint length, string delimiter)
    {
        string convert = null;

        for (uint x = 0; x < length; x += sizeof(byte))
        {
            if (x < (length - sizeof(byte)))
            {
                convert += buffer[(int)index + x].ToString() + " , ";
            }
            else
            {
                convert += buffer[(int)index + x].ToString();
            }
        }
        return convert;
    }

    public string ConvertUInt16(byte[] buffer, uint index, uint length, string delimiter)
    {
        string convert = null;

        for (uint x = 0; x < length; x += sizeof(UInt16))
        {
            UInt16 tmp = BitConverter.ToUInt16(buffer , (int)(index + x));

            if (x < (length - sizeof(UInt16)))
            {
                convert += tmp.ToString() + " , ";
            }
            else
            {
                convert += tmp.ToString();
            }
        }
        return convert;
    }

    public string ConvertUInt32(byte[] buffer, uint index, uint length, string delimiter)
    {
        string convert = null;

        for (uint x = 0; x < length; x += sizeof(UInt32))
        {
            UInt32 tmp = BitConverter.ToUInt32(buffer, (int)(index + x));

            if (x < (length - sizeof(UInt32)))
            {
                convert += tmp.ToString() + " , ";
            }
            else
            {
                convert += tmp.ToString();
            }
        }
        return convert;
    }

    public string ConvertToFloat(byte[] buffer, uint index , uint length, string delimiter)
    {
        string convert = null;

        for (uint x = 0; x < length; x += sizeof(Single))
        {
            Single tmp = BitConverter.ToSingle(buffer, (int)(index + x));

            if (x < (length - sizeof(Single)))
            {
                convert += tmp.ToString() + " , ";
            }
            else
            {
                convert += tmp.ToString();
            }
        }
        return convert;
    }

    public string ConvertToString(byte[] buffer , int index , int length)
    {
        return Encoding.ASCII.GetString(buffer, index, length);
    }
}

public class Serial
{
    private Cmd ParserInstance = null;

    /// <summary>
    /// Verweis auf den Parser der benutzt werden soll um die eingehenden
    /// Daten zu überprüfen.
    /// </summary>
    public Serial( Cmd Instance )
    {
        ParserInstance = Instance;
    }


    public SerialPort Client = new SerialPort();


    public int Init( string port, int baud)
    {
        if (Client.IsOpen) return -1;
        if (port == null) return -2;

        if (port.Contains("COM") != false)
        {
            Client.PortName = port;
        }
        else
        {
            return -3;
        }

        if ( baud >= 50 && baud <= 500000 )
        {
            Client.BaudRate = baud;
        }
        else
        {
            return -4;
        }

        Client.ReceivedBytesThreshold = 1;

        /// Event abbonieren
        Client.DataReceived += new SerialDataReceivedEventHandler(Client_DataReceived);

        return 0;
    }

    public void Open()
    {
        if (!Client.IsOpen)
        {
            try
            {
                Client.Open();
                Client.DiscardInBuffer();
                Client.DiscardOutBuffer();
            }
            catch
            {
                Debug.WriteLine("Serial Port: " + "Port cant opened");
            }              
        }
    }

    /// <summary>
    /// Spezielle Funktion um die Serielle Schnittstelle zu schließen.
    /// Bei dem Normalen benutzen der "Close()" Funktion, kann es passieren das 
    /// die "Form" einfriert.
    /// </summary>
    public void Close()
    {
        if (Client.IsOpen)
        {
            Thread CloseDown = new Thread(new ThreadStart(CloseSerialOnPortClose)); //close port in new thread to avoid hang
            CloseDown.Start(); //close port in new thread to avoid hang
        }
    }

    public void WriteString(string str)
    {
        Client.Write(str);
    }

    public void WriteBytes(byte[] data, int index)
    {
        Client.Write(data, index, data.Length);
    }

    public void WriteCommando( byte[] buff )
    {
        if ( !Client.IsOpen ) return;

        try
        {
            Client.Write("-+");
            Client.Write(buff, 0, buff.Length);
            Client.Write("\r\n");
        }
        catch { }
    }

    public UInt32[] GetBaudrates()
    {
        UInt32[] bauds =
        {
            50,
            110,
            150,
            300,
            1200,
            2400,
            4800,
            9600,
            19200,
            38400,
            57600,
            115200,
            230400,
            460800,
            500000
        };

        return bauds;
    }

    public string[] GetPortNames()
    {
        int coms = SerialPort.GetPortNames().Length;
        string[] ports = new string[coms];

        if (coms == 0)
        {
            return null;
        }

        int x = 0;
        foreach (string s in SerialPort.GetPortNames())
        {
            ports[x++] = s;
        }

        return ports;
    }

    public void Dispose()
    {
        Client.Dispose();
    }

    Ringbuffer RingBuff = new Ringbuffer(65535);
    public void Client_DataReceived(object sender, SerialDataReceivedEventArgs e)
    {
        /* Protokoll
            *
            *  Nachrichtenlänge             : 0..255 ( 8 davon für den Header , Rest Nutzdaten.. )
            *  Daten Type                   : 0..6
            *  Nachrichten Identifikation   : 0..255
            *  Funktionsrückgabe Code       : 0..255
            *  Checksumme                   : 0..255
            *  Nutzdaten                    : 0..255
        */

        try
        {
            byte[] tmp = new byte[Client.BytesToRead];

            int Length = Client.Read(tmp, 0, tmp.Length);

            if (Length <= 0) return;
            RingBuff.Push(tmp);

            do
            {
                tmp = RingBuff.Peek(1);
                if ( RingBuff.Length >= tmp[0] )
                {
                    ParserInstance.Parse( RingBuff.Pull(tmp[0]) , ref ParserInstance.CommandoParsed);
                }
                else
                {
                    break;
                }
            } while (RingBuff.Length > 1);

        }
        catch(ArgumentException message)
        {
            Debug.WriteLine(message.Message);
        }
    }

    private void CloseSerialOnPortClose()
    {
        Client.Close(); //close the serial port
    }

    uint Position = 0;
    private void WriteDebugBuffer( string msg , byte[] buffer, int length )
    {
        Position++;
        using (System.IO.StreamWriter file = new System.IO.StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "\\log.txt", true))
        {
            file.WriteLine( "[" + Position.ToString() + "]" + ": " + msg + " -> " + BitConverter.ToString(buffer,0, length) + "\r\n");
        }
    }
}