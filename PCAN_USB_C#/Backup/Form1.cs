using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Text;

namespace PCAN_USB_Demo
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class Form1 : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Button VersionInfoButton;
		private System.Windows.Forms.Button InitButton;
		private System.Windows.Forms.Button CloseButton;
		private System.Windows.Forms.Button WriteButton;
		private System.Windows.Forms.Button ReadButton;
		private System.Windows.Forms.Label InitLabel;
		private System.Windows.Forms.Label CloseLabel;
		private System.Windows.Forms.Label WriteLabel;
		private System.Windows.Forms.Label ReadLabel;
		private System.Windows.Forms.Label VersionLabel;
		private System.Windows.Forms.Button StatusButton;
		private System.Windows.Forms.Label StatusLabel;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public Form1()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.VersionInfoButton = new System.Windows.Forms.Button();
			this.InitButton = new System.Windows.Forms.Button();
			this.InitLabel = new System.Windows.Forms.Label();
			this.CloseButton = new System.Windows.Forms.Button();
			this.CloseLabel = new System.Windows.Forms.Label();
			this.WriteButton = new System.Windows.Forms.Button();
			this.WriteLabel = new System.Windows.Forms.Label();
			this.ReadButton = new System.Windows.Forms.Button();
			this.ReadLabel = new System.Windows.Forms.Label();
			this.VersionLabel = new System.Windows.Forms.Label();
			this.StatusButton = new System.Windows.Forms.Button();
			this.StatusLabel = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// VersionInfoButton
			// 
			this.VersionInfoButton.Location = new System.Drawing.Point(16, 16);
			this.VersionInfoButton.Name = "VersionInfoButton";
			this.VersionInfoButton.Size = new System.Drawing.Size(104, 23);
			this.VersionInfoButton.TabIndex = 0;
			this.VersionInfoButton.Text = "CAN_VersionInfo";
			this.VersionInfoButton.Click += new System.EventHandler(this.VersionInfoButton_Click);
			// 
			// InitButton
			// 
			this.InitButton.Location = new System.Drawing.Point(16, 120);
			this.InitButton.Name = "InitButton";
			this.InitButton.TabIndex = 2;
			this.InitButton.Text = "CAN_Init";
			this.InitButton.Click += new System.EventHandler(this.InitButton_Click);
			// 
			// InitLabel
			// 
			this.InitLabel.Location = new System.Drawing.Point(104, 123);
			this.InitLabel.Name = "InitLabel";
			this.InitLabel.Size = new System.Drawing.Size(144, 19);
			this.InitLabel.TabIndex = 3;
			// 
			// CloseButton
			// 
			this.CloseButton.Enabled = false;
			this.CloseButton.Location = new System.Drawing.Point(16, 248);
			this.CloseButton.Name = "CloseButton";
			this.CloseButton.TabIndex = 10;
			this.CloseButton.Text = "CAN_Close";
			this.CloseButton.Click += new System.EventHandler(this.CloseButton_Click);
			// 
			// CloseLabel
			// 
			this.CloseLabel.Location = new System.Drawing.Point(104, 252);
			this.CloseLabel.Name = "CloseLabel";
			this.CloseLabel.Size = new System.Drawing.Size(144, 16);
			this.CloseLabel.TabIndex = 11;
			// 
			// WriteButton
			// 
			this.WriteButton.Enabled = false;
			this.WriteButton.Location = new System.Drawing.Point(16, 184);
			this.WriteButton.Name = "WriteButton";
			this.WriteButton.TabIndex = 6;
			this.WriteButton.Text = "CAN_Write";
			this.WriteButton.Click += new System.EventHandler(this.WriteButton_Click);
			// 
			// WriteLabel
			// 
			this.WriteLabel.Location = new System.Drawing.Point(104, 188);
			this.WriteLabel.Name = "WriteLabel";
			this.WriteLabel.Size = new System.Drawing.Size(144, 19);
			this.WriteLabel.TabIndex = 7;
			// 
			// ReadButton
			// 
			this.ReadButton.Enabled = false;
			this.ReadButton.Location = new System.Drawing.Point(16, 216);
			this.ReadButton.Name = "ReadButton";
			this.ReadButton.TabIndex = 8;
			this.ReadButton.Text = "CAN_Read";
			this.ReadButton.Click += new System.EventHandler(this.ReadButton_Click);
			// 
			// ReadLabel
			// 
			this.ReadLabel.Location = new System.Drawing.Point(104, 220);
			this.ReadLabel.Name = "ReadLabel";
			this.ReadLabel.Size = new System.Drawing.Size(184, 16);
			this.ReadLabel.TabIndex = 9;
			// 
			// VersionLabel
			// 
			this.VersionLabel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.VersionLabel.Location = new System.Drawing.Point(16, 44);
			this.VersionLabel.Name = "VersionLabel";
			this.VersionLabel.Size = new System.Drawing.Size(272, 64);
			this.VersionLabel.TabIndex = 1;
			// 
			// StatusButton
			// 
			this.StatusButton.Enabled = false;
			this.StatusButton.Location = new System.Drawing.Point(16, 152);
			this.StatusButton.Name = "StatusButton";
			this.StatusButton.TabIndex = 4;
			this.StatusButton.Text = "CAN_Status";
			this.StatusButton.Click += new System.EventHandler(this.StatusButton_Click);
			// 
			// StatusLabel
			// 
			this.StatusLabel.Location = new System.Drawing.Point(104, 156);
			this.StatusLabel.Name = "StatusLabel";
			this.StatusLabel.Size = new System.Drawing.Size(144, 19);
			this.StatusLabel.TabIndex = 5;
			// 
			// Form1
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(304, 285);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.StatusLabel,
																		  this.StatusButton,
																		  this.VersionLabel,
																		  this.ReadLabel,
																		  this.ReadButton,
																		  this.WriteLabel,
																		  this.WriteButton,
																		  this.CloseLabel,
																		  this.CloseButton,
																		  this.InitLabel,
																		  this.InitButton,
																		  this.VersionInfoButton});
			this.Name = "Form1";
			this.Text = "PCANLight C# Demo";
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new Form1());
		}

		private void VersionInfoButton_Click(object sender, System.EventArgs e)
		{
			// Versionsinfo abfragen
			StringBuilder buff = new StringBuilder(256);
			uint res = PCAN_USB.VersionInfo(buff);
			if (res == PCAN_USB.CAN_ERR_OK)
			{
				// Versionsinfo anzeigen
				VersionLabel.Text = buff.ToString();
			}
			else
				VersionLabel.Text = ErrToText(res);	// Fehler anzeigen
		}

		private void InitButton_Click(object sender, System.EventArgs e)
		{
			// USB-Hardware mit 1 MBit/s initialisieren, Extended frames
			uint res = PCAN_USB.Init(PCAN_USB.CAN_BAUD_1M, PCAN_USB.CAN_INIT_TYPE_EX);
			InitLabel.Text = ErrToText(res);	// Ergebnis anzeigen
			if (res == PCAN_USB.CAN_ERR_OK)
			{
				InitButton.Enabled = false;
				StatusButton.Enabled = true;
				WriteButton.Enabled = true;
				ReadButton.Enabled = true;
				CloseButton.Enabled = true;
				CloseLabel.Text = "";
			}
		}

		private void StatusButton_Click(object sender, System.EventArgs e)
		{
			// Status abfragen
			uint res = PCAN_USB.Status();
			StatusLabel.Text = ErrToText(res);
		}

		private void WriteButton_Click(object sender, System.EventArgs e)
		{
			// Eine CAN-Nachricht senden
			PCAN_USB.TCANMsg msg = new PCAN_USB.TCANMsg();
			// Testnachricht zusammenbauen
			msg.ID = 0x100;
			msg.LEN = 8;
			msg.MSGTYPE = 0;
			msg.DATA = 0x8877665544332211;
			uint res = PCAN_USB.Write(ref msg);
			WriteLabel.Text = ErrToText(res);	// Ergebnis anzeigen
		}

		private void ReadButton_Click(object sender, System.EventArgs e)
		{
			// Eine CAN-Nachricht lesen
			PCAN_USB.TCANMsg msg = new PCAN_USB.TCANMsg();
			uint res = PCAN_USB.Read(out msg);
			if (res == PCAN_USB.CAN_ERR_OK)
			{
				// Es wurde eine CAN-Nachricht empfangen
				// Nachricht als String formatieren und ausgeben
				String str = msg.ID.ToString("X3") + "  " + msg.LEN.ToString() + "  ";
				if ((msg.MSGTYPE & PCAN_USB.MSGTYPE_RTR) == 0)
				{
					// Data Frame
					for (int i=0; i < msg.LEN; i++)
						str += ((byte)(msg.DATA >> i*8)).ToString("X2") + " ";
				}
				else
				{
					// Remote Request Frame
					str += "RTR";
				}
				ReadLabel.Text = str;
			}
			else
				ReadLabel.Text = ErrToText(res);	// Fehler anzeigen
		}

		private void CloseButton_Click(object sender, System.EventArgs e) 
		{
			// USB-Hardware schlieﬂen
			uint res = PCAN_USB.Close();
			CloseLabel.Text = ErrToText(res);	// Ergebnis anzeigen
			InitButton.Enabled = true;
			InitLabel.Text = "";
			StatusButton.Enabled = false;
			StatusLabel.Text = "";
			WriteButton.Enabled = false;
			WriteLabel.Text = "";
			ReadButton.Enabled = false;
			ReadLabel.Text = "";
			CloseButton.Enabled = false;
		}

		// CAN-Fehler in Text umformen
		private string ErrToText(uint err)
		{
			if (err == PCAN_USB.CAN_ERR_OK)
				return "OK";
			string str = "";
			if ((err & PCAN_USB.CAN_ERR_XMTFULL) != 0)
				str += "XMTFULL ";
			if ((err & PCAN_USB.CAN_ERR_OVERRUN) != 0)
				str += "OVERRUN ";
			if ((err & PCAN_USB.CAN_ERR_BUSLIGHT) != 0)
				str += "BUSLIGHT ";
			if ((err & PCAN_USB.CAN_ERR_BUSHEAVY) != 0)
				str += "BUSHEAVY ";
			if ((err & PCAN_USB.CAN_ERR_BUSOFF) != 0)
				str += "BUSOFF ";
			if ((err & PCAN_USB.CAN_ERR_QRCVEMPTY) != 0)
				str += "QRCVEMPTY ";
			if ((err & PCAN_USB.CAN_ERR_QOVERRUN) != 0)
				str += "QOVERRUN ";
			if ((err & PCAN_USB.CAN_ERR_QXMTFULL) != 0)
				str += "QXMTFULL ";
			if ((err & PCAN_USB.CAN_ERR_REGTEST) != 0)
				str += "REGTEST ";
			if ((err & PCAN_USB.CAN_ERR_NOVXD) != 0)
				str += "NOVXD ";
			if ((err & PCAN_USB.CAN_ERR_RESOURCE) != 0)
				str += "RESOURCE ";
			return str;
		}
	}
}
