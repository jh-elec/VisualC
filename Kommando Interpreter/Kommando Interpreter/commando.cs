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
        CMD_HEADER_START_BYTE1, // Kommando Start Byte 1
        CMD_HEADER_START_BYTE2, // .. Byte 2
        CMD_HEADER_LENGHT,      // Länge des ganzen Streams
        CMD_HEADER_DATA_LENGHT, // Länge der Roh Daten
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
        public byte msgLen;
        public byte dataLen;
        public byte dataTyp;
        public byte id;
        public byte exitcode;
        public byte crc;
        public byte[] data;

        public Commando_Struct( int rawDataSize )
        {
            msgLen      = 0;
            dataLen     = 0;
            dataTyp     = 0;
            id          = 0;
            exitcode    = 0;
            crc         = 0;
            data        = new byte[rawDataSize];
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

    public int Parse(byte[] buffer, ref Commando_Struct CmdStruct)
    {
        CmdStruct.data = new byte[50];

        try
        {
            CmdStruct.msgLen = buffer[CommandoHeaderIndex + (byte)Communication_Header_Enum.CMD_HEADER_LENGHT];
            CmdStruct.dataLen = buffer[CommandoHeaderIndex + (byte)Communication_Header_Enum.CMD_HEADER_DATA_LENGHT];
            CmdStruct.dataTyp = buffer[CommandoHeaderIndex + (byte)Communication_Header_Enum.CMD_HEADER_DATA_TYP];
            CmdStruct.id = buffer[CommandoHeaderIndex + (byte)Communication_Header_Enum.CMD_HEADER_ID];
            CmdStruct.exitcode = buffer[CommandoHeaderIndex + (byte)Communication_Header_Enum.CMD_HEADER_EXITCODE];
            CmdStruct.crc = buffer[CommandoHeaderIndex + (byte)Communication_Header_Enum.CMD_HEADER_CRC];
        }
        catch
        {
            return -1;
        }

        /*  Temporärer Speicher für die Checksummenbildung
        */
        byte[] stream = new byte[8];
        try
        {
            stream[0] = buffer[CommandoHeaderIndex + (byte)Communication_Header_Enum.CMD_HEADER_START_BYTE1];
            stream[1] = buffer[CommandoHeaderIndex + (byte)Communication_Header_Enum.CMD_HEADER_START_BYTE2];
            stream[2] = buffer[CommandoHeaderIndex + (byte)Communication_Header_Enum.CMD_HEADER_LENGHT];
            stream[3] = buffer[CommandoHeaderIndex + (byte)Communication_Header_Enum.CMD_HEADER_DATA_LENGHT];
            stream[4] = buffer[CommandoHeaderIndex + (byte)Communication_Header_Enum.CMD_HEADER_DATA_TYP];
            stream[5] = buffer[CommandoHeaderIndex + (byte)Communication_Header_Enum.CMD_HEADER_ID];
            stream[6] = buffer[CommandoHeaderIndex + (byte)Communication_Header_Enum.CMD_HEADER_EXITCODE];
            stream[7] = 0; // CRC
        }
        catch
        {
            return -2;
        }

        /*	Checksumme vom Header bilden
        */
        byte crc = 0;
        for (int x = 0; x < (byte)Cmd.Communication_Header_Enum.__CMD_HEADER_ENTRYS__; x++)
        {
            try
            {
                crc = CmdCrc8CCITTUpdate(crc, stream[x]);
            }
            catch
            {
                return -3;
            }
        }

        /*	Checksumme von Nutzdaten bilden
        */
        if (CmdStruct.dataLen > 0)
        {
            for (int x = 0; x < CmdStruct.dataLen; x++)
            {
                try
                {
                    crc = CmdCrc8CCITTUpdate(crc, buffer[(CommandoHeaderIndex + (byte)Communication_Header_Enum.__CMD_HEADER_ENTRYS__) + x]);
                    CmdStruct.data[x] = buffer[(CommandoHeaderIndex + (byte)Communication_Header_Enum.__CMD_HEADER_ENTRYS__) + x];
                }
                catch
                {
                    return -4;
                }
            }
        }

        if (crc == CmdStruct.crc)
        {
            CrcOk = true;
            CrcOkCnt++;

            // Event feuern..
            CommandoFrameEventFnc( buffer , CmdStruct.msgLen );
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
        byte[] cmdMsg = new byte[(byte)Communication_Header_Enum.__CMD_HEADER_ENTRYS__ + send.dataLen];

        cmdMsg[(int)Communication_Header_Enum.CMD_HEADER_CRC] = 0;

        byte msgLen = (byte)Communication_Header_Enum.__CMD_HEADER_ENTRYS__;
        msgLen += send.dataLen;

        cmdMsg[(byte)Communication_Header_Enum.CMD_HEADER_START_BYTE1]   = (byte)'-';       // Start Byte 1
        cmdMsg[(byte)Communication_Header_Enum.CMD_HEADER_START_BYTE2]   = (byte)'+';        // Start Byte 2 
        cmdMsg[(byte)Communication_Header_Enum.CMD_HEADER_LENGHT]        = msgLen;
        cmdMsg[(byte)Communication_Header_Enum.CMD_HEADER_DATA_LENGHT]   = send.dataLen;
        cmdMsg[(byte)Communication_Header_Enum.CMD_HEADER_DATA_TYP]      = send.dataTyp;
        cmdMsg[(byte)Communication_Header_Enum.CMD_HEADER_ID]            = send.id;
        cmdMsg[(byte)Communication_Header_Enum.CMD_HEADER_EXITCODE]      = send.exitcode;
        cmdMsg[(byte)Communication_Header_Enum.CMD_HEADER_CRC]           = 0;

        byte crc = 0;

        for ( int x = 0; x < (byte)Communication_Header_Enum.__CMD_HEADER_ENTRYS__; x++)
        {
            crc = CmdCrc8CCITTUpdate(crc, cmdMsg[x]);
        }

        if ( send.dataLen > 0 )
        {
            for ( int x = 0; x < send.dataLen; x++ )
            {
                crc = CmdCrc8CCITTUpdate(crc, send.data[x]);
                cmdMsg[(byte)Communication_Header_Enum.__CMD_HEADER_ENTRYS__ + x] = send.data[x];
            }
        }

        cmdMsg[(byte)Communication_Header_Enum.CMD_HEADER_CRC] = crc;

        send.crc        = crc;
        send.msgLen     = msgLen;

        return cmdMsg;
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

        public int[]   Index;
        public int[]   FrameSize;

        public Search_Frame_Struct( int size )
        {
            Count       = 0;
            LastRead    = 0;
            Index       = new int[size];
            FrameSize   = new int[size];
        }
    }

    private Search_Frame_Struct SearchCommandoFrame( byte[] buffer , int length )
    {
        Search_Frame_Struct FrameInfo = new Search_Frame_Struct(length);
        uint IndexFound = 0;
        for ( int x = 0; x < length; x++ )
        {
            if ( buffer[x] == (char)'-')
            {
                if ( buffer[x+1] == (char)'+')
                {
                    FrameInfo.FrameSize[IndexFound]  = buffer[x + 2];
                    FrameInfo.Index[IndexFound++]    = x;
                    FrameInfo.Count++;
                }
            }
        }
        return FrameInfo;
    }

    private byte[] GetCommandoFrame(ref byte[] buffer, ref Search_Frame_Struct FrameInfo , uint SelectFrame )
    {
        if (SelectFrame > FrameInfo.Count || FrameInfo.LastRead > FrameInfo.Count) return null;

        byte[] Frame = new byte[FrameInfo.FrameSize[SelectFrame]];

        for ( int x = 0; x < FrameInfo.FrameSize[SelectFrame]; x++ )
        {
            Frame[x] = buffer[FrameInfo.Index[SelectFrame] + x];
        }

        FrameInfo.LastRead++;

        return Frame;
    }

    int Length = 0;
    byte[] buffer = new byte[4096];

    public void Client_DataReceived(object sender, SerialDataReceivedEventArgs e)
    {
        /* Protokoll
            *
            *  Startbyte[0]                    : '-'
            *  Startbyte[1]                    : '+'
            *  Nachrichtenlänge[2]             : 0..255
            *  Anzahl Nutzdaten[3]             : 0..255
            *  Daten Type[4]                   : 0..6
            *  Nachrichten Identifikation[5]   : 0..255
            *  Funktionsrückgabe Code[6]       : 0..255
            *  Checksumme[7]                   : 0..255
            *  Nutzdaten[8..n]                 : 0..255
        */

        /*  Empfangene Daten abholen
        */
        try
        {
            Length += Client.Read(buffer, Length , Client.BytesToRead);

            Search_Frame_Struct FrameInfo = SearchCommandoFrame(buffer, Length);

            if ( FrameInfo.Count > 0 )
            {
                for ( uint x = 0; x < FrameInfo.Count; x++ )
                {
                    byte[] Frame = GetCommandoFrame(ref buffer, ref FrameInfo, x);
                    ParserInstance.Parse(Frame, ref ParserInstance.CommandoParsed);
                }
                Length = 0;
            }

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

    private void WriteDebugBuffer(byte[] buffer, int buffLength, int bytesIsReceived, int crc)
    {
        using (System.IO.StreamWriter file = new System.IO.StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "\\log.txt", true))
        {
            file.WriteLine("[" + buffLength.ToString("00") + "]" + " " + "[" + crc.ToString() + "]" + " " + "[" + bytesIsReceived.ToString("00") + "]" + " " + "[" + DateTime.Now.ToLongTimeString() + "]" + " " + "[" + BitConverter.ToString(buffer, 0, buffLength) + "]");
        }
    }
}