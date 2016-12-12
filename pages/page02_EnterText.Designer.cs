using System.Collections.Generic;
using System.Drawing;

namespace ToolsGenGkode.pages
{
    partial class page02_EnterText
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.buttonSetFontFile = new System.Windows.Forms.Button();
            this.rbFontToImage = new System.Windows.Forms.RadioButton();
            this.rbFontToVector = new System.Windows.Forms.RadioButton();
            this.textString = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textSize = new System.Windows.Forms.NumericUpDown();
            this.comboBoxFont = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.rbUseSystemFont = new System.Windows.Forms.RadioButton();
            this.rbFontFromFile = new System.Windows.Forms.RadioButton();
            this.nameFontFile = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.textSize)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonSetFontFile
            // 
            this.buttonSetFontFile.Location = new System.Drawing.Point(311, 41);
            this.buttonSetFontFile.Name = "buttonSetFontFile";
            this.buttonSetFontFile.Size = new System.Drawing.Size(29, 22);
            this.buttonSetFontFile.TabIndex = 70;
            this.buttonSetFontFile.Text = "...";
            this.buttonSetFontFile.UseVisualStyleBackColor = true;
            this.buttonSetFontFile.Visible = false;
            this.buttonSetFontFile.Click += new System.EventHandler(this.buttonSetFontFile_Click);
            // 
            // rbFontToImage
            // 
            this.rbFontToImage.AutoSize = true;
            this.rbFontToImage.Location = new System.Drawing.Point(277, 19);
            this.rbFontToImage.Name = "rbFontToImage";
            this.rbFontToImage.Size = new System.Drawing.Size(178, 17);
            this.rbFontToImage.TabIndex = 69;
            this.rbFontToImage.Tag = "_textToImage_";
            this.rbFontToImage.Text = "Текст преобразуем в рисунок";
            this.rbFontToImage.UseVisualStyleBackColor = true;
            this.rbFontToImage.CheckedChanged += new System.EventHandler(this.rbFontToImage_CheckedChanged);
            // 
            // rbFontToVector
            // 
            this.rbFontToVector.AutoSize = true;
            this.rbFontToVector.Checked = true;
            this.rbFontToVector.Location = new System.Drawing.Point(29, 19);
            this.rbFontToVector.Name = "rbFontToVector";
            this.rbFontToVector.Size = new System.Drawing.Size(217, 17);
            this.rbFontToVector.TabIndex = 68;
            this.rbFontToVector.TabStop = true;
            this.rbFontToVector.Tag = "_textToVectors_";
            this.rbFontToVector.Text = "Текст преобразуем в набор отрезков";
            this.rbFontToVector.UseVisualStyleBackColor = true;
            this.rbFontToVector.CheckedChanged += new System.EventHandler(this.rbFontToVector_CheckedChanged);
            // 
            // textString
            // 
            this.textString.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textString.Location = new System.Drawing.Point(6, 124);
            this.textString.Multiline = true;
            this.textString.Name = "textString";
            this.textString.Size = new System.Drawing.Size(636, 113);
            this.textString.TabIndex = 61;
            this.textString.Text = "CNC-CLUB.RU";
            this.textString.TextChanged += new System.EventHandler(this.textString_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(359, 45);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(49, 13);
            this.label2.TabIndex = 64;
            this.label2.Tag = "_sizeFont_";
            this.label2.Text = "Размер:";
            // 
            // textSize
            // 
            this.textSize.Location = new System.Drawing.Point(414, 43);
            this.textSize.Maximum = new decimal(new int[] {
            4000,
            0,
            0,
            0});
            this.textSize.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.textSize.Name = "textSize";
            this.textSize.Size = new System.Drawing.Size(69, 20);
            this.textSize.TabIndex = 62;
            this.textSize.Value = new decimal(new int[] {
            40,
            0,
            0,
            0});
            this.textSize.ValueChanged += new System.EventHandler(this.textSize_ValueChanged);
            // 
            // comboBoxFont
            // 
            this.comboBoxFont.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxFont.FormattingEnabled = true;
            this.comboBoxFont.Location = new System.Drawing.Point(52, 42);
            this.comboBoxFont.Name = "comboBoxFont";
            this.comboBoxFont.Size = new System.Drawing.Size(253, 21);
            this.comboBoxFont.TabIndex = 65;
            this.comboBoxFont.SelectedIndexChanged += new System.EventHandler(this.comboBoxFont_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 45);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(44, 13);
            this.label4.TabIndex = 66;
            this.label4.Tag = "_font_";
            this.label4.Text = "Шрифт:";
            // 
            // rbUseSystemFont
            // 
            this.rbUseSystemFont.AutoSize = true;
            this.rbUseSystemFont.Checked = true;
            this.rbUseSystemFont.Location = new System.Drawing.Point(13, 10);
            this.rbUseSystemFont.Name = "rbUseSystemFont";
            this.rbUseSystemFont.Size = new System.Drawing.Size(184, 17);
            this.rbUseSystemFont.TabIndex = 72;
            this.rbUseSystemFont.TabStop = true;
            this.rbUseSystemFont.Tag = "_usedSystemFont_";
            this.rbUseSystemFont.Text = "Используем системный шрифт";
            this.rbUseSystemFont.UseVisualStyleBackColor = true;
            this.rbUseSystemFont.CheckedChanged += new System.EventHandler(this.rbUseSystemFont_CheckedChanged);
            // 
            // rbFontFromFile
            // 
            this.rbFontFromFile.AutoSize = true;
            this.rbFontFromFile.Location = new System.Drawing.Point(219, 10);
            this.rbFontFromFile.Name = "rbFontFromFile";
            this.rbFontFromFile.Size = new System.Drawing.Size(174, 17);
            this.rbFontFromFile.TabIndex = 73;
            this.rbFontFromFile.TabStop = true;
            this.rbFontFromFile.Tag = "_usedExternFont_";
            this.rbFontFromFile.Text = "Используем шрифт из файла";
            this.rbFontFromFile.UseVisualStyleBackColor = true;
            this.rbFontFromFile.CheckedChanged += new System.EventHandler(this.rbFontFromFile_CheckedChanged);
            // 
            // nameFontFile
            // 
            this.nameFontFile.Location = new System.Drawing.Point(53, 42);
            this.nameFontFile.Name = "nameFontFile";
            this.nameFontFile.Size = new System.Drawing.Size(252, 20);
            this.nameFontFile.TabIndex = 74;
            this.nameFontFile.Visible = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rbFontToVector);
            this.groupBox1.Controls.Add(this.rbFontToImage);
            this.groupBox1.Location = new System.Drawing.Point(6, 71);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(636, 47);
            this.groupBox1.TabIndex = 75;
            this.groupBox1.TabStop = false;
            // 
            // page02_EnterText
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.nameFontFile);
            this.Controls.Add(this.rbFontFromFile);
            this.Controls.Add(this.rbUseSystemFont);
            this.Controls.Add(this.buttonSetFontFile);
            this.Controls.Add(this.textString);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textSize);
            this.Controls.Add(this.comboBoxFont);
            this.Controls.Add(this.label4);
            this.Name = "page02_EnterText";
            this.Size = new System.Drawing.Size(645, 240);
            this.Tag = "_page02_";
            this.Load += new System.EventHandler(this.SelectFont_Load);
            ((System.ComponentModel.ISupportInitialize)(this.textSize)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonSetFontFile;
        private System.Windows.Forms.RadioButton rbFontToImage;
        private System.Windows.Forms.RadioButton rbFontToVector;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.RadioButton rbUseSystemFont;
        private System.Windows.Forms.RadioButton rbFontFromFile;
        public System.Windows.Forms.TextBox nameFontFile;
        private System.Windows.Forms.GroupBox groupBox1;
        public System.Windows.Forms.TextBox textString;
        public System.Windows.Forms.NumericUpDown textSize;
        public System.Windows.Forms.ComboBox comboBoxFont;

        public string PageName { get; set; }
        public int LastPage { get; set; }
        public int CurrPage { get; set; }
        public int NextPage { get; set; }



    }
}
