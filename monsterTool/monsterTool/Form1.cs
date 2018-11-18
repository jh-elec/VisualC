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
        Serial  Port    = new Serial();
        Cmd     Parser  = new Cmd();

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

            tabControl1.SelectTab(1);

            if (listView1.Items.Count > 500)
            {
                listView1.Items.Clear();
            }

            lbl_crc_statistik.Text = "Erfolgreich: " + Cmd.CrcOkCnt.ToString() + "\r\n" + "Fehlgeschlagen: " + Cmd.CrcErrorCnt.ToString();

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
                cmdItems.SubItems.Add(BitConverter.ToString(Cmd.CommandoParsed.data, 0 , Cmd.CommandoParsed.dataLen));
            }

            listView1.Items.Add(cmdItems);
            listView1.Items[listView1.Items.Count - 1].EnsureVisible();
        }

        uint Sended = 0;
        private void btn_data_send_Click(object sender, EventArgs e)
        {
            int messageLength = 0;
            string[] message = new string[500];

            if ( richtxtbx_message.TextLength != 0 )
            {
                message = richtxtbx_message.Text.Split(new char[] { ',' });
                messageLength = richtxtbx_message.Text.Split(new char[] { ',' }).Length;
            }

            int CalculatetArraySize = 0;
            switch( cmbbx_data_typ.SelectedIndex )
            {
                case (byte)Cmd.Data_Typ_Enum.DATA_TYP_UINT8:
                {
                        CalculatetArraySize = messageLength * sizeof(byte);
                }break;

                case (byte)Cmd.Data_Typ_Enum.DATA_TYP_UINT16:
                {
                        CalculatetArraySize = messageLength * sizeof(UInt16);
                }break;
                    
                case (byte)Cmd.Data_Typ_Enum.DATA_TYP_UINT32:
                {
                        CalculatetArraySize = messageLength * sizeof(UInt32);
                }break;

                case (byte)Cmd.Data_Typ_Enum.DATA_TYP_STRING:
                {
                        CalculatetArraySize = messageLength * sizeof(char);
                }break;
            }

            Cmd.Commando_Struct Commando = new Cmd.Commando_Struct(CalculatetArraySize);

            Commando.id      = (byte)numeric_msg_id.Value;
            Commando.dataTyp = (byte)cmbbx_data_typ.SelectedIndex;

            int IndexPos = 0;
            for ( int x = 0; x < messageLength ; x++)
            {
                switch ( cmbbx_data_typ.SelectedIndex )
                {
                    case (byte)Cmd.Data_Typ_Enum.DATA_TYP_UINT8:
                    {
                        try
                        {
                            Commando.data[IndexPos++] = byte.Parse(message[x]);
                        }catch
                        {
                            richtxtbx_data_was_send.AppendText("Ungültiger Parameter \r\n");
                            return;
                        }
                        
                    }break;

                    case (byte)Cmd.Data_Typ_Enum.DATA_TYP_UINT16:
                    {
                        try
                        {
                            UInt16 tmp = UInt16.Parse(message[x]);
                            Commando.data[IndexPos++]   = (byte)(tmp & 0x00FF);
                            Commando.data[IndexPos++]  = (byte)((tmp & 0xFF00)>>8);
                        }catch
                        {
                            richtxtbx_data_was_send.AppendText("Ungültiger Parameter \r\n");
                            return;
                        }

                    }break;

                    case (byte)Cmd.Data_Typ_Enum.DATA_TYP_UINT32:
                    {
                        try
                        {
                            UInt32 tmp = UInt32.Parse(message[x]);

                            Commando.data[IndexPos++] = (byte)(tmp & 0x000000FF);
                            Commando.data[IndexPos++] = (byte)((tmp & 0x0000FF00) >> 8);
                            Commando.data[IndexPos++] = (byte)((tmp & 0x00FF0000) >> 16);
                            Commando.data[IndexPos++] = (byte)((tmp & 0xFF000000) >> 24);
                        }catch
                        {
                            richtxtbx_data_was_send.AppendText("Ungültiger Parameter \r\n");
                            return;
                        }

                    }break;

                    case (byte)Cmd.Data_Typ_Enum.DATA_TYP_STRING:
                    {
                        try
                        {
                            Commando.data = Encoding.ASCII.GetBytes(message[x]);
                        }catch
                        {
                            richtxtbx_data_was_send.AppendText("Ungültiger Parameter \r\n");
                            return;
                        }
                            
                    }break;

                    case (byte)Cmd.Data_Typ_Enum.DATA_TYP_FLOAT:
                    {
                        try
                        {
                            float tmp = Convert.ToSingle(message[x]);
                            Commando.data = BitConverter.GetBytes(tmp);
                        }catch
                        {
                            richtxtbx_data_was_send.AppendText("Ungültiger Parameter \r\n");
                            return;
                        }
                    }break;

                    default:
                    {
                            IndexPos = 0;
                    }break;
                }
            }

            Commando.dataLen = (byte)Commando.data.Length;

            byte[] SendCommando = Parser.BuildCommandoHeader(Commando);

            richtxtbx_data_was_send.AppendText(BitConverter.ToString(SendCommando, 0) + "\r\n");

            Port.WriteCommando(SendCommando);

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
    }
}
