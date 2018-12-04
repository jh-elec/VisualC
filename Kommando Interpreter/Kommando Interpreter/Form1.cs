using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Diagnostics;

namespace Interpreter
{
    public partial class Form1 : Form
    {
        static Cmd Parser = new Cmd();
        Serial  Port = new Serial(Parser);

        Stopwatch stopwatch = new Stopwatch();

        private ComboBox GetComports( string[] ports)
        {
            ComboBox Ports = new ComboBox();

            Ports.Items.Add(ports);

            return Ports;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Parser.CommandoFrameEvent += new Cmd.EventDelegate(FrameToGUI);
            Parser.SyncForm = this;

            string[] foundPorts = Port.GetPortNames();

            if (foundPorts == null)
            {
                btn_port_open.Enabled = false;
                cmbbx_baudrate.Enabled = false;
                cmbbx_port.Enabled = false;
            }
            else
            {
                for (int x = 0; x < foundPorts.Length; x++)
                {
                    cmbbx_port.Items.Add(foundPorts[x].ToString());
                }
                cmbbx_port.Text = "";
                cmbbx_port.SelectedItem = "COM1";
            }cmbbx_port.Items.Add(">>Refresh<<");

            cmbbx_baudrate.Text = "";
            uint[] baudrates = Port.GetBaudrates();
            for (int x = 0; x < baudrates.Length; x++)
            {
                cmbbx_baudrate.Items.Add(baudrates[x].ToString());
            }

            cmbbx_baudrate.SelectedItem = "115200";

            for (int x = 0; x < (int)Cmd.Data_Type_Enum.__DATA_TYP_MAX_INDEX__; x++)
            {
                cmbbx_data_typ.Items.Add((Cmd.Data_Type_Enum.DATA_TYP_UINT8 + x).ToString());
            }
            cmbbx_data_typ.SelectedItem = "DATA_TYP_UINT8";
        }

        public Form1()
        {
            InitializeComponent();
        }

        private void StartPing()
        {
            Cmd.Commando_Struct ping = new Cmd.Commando_Struct();

            ping.MessageID = 0;
            byte[] send = Parser.BuildHeader(ping);

            Port.WriteCommando(send);
        }

        private void btn_port_open_Click(object sender, EventArgs e)
        {
            switch (Port.Client.IsOpen)
            {
                case true:
                    {
                        Port.Close();
                        btn_port_open.Text = "Öffnen";
                        cmbbx_port.Enabled = true;
                        cmbbx_baudrate.Enabled = true;
                        checkBox1.Checked = false;

                        Debug.WriteLine("Schnittstelle wurde geschlossen..");
                    }
                    break;

                case false:
                    {
                        int PortInit = Port.Init(cmbbx_port.Text, Convert.ToInt32(cmbbx_baudrate.SelectedItem));

                        Debug.WriteLine("Port Init State: " + PortInit);

                        if (PortInit == 0)
                        {
                            Port.Open();
                        }
                        else
                        {
                            Debug.WriteLine("Port Initalisierung fehlerhaft.."); 
                        }

                        btn_port_open.Text = "Schließen";
                        cmbbx_port.Enabled = false;
                        cmbbx_baudrate.Enabled = false;

                        Debug.WriteLine("Schnittstelle wurde geöffnet..");
                    }break;
            }
        }

        private void FrameToGUI( Cmd.Commando_Struct Parsed )
        {
            string DecStrParas = "";

            if ( listView1.Items.Count > 1000 )
            {
                listView1.Items.Clear();
            }


            switch (Parsed.DataType)
            {
                /*  Vorzeichenbehaftete Werte
                 */
                case (byte)Cmd.Data_Type_Enum.DATA_TYP_INT8:
                    {
                        DecStrParas = Parser.ConvertByteToSignedByte(Parsed.Data , 0 , Parsed.DataLength, " , ");
                    }break;
                    
                case (byte)Cmd.Data_Type_Enum.DATA_TYP_INT16:
                    {
                        DecStrParas = Parser.ConvertInt16ToInt16(Parsed.Data , 0 , Parsed.DataLength, " , ");
                    }break;

                case (byte)Cmd.Data_Type_Enum.DATA_TYP_INT32:
                    {
                        DecStrParas = Parser.ConvertInt32ToInt32(Parsed.Data, 0 , Parsed.DataLength, " , ");
                    }break;


                /*  Vorzeichenlose Werte
                 */
                case (byte)Cmd.Data_Type_Enum.DATA_TYP_UINT8:
                    {
                        DecStrParas = Parser.ConvertByte(Parsed.Data, 0 , Parsed.DataLength, " , ");
                    }break;
                    

                case (byte)Cmd.Data_Type_Enum.DATA_TYP_UINT16:
                    {
                        DecStrParas = Parser.ConvertUInt16(Parsed.Data, 0 , Parsed.DataLength, " , ");
                    }break;
                    

                case (byte)Cmd.Data_Type_Enum.DATA_TYP_UINT32:
                    {
                        DecStrParas = Parser.ConvertUInt32(Parsed.Data, 0 , Parsed.DataLength, " , ");
                    }break;
                    

                case (byte)Cmd.Data_Type_Enum.DATA_TYP_FLOAT:
                    {
                        DecStrParas = Parser.ConvertToFloat(Parsed.Data, 0 , Parsed.DataLength, " , ");
                    }break;

                /*  String
                 */
                case (byte)Cmd.Data_Type_Enum.DATA_TYP_STRING:
                    {
                        DecStrParas = Parser.ConvertToString(Parsed.Data, 0 , Parsed.DataLength);
                    }break;  
            }


            ListViewItem cmdItems = new ListViewItem(Parsed.MessageID.ToString());
            cmdItems.SubItems.Add(Parsed.DataLength.ToString());
            cmdItems.SubItems.Add(Parsed.Exitcode.ToString());
            cmdItems.SubItems.Add(Parsed.DataType.ToString());

            if (Parsed.DataLength > 0)
            {
                cmdItems.SubItems.Add(DecStrParas);
            }
            else
            {
                cmdItems.SubItems.Add("-");
            }

            listView1.Items.Add(cmdItems);
            listView1.Items[listView1.Items.Count - 1].EnsureVisible();

            float FrameErrorPercent = (((float)Parser.BadFrameCount / (float)Parser.GoodFrameCount))*100;

            lbl_crc_statistik.Text = "Erfolgreich: " + Parser.GoodFrameCount.ToString() + "\r\n" + "Fehlgeschlagen: " + Parser.BadFrameCount.ToString() + " " + "( " + FrameErrorPercent.ToString("0.0000") + "% )";


            if (messageBoxAnzeigenToolStripMenuItem.CheckState == CheckState.Checked)
            {
                this.Show();
            }
                
            this.Text = "Kommando Interpreter" + "          " + ">>[" + Parser.GoodFrameCount.ToString() + "]<<" + " " + "Kommando(s) empfangen!";
        }

        private void SendCommando( object sender , EventArgs e )
        {
            string[] message     = richtxtbx_message.Text.Split(new char[] { ',' });
            int messageLength    = richtxtbx_message.Text.Split(new char[] { ',' }).Length;
            int messageBytes     = 0;

            switch ( cmbbx_data_typ.SelectedIndex )
            {
                case (byte)Cmd.Data_Type_Enum.DATA_TYP_UINT8:    { messageBytes = messageLength * sizeof(byte);     }   break;
                case (byte)Cmd.Data_Type_Enum.DATA_TYP_UINT16:   { messageBytes = messageLength * sizeof(UInt16);   }   break;
                case (byte)Cmd.Data_Type_Enum.DATA_TYP_UINT32:   { messageBytes = messageLength * sizeof(UInt32);   }   break;
                case (byte)Cmd.Data_Type_Enum.DATA_TYP_FLOAT:    { messageBytes = messageLength * sizeof(float);    }   break;
                case (byte)Cmd.Data_Type_Enum.DATA_TYP_STRING:   { messageBytes = messageLength * sizeof(char);     }   break;
            }

            if (richtxtbx_message.TextLength == 0) messageBytes = 0;

            Cmd.Commando_Struct CommandoToSend = new Cmd.Commando_Struct(messageBytes);

            uint index = 0;
            for ( uint x = 0; x < messageLength && richtxtbx_message.TextLength > 0; x++ )
            {
                switch( cmbbx_data_typ.SelectedIndex )
                {
                    case (byte)Cmd.Data_Type_Enum.DATA_TYP_UINT8:
                        {
                            try
                            {
                                CommandoToSend.Data[index++] = Convert.ToByte(message[x]);
                            }catch
                            {
                                MessageBox.Show("Falsches Format");
                                return;
                            }
                            
                        } break;

                    case (byte)Cmd.Data_Type_Enum.DATA_TYP_UINT16:
                        {
                            try
                            {
                                UInt16 tmp = Convert.ToUInt16(message[x]);
                                CommandoToSend.Data[index++] = (byte)(tmp & 0x00FF);
                                CommandoToSend.Data[index++] = (byte)((tmp & 0xFF00) >> 8);
                            }catch
                            {
                                MessageBox.Show("Falsches Format");
                                return;
                            }

                        } break;

                    case (byte)Cmd.Data_Type_Enum.DATA_TYP_UINT32:
                        {
                            try
                            {
                                UInt32 tmp = Convert.ToUInt32(message[x]);
                                CommandoToSend.Data[index++] = (byte)(tmp & 0x000000FF);
                                CommandoToSend.Data[index++] = (byte)((tmp & 0x0000FF00) >> 8);
                                CommandoToSend.Data[index++] = (byte)((tmp & 0x00FF0000) >> 16);
                                CommandoToSend.Data[index++] = (byte)((tmp & 0xFF000000) >> 26);
                            }catch
                            {
                                MessageBox.Show("Falsches Format");
                                return;
                            }

                        } break;

                    case (byte)Cmd.Data_Type_Enum.DATA_TYP_FLOAT:
                        {
                            try
                            {
                                float tmp = Convert.ToSingle(message[x]);
                                CommandoToSend.Data = BitConverter.GetBytes(tmp);
                            }catch
                            {
                                MessageBox.Show("Falsches Format");
                                return;
                            }
                        
                        } break;

                    case (byte)Cmd.Data_Type_Enum.DATA_TYP_STRING:
                        {
                            try
                            {
                                CommandoToSend.Data = Encoding.ASCII.GetBytes(message[x]);
                            }catch
                            {
                                MessageBox.Show("Falsches Format");
                                return;
                            }

                        } break;
                }
            }

            CommandoToSend.MessageID    = (byte)numeric_msg_id.Value;           // Nachrichten Type
            CommandoToSend.DataType     = (byte)cmbbx_data_typ.SelectedIndex;   // Datentyp der Bytes
            CommandoToSend.DataLength   = (byte)CommandoToSend.Data.Length;     // Länge der gesamten Nachricht

            byte[] send = Parser.BuildHeader(CommandoToSend);

            Port.WriteCommando(send);

            richtxtbx_data_was_send.AppendText( "[Hex] Header: " + BitConverter.ToString(send, 0 , (byte)Cmd.Communication_Header_Enum.__CMD_HEADER_ENTRYS__) + "   -   ");

            if ( messageBytes > 0 )
            {
                richtxtbx_data_was_send.AppendText("Nutzdaten: " + BitConverter.ToString(send, (byte)Cmd.Communication_Header_Enum.__CMD_HEADER_ENTRYS__, CommandoToSend.DataLength) + "\r\n");
            }
            else
            {
                richtxtbx_data_was_send.AppendText("Nutzdaten: -\r\n");
            }
        }


        private void btn_data_send_Click(object sender, EventArgs e)
        {
            if (checkbx_send_start_bytes.CheckState == CheckState.Checked)
            {
                Port.StartBytes = true;
            }
            else
            {
                Port.StartBytes = false;
            }

            SendCommando(sender, e);
        }

        private void richtxtbx_data_was_send_DoubleClick(object sender, EventArgs e)
        {
            richtxtbx_data_was_send.Clear();
        }

        private void listView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete) listView1.Items.Clear();
        }

        private void richtxtbx_data_was_send_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete) richtxtbx_data_was_send.Clear();
        }

        private void richtxtbx_data_was_send_TextChanged(object sender, EventArgs e)
        {
            richtxtbx_data_was_send.SelectionStart = this.richtxtbx_data_was_send.Text.Length;
            richtxtbx_data_was_send.ScrollToCaret();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult res = MessageBox.Show("Daten wirklich löschen?", "Achtung!", MessageBoxButtons.YesNo);
            if (res == DialogResult.Yes)
            {
                richtxtbx_data_was_send.Clear();
            }
            else
            {
                return;
            }
        }

        private void richtxtbx_message_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete) richtxtbx_message.Clear();
        }

        private void label7_Click(object sender, EventArgs e)
        {
            lbl_crc_statistik.Text = "Erfolgreich: 0\r\nFehlgeschlagen: 0";
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Port.Close();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if ( checkBox1.CheckState == CheckState.Checked )
            {
                SendCycleTimer.Interval = (int)numeric_send_cycle_timer_interval.Value;
                SendCycleTimer.Enabled = true;
                SendCycleTimer.Start();
                stopwatch.Start();
            }
            else
            {
                SendCycleTimer.Enabled = false;
                stopwatch.Stop();
            }
        }

        private void SendCycleTimer_Tick(object sender, EventArgs e)
        {
            if ( checkbx_send_start_bytes.CheckState == CheckState.Checked )
            {
                Port.StartBytes = true;
            }
            else
            {
                Port.StartBytes = false;
            }

            SendCommando(sender, e); 
        }

        private void messageBoxAnzeigenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (messageBoxAnzeigenToolStripMenuItem.CheckState == CheckState.Unchecked)
            {
                messageBoxAnzeigenToolStripMenuItem.Checked = true;
            }
            else
            {
                messageBoxAnzeigenToolStripMenuItem.Checked = false;
            }
        }

        private void inDenTrayToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void notifyIcon1_DoubleClick(object sender, EventArgs e)
        {
            this.Show();
        }

        private void richtxtbx_data_was_send_KeyDown_1(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                richtxtbx_data_was_send.Clear();
            }
        }

        private void cmbbx_port_TextChanged(object sender, EventArgs e)
        {
            if ( (string)cmbbx_port.SelectedItem == ">>Refresh<<")
            {
                cmbbx_port.Items.Clear();
                string[] foundPorts = Port.GetPortNames();
                for (int x = 0; x < foundPorts.Length; x++)
                {
                    cmbbx_port.Items.Add(foundPorts[x].ToString());
                }
                cmbbx_port.Text = "";
                cmbbx_port.SelectedItem = "COM1";
                cmbbx_port.Items.Add(">>Refresh<<");
            }
        }

        private void bugMeldenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start
            ("mailto:J.Homann@jh-elec.de?subject=" + "Kommando Interpreter" + " - " + "Ver.:" + Application.ProductVersion + "&body=" + "Mir ist folgendes aufgefallen,");
        }

        private void checkbx_send_start_bytes_CheckedChanged(object sender, EventArgs e)
        {
            if (checkbx_send_start_bytes.CheckState == CheckState.Checked)
            {
                Port.StartBytes = true;
            }
            else
            {
                Port.StartBytes = false;
            }
        }
    }
}
