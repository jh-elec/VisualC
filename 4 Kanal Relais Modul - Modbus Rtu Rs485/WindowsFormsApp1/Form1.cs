using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;
using System.Threading;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        SerialPort Client = new SerialPort();

        struct RelaisState_Struct
        {
            public byte[] Num;

            public RelaisState_Struct( byte Size )
            {
                Num = new byte[4];
            }
        }

        RelaisState_Struct RelaisState = new RelaisState_Struct(4);

        public Form1()
        {
            InitializeComponent();
        }


        private void OpenComPort()
        {  
            try
            {
                if (Client.IsOpen)
                {
                    button5.Text = "Öffnen";
                    Client.Close();
                    return;
                }
                else
                {
                    Client.BaudRate = 9600;
                    if(comboBox1.SelectedItem == null)
                    {
                        MessageBox.Show("Keine Schnittstelle ausgewählt!");
                        return;
                    }
                    Client.PortName = comboBox1.SelectedItem.ToString(); ;
                    Client.Open();
                    button5.Text = "Schließen";
                }
            }
            finally { }

        }

        private void LoadComPorts()
        {
            foreach (string Ports in System.IO.Ports.SerialPort.GetPortNames())
            {
                comboBox1.Items.Add(Ports);
            }

            try
            {
                comboBox1.SelectedText = "COM1";
            }
            finally { }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadComPorts();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            OpenComPort();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            byte[] CommandoSetRelais = { 0x01,0x05,0x00,0x00,0x01,0x00,0xCC,0x5A }; // Relais setzen
            byte[] CommandoUnsetRelais = { 0x01, 0x05, 0x00, 0x00, 0x00, 0x00, 0xCD, 0xCA }; // Relais rücksetzen

            if(Client.IsOpen)
            {
                if(RelaisState.Num[0] == 0x00)
                {
                    Client.Write(CommandoSetRelais, 0, CommandoSetRelais.Length);
                    RelaisState.Num[0] = 0x01;
                }
                else
                {
                    Client.Write(CommandoUnsetRelais, 0, CommandoUnsetRelais.Length);
                    RelaisState.Num[0] = 0x00;
                }  
            }     
        }

        private void button2_Click(object sender, EventArgs e)
        {
            byte[] CommandoSetRelais = { 0x01, 0x05, 0x00, 0x01, 0x01, 0x00, 0x9d, 0x9a }; // Relais setzen
            byte[] CommandoUnsetRelais = { 0x01, 0x05, 0x00, 0x01, 0x00, 0x00, 0x9c, 0x0a }; // Relais rücksetzen

            if(Client.IsOpen)
            {
                if (RelaisState.Num[1] == 0x00)
                {
                    Client.Write(CommandoSetRelais, 0, CommandoSetRelais.Length);
                    RelaisState.Num[1] = 0x01;
                }
                else
                {
                    Client.Write(CommandoUnsetRelais, 0, CommandoUnsetRelais.Length);
                    RelaisState.Num[1] = 0x00;
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            byte[] CommandoSetRelais = { 0x01, 0x05, 0x00, 0x03, 0x01, 0x00, 0x3c, 0x5a }; // Relais setzen
            byte[] CommandoUnsetRelais = { 0x01, 0x05, 0x00, 0x03, 0x00, 0x00, 0x3d, 0xca }; // Relais rücksetzen

            if(Client.IsOpen)
            {
                if (RelaisState.Num[3] == 0x00)
                {
                    Client.Write(CommandoSetRelais, 0, CommandoSetRelais.Length);
                    RelaisState.Num[3] = 0x01;
                }
                else
                {
                    Client.Write(CommandoUnsetRelais, 0, CommandoUnsetRelais.Length);
                    RelaisState.Num[3] = 0x00;
                } 
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            byte[] CommandoSetRelais = { 0x01, 0x05, 0x00, 0x02, 0x01, 0x00, 0x6d, 0x9a }; // Relais setzen
            byte[] CommandoUnsetRelais = { 0x01, 0x05, 0x00, 0x02, 0x00, 0x00, 0x6c, 0x0a }; // Relais rücksetzen

            if(Client.IsOpen)
            {
                if (RelaisState.Num[2] == 0x00)
                {
                    Client.Write(CommandoSetRelais, 0, CommandoSetRelais.Length);
                    RelaisState.Num[2] = 0x01;
                }
                else
                {
                    Client.Write(CommandoUnsetRelais, 0, CommandoUnsetRelais.Length);
                    RelaisState.Num[2] = 0x00;
                }
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            byte[] CommandoResetAll = { 0x01, 0x05, 0x00, 0xff, 0xff, 0xff, 0xfc, 0x4a };
            if (Client.IsOpen)
            {
                for (uint i = 0; i < RelaisState.Num.Length; i++)
                {
                    RelaisState.Num[i] = 0x01;
                }
                Client.Write(CommandoResetAll, 0, CommandoResetAll.Length);
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            byte[] CommandoSetAll = { 0x01, 0x05, 0x00, 0xff, 0x00, 0x00, 0xfd, 0xfa };
            if (Client.IsOpen)
            {
                for(uint i = 0; i < RelaisState.Num.Length; i++)
                {
                    RelaisState.Num[i] = 0x00;
                }
                Client.Write(CommandoSetAll, 0, CommandoSetAll.Length);
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Client.Close();
        }
    }
}
