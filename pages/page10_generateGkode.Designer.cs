using System.Collections.Generic;
using System.Drawing;

namespace ToolsGenGkode.pages
{
    partial class page10_generateGkode
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
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.labelCountRow = new System.Windows.Forms.Label();
            this.buttonSaveToFile = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.textBoxGkod = new System.Windows.Forms.TextBox();
            this.btGenerateCode = new System.Windows.Forms.Button();
            this.cbProfile = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.buttonAddNewProfile = new System.Windows.Forms.Button();
            this.btEditProfile = new System.Windows.Forms.Button();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.labelCountRow);
            this.groupBox4.Controls.Add(this.buttonSaveToFile);
            this.groupBox4.Controls.Add(this.button1);
            this.groupBox4.Controls.Add(this.textBoxGkod);
            this.groupBox4.Controls.Add(this.btGenerateCode);
            this.groupBox4.Location = new System.Drawing.Point(9, 32);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(633, 203);
            this.groupBox4.TabIndex = 8;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "G-код";
            // 
            // labelCountRow
            // 
            this.labelCountRow.Location = new System.Drawing.Point(491, 161);
            this.labelCountRow.Name = "labelCountRow";
            this.labelCountRow.Size = new System.Drawing.Size(134, 36);
            this.labelCountRow.TabIndex = 8;
            this.labelCountRow.Text = "Размер: 0 байт";
            this.labelCountRow.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // buttonSaveToFile
            // 
            this.buttonSaveToFile.Image = global::ToolsGenGkode.Properties.Resources.diskette;
            this.buttonSaveToFile.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonSaveToFile.Location = new System.Drawing.Point(488, 118);
            this.buttonSaveToFile.Name = "buttonSaveToFile";
            this.buttonSaveToFile.Size = new System.Drawing.Size(138, 41);
            this.buttonSaveToFile.TabIndex = 2;
            this.buttonSaveToFile.Text = "           Сохранить в файл";
            this.buttonSaveToFile.UseVisualStyleBackColor = true;
            this.buttonSaveToFile.Click += new System.EventHandler(this.buttonSaveToFile_Click);
            // 
            // button1
            // 
            this.button1.Image = global::ToolsGenGkode.Properties.Resources.computer_go;
            this.button1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button1.Location = new System.Drawing.Point(488, 67);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(138, 45);
            this.button1.TabIndex = 1;
            this.button1.Text = "        Скопировать в буффер";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // textBoxGkod
            // 
            this.textBoxGkod.Location = new System.Drawing.Point(6, 15);
            this.textBoxGkod.Multiline = true;
            this.textBoxGkod.Name = "textBoxGkod";
            this.textBoxGkod.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxGkod.Size = new System.Drawing.Size(476, 182);
            this.textBoxGkod.TabIndex = 0;
            // 
            // btGenerateCode
            // 
            this.btGenerateCode.Image = global::ToolsGenGkode.Properties.Resources.control_play_blue;
            this.btGenerateCode.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btGenerateCode.Location = new System.Drawing.Point(488, 15);
            this.btGenerateCode.Name = "btGenerateCode";
            this.btGenerateCode.Size = new System.Drawing.Size(138, 46);
            this.btGenerateCode.TabIndex = 7;
            this.btGenerateCode.Text = "            Генерация       G-кода";
            this.btGenerateCode.UseVisualStyleBackColor = true;
            this.btGenerateCode.Click += new System.EventHandler(this.btGenerateCode_Click);
            // 
            // cbProfile
            // 
            this.cbProfile.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbProfile.Location = new System.Drawing.Point(162, 5);
            this.cbProfile.Name = "cbProfile";
            this.cbProfile.Size = new System.Drawing.Size(401, 21);
            this.cbProfile.TabIndex = 17;
            this.cbProfile.DropDown += new System.EventHandler(this.cbProfile_DropDown);
            this.cbProfile.SelectedIndexChanged += new System.EventHandler(this.cbProfile_SelectedIndexChanged);
            this.cbProfile.TextChanged += new System.EventHandler(this.cbProfile_TextChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(6, 8);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(150, 13);
            this.label8.TabIndex = 18;
            this.label8.Text = "Профиль генерации G-кода:";
            // 
            // buttonAddNewProfile
            // 
            this.buttonAddNewProfile.Image = global::ToolsGenGkode.Properties.Resources.book_add;
            this.buttonAddNewProfile.Location = new System.Drawing.Point(569, 4);
            this.buttonAddNewProfile.Name = "buttonAddNewProfile";
            this.buttonAddNewProfile.Size = new System.Drawing.Size(30, 23);
            this.buttonAddNewProfile.TabIndex = 20;
            this.buttonAddNewProfile.UseVisualStyleBackColor = true;
            // 
            // btEditProfile
            // 
            this.btEditProfile.Image = global::ToolsGenGkode.Properties.Resources.book_edit;
            this.btEditProfile.Location = new System.Drawing.Point(605, 4);
            this.btEditProfile.Name = "btEditProfile";
            this.btEditProfile.Size = new System.Drawing.Size(30, 23);
            this.btEditProfile.TabIndex = 19;
            this.btEditProfile.UseVisualStyleBackColor = true;
            this.btEditProfile.Click += new System.EventHandler(this.btOpenProfile_Click);
            // 
            // page10_generateGkode
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.buttonAddNewProfile);
            this.Controls.Add(this.btEditProfile);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.cbProfile);
            this.Controls.Add(this.groupBox4);
            this.Name = "page10_generateGkode";
            this.Size = new System.Drawing.Size(645, 240);
            this.Tag = "_page10_";
            this.Load += new System.EventHandler(this.page09_generateGkode_Load);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btGenerateCode;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.TextBox textBoxGkod;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ComboBox cbProfile;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button buttonSaveToFile;
        private System.Windows.Forms.Button btEditProfile;
        private System.Windows.Forms.Label labelCountRow;
        private System.Windows.Forms.Button buttonAddNewProfile;

        public string PageName { get; set; }
        public int LastPage { get; set; }
        public int CurrPage { get; set; }
        public int NextPage { get; set; }


    }
}
