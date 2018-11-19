﻿using System;
using System.Windows.Forms;
using System.IO.Ports;
using System.Threading;
using System.Threading.Tasks;

namespace Commando
{
    public class Cmd
    {
        /*  Event Delegate
         */
        public delegate void EventDelegate();

        /* Abbonieren dieses Events von anderen Klassen aus
         * Cmd.NewCommandoPackageEvent += new Cmd.EventDelegate(WriteCommandoToGui);
         */        
        public static event EventDelegate CrcErrorEvent;
        public static void CrcErrorEventFunc()
        {
            // Prüft ob das Event überhaupt einen Abonnenten hat.
            CrcErrorEvent?.Invoke();
        }

        public static event EventDelegate NewCommandoPackageEvent;
        public static void NewCommandoPackageEventFunc()
        {
            // Prüft ob das Event überhaupt einen Abonnenten hat.
            NewCommandoPackageEvent?.Invoke();    
        }
        
        public static bool CrcOk = false;
        public static bool CrcBad = false;

        public static uint CrcOkCnt = 0;
        public static uint CrcErrorCnt = 0;
        private static int CommandoHeaderIndex = 0;

        private static Byte CmdCrc8CCITTUpdate(Byte inCrc, Byte inData)
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
        private static Byte CmdCrc8StrCCITT(string str)
        {
            Byte crc = 0;
            Byte x = 0;

            for (x = 0; x < str.Length; x++)
            {
                crc = CmdCrc8CCITTUpdate(crc, Convert.ToByte(str[x]));
            }

            return crc;
        }

        public enum Commando_List_Enum          
        {
            CMD_BEGINN_WITH_PARA = 0,
            CMD_END,
            CMD_CRC_SIGN,
            CMD_PARA_SIGN,

            CMD_RELAIS,
            CMD_INIT,
            CMD_EEPROM,
            CMD_CAL,
            CMD_GET_VOLTAGE,
            CMD_GET_CURRENT,
            CMD_SET_ADMIN,
            CMD_GET_STATE,

            __MAX_COMMANDOS__
        }

        public enum Data_Typ_Enum               
        {
            DATA_TYP_UINT8,

            DATA_TYP_UINT16,

            DATA_TYP_UINT32,

            DATA_TYP_FLOAT,

            DATA_TYP_STRING,

            __DATA_TYP_MAX_INDEX__
        };

        public enum Communication_Header_Enum   : byte
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

        public enum Id_Codes_Enum               
        {
            ID_RELAIS,
            ID_REINIT,
            ID_EEPROM,
            ID_VOLTAGE,
            ID_CURRENT,
            ID_CALIBRATION,
            ID_STATE,
            ID_ADMIN,
            ID_STRING_DISPLAY,
            ID_VERSION,

            ID_CALIBRATION_DATE,
            ID_SERIALNUMBER,
            ID_HARDWARE_VERSION,

            ID_PERIOD,
            ID_PHASE_ANGEL,

            ID_APPAREND_RMS_POWER,
            ID_APPAREND_ENERGY,

            ID_STPM32_MONITOR_DATA,

            ID_MANUAL_READ_STPM32,
            ID_MANUAL_WRITE_STPM32,

            ID_BOOT_MSG,
            ID_COMMAND,

            __ID_MAX_INDEX__
        };

        public enum Return_Codes_Enum           
        {
            RETURN_ALL_OK,
            RETURN_USART_CRC_OK,
            RETURN_USART_CRC_BAD,
            RETURN_CMD_OK,
            RETURN_CMD_BAD,
            RETURN_ADMIN_OK,
            RETURN_ADMIN_BAD,
            RETURN_OVERVOLTAGE_PROTECTION,

            RETURN_CALIBRATION_START,

            RETURN_CHV_ERROR,
            RETURN_CHC_ERROR,

            __RETURN_MAX_INDEX__
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

        public static Commando_Struct CommandoParsed;

        public  int       ParseStart  ( byte[] buffer )           
        {
            int startByte1Index = 0;
            int startByte2Index = 0;

            CommandoHeaderIndex   = 0;

            startByte1Index = Array.IndexOf(buffer, (byte)'-', 0);
            startByte2Index = Array.IndexOf(buffer, (byte)'+', 0);

            try
            {
                CommandoHeaderIndex = startByte2Index - startByte1Index;
            }
            catch( Exception e)
            {
                Console.WriteLine(e.Message);
            }

            if ( CommandoHeaderIndex == 1 )
            {
                CommandoHeaderIndex = startByte1Index;
                return startByte1Index;
            }

            return -1;
        }

        public  void      Parse(byte[] buffer, ref Commando_Struct CmdStruct)
        {
            CmdStruct.data = new byte[50];

            try
            {
                CmdStruct.msgLen    = buffer[CommandoHeaderIndex + (byte)Communication_Header_Enum.CMD_HEADER_LENGHT ];
                CmdStruct.dataLen   = buffer[CommandoHeaderIndex + (byte)Communication_Header_Enum.CMD_HEADER_DATA_LENGHT ];
                CmdStruct.dataTyp   = buffer[CommandoHeaderIndex + (byte)Communication_Header_Enum.CMD_HEADER_DATA_TYP];
                CmdStruct.id        = buffer[CommandoHeaderIndex + (byte)Communication_Header_Enum.CMD_HEADER_ID];
                CmdStruct.exitcode  = buffer[CommandoHeaderIndex + (byte)Communication_Header_Enum.CMD_HEADER_EXITCODE];
                CmdStruct.crc       = buffer[CommandoHeaderIndex + (byte)Communication_Header_Enum.CMD_HEADER_CRC];
            }
            catch
            {
                return;
            }

            /*  Temporärer Speicher für die Checksummenbildung
            */
            byte[] stream = new byte[8];
            try
            {
                stream[0] = buffer[CommandoHeaderIndex + (byte)Communication_Header_Enum.CMD_HEADER_START_BYTE1];
                stream[1] = buffer[CommandoHeaderIndex + (byte)Communication_Header_Enum.CMD_HEADER_START_BYTE2 ];
                stream[2] = buffer[CommandoHeaderIndex + (byte)Communication_Header_Enum.CMD_HEADER_LENGHT];
                stream[3] = buffer[CommandoHeaderIndex + (byte)Communication_Header_Enum.CMD_HEADER_DATA_LENGHT];
                stream[4] = buffer[CommandoHeaderIndex + (byte)Communication_Header_Enum.CMD_HEADER_DATA_TYP];
                stream[5] = buffer[CommandoHeaderIndex + (byte)Communication_Header_Enum.CMD_HEADER_ID];
                stream[6] = buffer[CommandoHeaderIndex + (byte)Communication_Header_Enum.CMD_HEADER_EXITCODE];
                stream[7] = 0; // CRC
            }
            catch
            {
                return;
            }

            /*	Checksumme vom Header bilden
            */
            byte crc = 0;
            for ( int x = 0 ; x < (byte)Cmd.Communication_Header_Enum.__CMD_HEADER_ENTRYS__ ; x++ )
            {
                try
                {
                    crc = CmdCrc8CCITTUpdate(crc, stream[x]);
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                }   
            }

            /*	Checksumme von Nutzdaten bilden
            */
            if (CmdStruct.dataLen > 0 )
            {
                for (int x = 0; x < CmdStruct.dataLen; x++)
                {
                    try
                    {
                        crc = CmdCrc8CCITTUpdate(crc, buffer[(CommandoHeaderIndex + (byte)Communication_Header_Enum.__CMD_HEADER_ENTRYS__) + x ] );
                        CmdStruct.data[x] = buffer[(CommandoHeaderIndex + (byte)Communication_Header_Enum.__CMD_HEADER_ENTRYS__) + x];
                    }
                    catch( Exception e )
                    {
                        Console.WriteLine(e.Message);
                    }
                }
            }        

            if (crc == CmdStruct.crc)
            {
                CrcOk = true;
                CrcOkCnt++;

                // Event feuern..
                NewCommandoPackageEventFunc();
            }
            else
            {
                CrcBad = true;
                CrcErrorCnt++;

                //Event feuern..
                CrcErrorEventFunc();
            }
        }

        public  byte[]    BuildHeader ( Commando_Struct send )    
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
    }

    public class Serial
    {
        Cmd Parser = null;

        public Serial( Cmd inst )
        {
            Parser = inst;
        }

        private static SerialPort Client = new SerialPort();

        public  int Init( string port, int baud)
        {
            if (!Client.IsOpen)
            {
                if ( port.Contains("COM") != false && port != null )
                {
                    Client.PortName = port;
                }
                else
                {
                    return -1;
                }
                
                Client.BaudRate = baud;
            }
            else
            {
                return -2;
            }

            Client.ReceivedBytesThreshold = 1;

            /// Event abbonieren
            Client.DataReceived += new SerialDataReceivedEventHandler(Client_DataReceived);

            return 0;
        }

        public  void Open()
        {
            if (!Client.IsOpen)
            {
                Client.Open();
                Client.DiscardInBuffer();
                Client.DiscardOutBuffer();
            }
        }

        public  void Close()
        {
            if (Client.IsOpen)
            {
                Thread CloseDown = new Thread(new ThreadStart(CloseSerialOnPortClose)); //close port in new thread to avoid hang
                CloseDown.Start(); //close port in new thread to avoid hang
            }
        }

        public  bool IsOpen()
        {
            return Client.IsOpen;
        }

        public  int BytesToRead()
        {
            return Client.BytesToRead;
        }

        public  int BytesToWrite()
        {
            return Client.BytesToWrite;
        }

        public  string ReadLine()
        {
            return Client.ReadLine();
        }

        public string ReadExist()
        {
            return Client.ReadExisting();
        }

        public  void WriteString(string str)
        {
            Client.Write(str);
        }

        public  void WriteBytes(byte[] data, int index)
        {
            Client.Write(data, index, data.Length);
        }

        public  void WriteCommando( byte[] buff )
        {
            if ( !Client.IsOpen ) return;

            Client.Write(buff, 0, buff.Length);
            Client.Write("\r\n");
        }

        public  UInt32[] GetBaudrates()
        {
          UInt32[] bauds =
        {
                300,
                600,
                1200,
                2400,
                9600,
                14400,
                19200,
                38400,
                57600,
                115200,
            };

            return bauds;
        }

        public  string[] GetPortNames()
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

        public  void Dispose()
        {
            Client.Dispose();
        }

        static int length = 0;
        static uint bytesToReceive = 0;
        public byte[] buffer = new byte[500];

        public void Client_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            if (Client.BytesToRead < (byte)Cmd.Communication_Header_Enum.__CMD_HEADER_ENTRYS__) return;

            /*  Empfangene Daten abholen
            */
            try
            {
                length += Client.Read( buffer , length , 250 );
            }catch{}

            /*  Kommando Start Parsen
             *  Rückgabewert: - 1 = Kein Start gefunden..
            */
            int index = Parser.ParseStart(buffer);
            if (index != -1)
            {
                /*  Anzahl der zu empfangenen Bytes auslesen
                    * HIER WIRD NOCH KEIN CRC BERECHNET!
                    * Es könnten Übertragungsfehler nicht erkannt werden..
                */
                bytesToReceive = buffer[index + (byte)Cmd.Communication_Header_Enum.CMD_HEADER_LENGHT];
            }

            /*  Wurden alle Bytes empfangen?
                *  Wenn nicht, direkt wieder raus hier!
            */
            if (length < bytesToReceive)
            {
                return;
            }
            else
            {
                length = 0;
                bytesToReceive = 0;
            }

            /*  Kommando untersuchen..
            */
            Parser.Parse( buffer, ref Cmd.CommandoParsed );
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
    }
}

