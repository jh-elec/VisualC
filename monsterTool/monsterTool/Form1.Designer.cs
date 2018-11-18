namespace monsterTool
{
    partial class Form1
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.cmbbx_port = new System.Windows.Forms.ComboBox();
            this.cmbbx_baudrate = new System.Windows.Forms.ComboBox();
            this.btn_port_open = new System.Windows.Forms.Button();
            this.listView1 = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.label8 = new System.Windows.Forms.Label();
            this.lbl_crc_statistik = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.lbl_info = new System.Windows.Forms.Label();
            this.btn_data_send = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.richtxtbx_message = new System.Windows.Forms.RichTextBox();
            this.cmbbx_data_typ = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.numeric_msg_id = new System.Windows.Forms.NumericUpDown();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.label9 = new System.Windows.Forms.Label();
            this.lbl_was_send = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.richtxtbx_data_was_send = new System.Windows.Forms.RichTextBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.checkbx_show_messages = new System.Windows.Forms.CheckBox();
            this.tabControl1.SuspendLayout();
            this.tabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numeric_msg_id)).BeginInit();
            this.tabPage4.SuspendLayout();
            this.SuspendLayout();
            // 
            // cmbbx_port
            // 
            this.cmbbx_port.FormattingEnabled = true;
            this.cmbbx_port.Location = new System.Drawing.Point(82, 12);
            this.cmbbx_port.Name = "cmbbx_port";
            this.cmbbx_port.Size = new System.Drawing.Size(121, 21);
            this.cmbbx_port.TabIndex = 0;
            // 
            // cmbbx_baudrate
            // 
            this.cmbbx_baudrate.FormattingEnabled = true;
            this.cmbbx_baudrate.Location = new System.Drawing.Point(82, 39);
            this.cmbbx_baudrate.Name = "cmbbx_baudrate";
            this.cmbbx_baudrate.Size = new System.Drawing.Size(121, 21);
            this.cmbbx_baudrate.TabIndex = 1;
            // 
            // btn_port_open
            // 
            this.btn_port_open.Location = new System.Drawing.Point(209, 12);
            this.btn_port_open.Name = "btn_port_open";
            this.btn_port_open.Size = new System.Drawing.Size(188, 48);
            this.btn_port_open.TabIndex = 2;
            this.btn_port_open.Text = "Öffnen";
            this.btn_port_open.UseVisualStyleBackColor = true;
            this.btn_port_open.Click += new System.EventHandler(this.btn_port_open_Click);
            // 
            // listView1
            // 
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4,
            this.columnHeader5});
            this.listView1.FullRowSelect = true;
            this.listView1.GridLines = true;
            this.listView1.HideSelection = false;
            this.listView1.Location = new System.Drawing.Point(6, 6);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(405, 171);
            this.listView1.TabIndex = 3;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            this.listView1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.listView1_KeyDown);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "ID";
            this.columnHeader1.Width = 38;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Nutz Bytes";
            this.columnHeader2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader2.Width = 68;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Exit Code";
            this.columnHeader3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader3.Width = 57;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "CRC";
            this.columnHeader4.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader4.Width = 35;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "Nutzbytes";
            this.columnHeader5.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader5.Width = 199;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(425, 209);
            this.tabControl1.TabIndex = 6;
            // 
            // tabPage3
            // 
            this.tabPage3.BackColor = System.Drawing.SystemColors.Window;
            this.tabPage3.Controls.Add(this.checkbx_show_messages);
            this.tabPage3.Controls.Add(this.label8);
            this.tabPage3.Controls.Add(this.lbl_crc_statistik);
            this.tabPage3.Controls.Add(this.label7);
            this.tabPage3.Controls.Add(this.label2);
            this.tabPage3.Controls.Add(this.label1);
            this.tabPage3.Controls.Add(this.cmbbx_port);
            this.tabPage3.Controls.Add(this.cmbbx_baudrate);
            this.tabPage3.Controls.Add(this.btn_port_open);
            this.tabPage3.Controls.Add(this.pictureBox1);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(417, 183);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Einstellungen";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(266, 85);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(73, 13);
            this.label8.TabIndex = 7;
            this.label8.Text = "Supported by:";
            // 
            // lbl_crc_statistik
            // 
            this.lbl_crc_statistik.AutoSize = true;
            this.lbl_crc_statistik.Location = new System.Drawing.Point(15, 141);
            this.lbl_crc_statistik.Name = "lbl_crc_statistik";
            this.lbl_crc_statistik.Size = new System.Drawing.Size(94, 26);
            this.lbl_crc_statistik.TabIndex = 6;
            this.lbl_crc_statistik.Text = "Erfolgreich: 0\r\nFehlgeschlagen: 0\r\n";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(15, 119);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(69, 13);
            this.label7.TabIndex = 5;
            this.label7.Text = "CRC Statistik";
            this.label7.Click += new System.EventHandler(this.label7_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(29, 42);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Baudate";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Schnittstelle";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::monsterTool.Properties.Resources.SoftwareLogo_100x100;
            this.pictureBox1.Location = new System.Drawing.Point(311, 81);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(103, 102);
            this.pictureBox1.TabIndex = 8;
            this.pictureBox1.TabStop = false;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.listView1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(417, 183);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Empfangen";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.BackColor = System.Drawing.SystemColors.Window;
            this.tabPage2.Controls.Add(this.lbl_info);
            this.tabPage2.Controls.Add(this.btn_data_send);
            this.tabPage2.Controls.Add(this.label6);
            this.tabPage2.Controls.Add(this.label5);
            this.tabPage2.Controls.Add(this.richtxtbx_message);
            this.tabPage2.Controls.Add(this.cmbbx_data_typ);
            this.tabPage2.Controls.Add(this.label4);
            this.tabPage2.Controls.Add(this.label3);
            this.tabPage2.Controls.Add(this.numeric_msg_id);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(417, 183);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Senden";
            // 
            // lbl_info
            // 
            this.lbl_info.AutoSize = true;
            this.lbl_info.Location = new System.Drawing.Point(232, 43);
            this.lbl_info.Name = "lbl_info";
            this.lbl_info.Size = new System.Drawing.Size(29, 13);
            this.lbl_info.TabIndex = 10;
            this.lbl_info.Text = "*Info";
            // 
            // btn_data_send
            // 
            this.btn_data_send.Location = new System.Drawing.Point(79, 143);
            this.btn_data_send.Name = "btn_data_send";
            this.btn_data_send.Size = new System.Drawing.Size(75, 23);
            this.btn_data_send.TabIndex = 8;
            this.btn_data_send.Text = "Senden";
            this.btn_data_send.UseVisualStyleBackColor = true;
            this.btn_data_send.Click += new System.EventHandler(this.btn_data_send_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(76, 114);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(222, 13);
            this.label6.TabIndex = 7;
            this.label6.Text = "Einzelne Bytes werden mit einem \",\" getrennt.";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 70);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 13);
            this.label5.TabIndex = 6;
            this.label5.Text = "Nachricht";
            // 
            // richtxtbx_message
            // 
            this.richtxtbx_message.Location = new System.Drawing.Point(79, 67);
            this.richtxtbx_message.Name = "richtxtbx_message";
            this.richtxtbx_message.Size = new System.Drawing.Size(264, 44);
            this.richtxtbx_message.TabIndex = 5;
            this.richtxtbx_message.Text = "";
            this.richtxtbx_message.KeyDown += new System.Windows.Forms.KeyEventHandler(this.richtxtbx_message_KeyDown);
            // 
            // cmbbx_data_typ
            // 
            this.cmbbx_data_typ.FormattingEnabled = true;
            this.cmbbx_data_typ.Location = new System.Drawing.Point(79, 40);
            this.cmbbx_data_typ.Name = "cmbbx_data_typ";
            this.cmbbx_data_typ.Size = new System.Drawing.Size(147, 21);
            this.cmbbx_data_typ.TabIndex = 4;
            this.cmbbx_data_typ.SelectedIndexChanged += new System.EventHandler(this.cmbbx_data_typ_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 43);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(63, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "Daten Type";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 17);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(67, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "Nachricht ID";
            // 
            // numeric_msg_id
            // 
            this.numeric_msg_id.Location = new System.Drawing.Point(79, 15);
            this.numeric_msg_id.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.numeric_msg_id.Name = "numeric_msg_id";
            this.numeric_msg_id.Size = new System.Drawing.Size(147, 20);
            this.numeric_msg_id.TabIndex = 0;
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.label9);
            this.tabPage4.Controls.Add(this.lbl_was_send);
            this.tabPage4.Controls.Add(this.button1);
            this.tabPage4.Controls.Add(this.richtxtbx_data_was_send);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(417, 183);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "Gesendet";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(6, 12);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(50, 13);
            this.label9.TabIndex = 13;
            this.label9.Text = "Struktur?";
            // 
            // lbl_was_send
            // 
            this.lbl_was_send.AutoSize = true;
            this.lbl_was_send.Location = new System.Drawing.Point(87, 165);
            this.lbl_was_send.Name = "lbl_was_send";
            this.lbl_was_send.Size = new System.Drawing.Size(65, 13);
            this.lbl_was_send.TabIndex = 12;
            this.lbl_was_send.Text = "Gesendet: 0";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(6, 155);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 11;
            this.button1.Text = "Löschen";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // richtxtbx_data_was_send
            // 
            this.richtxtbx_data_was_send.Location = new System.Drawing.Point(6, 28);
            this.richtxtbx_data_was_send.Name = "richtxtbx_data_was_send";
            this.richtxtbx_data_was_send.ReadOnly = true;
            this.richtxtbx_data_was_send.Size = new System.Drawing.Size(405, 121);
            this.richtxtbx_data_was_send.TabIndex = 10;
            this.richtxtbx_data_was_send.Text = "";
            // 
            // checkbx_show_messages
            // 
            this.checkbx_show_messages.AutoSize = true;
            this.checkbx_show_messages.Checked = true;
            this.checkbx_show_messages.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkbx_show_messages.Location = new System.Drawing.Point(135, 150);
            this.checkbx_show_messages.Name = "checkbx_show_messages";
            this.checkbx_show_messages.Size = new System.Drawing.Size(127, 17);
            this.checkbx_show_messages.TabIndex = 9;
            this.checkbx_show_messages.Text = "Benachrichtigungen?";
            this.checkbx_show_messages.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(449, 233);
            this.Controls.Add(this.tabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "Kommando Parser";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numeric_msg_id)).EndInit();
            this.tabPage4.ResumeLayout(false);
            this.tabPage4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox cmbbx_port;
        private System.Windows.Forms.ComboBox cmbbx_baudrate;
        private System.Windows.Forms.Button btn_port_open;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.RichTextBox richtxtbx_message;
        private System.Windows.Forms.ComboBox cmbbx_data_typ;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown numeric_msg_id;
        private System.Windows.Forms.Button btn_data_send;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label lbl_info;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.RichTextBox richtxtbx_data_was_send;
        private System.Windows.Forms.Label lbl_crc_statistik;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label lbl_was_send;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.CheckBox checkbx_show_messages;
    }
}

