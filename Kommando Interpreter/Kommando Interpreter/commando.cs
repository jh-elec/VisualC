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
    public delegate void EventDelegate( byte[] buffer , uint length );

    public event EventDelegate CommandoFrameEvent;

    public void CommandoFrameEventFnc( byte[] buffer, uint length )
    {
        // Prüft ob das Event überhaupt einen Abonnenten hat.
        CommandoFrameEvent?.Invoke( buffer , length );    
    }
        
    public bool CrcOk = false;
    public bool CrcBad = false;

    public  uint CrcOkCnt = 0;
    public  uint CrcErrorCnt = 0;
    private int CommandoHeaderIndex = 0;

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
        public byte SlaveCRC;

        public byte[] Data;

        public Commando_Struct( int Size )
        {
            DataLength  = 0;
            DataType    = 0;
            MessageID   = 0;
            Exitcode    = 0;
            SlaveCRC    = 0;

            Data = new byte[Size];
        }
    }

    public Commando_Struct CommandoParsed;

    public int ParseStart(byte[] buffer)
    {
        int startByte1Index = 0;
        int startByte2Index = 0;

        CommandoHeaderIndex = 0;

        startByte1Index = Array.IndexOf(buffer, (byte)'-', 0);
        startByte2Index = Array.IndexOf(buffer, (byte)'+', 0);

        if ( ( startByte2Index - startByte1Index) == 1 )
        {
            CommandoHeaderIndex = startByte1Index;
            return CommandoHeaderIndex;
        }
     
        return -1;
    }

    public int Parse(byte[] buffer, ref Commando_Struct Commando)
    {
        if (buffer.Length < (byte)Communication_Header_Enum.__CMD_HEADER_ENTRYS__)
        {
            return -1;
        }
        
        Commando.Data = new byte[buffer[(byte)Cmd.Communication_Header_Enum.CMD_HEADER_LENGHT]];

        try
        {
            Commando.DataLength = buffer[CommandoHeaderIndex + (byte)Communication_Header_Enum.CMD_HEADER_LENGHT];
            Commando.DataType   = buffer[CommandoHeaderIndex + (byte)Communication_Header_Enum.CMD_HEADER_DATA_TYP];
            Commando.MessageID  = buffer[CommandoHeaderIndex + (byte)Communication_Header_Enum.CMD_HEADER_ID];
            Commando.Exitcode   = buffer[CommandoHeaderIndex + (byte)Communication_Header_Enum.CMD_HEADER_EXITCODE];
            Commando.SlaveCRC   = buffer[CommandoHeaderIndex + (byte)Communication_Header_Enum.CMD_HEADER_CRC];
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
        byte FrameSlaveCRC = 0;
        for (int x = 0; x < (byte)Cmd.Communication_Header_Enum.__CMD_HEADER_ENTRYS__; x++)
        {
            FrameSlaveCRC = CmdCrc8CCITTUpdate(FrameSlaveCRC, Frame_[x]);
        }

        /*	Checksumme von Nutzdaten bilden
        */
        if (Commando.DataLength > 0)
        {
            for (int x = 0; x < Commando.DataLength; x++)
            {
                try
                {
                    FrameSlaveCRC = CmdCrc8CCITTUpdate(FrameSlaveCRC, buffer[(CommandoHeaderIndex + (byte)Communication_Header_Enum.__CMD_HEADER_ENTRYS__) + x]);
                    Commando.Data[x] = buffer[(CommandoHeaderIndex + (byte)Communication_Header_Enum.__CMD_HEADER_ENTRYS__) + x];
                }
                catch
                {
                    return -4;
                }
            }
        }

        if (FrameSlaveCRC == Commando.SlaveCRC)
        {
            CrcOk = true;
            CrcOkCnt++;

            // Event feuern..
            CommandoFrameEventFnc( buffer , Commando.DataLength );
        }
        else
        {
            CrcBad = true;
            CrcErrorCnt++;

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
        return ASCIIEncoding.ASCII.GetString(buffer, index, length);
    }
}

public class Serial
{
    private Cmd ParserInstance = null;

    System.Windows.Forms.Timer ReadRingBuffTimer = new System.Windows.Forms.Timer();

    /// <summary>
    /// Verweis auf den Parser der benutzt werden soll um die eingehenden
    /// Daten zu überprüfen.
    /// </summary>
    public Serial( Cmd Instance )
    {
        ParserInstance = Instance;
    }


    private static SerialPort Client = new SerialPort();

    public void Init( string port, int baud)
    {
        if (!Client.IsOpen)
        {
            if ( port.Contains("COM") != false && port != null )
            {
                Client.PortName = port;
                Client.BaudRate = baud;
                Client.ReceivedBytesThreshold = 1;
            }
        }

        /// Event abbonieren
        Client.DataReceived += new SerialDataReceivedEventHandler(Client_DataReceived);
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
            catch { }              
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

    public bool IsOpen()
    {
        return Client.IsOpen;
    }

    public int BytesToRead()
    {
        return Client.BytesToRead;
    }

    public int BytesToWrite()
    {
        return Client.BytesToWrite;
    }

    public string ReadLine()
    {
        return Client.ReadLine();
    }

    public string ReadExist()
    {
        return Client.ReadExisting();
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

    struct Search_Frame_Struct
    {
        public int     Count;
        public int     LastRead;

        public int     FrameLengMust;

        public int     Index;
        public int     FrameSize;
        public byte[]  Frame;

        public Search_Frame_Struct( int size )
        {
            Count           = 0; // Wie viele Frames wurden gefunden?
            LastRead        = 0;
            FrameLengMust   = 0; // Wie groß ist der aktuell zu empfangender Frame?
            Index           = 0;
            FrameSize       = 0;
            Frame           = new byte[size];
        }
    }


    private int GetStartOfFrame( byte[] buffer )
    {
        int StartOfFrame = 0;
        
        for (; StartOfFrame < buffer.Length; StartOfFrame++)
        {
            if ( buffer[StartOfFrame] == '-' )
            {
                if ( buffer[StartOfFrame + 1] == '+')
                {
                    return StartOfFrame;
                }
            }
        }

        return -1;
    }

    private int GetEndOfFrame( byte[] buffer )
    {
        int EndOfFrame = 0;

        for (; EndOfFrame < buffer.Length; EndOfFrame++)
        {
            if ( buffer[EndOfFrame] == '\r' )
            {
                if ( buffer[EndOfFrame+1] == '\n')
                {
                    return EndOfFrame;
                }
            }
        }

        return -1;
    }


    int TotalBytes = 0;
    byte[] buffer = new byte[1024];

    Ringbuffer RingBuffer = new Ringbuffer(1024);

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

        /*  Empfangene Daten abholen
        */
        try
        {
            int BytesRead = Client.Read(buffer, TotalBytes , Client.BytesToRead);
            TotalBytes += BytesRead;

            if (buffer[0] < TotalBytes) return;
            MessageBox.Show(BitConverter.ToString(buffer, 0, buffer[0]));
            ParserInstance.Parse(buffer, ref ParserInstance.CommandoParsed);
        }
        catch
        {
            return;
        }
    }

    private static void CloseSerialOnPortClose()
    {
        try
        {
            Client.Close(); //close the serial port
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message); //catch any serial port closing error messages
        }
    }

    private void WriteDebugBuffer(byte[] buffer, int length )
    {
        using (System.IO.StreamWriter file = new System.IO.StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "\\log.txt", true))
        {
            file.WriteLine(BitConverter.ToString(buffer,0, length) + "\r\n");
        }
    }
}