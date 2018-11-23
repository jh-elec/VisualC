namespace CodeDescriptor
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
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.dateiToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.checkUpdatesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.plToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.btn_clear = new System.Windows.Forms.Button();
            this.btn_create = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.txtbx_made_by = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lbl_name_of_header = new System.Windows.Forms.Label();
            this.txtbx_name_of_header = new System.Windows.Forms.TextBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.label4 = new System.Windows.Forms.Label();
            this.numeric_major = new System.Windows.Forms.NumericUpDown();
            this.numeric_minor = new System.Windows.Forms.NumericUpDown();
            this.menuStrip1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numeric_major)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numeric_minor)).BeginInit();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 1000;
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.dateiToolStripMenuItem,
            this.plToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(8, 2, 0, 2);
            this.menuStrip1.Size = new System.Drawing.Size(421, 28);
            this.menuStrip1.TabIndex = 23;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // dateiToolStripMenuItem
            // 
            this.dateiToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.checkUpdatesToolStripMenuItem});
            this.dateiToolStripMenuItem.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dateiToolStripMenuItem.Image = global::CodeDescriptor.Properties.Resources.Hopstarter_Sleek_Xp_Basic_Files;
            this.dateiToolStripMenuItem.Name = "dateiToolStripMenuItem";
            this.dateiToolStripMenuItem.Size = new System.Drawing.Size(32, 24);
            // 
            // checkUpdatesToolStripMenuItem
            // 
            this.checkUpdatesToolStripMenuItem.Name = "checkUpdatesToolStripMenuItem";
            this.checkUpdatesToolStripMenuItem.Size = new System.Drawing.Size(281, 36);
            this.checkUpdatesToolStripMenuItem.Text = "Check Updates";
            // 
            // plToolStripMenuItem
            // 
            this.plToolStripMenuItem.Image = global::CodeDescriptor.Properties.Resources.communication_email_blue_icon;
            this.plToolStripMenuItem.Name = "plToolStripMenuItem";
            this.plToolStripMenuItem.Size = new System.Drawing.Size(32, 24);
            this.plToolStripMenuItem.Click += new System.EventHandler(this.plToolStripMenuItem_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Location = new System.Drawing.Point(12, 45);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(397, 176);
            this.tabControl1.TabIndex = 24;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.numeric_minor);
            this.tabPage1.Controls.Add(this.numeric_major);
            this.tabPage1.Controls.Add(this.label4);
            this.tabPage1.Controls.Add(this.btn_clear);
            this.tabPage1.Controls.Add(this.btn_create);
            this.tabPage1.Controls.Add(this.label3);
            this.tabPage1.Controls.Add(this.txtbx_made_by);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.lbl_name_of_header);
            this.tabPage1.Controls.Add(this.txtbx_name_of_header);
            this.tabPage1.Location = new System.Drawing.Point(4, 25);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(389, 147);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Header";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // btn_clear
            // 
            this.btn_clear.Location = new System.Drawing.Point(152, 118);
            this.btn_clear.Name = "btn_clear";
            this.btn_clear.Size = new System.Drawing.Size(75, 23);
            this.btn_clear.TabIndex = 8;
            this.btn_clear.Text = "Löschen";
            this.btn_clear.UseVisualStyleBackColor = true;
            // 
            // btn_create
            // 
            this.btn_create.Location = new System.Drawing.Point(9, 118);
            this.btn_create.Name = "btn_create";
            this.btn_create.Size = new System.Drawing.Size(75, 23);
            this.btn_create.TabIndex = 6;
            this.btn_create.Text = "Erstellen";
            this.btn_create.UseVisualStyleBackColor = true;
            this.btn_create.Click += new System.EventHandler(this.btn_create_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 82);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(82, 17);
            this.label3.TabIndex = 4;
            this.label3.Text = "Erstellt von:";
            // 
            // txtbx_made_by
            // 
            this.txtbx_made_by.Location = new System.Drawing.Point(94, 79);
            this.txtbx_made_by.Name = "txtbx_made_by";
            this.txtbx_made_by.Size = new System.Drawing.Size(133, 22);
            this.txtbx_made_by.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(233, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(20, 17);
            this.label1.TabIndex = 2;
            this.label1.Text = ".h";
            // 
            // lbl_name_of_header
            // 
            this.lbl_name_of_header.AutoSize = true;
            this.lbl_name_of_header.Location = new System.Drawing.Point(6, 28);
            this.lbl_name_of_header.Name = "lbl_name_of_header";
            this.lbl_name_of_header.Size = new System.Drawing.Size(49, 17);
            this.lbl_name_of_header.TabIndex = 1;
            this.lbl_name_of_header.Text = "Name:";
            // 
            // txtbx_name_of_header
            // 
            this.txtbx_name_of_header.Location = new System.Drawing.Point(94, 23);
            this.txtbx_name_of_header.Name = "txtbx_name_of_header";
            this.txtbx_name_of_header.Size = new System.Drawing.Size(133, 22);
            this.txtbx_name_of_header.TabIndex = 0;
            // 
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(4, 25);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(389, 147);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Prototype";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // tabPage3
            // 
            this.tabPage3.Location = new System.Drawing.Point(4, 25);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(389, 147);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Änderungen";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 54);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(60, 17);
            this.label4.TabIndex = 10;
            this.label4.Text = "Version:";
            // 
            // numeric_major
            // 
            this.numeric_major.Location = new System.Drawing.Point(94, 52);
            this.numeric_major.Name = "numeric_major";
            this.numeric_major.Size = new System.Drawing.Size(45, 22);
            this.numeric_major.TabIndex = 11;
            // 
            // numeric_minor
            // 
            this.numeric_minor.Location = new System.Drawing.Point(145, 52);
            this.numeric_minor.Name = "numeric_minor";
            this.numeric_minor.Size = new System.Drawing.Size(45, 22);
            this.numeric_minor.TabIndex = 12;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(421, 229);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Form1";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numeric_major)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numeric_minor)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem dateiToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem checkUpdatesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem plToolStripMenuItem;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Button btn_clear;
        private System.Windows.Forms.Button btn_create;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtbx_made_by;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lbl_name_of_header;
        private System.Windows.Forms.TextBox txtbx_name_of_header;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown numeric_minor;
        private System.Windows.Forms.NumericUpDown numeric_major;
    }
}

