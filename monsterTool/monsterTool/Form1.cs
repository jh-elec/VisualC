using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Commando;

namespace monsterTool
{
    public partial class Form1 : Form
    {
        Cmd     Parser  = new Cmd();
        Serial  Port    = new Serial();


        public Form1()
        {
            InitializeComponent();
        }

        private void btn_port_open_Click(object sender, EventArgs e)
        {
            if (Port.IsOpen())
            {
                btn_port_open.Text = "Öffnen";
                Port.Close();
                cmbbx_port.Enabled = true;
                cmbbx_baudrate.Enabled = true;
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
                }
                else
                {
                    MessageBox.Show("Return Code: " + InitState.ToString(), "WSQ - Portmanager", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Cmd.NewCommandoPackageEvent += new Cmd.EventDelegate(WriteToGui);

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

            cmbbx_baudrate.SelectedItem = "9600";

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

            if (listView1.Items.Count > 500)
            {
                listView1.Items.Clear();
            }

            lbl_crc_statistik.Text = "Erfolgreich: " + Parser.CrcOkCnt.ToString() + "\r\n" + "Fehlgeschlagen: " + Parser.CrcErrorCnt.ToString();

            ListViewItem cmdItems = new ListViewItem(Cmd.CommandoParsed.id.ToString());
            cmdItems.SubItems.Add(Cmd.CommandoParsed.dataLen.ToString());
            cmdItems.SubItems.Add(Cmd.CommandoParsed.exitcode.ToString());
            cmdItems.SubItems.Add(Cmd.CommandoParsed.crc.ToString());

            if ( Cmd.CommandoParsed.dataTyp == (byte)Cmd.Data_Typ_Enum.DATA_TYP_STRING )
            {
                cmdItems.SubItems.Add(System.Text.Encoding.UTF8.GetString(Cmd.CommandoParsed.data, 0, Cmd.CommandoParsed.dataLen));
            }
            else
            {
                cmdItems.SubItems.Add(BitConverter.ToString(Cmd.CommandoParsed.data, 0 , Cmd.CommandoParsed.dataLen) + " Hex");
            }

            listView1.Items.Add(cmdItems);
            listView1.Items[listView1.Items.Count - 1].EnsureVisible();

            if ( checkbx_show_messages.CheckState == CheckState.Checked ) MessageBox.Show("Neue Daten empfangen..");
        }


        uint Sended = 0;
        private void btn_data_send_Click(object sender, EventArgs e)
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

            Sended++;
            lbl_was_send.Text = "Gesendet: " + Sended.ToString();
        }

        private void richtxtbx_data_was_send_DoubleClick(object sender, EventArgs e)
        {
            richtxtbx_data_was_send.Clear();
        }

        private void cmbbx_data_typ_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cmbbx_data_typ.SelectedIndex )
            {
                case (int)Cmd.Data_Typ_Enum.DATA_TYP_FLOAT:
                    {
                        lbl_info.Text = "Kommastellen mit Punkt angeben";
                    }break;

                case (int)Cmd.Data_Typ_Enum.DATA_TYP_UINT8:
                    {
                        lbl_info.Text = "Dezimales Format";
                    }break;

                case (int)Cmd.Data_Typ_Enum.DATA_TYP_UINT16:
                    {
                        lbl_info.Text = "Dezimales Format";
                    }break;
                    
                case (int)Cmd.Data_Typ_Enum.DATA_TYP_UINT32:
                    {
                        lbl_info.Text = "Dezimales Format";
                    }break;
                    
                case (int)Cmd.Data_Typ_Enum.DATA_TYP_STRING:
                    {
                        lbl_info.Text = "ASCII Format";
                    }break;

                default:
                    {
                        lbl_info.Text = "*Info";
                    }break;
            }
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
            Sended = 0;
            lbl_was_send.Text = "Gesendet: 0";
            richtxtbx_data_was_send.Clear();
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
    }
}
