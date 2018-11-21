namespace Interpreter
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
            this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.label8 = new System.Windows.Forms.Label();
            this.lbl_crc_statistik = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.numeric_send_cycle_timer_interval = new System.Windows.Forms.NumericUpDown();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.label10 = new System.Windows.Forms.Label();
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
            this.SendCycleTimer = new System.Windows.Forms.Timer(this.components);
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.dateiToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.meldungenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.messageBoxAnzeigenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.inDenTrayToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.tabControl1.SuspendLayout();
            this.tabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numeric_send_cycle_timer_interval)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numeric_msg_id)).BeginInit();
            this.tabPage4.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // cmbbx_port
            // 
            this.cmbbx_port.FormattingEnabled = true;
            this.cmbbx_port.Location = new System.Drawing.Point(82, 26);
            this.cmbbx_port.Name = "cmbbx_port";
            this.cmbbx_port.Size = new System.Drawing.Size(121, 21);
            this.cmbbx_port.TabIndex = 0;
            // 
            // cmbbx_baudrate
            // 
            this.cmbbx_baudrate.FormattingEnabled = true;
            this.cmbbx_baudrate.Location = new System.Drawing.Point(82, 53);
            this.cmbbx_baudrate.Name = "cmbbx_baudrate";
            this.cmbbx_baudrate.Size = new System.Drawing.Size(121, 21);
            this.cmbbx_baudrate.TabIndex = 1;
            // 
            // btn_port_open
            // 
            this.btn_port_open.Location = new System.Drawing.Point(15, 96);
            this.btn_port_open.Name = "btn_port_open";
            this.btn_port_open.Size = new System.Drawing.Size(188, 48);
            this.btn_port_open.TabIndex = 2;
            this.btn_port_open.Text = "Öffnen";
            this.btn_port_open.UseVisualStyleBackColor = true;
            this.btn_port_open.Click += new System.EventHandler(this.btn_port_open_Click);
            // 
            // listView1
            // 
            this.listView1.BackColor = System.Drawing.SystemColors.Window;
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4,
            this.columnHeader6,
            this.columnHeader5});
            this.listView1.FullRowSelect = true;
            this.listView1.GridLines = true;
            this.listView1.HideSelection = false;
            this.listView1.Location = new System.Drawing.Point(6, 6);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(435, 183);
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
            // columnHeader6
            // 
            this.columnHeader6.Text = "Type";
            this.columnHeader6.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "Nutzbytes";
            this.columnHeader5.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader5.Width = 135;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Location = new System.Drawing.Point(20, 27);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(455, 221);
            this.tabControl1.TabIndex = 6;
            // 
            // tabPage3
            // 
            this.tabPage3.BackColor = System.Drawing.SystemColors.ScrollBar;
            this.tabPage3.Controls.Add(this.groupBox2);
            this.tabPage3.Controls.Add(this.label8);
            this.tabPage3.Controls.Add(this.label2);
            this.tabPage3.Controls.Add(this.label1);
            this.tabPage3.Controls.Add(this.cmbbx_port);
            this.tabPage3.Controls.Add(this.cmbbx_baudrate);
            this.tabPage3.Controls.Add(this.btn_port_open);
            this.tabPage3.Controls.Add(this.pictureBox1);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(447, 195);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Einstellungen";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(296, 94);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(73, 13);
            this.label8.TabIndex = 7;
            this.label8.Text = "Supported by:";
            // 
            // lbl_crc_statistik
            // 
            this.lbl_crc_statistik.AutoSize = true;
            this.lbl_crc_statistik.Location = new System.Drawing.Point(9, 19);
            this.lbl_crc_statistik.Name = "lbl_crc_statistik";
            this.lbl_crc_statistik.Size = new System.Drawing.Size(94, 26);
            this.lbl_crc_statistik.TabIndex = 6;
            this.lbl_crc_statistik.Text = "Erfolgreich: 0\r\nFehlgeschlagen: 0\r\n";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(29, 56);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Baudate";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Schnittstelle";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::Interpreter.Properties.Resources.SoftwareLogo_100x100;
            this.pictureBox1.Location = new System.Drawing.Point(341, 90);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(103, 102);
            this.pictureBox1.TabIndex = 8;
            this.pictureBox1.TabStop = false;
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.SystemColors.ScrollBar;
            this.tabPage1.Controls.Add(this.listView1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(447, 195);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Empfangen";
            // 
            // tabPage2
            // 
            this.tabPage2.BackColor = System.Drawing.SystemColors.ScrollBar;
            this.tabPage2.Controls.Add(this.groupBox1);
            this.tabPage2.Controls.Add(this.label10);
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
            this.tabPage2.Size = new System.Drawing.Size(447, 195);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Senden";
            // 
            // numeric_send_cycle_timer_interval
            // 
            this.numeric_send_cycle_timer_interval.Location = new System.Drawing.Point(62, 21);
            this.numeric_send_cycle_timer_interval.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numeric_send_cycle_timer_interval.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numeric_send_cycle_timer_interval.Name = "numeric_send_cycle_timer_interval";
            this.numeric_send_cycle_timer_interval.Size = new System.Drawing.Size(58, 20);
            this.numeric_send_cycle_timer_interval.TabIndex = 13;
            this.numeric_send_cycle_timer_interval.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.numeric_send_cycle_timer_interval, "Ein neu eingetragener Wert, wird erst nach ändern\r\nder Checkbox \"Zyklisch\" überno" +
        "mmen.\r\n");
            this.numeric_send_cycle_timer_interval.Value = new decimal(new int[] {
            500,
            0,
            0,
            0});
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(6, 22);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(50, 17);
            this.checkBox1.TabIndex = 12;
            this.checkBox1.Text = "Aktiv";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(232, 15);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(34, 13);
            this.label10.TabIndex = 11;
            this.label10.Text = "(Ping)";
            // 
            // lbl_info
            // 
            this.lbl_info.AutoSize = true;
            this.lbl_info.Location = new System.Drawing.Point(232, 40);
            this.lbl_info.Name = "lbl_info";
            this.lbl_info.Size = new System.Drawing.Size(29, 13);
            this.lbl_info.TabIndex = 10;
            this.lbl_info.Text = "*Info";
            // 
            // btn_data_send
            // 
            this.btn_data_send.Location = new System.Drawing.Point(79, 117);
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
            this.label6.Location = new System.Drawing.Point(232, 67);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(133, 26);
            this.label6.TabIndex = 7;
            this.label6.Text = "Einzelne Bytes werden mit \r\neinem \",\" getrennt.";
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
            this.richtxtbx_message.Size = new System.Drawing.Size(147, 44);
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
            this.numeric_msg_id.Size = new System.Drawing.Size(146, 20);
            this.numeric_msg_id.TabIndex = 0;
            this.numeric_msg_id.ValueChanged += new System.EventHandler(this.numeric_msg_id_ValueChanged);
            // 
            // tabPage4
            // 
            this.tabPage4.BackColor = System.Drawing.SystemColors.ScrollBar;
            this.tabPage4.Controls.Add(this.label9);
            this.tabPage4.Controls.Add(this.lbl_was_send);
            this.tabPage4.Controls.Add(this.button1);
            this.tabPage4.Controls.Add(this.richtxtbx_data_was_send);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(447, 195);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "Gesendet";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(6, 12);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(50, 13);
            this.label9.TabIndex = 13;
            this.label9.Text = "Struktur?";
            this.label9.Click += new System.EventHandler(this.label9_Click);
            // 
            // lbl_was_send
            // 
            this.lbl_was_send.AutoSize = true;
            this.lbl_was_send.Location = new System.Drawing.Point(87, 176);
            this.lbl_was_send.Name = "lbl_was_send";
            this.lbl_was_send.Size = new System.Drawing.Size(65, 13);
            this.lbl_was_send.TabIndex = 12;
            this.lbl_was_send.Text = "Gesendet: 0";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(6, 166);
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
            this.richtxtbx_data_was_send.Size = new System.Drawing.Size(435, 132);
            this.richtxtbx_data_was_send.TabIndex = 10;
            this.richtxtbx_data_was_send.Text = "";
            // 
            // SendCycleTimer
            // 
            this.SendCycleTimer.Tick += new System.EventHandler(this.SendCycleTimer_Tick);
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.dateiToolStripMenuItem,
            this.meldungenToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(495, 24);
            this.menuStrip1.TabIndex = 8;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // dateiToolStripMenuItem
            // 
            this.dateiToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.inDenTrayToolStripMenuItem});
            this.dateiToolStripMenuItem.Name = "dateiToolStripMenuItem";
            this.dateiToolStripMenuItem.Size = new System.Drawing.Size(46, 20);
            this.dateiToolStripMenuItem.Text = "Datei";
            // 
            // meldungenToolStripMenuItem
            // 
            this.meldungenToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.messageBoxAnzeigenToolStripMenuItem});
            this.meldungenToolStripMenuItem.Name = "meldungenToolStripMenuItem";
            this.meldungenToolStripMenuItem.Size = new System.Drawing.Size(80, 20);
            this.meldungenToolStripMenuItem.Text = "Meldungen";
            // 
            // messageBoxAnzeigenToolStripMenuItem
            // 
            this.messageBoxAnzeigenToolStripMenuItem.Name = "messageBoxAnzeigenToolStripMenuItem";
            this.messageBoxAnzeigenToolStripMenuItem.Size = new System.Drawing.Size(345, 22);
            this.messageBoxAnzeigenToolStripMenuItem.Text = "Bei neuen Daten, Fenster in den Vordergrund rufen?";
            this.messageBoxAnzeigenToolStripMenuItem.Click += new System.EventHandler(this.messageBoxAnzeigenToolStripMenuItem_Click);
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "Kommando Interpreter";
            this.notifyIcon1.Visible = true;
            this.notifyIcon1.DoubleClick += new System.EventHandler(this.notifyIcon1_DoubleClick);
            // 
            // inDenTrayToolStripMenuItem
            // 
            this.inDenTrayToolStripMenuItem.Name = "inDenTrayToolStripMenuItem";
            this.inDenTrayToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.inDenTrayToolStripMenuItem.Text = "In den Tray";
            this.inDenTrayToolStripMenuItem.Click += new System.EventHandler(this.inDenTrayToolStripMenuItem_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.checkBox1);
            this.groupBox1.Controls.Add(this.numeric_send_cycle_timer_interval);
            this.groupBox1.Location = new System.Drawing.Point(235, 128);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(133, 49);
            this.groupBox1.TabIndex = 15;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Zyklisch schreiben [ms]";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.lbl_crc_statistik);
            this.groupBox2.Location = new System.Drawing.Point(335, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(109, 51);
            this.groupBox2.TabIndex = 9;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "CRC Statistik";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 24);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(14, 244);
            this.panel1.TabIndex = 9;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.panel2.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel2.Location = new System.Drawing.Point(481, 24);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(14, 244);
            this.panel2.TabIndex = 10;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(14, 254);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(467, 14);
            this.panel3.TabIndex = 10;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(495, 268);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form1";
            this.Text = "Kommando Interpreter";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseClick);
            this.tabControl1.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numeric_send_cycle_timer_interval)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numeric_msg_id)).EndInit();
            this.tabPage4.ResumeLayout(false);
            this.tabPage4.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

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
        private System.Windows.Forms.Label lbl_was_send;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Timer SendCycleTimer;
        private System.Windows.Forms.NumericUpDown numeric_send_cycle_timer_interval;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem dateiToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem meldungenToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem messageBoxAnzeigenToolStripMenuItem;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.ToolStripMenuItem inDenTrayToolStripMenuItem;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
    }
}

