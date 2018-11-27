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

namespace Interpreter
{
    public partial class Form1 : Form
    {
        static Cmd Parser = new Cmd();
        Serial  Port = new Serial(Parser);

        public uint NewCommandoCnt = 0;

        private ComboBox GetComports( string[] ports)
        {
            ComboBox Ports = new ComboBox();

            Ports.Items.Add(ports);

            return Ports;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Parser.CommandoFrameEvent += new Cmd.EventDelegate(WriteDecodedBuffer);

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

        private async void StartPing()
        {
            Cmd.Commando_Struct ping = new Cmd.Commando_Struct();

            ping.MessageID = 0;
            byte[] send = Parser.BuildHeader(ping);
            Port.WriteCommando(send);
            await Task.Delay(500);
        }

        private void btn_port_open_Click(object sender, EventArgs e)
        {
            if (Port.IsOpen())
            {
                btn_port_open.Text = "Öffnen";
                Port.Close();
                Port.Dispose();
                cmbbx_port.Enabled = true;
                cmbbx_baudrate.Enabled = true;
                checkBox1.Checked = false;
            }
            else
            {
                Port.Init(cmbbx_port.Text, Convert.ToInt32(cmbbx_baudrate.Text));
                Port.Open();
                btn_port_open.Text = "Schließen";
                cmbbx_port.Enabled = false;
                cmbbx_baudrate.Enabled = false;
                StartPing();
            }
        }

        private void WriteDecodedBuffer( byte[] buffer , uint length )
        {
            NewCommandoCnt++;

            string DecStrHeader = Parser.ConvertByte(buffer, 0, (byte)Cmd.Communication_Header_Enum.__CMD_HEADER_ENTRYS__ , " , ");

            string DecStrParas = null;


            switch (Parser.CommandoParsed.DataType)
            {
                /*  Vorzeichenbehaftete Werte
                 */
                case (byte)Cmd.Data_Type_Enum.DATA_TYP_INT8:
                    {
                        DecStrParas = Parser.ConvertByteToSignedByte(buffer , (byte)Cmd.Communication_Header_Enum.__CMD_HEADER_ENTRYS__, Parser.CommandoParsed.DataLength, " , ");
                    }break;
                    
                case (byte)Cmd.Data_Type_Enum.DATA_TYP_INT16:
                    {
                        DecStrParas = Parser.ConvertInt16ToInt16(buffer , (byte)Cmd.Communication_Header_Enum.__CMD_HEADER_ENTRYS__, Parser.CommandoParsed.DataLength, " , ");
                    }break;

                case (byte)Cmd.Data_Type_Enum.DATA_TYP_INT32:
                    {
                        DecStrParas = Parser.ConvertInt32ToInt32(buffer, (byte)Cmd.Communication_Header_Enum.__CMD_HEADER_ENTRYS__, Parser.CommandoParsed.DataLength, " , ");
                    }break;


                /*  Vorzeichenlose Werte
                 */
                case (byte)Cmd.Data_Type_Enum.DATA_TYP_UINT8:
                    {
                        DecStrParas = Parser.ConvertByte(buffer, (byte)Cmd.Communication_Header_Enum.__CMD_HEADER_ENTRYS__, Parser.CommandoParsed.DataLength, " , ");
                    }break;
                    

                case (byte)Cmd.Data_Type_Enum.DATA_TYP_UINT16:
                    {
                        DecStrParas = Parser.ConvertUInt16(buffer, (byte)Cmd.Communication_Header_Enum.__CMD_HEADER_ENTRYS__, Parser.CommandoParsed.DataLength, " , ");
                    }break;
                    

                case (byte)Cmd.Data_Type_Enum.DATA_TYP_UINT32:
                    {
                        DecStrParas = Parser.ConvertUInt32(buffer, (byte)Cmd.Communication_Header_Enum.__CMD_HEADER_ENTRYS__, Parser.CommandoParsed.DataLength, " , ");
                    }break;
                    

                case (byte)Cmd.Data_Type_Enum.DATA_TYP_FLOAT:
                    {
                        DecStrParas = Parser.ConvertToFloat(buffer, (byte)Cmd.Communication_Header_Enum.__CMD_HEADER_ENTRYS__, Parser.CommandoParsed.DataLength, " , ");
                    }break;

                /*  String
                 */
                case (byte)Cmd.Data_Type_Enum.DATA_TYP_STRING:
                    {
                        DecStrParas = Parser.ConvertToString(buffer, (byte)Cmd.Communication_Header_Enum.__CMD_HEADER_ENTRYS__, Parser.CommandoParsed.DataLength);
                    }break;
            }


            ListViewItem cmdItems = new ListViewItem(Parser.CommandoParsed.MessageID.ToString());
            cmdItems.SubItems.Add(Parser.CommandoParsed.DataLength.ToString());
            cmdItems.SubItems.Add(Parser.CommandoParsed.Exitcode.ToString());
            cmdItems.SubItems.Add(Parser.CommandoParsed.SlaveCRC.ToString());
            cmdItems.SubItems.Add(Parser.CommandoParsed.DataType.ToString());

            if (Parser.CommandoParsed.DataLength > 0)
            {
                cmdItems.SubItems.Add(DecStrParas);
            }
            else
            {
                cmdItems.SubItems.Add("-");
            }

            try
            {
                richtxtbx_receive_decodet.Invoke(new Action(() =>
                {
                    richtxtbx_receive_decodet.AppendText( Parser.ConvertByte(buffer , 0 , (uint)Cmd.Communication_Header_Enum.__CMD_HEADER_ENTRYS__ , " , ") + "   -   " + Parser.ConvertByte(buffer, (uint)Cmd.Communication_Header_Enum.__CMD_HEADER_ENTRYS__, Parser.CommandoParsed.DataLength , " , ") + "\r\n");
                }
                ));

                listView1.Invoke( new Action(() =>
                {
                    if (listView1.Items.Count > 1500) listView1.Items.Clear();
                    listView1.Items.Add(cmdItems);
                    listView1.Items[listView1.Items.Count - 1].EnsureVisible();
                }   
                ));

                lbl_crc_statistik.Invoke(new Action(() =>
                {
                    lbl_crc_statistik.Text = "Erfolgreich: " + Parser.CrcOkCnt.ToString() + "\r\n" + "Fehlgeschlagen: " + Parser.CrcErrorCnt.ToString();
                }
                ));

                if (messageBoxAnzeigenToolStripMenuItem.CheckState == CheckState.Checked) this.Show();

                tabControl1.Invoke(new Action(() =>
                {
                    if (tabControl1.SelectedIndex != 1 && tabControl1.SelectedIndex != 2)
                    {
                        this.Text = "Kommando Interpreter" + "          " + ">>[" + NewCommandoCnt.ToString() + "]<<" + " " + "Neue(s) Kommando(s) empfangen!";
                    }
                }));
             
            }
            catch
            {
                return;
            }
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
          
            richtxtbx_data_was_send.AppendText( "Header: " + BitConverter.ToString(send, 0 , 8) + "   -   ");

            if ( messageBytes > 0 )
            {
                richtxtbx_data_was_send.AppendText("Nutzdaten: " + BitConverter.ToString(send, 8, CommandoToSend.DataLength) + "\r\n");
            }
            else
            {
                richtxtbx_data_was_send.AppendText("Nutzdaten: -\r\n");
            }
        }


        
        private void btn_data_send_Click(object sender, EventArgs e)
        {
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
            }
            else
            {
                SendCycleTimer.Enabled = false;
            }
        }

        private void SendCycleTimer_Tick(object sender, EventArgs e)
        {
            SendCommando(sender, e);
        }

        private void label9_Click(object sender, EventArgs e)
        {
            string Message = null;

            for ( int x = 0; x < (byte)Cmd.Communication_Header_Enum.__CMD_HEADER_ENTRYS__; x++ )
            {
                Message += "[" + x.ToString() + "]: " + (Cmd.Communication_Header_Enum.CMD_HEADER_START_BYTE1 + x).ToString() + "\r\n";
            }

            MessageBox.Show
            (
                Message , "Kommunikations Header" , MessageBoxButtons.OK , MessageBoxIcon.Information
            );
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

        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            this.Text = "Kommando Interpreter";
            NewCommandoCnt = 0;
        }

        private void Form1_Click(object sender, EventArgs e)
        {

        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex == 1 || tabControl1.SelectedIndex == 2)
            {
                Form1_MouseClick(this, null);
            }
            
        }

        private void richtxtbx_receive_decodet_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                richtxtbx_receive_decodet.Clear();
            }
        }

        private void richtxtbx_data_was_send_KeyDown_1(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                richtxtbx_data_was_send.Clear();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + "\\decoded_received_log.txt"))
            {
                File.Delete(AppDomain.CurrentDomain.BaseDirectory + "\\decoded_received_log.txt");
            }

            using (System.IO.StreamWriter file = new System.IO.StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "\\decoded_received_log.txt", true))
            {
                for ( uint x = 0; x < richtxtbx_receive_decodet.Lines.Length; x++)
                {
                    file.Write("[" + x.ToString() + "]:" + " " + richtxtbx_receive_decodet.Lines[x] + "\r\n");
                }
            }

            MessageBox.Show("Logdatei erfolgreich gespeichert!");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DialogResult res = MessageBox.Show("Daten wirklich löschen?", "Achtung!", MessageBoxButtons.YesNo);
            if ( res == DialogResult.Yes )
            {
                richtxtbx_receive_decodet.Clear();
            }
            else
            {
                return;
            }
        }

        private void richtxtbx_data_was_send_TextChanged_1(object sender, EventArgs e)
        {
            if (richtxtbx_data_was_send.Lines.Length >= 1500) richtxtbx_data_was_send.Clear();
            lbl_was_send.Text = "Gesendet: " + richtxtbx_data_was_send.Lines.Length.ToString();
            progressBar1.Maximum = 1500;
            progressBar1.Value = richtxtbx_data_was_send.Lines.Length;
        }

        private void richtxtbx_receive_decodet_TextChanged(object sender, EventArgs e)
        {
            if (richtxtbx_receive_decodet.Lines.Length >= 1500) richtxtbx_receive_decodet.Clear();
            lbl_receive_cnt.Text = "Empfangen: " + (richtxtbx_receive_decodet.Lines.Length-1).ToString();
            progressBar2.Maximum = 1500;
            progressBar2.Value = richtxtbx_receive_decodet.Lines.Length;
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
    }
}
