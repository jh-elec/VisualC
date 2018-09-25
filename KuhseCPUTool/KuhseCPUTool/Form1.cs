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
        public enum StarprogShortcuts : uint
        {
            BLANK_CHECK_PROGRAMM_VERIFY,
            ERASE,
            VERIFY,
            RUN,
            SECURE,
        }

        public string[] starprog_shortcuts =
        {
            "^(a)", // Blank Check ,Programm , Verify
            "^(e)", // Erase
            "^(v)", // Verify
            "^(u)", // Run

        };

        public Form1()
        {
            InitializeComponent();
        }

        string starprogPath = null;
        private void Form1_Load(object sender, EventArgs e)
        {
            starprogPath = System.IO.File.Exists(Application.StartupPath + "\\StarProg.exe").ToString();

            button1.BackColor = Color.Red;
            if (starprogPath.Contains("True"))
            {
                button1.BackColor = Color.LimeGreen;
                starprogPath = Application.StartupPath;
            }
        }

        [DllImport("User32.dll", SetLastError = true)]
        public static extern int SetForegroundWindow(IntPtr hwnd);
        static int ID = 0;
        private void ProzessErstellen( string path )
        {
            Process P = new Process();
            P.StartInfo.FileName = path;
            P.StartInfo.Arguments = s19FilePath;
            P.Start();
            ID = P.Id;
        }

        private void button1_Click(object sender, EventArgs e)
        {
        }

        private void button2_Click(object sender, EventArgs e)
        {            
            ProzessErstellen(starprogPath+"\\StarProg.exe");
            System.IntPtr MainHandle = Process.GetProcessById(ID).MainWindowHandle;
            SetForegroundWindow(MainHandle);
            starprogSend(StarprogShortcuts.BLANK_CHECK_PROGRAMM_VERIFY);

            if (checkBox1.CheckState == CheckState.Checked && checkBox1.Checked)
            {
                MessageBox.Show(starprogPath);
            }
        }

        private void starprogSend( StarprogShortcuts cmd )
        {
            System.Threading.Thread.Sleep(500);
            SendKeys.Send( starprog_shortcuts[(int)cmd]);
        }

        string s19FilePath = null;
        private void button3_Click(object sender, EventArgs e)
        {
            OpenFileDialog path = new OpenFileDialog();
            path.ShowDialog();
            s19FilePath = path.InitialDirectory + path.FileName;
            textBox1.Text = s19FilePath;
        }
    }
}
