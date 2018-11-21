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
using Commando;

namespace Interpreter
{
    public partial class Form1 : Form
    {
        static Cmd Parser = new Cmd();
        Serial Port = new Serial(Parser);

        public uint NewCommandoCnt = 0;

        public Form1()
        {
            InitializeComponent();
        }

        private async void StartPing()
        {
            Cmd.Commando_Struct ping = new Cmd.Commando_Struct();

            ping.id = 0;
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
                int InitState = Port.Init(cmbbx_port.Text, Convert.ToInt32(cmbbx_baudrate.Text));

                if (InitState == 0)
                {
                    Port.Open();
                    btn_port_open.Text = "Schließen";
                    cmbbx_port.Enabled = false;
                    cmbbx_baudrate.Enabled = false;
                    StartPing();
                }
            }
        }

        private void WriteDecodedBuffer( byte[] buffer , uint length )
        {
            try
            {
                richtxtbx_receive_decodet.Invoke(new Action(() =>
                {
                    richtxtbx_receive_decodet.AppendText(BitConverter.ToString(buffer,0,(int)length) + "\r\n");
                }
                )); 
            }catch
            {
                return;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Cmd.NewCommandoPackageEvent += new Cmd.EventDelegate(WriteToGui);
            Serial.DataReceivedEvent += new Serial.DataReceived(WriteDecodedBuffer);

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
            }

            cmbbx_baudrate.Text = "";
            uint[] baudrates = Port.GetBaudrates();
            for (int x = 0; x < baudrates.Length; x++)
            {
                cmbbx_baudrate.Items.Add(baudrates[x].ToString());
            }

            cmbbx_baudrate.SelectedItem = "115200";

            for( int x = 0; x < (int)Cmd.Data_Typ_Enum.__DATA_TYP_MAX_INDEX__; x++ )
            {
                cmbbx_data_typ.Items.Add((Cmd.Data_Typ_Enum.DATA_TYP_UINT8 + x).ToString());
            }
            cmbbx_data_typ.SelectedItem = "DATA_TYP_UINT8";
        }


        private void WriteToGui()
        {
            if (InvokeRequired)
            {
                try
                {
                    Invoke(new MethodInvoker(WriteToGui));
                }
                catch {}

                return;
            }

            if (listView1.Items.Count > 1500) listView1.Items.Clear();

            lbl_crc_statistik.Text = "Erfolgreich: " + Cmd.CrcOkCnt.ToString() + "\r\n" + "Fehlgeschlagen: " + Cmd.CrcErrorCnt.ToString();

            ListViewItem cmdItems = new ListViewItem(Cmd.CommandoParsed.id.ToString());
            cmdItems.SubItems.Add(Cmd.CommandoParsed.dataLen.ToString());
            cmdItems.SubItems.Add(Cmd.CommandoParsed.exitcode.ToString());
            cmdItems.SubItems.Add(Cmd.CommandoParsed.crc.ToString());
            cmdItems.SubItems.Add(Cmd.CommandoParsed.dataTyp.ToString());

            if (Cmd.CommandoParsed.dataTyp == (byte)Cmd.Data_Typ_Enum.DATA_TYP_STRING )
            {
                cmdItems.SubItems.Add(System.Text.Encoding.UTF8.GetString(Cmd.CommandoParsed.data, 0, Cmd.CommandoParsed.dataLen));
            }
            else if ( Cmd.CommandoParsed.dataLen > 0 )
            {
                cmdItems.SubItems.Add(BitConverter.ToString(Cmd.CommandoParsed.data, 0 , Cmd.CommandoParsed.dataLen) + " Hex");
            }
            else
            {
                cmdItems.SubItems.Add(BitConverter.ToString(Cmd.CommandoParsed.data, 0, Cmd.CommandoParsed.dataLen) + "-");
            }

            listView1.Items.Add(cmdItems);
            listView1.Items[listView1.Items.Count - 1].EnsureVisible();

            if (messageBoxAnzeigenToolStripMenuItem.CheckState == CheckState.Checked) this.Show();

            NewCommandoCnt++;

            if ( tabControl1.SelectedIndex != 1 && tabControl1.SelectedIndex != 2 )
            {
                this.Text = "Kommando Interpreter" + "          " + ">>[" + NewCommandoCnt.ToString() + "]<<" + " " + "Neue(s) Kommando(s) empfangen!";   
            }
        }

        private void SendCommando( object sender , EventArgs e )
        {
            string[] message     = richtxtbx_message.Text.Split(new char[] { ',' });
            int messageLength    = richtxtbx_message.Text.Split(new char[] { ',' }).Length;
            int messageBytes     = 0;

            switch ( cmbbx_data_typ.SelectedIndex )
            {
                case (byte)Cmd.Data_Typ_Enum.DATA_TYP_UINT8:    { messageBytes = messageLength * sizeof(byte);     }   break;
                case (byte)Cmd.Data_Typ_Enum.DATA_TYP_UINT16:   { messageBytes = messageLength * sizeof(UInt16);   }   break;
                case (byte)Cmd.Data_Typ_Enum.DATA_TYP_UINT32:   { messageBytes = messageLength * sizeof(UInt32);   }   break;
                case (byte)Cmd.Data_Typ_Enum.DATA_TYP_FLOAT:    { messageBytes = messageLength * sizeof(float);    }   break;
                case (byte)Cmd.Data_Typ_Enum.DATA_TYP_STRING:   { messageBytes = messageLength * sizeof(char);     }   break;
            }

            if (richtxtbx_message.TextLength == 0) messageBytes = 0;

            Cmd.Commando_Struct CommandoToSend = new Cmd.Commando_Struct(messageBytes);

            uint index = 0;
            for ( uint x = 0; x < messageLength && richtxtbx_message.TextLength > 0; x++ )
            {
                switch( cmbbx_data_typ.SelectedIndex )
                {
                    case (byte)Cmd.Data_Typ_Enum.DATA_TYP_UINT8:
                        {
                            try
                            {
                                CommandoToSend.data[index++] = Convert.ToByte(message[x]);
                            }catch
                            {
                                MessageBox.Show("Falsches Format");
                                return;
                            }
                            
                        } break;

                    case (byte)Cmd.Data_Typ_Enum.DATA_TYP_UINT16:
                        {
                            try
                            {
                                UInt16 tmp = Convert.ToUInt16(message[x]);
                                CommandoToSend.data[index++] = (byte)(tmp & 0x00FF);
                                CommandoToSend.data[index++] = (byte)((tmp & 0xFF00) >> 8);
                            }catch
                            {
                                MessageBox.Show("Falsches Format");
                                return;
                            }

                        } break;

                    case (byte)Cmd.Data_Typ_Enum.DATA_TYP_UINT32:
                        {
                            try
                            {
                                UInt32 tmp = Convert.ToUInt32(message[x]);
                                CommandoToSend.data[index++] = (byte)(tmp & 0x000000FF);
                                CommandoToSend.data[index++] = (byte)((tmp & 0x0000FF00) >> 8);
                                CommandoToSend.data[index++] = (byte)((tmp & 0x00FF0000) >> 16);
                                CommandoToSend.data[index++] = (byte)((tmp & 0xFF000000) >> 26);
                            }catch
                            {
                                MessageBox.Show("Falsches Format");
                                return;
                            }

                        } break;

                    case (byte)Cmd.Data_Typ_Enum.DATA_TYP_FLOAT:
                        {
                            try
                            {
                                float tmp = Convert.ToSingle(message[x]);
                                CommandoToSend.data = BitConverter.GetBytes(tmp);
                            }catch
                            {
                                MessageBox.Show("Falsches Format");
                                return;
                            }

                        } break;

                    case (byte)Cmd.Data_Typ_Enum.DATA_TYP_STRING:
                        {
                            try
                            {
                                CommandoToSend.data = Encoding.ASCII.GetBytes(message[x]);
                            }catch
                            {
                                MessageBox.Show("Falsches Format");
                                return;
                            }

                        } break;
                }
            }

            CommandoToSend.id       = (byte)numeric_msg_id.Value;           // Nachrichten Type
            CommandoToSend.dataTyp  = (byte)cmbbx_data_typ.SelectedIndex;   // Datentyp der Bytes
            CommandoToSend.dataLen  = (byte)CommandoToSend.data.Length;     // Länge der gesamten Nachricht

            byte[] send = Parser.BuildHeader(CommandoToSend);

            Port.WriteCommando(send);
          
            richtxtbx_data_was_send.AppendText( "Header: " + BitConverter.ToString(send, 0 , 8) + "   -   ");

            if ( messageBytes > 0 )
            {
                richtxtbx_data_was_send.AppendText("Nutzdaten: " + BitConverter.ToString(send, 8, CommandoToSend.dataLen) + "\r\n");
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
            lbl_receive_cnt.Text = "Empfangen: " + richtxtbx_receive_decodet.Lines.Length.ToString();
            progressBar2.Maximum = 1500;
            progressBar2.Value = richtxtbx_receive_decodet.Lines.Length;
        }

    }
}
