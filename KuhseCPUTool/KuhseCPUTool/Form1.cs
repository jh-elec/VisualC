using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Runtime.InteropServices;


namespace KuhseCPUTool
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        [DllImport("User32.dll", SetLastError = true)]
        public static extern int SetForegroundWindow(IntPtr hwnd);
        int ID = 0;

        private void ProzessErstellen(string Programmname)
        {
            Process P = new Process();
            P.StartInfo.FileName = "C:\\Users\\Hm\\Desktop\\test.txt";
            P.Start();
            ID = P.Id;
        }
        

        private void button1_Click(object sender, EventArgs e)
        {
            ProzessErstellen(null);

            System.IntPtr MainHandle = Process.GetProcessById(ID).MainWindowHandle;
            System.Threading.Thread.Sleep(1000);
            SendKeys.Send(Keys.Alt.ToString() + Keys.F4.ToString());

        }

        private void button2_Click(object sender, EventArgs e)
        {
        }
    }
}
