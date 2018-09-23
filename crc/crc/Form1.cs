using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Crc;

namespace crc
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Text = "CRC by J.H - Elec.";
            listBox1.SelectedIndex = 0;
            listBox2.SelectedIndex = 0; 
        }

        private void button1_Click(object sender, EventArgs e)
        {
            uint isSelected = 0;
            for( uint x = 0; x<listBox1.Items.Count;x++)
            {
                if( listBox1.GetSelected((int)x) )
                {
                    isSelected = 1;
                    break;
                }
            }
            
            if( isSelected == (uint)0 )
            {
                MessageBox.Show("Es wurde kein \"CRC-Mode\" gewählt!" , "CRC" , MessageBoxButtons.OK , MessageBoxIcon.Error);
                return;
            }

            if( textBox1.Text.Length == 0)
            {
                MessageBox.Show("Nichts zum berechnen!", "CRC", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            isSelected = 0;
            for (uint x = 0; x < listBox2.Items.Count; x++)
            {
                if (listBox2.GetSelected((int)x))
                {
                    isSelected = 1;
                    break;
                }
            }

            if( isSelected == (uint)0)
            {
                MessageBox.Show("Es wurde kein \"CRC-Polynom\" gewählt!", "CRC", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }



            if ( listBox1.GetSelected(0) )
            {
                Crc8_CCITT crcInst = new Crc8_CCITT();

                char[] trimm = new char[] { '0' , 'x'};
                string polynomHexString = listBox2.Text.TrimStart(trimm);
                byte polynom = byte.Parse( polynomHexString, System.Globalization.NumberStyles.HexNumber);

                byte crcResult = crcInst.cmdCrc8StrCCITT(textBox1.Text , polynom , (byte)numericUpDown1.Value);
                MessageBox.Show(listBox1.SelectedItem.ToString() + " -> " + crcResult.ToString() + "\r\n\n\n" + "Ergebniss befindet sich im Zwischenspeicher!"  , "CRC" , MessageBoxButtons.OK , MessageBoxIcon.Information);
                Clipboard.SetText(crcResult.ToString());
            }
        }

        private void textBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            textBox1.ResetText();
        }

        private void textBox1_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyValue == (int)Keys.Enter )
            {
                button1_Click( this , null);
            }
        }
    }
}
