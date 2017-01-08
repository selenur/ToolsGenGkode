namespace ToolsGenGkode
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.MainMenu = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dopFunctionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.testLaserToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.languageCurrent = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel3 = new System.Windows.Forms.ToolStripStatusLabel();
            this.serialPort1 = new System.IO.Ports.SerialPort(this.components);
            this.panel2 = new System.Windows.Forms.Panel();
            this.groupBoxGages = new System.Windows.Forms.GroupBox();
            this.MainPanel = new System.Windows.Forms.Panel();
            this.groupBoxCommand = new System.Windows.Forms.GroupBox();
            this.btBACK = new System.Windows.Forms.Button();
            this.btFORWARD = new System.Windows.Forms.Button();
            this.PageName = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.panelPreview = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.MainMenu.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.groupBoxGages.SuspendLayout();
            this.groupBoxCommand.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.panelPreview.SuspendLayout();
            this.SuspendLayout();
            // 
            // MainMenu
            // 
            this.MainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.dopFunctionsToolStripMenuItem,
            this.languageCurrent,
            this.aboutToolStripMenuItem});
            this.MainMenu.Location = new System.Drawing.Point(0, 0);
            this.MainMenu.Name = "MainMenu";
            this.MainMenu.Size = new System.Drawing.Size(683, 24);
            this.MainMenu.TabIndex = 12;
            this.MainMenu.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
            this.fileToolStripMenuItem.Tag = "_file_";
            this.fileToolStripMenuItem.Text = "Файл";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Image = global::ToolsGenGkode.Properties.Resources.door;
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(108, 22);
            this.exitToolStripMenuItem.Tag = "_exit_";
            this.exitToolStripMenuItem.Text = "Выход";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // dopFunctionsToolStripMenuItem
            // 
            this.dopFunctionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.testLaserToolStripMenuItem});
            this.dopFunctionsToolStripMenuItem.Name = "dopFunctionsToolStripMenuItem";
            this.dopFunctionsToolStripMenuItem.Size = new System.Drawing.Size(107, 20);
            this.dopFunctionsToolStripMenuItem.Text = "Дополнительно";
            // 
            // testLaserToolStripMenuItem
            // 
            this.testLaserToolStripMenuItem.Name = "testLaserToolStripMenuItem";
            this.testLaserToolStripMenuItem.Size = new System.Drawing.Size(336, 22);
            this.testLaserToolStripMenuItem.Text = "Калибровка параметров, для лазер. выжигания";
            this.testLaserToolStripMenuItem.Click += new System.EventHandler(this.testLaserToolStripMenuItem_Click);
            // 
            // languageCurrent
            // 
            this.languageCurrent.Name = "languageCurrent";
            this.languageCurrent.Size = new System.Drawing.Size(119, 20);
            this.languageCurrent.Text = "Язык - НЕВЫБРАН";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Image = global::ToolsGenGkode.Properties.Resources.snowman_head;
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(110, 20);
            this.aboutToolStripMenuItem.Tag = "_about_";
            this.aboutToolStripMenuItem.Text = "О программе";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.toolStripStatusLabel2,
            this.toolStripStatusLabel3});
            this.statusStrip1.Location = new System.Drawing.Point(0, 650);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(683, 22);
            this.statusStrip1.SizingGrip = false;
            this.statusStrip1.TabIndex = 13;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(118, 17);
            this.toolStripStatusLabel1.Tag = "_RamSize_";
            this.toolStripStatusLabel1.Text = "toolStripStatusLabel1";
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(118, 17);
            this.toolStripStatusLabel2.Text = "toolStripStatusLabel2";
            // 
            // toolStripStatusLabel3
            // 
            this.toolStripStatusLabel3.Name = "toolStripStatusLabel3";
            this.toolStripStatusLabel3.Size = new System.Drawing.Size(118, 17);
            this.toolStripStatusLabel3.Tag = "_MbRAM_";
            this.toolStripStatusLabel3.Text = "toolStripStatusLabel3";
            // 
            // serialPort1
            // 
            this.serialPort1.PortName = "COM2";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.groupBoxGages);
            this.panel2.Controls.Add(this.groupBoxCommand);
            this.panel2.Location = new System.Drawing.Point(4, 32);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(675, 335);
            this.panel2.TabIndex = 20;
            // 
            // groupBoxGages
            // 
            this.groupBoxGages.Controls.Add(this.MainPanel);
            this.groupBoxGages.Location = new System.Drawing.Point(9, 61);
            this.groupBoxGages.Name = "groupBoxGages";
            this.groupBoxGages.Size = new System.Drawing.Size(658, 266);
            this.groupBoxGages.TabIndex = 5;
            this.groupBoxGages.TabStop = false;
            // 
            // MainPanel
            // 
            this.MainPanel.Location = new System.Drawing.Point(6, 16);
            this.MainPanel.Name = "MainPanel";
            this.MainPanel.Size = new System.Drawing.Size(645, 243);
            this.MainPanel.TabIndex = 0;
            // 
            // groupBoxCommand
            // 
            this.groupBoxCommand.Controls.Add(this.btBACK);
            this.groupBoxCommand.Controls.Add(this.btFORWARD);
            this.groupBoxCommand.Controls.Add(this.PageName);
            this.groupBoxCommand.Location = new System.Drawing.Point(9, 3);
            this.groupBoxCommand.Name = "groupBoxCommand";
            this.groupBoxCommand.Size = new System.Drawing.Size(658, 56);
            this.groupBoxCommand.TabIndex = 4;
            this.groupBoxCommand.TabStop = false;
            // 
            // btBACK
            // 
            this.btBACK.Enabled = false;
            this.btBACK.Image = global::ToolsGenGkode.Properties.Resources.arrowLeft;
            this.btBACK.Location = new System.Drawing.Point(6, 11);
            this.btBACK.Name = "btBACK";
            this.btBACK.Size = new System.Drawing.Size(70, 40);
            this.btBACK.TabIndex = 1;
            this.btBACK.UseVisualStyleBackColor = true;
            this.btBACK.Click += new System.EventHandler(this.btBACK_Click);
            // 
            // btFORWARD
            // 
            this.btFORWARD.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btFORWARD.Image = global::ToolsGenGkode.Properties.Resources.arrowRight;
            this.btFORWARD.Location = new System.Drawing.Point(582, 11);
            this.btFORWARD.Name = "btFORWARD";
            this.btFORWARD.Size = new System.Drawing.Size(70, 40);
            this.btFORWARD.TabIndex = 3;
            this.btFORWARD.UseVisualStyleBackColor = true;
            this.btFORWARD.Click += new System.EventHandler(this.btFORWARD_Click);
            // 
            // PageName
            // 
            this.PageName.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.PageName.Location = new System.Drawing.Point(82, 11);
            this.PageName.Name = "PageName";
            this.PageName.Size = new System.Drawing.Size(494, 37);
            this.PageName.TabIndex = 2;
            this.PageName.Text = "СТРАНИЦА НЕ УСТАНОВЛЕНА";
            this.PageName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.panelPreview);
            this.groupBox1.Controls.Add(this.radioButton2);
            this.groupBox1.Controls.Add(this.radioButton1);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(13, 370);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(658, 274);
            this.groupBox1.TabIndex = 22;
            this.groupBox1.TabStop = false;
            this.groupBox1.Tag = "_preview_";
            this.groupBox1.Text = "Предварительный просмотр";
            // 
            // panelPreview
            // 
            this.panelPreview.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelPreview.Controls.Add(this.label2);
            this.panelPreview.Location = new System.Drawing.Point(6, 40);
            this.panelPreview.Name = "panelPreview";
            this.panelPreview.Size = new System.Drawing.Size(646, 228);
            this.panelPreview.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.label2.Location = new System.Drawing.Point(117, 76);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(334, 25);
            this.label2.TabIndex = 0;
            this.label2.Tag = "_notFoundData_";
            this.label2.Text = "Нет данных для отображения";
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Enabled = false;
            this.radioButton2.Location = new System.Drawing.Point(201, 17);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(102, 17);
            this.radioButton2.TabIndex = 2;
            this.radioButton2.Tag = "_vectorData_";
            this.radioButton2.Text = "Векторы/точки";
            this.radioButton2.UseVisualStyleBackColor = true;
            this.radioButton2.CheckedChanged += new System.EventHandler(this.radioButton2_CheckedChanged);
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Checked = true;
            this.radioButton1.Enabled = false;
            this.radioButton1.Location = new System.Drawing.Point(100, 17);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(95, 17);
            this.radioButton1.TabIndex = 1;
            this.radioButton1.TabStop = true;
            this.radioButton1.Tag = "_imageData_";
            this.radioButton1.Text = "Изображение";
            this.radioButton1.UseVisualStyleBackColor = true;
            this.radioButton1.CheckedChanged += new System.EventHandler(this.radioButton1_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(73, 13);
            this.label1.TabIndex = 0;
            this.label1.Tag = "_showData_";
            this.label1.Text = "Показывать:";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(683, 672);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.MainMenu);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.MainMenu;
            this.Name = "MainForm";
            this.Tag = "_captionApp_";
            this.Text = "Генератор G-кода";
            this.Load += new System.EventHandler(this.Form_Load);
            this.MainMenu.ResumeLayout(false);
            this.MainMenu.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.groupBoxGages.ResumeLayout(false);
            this.groupBoxCommand.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panelPreview.ResumeLayout(false);
            this.panelPreview.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip MainMenu;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem languageCurrent;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.IO.Ports.SerialPort serialPort1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btFORWARD;
        private System.Windows.Forms.Button btBACK;
        private System.Windows.Forms.Panel MainPanel;
        private System.Windows.Forms.GroupBox groupBoxGages;
        private System.Windows.Forms.GroupBox groupBoxCommand;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.ToolStripMenuItem dopFunctionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem testLaserToolStripMenuItem;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Panel panelPreview;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel3;
        public System.Windows.Forms.Label PageName;
    }
}

