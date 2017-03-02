using System.Collections.Generic;
using System.Drawing;

namespace ToolsGenGkode.pages
{
    partial class page01_start
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radioButtonTypeSourceDXF = new System.Windows.Forms.RadioButton();
            this.radioButtonTypeSourcePicture2 = new System.Windows.Forms.RadioButton();
            this.radioButtonTypeSourceText = new System.Windows.Forms.RadioButton();
            this.radioButtonTypeSourcePicture1 = new System.Windows.Forms.RadioButton();
            this.radioButtonTypeSourcePLT = new System.Windows.Forms.RadioButton();
            this.OrientationVar2 = new System.Windows.Forms.RadioButton();
            this.OrientationVar1 = new System.Windows.Forms.RadioButton();
            this.OrientationVar3 = new System.Windows.Forms.RadioButton();
            this.OrientationVar4 = new System.Windows.Forms.RadioButton();
            this.pictureBoxAxes = new System.Windows.Forms.PictureBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxAxes)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radioButtonTypeSourceDXF);
            this.groupBox1.Controls.Add(this.radioButtonTypeSourcePicture2);
            this.groupBox1.Controls.Add(this.radioButtonTypeSourceText);
            this.groupBox1.Controls.Add(this.radioButtonTypeSourcePicture1);
            this.groupBox1.Controls.Add(this.radioButtonTypeSourcePLT);
            this.groupBox1.Location = new System.Drawing.Point(372, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(270, 230);
            this.groupBox1.TabIndex = 22;
            this.groupBox1.TabStop = false;
            this.groupBox1.Tag = "_SorceData_";
            this.groupBox1.Text = "Источник данных";
            // 
            // radioButtonTypeSourceDXF
            // 
            this.radioButtonTypeSourceDXF.Image = global::ToolsGenGkode.Properties.Resources.blueprint;
            this.radioButtonTypeSourceDXF.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.radioButtonTypeSourceDXF.Location = new System.Drawing.Point(63, 173);
            this.radioButtonTypeSourceDXF.Name = "radioButtonTypeSourceDXF";
            this.radioButtonTypeSourceDXF.Size = new System.Drawing.Size(134, 25);
            this.radioButtonTypeSourceDXF.TabIndex = 20;
            this.radioButtonTypeSourceDXF.TabStop = true;
            this.radioButtonTypeSourceDXF.Tag = "_sourceDXF_";
            this.radioButtonTypeSourceDXF.Text = "       DXF файл";
            this.radioButtonTypeSourceDXF.UseVisualStyleBackColor = true;
            this.radioButtonTypeSourceDXF.CheckedChanged += new System.EventHandler(this.radioButtonTypeSourceDXF_CheckedChanged);
            // 
            // radioButtonTypeSourcePicture2
            // 
            this.radioButtonTypeSourcePicture2.Image = global::ToolsGenGkode.Properties.Resources.file_extension_jpg;
            this.radioButtonTypeSourcePicture2.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.radioButtonTypeSourcePicture2.Location = new System.Drawing.Point(63, 142);
            this.radioButtonTypeSourcePicture2.Name = "radioButtonTypeSourcePicture2";
            this.radioButtonTypeSourcePicture2.Size = new System.Drawing.Size(134, 25);
            this.radioButtonTypeSourcePicture2.TabIndex = 19;
            this.radioButtonTypeSourcePicture2.Tag = "_sourceRastr_";
            this.radioButtonTypeSourcePicture2.Text = "       Рисунок (Растр)";
            this.radioButtonTypeSourcePicture2.UseVisualStyleBackColor = true;
            this.radioButtonTypeSourcePicture2.CheckedChanged += new System.EventHandler(this.radioButtonTypeSourcePicture2_CheckedChanged);
            // 
            // radioButtonTypeSourceText
            // 
            this.radioButtonTypeSourceText.Checked = true;
            this.radioButtonTypeSourceText.Image = global::ToolsGenGkode.Properties.Resources.font;
            this.radioButtonTypeSourceText.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.radioButtonTypeSourceText.Location = new System.Drawing.Point(63, 49);
            this.radioButtonTypeSourceText.Name = "radioButtonTypeSourceText";
            this.radioButtonTypeSourceText.Size = new System.Drawing.Size(87, 25);
            this.radioButtonTypeSourceText.TabIndex = 16;
            this.radioButtonTypeSourceText.TabStop = true;
            this.radioButtonTypeSourceText.Tag = "_sourceText_";
            this.radioButtonTypeSourceText.Text = "       Текст";
            this.radioButtonTypeSourceText.UseVisualStyleBackColor = true;
            this.radioButtonTypeSourceText.CheckedChanged += new System.EventHandler(this.radioButtonTypeSourceText_CheckedChanged);
            // 
            // radioButtonTypeSourcePicture1
            // 
            this.radioButtonTypeSourcePicture1.Image = global::ToolsGenGkode.Properties.Resources.file_extension_jpg;
            this.radioButtonTypeSourcePicture1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.radioButtonTypeSourcePicture1.Location = new System.Drawing.Point(63, 111);
            this.radioButtonTypeSourcePicture1.Name = "radioButtonTypeSourcePicture1";
            this.radioButtonTypeSourcePicture1.Size = new System.Drawing.Size(134, 25);
            this.radioButtonTypeSourcePicture1.TabIndex = 17;
            this.radioButtonTypeSourcePicture1.Tag = "_sourceContur_";
            this.radioButtonTypeSourcePicture1.Text = "       Рисунок (Контур)";
            this.radioButtonTypeSourcePicture1.UseVisualStyleBackColor = true;
            this.radioButtonTypeSourcePicture1.CheckedChanged += new System.EventHandler(this.radioButtonTypeSourcePicture_CheckedChanged);
            // 
            // radioButtonTypeSourcePLT
            // 
            this.radioButtonTypeSourcePLT.Image = global::ToolsGenGkode.Properties.Resources.smartart_change_color_gallery;
            this.radioButtonTypeSourcePLT.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.radioButtonTypeSourcePLT.Location = new System.Drawing.Point(63, 80);
            this.radioButtonTypeSourcePLT.Name = "radioButtonTypeSourcePLT";
            this.radioButtonTypeSourcePLT.Size = new System.Drawing.Size(122, 25);
            this.radioButtonTypeSourcePLT.TabIndex = 18;
            this.radioButtonTypeSourcePLT.Tag = "_sourcePLT_";
            this.radioButtonTypeSourcePLT.Text = "       PLT Corel Draw";
            this.radioButtonTypeSourcePLT.UseVisualStyleBackColor = true;
            this.radioButtonTypeSourcePLT.CheckedChanged += new System.EventHandler(this.radioButtonTypeSourcePLT_CheckedChanged);
            // 
            // OrientationVar2
            // 
            this.OrientationVar2.AutoSize = true;
            this.OrientationVar2.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.OrientationVar2.Location = new System.Drawing.Point(9, 81);
            this.OrientationVar2.Name = "OrientationVar2";
            this.OrientationVar2.Size = new System.Drawing.Size(76, 17);
            this.OrientationVar2.TabIndex = 21;
            this.OrientationVar2.Tag = "_variant2_";
            this.OrientationVar2.Text = "Вариант 2";
            this.OrientationVar2.UseVisualStyleBackColor = true;
            this.OrientationVar2.CheckedChanged += new System.EventHandler(this.OrientationVar2_CheckedChanged);
            // 
            // OrientationVar1
            // 
            this.OrientationVar1.AutoSize = true;
            this.OrientationVar1.Checked = true;
            this.OrientationVar1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.OrientationVar1.Location = new System.Drawing.Point(9, 40);
            this.OrientationVar1.Name = "OrientationVar1";
            this.OrientationVar1.Size = new System.Drawing.Size(76, 17);
            this.OrientationVar1.TabIndex = 20;
            this.OrientationVar1.TabStop = true;
            this.OrientationVar1.Tag = "_variant1_";
            this.OrientationVar1.Text = "Вариант 1";
            this.OrientationVar1.UseVisualStyleBackColor = true;
            this.OrientationVar1.CheckedChanged += new System.EventHandler(this.OrientationVar1_CheckedChanged);
            // 
            // OrientationVar3
            // 
            this.OrientationVar3.AutoSize = true;
            this.OrientationVar3.Location = new System.Drawing.Point(9, 122);
            this.OrientationVar3.Name = "OrientationVar3";
            this.OrientationVar3.Size = new System.Drawing.Size(76, 17);
            this.OrientationVar3.TabIndex = 24;
            this.OrientationVar3.TabStop = true;
            this.OrientationVar3.Tag = "_variant3_";
            this.OrientationVar3.Text = "Вариант 3";
            this.OrientationVar3.UseVisualStyleBackColor = true;
            this.OrientationVar3.CheckedChanged += new System.EventHandler(this.OrientationVar3_CheckedChanged);
            // 
            // OrientationVar4
            // 
            this.OrientationVar4.AutoSize = true;
            this.OrientationVar4.Location = new System.Drawing.Point(9, 163);
            this.OrientationVar4.Name = "OrientationVar4";
            this.OrientationVar4.Size = new System.Drawing.Size(76, 17);
            this.OrientationVar4.TabIndex = 25;
            this.OrientationVar4.TabStop = true;
            this.OrientationVar4.Tag = "_variant4_";
            this.OrientationVar4.Text = "Вариант 4";
            this.OrientationVar4.UseVisualStyleBackColor = true;
            this.OrientationVar4.CheckedChanged += new System.EventHandler(this.OrientationVar4_CheckedChanged);
            // 
            // pictureBoxAxes
            // 
            this.pictureBoxAxes.BackColor = System.Drawing.Color.White;
            this.pictureBoxAxes.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBoxAxes.Location = new System.Drawing.Point(109, 26);
            this.pictureBoxAxes.Name = "pictureBoxAxes";
            this.pictureBoxAxes.Size = new System.Drawing.Size(234, 189);
            this.pictureBoxAxes.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBoxAxes.TabIndex = 26;
            this.pictureBoxAxes.TabStop = false;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.OrientationVar1);
            this.groupBox2.Controls.Add(this.pictureBoxAxes);
            this.groupBox2.Controls.Add(this.OrientationVar2);
            this.groupBox2.Controls.Add(this.OrientationVar4);
            this.groupBox2.Controls.Add(this.OrientationVar3);
            this.groupBox2.Location = new System.Drawing.Point(11, 4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(355, 230);
            this.groupBox2.TabIndex = 27;
            this.groupBox2.TabStop = false;
            this.groupBox2.Tag = "_selectvariant_";
            this.groupBox2.Text = "Выбор расположения начала координат:";
            // 
            // page01_start
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "page01_start";
            this.Size = new System.Drawing.Size(645, 240);
            this.Tag = "_page01_";
            this.Load += new System.EventHandler(this.page01_Load);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxAxes)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        public string PageName { get; set; }
        public int LastPage { get; set; }
        public int CurrPage { get; set; }
        public int NextPage { get; set; }
        public Bitmap pageImageIN { get; set; }
        public Bitmap pageImageNOW { get; set; }
        public List<GroupPoint> pageVectorIN { get; set; }
        public List<GroupPoint> pageVectorNOW { get; set; }



        public List<cncPoint> PagePoints { get; set; }


        private System.Windows.Forms.PictureBox pictureBoxAxes;
        private System.Windows.Forms.RadioButton radioButtonTypeSourcePLT;
        private System.Windows.Forms.RadioButton radioButtonTypeSourcePicture1;
        private System.Windows.Forms.RadioButton radioButtonTypeSourceText;
        private System.Windows.Forms.RadioButton OrientationVar1;
        private System.Windows.Forms.RadioButton OrientationVar2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radioButtonTypeSourcePicture2;
        private System.Windows.Forms.RadioButton OrientationVar3;
        private System.Windows.Forms.RadioButton OrientationVar4;
        private System.Windows.Forms.RadioButton radioButtonTypeSourceDXF;
        private System.Windows.Forms.GroupBox groupBox2;
    }
}
