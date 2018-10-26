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
            this.lbl_last_commando = new System.Windows.Forms.Label();
            this.lbl_last_time = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // cmbbx_port
            // 
            this.cmbbx_port.FormattingEnabled = true;
            this.cmbbx_port.Location = new System.Drawing.Point(12, 160);
            this.cmbbx_port.Name = "cmbbx_port";
            this.cmbbx_port.Size = new System.Drawing.Size(121, 21);
            this.cmbbx_port.TabIndex = 0;
            // 
            // cmbbx_baudrate
            // 
            this.cmbbx_baudrate.FormattingEnabled = true;
            this.cmbbx_baudrate.Location = new System.Drawing.Point(139, 160);
            this.cmbbx_baudrate.Name = "cmbbx_baudrate";
            this.cmbbx_baudrate.Size = new System.Drawing.Size(121, 21);
            this.cmbbx_baudrate.TabIndex = 1;
            // 
            // btn_port_open
            // 
            this.btn_port_open.Location = new System.Drawing.Point(266, 158);
            this.btn_port_open.Name = "btn_port_open";
            this.btn_port_open.Size = new System.Drawing.Size(75, 23);
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
            this.listView1.Location = new System.Drawing.Point(12, 41);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(547, 111);
            this.listView1.TabIndex = 3;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "ID";
            this.columnHeader1.Width = 37;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Nutz Bytes";
            this.columnHeader2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader2.Width = 63;
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
            this.columnHeader4.Width = 34;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "Nutzbytes";
            this.columnHeader5.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader5.Width = 352;
            // 
            // lbl_last_commando
            // 
            this.lbl_last_commando.AutoSize = true;
            this.lbl_last_commando.Location = new System.Drawing.Point(12, 9);
            this.lbl_last_commando.Name = "lbl_last_commando";
            this.lbl_last_commando.Size = new System.Drawing.Size(100, 13);
            this.lbl_last_commando.TabIndex = 4;
            this.lbl_last_commando.Text = "Letztes Kommando:";
            // 
            // lbl_last_time
            // 
            this.lbl_last_time.AutoSize = true;
            this.lbl_last_time.Location = new System.Drawing.Point(118, 9);
            this.lbl_last_time.Name = "lbl_last_time";
            this.lbl_last_time.Size = new System.Drawing.Size(10, 13);
            this.lbl_last_time.TabIndex = 5;
            this.lbl_last_time.Text = "-";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(571, 193);
            this.Controls.Add(this.lbl_last_time);
            this.Controls.Add(this.lbl_last_commando);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.btn_port_open);
            this.Controls.Add(this.cmbbx_baudrate);
            this.Controls.Add(this.cmbbx_port);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "Kommando Parser";
            this.Load += new System.EventHandler(this.Form1_Load);
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
        private System.Windows.Forms.Label lbl_last_commando;
        private System.Windows.Forms.Label lbl_last_time;
    }
}

