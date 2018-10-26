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
        Serial Port = new Serial();

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

            cmbbx_baudrate.SelectedItem = "115200";
        }


        private void WriteToGui()
        {
            if (InvokeRequired)
            {
                try
                {
                    Invoke(new MethodInvoker(WriteToGui));
                }
                catch { }

                return;
            }

            if (listView1.Items.Count > 500)
            {
                listView1.Items.Clear();
            }

            lbl_last_time.Text = DateTime.Now.ToLongTimeString();

            ListViewItem cmdItems = new ListViewItem(Cmd.CommandoParsed.id.ToString());
            cmdItems.SubItems.Add(Cmd.CommandoParsed.dataLen.ToString());
            cmdItems.SubItems.Add(Cmd.CommandoParsed.exitcode.ToString());
            cmdItems.SubItems.Add(Cmd.CommandoParsed.crc.ToString());

            string raw = null;
            if (Cmd.CommandoParsed.dataLen > 0)
            {
                for (uint x = 0; x < Cmd.CommandoParsed.dataLen; x++)
                {
                    raw += Cmd.CommandoParsed.data[x].ToString() + " ";
                }
            }
            else
            {
                raw = "-";
            }

            cmdItems.SubItems.Add(raw);
            listView1.Items.Add(cmdItems);
            listView1.Items[listView1.Items.Count - 1].EnsureVisible();

        }
    }
}
