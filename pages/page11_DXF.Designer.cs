using System.Collections.Generic;
using System.Drawing;

namespace ToolsGenGkode.pages
{
    partial class page11_DXF
    {
        /// <summary> 
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором компонентов

        /// <summary> 
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.buttonSelectFile = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.btShowOriginalImage = new System.Windows.Forms.Button();
            this.textBoxFileName = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.textBoxInfo = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonSelectFile
            // 
            this.buttonSelectFile.Location = new System.Drawing.Point(554, 4);
            this.buttonSelectFile.Name = "buttonSelectFile";
            this.buttonSelectFile.Size = new System.Drawing.Size(35, 28);
            this.buttonSelectFile.TabIndex = 14;
            this.buttonSelectFile.Text = "...";
            this.buttonSelectFile.UseVisualStyleBackColor = true;
            this.buttonSelectFile.Click += new System.EventHandler(this.buttonSelectFile_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(15, 12);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(78, 13);
            this.label6.TabIndex = 15;
            this.label6.Tag = "_SelectFile_";
            this.label6.Text = "Выбор файла:";
            // 
            // btShowOriginalImage
            // 
            this.btShowOriginalImage.Image = global::ToolsGenGkode.Properties.Resources.arrow_refresh16;
            this.btShowOriginalImage.Location = new System.Drawing.Point(599, 4);
            this.btShowOriginalImage.Name = "btShowOriginalImage";
            this.btShowOriginalImage.Size = new System.Drawing.Size(35, 28);
            this.btShowOriginalImage.TabIndex = 13;
            this.btShowOriginalImage.UseVisualStyleBackColor = true;
            this.btShowOriginalImage.Click += new System.EventHandler(this.btShowOriginalImage_Click);
            // 
            // textBoxFileName
            // 
            this.textBoxFileName.Location = new System.Drawing.Point(99, 9);
            this.textBoxFileName.Name = "textBoxFileName";
            this.textBoxFileName.Size = new System.Drawing.Size(449, 20);
            this.textBoxFileName.TabIndex = 12;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.textBoxInfo);
            this.groupBox1.Location = new System.Drawing.Point(9, 38);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(625, 201);
            this.groupBox1.TabIndex = 16;
            this.groupBox1.TabStop = false;
            this.groupBox1.Tag = "_infoAboutFile_";
            this.groupBox1.Text = "Информация о файле";
            // 
            // textBoxInfo
            // 
            this.textBoxInfo.Location = new System.Drawing.Point(9, 19);
            this.textBoxInfo.Multiline = true;
            this.textBoxInfo.Name = "textBoxInfo";
            this.textBoxInfo.ReadOnly = true;
            this.textBoxInfo.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxInfo.Size = new System.Drawing.Size(610, 176);
            this.textBoxInfo.TabIndex = 0;
            // 
            // page11_DXF
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.buttonSelectFile);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.btShowOriginalImage);
            this.Controls.Add(this.textBoxFileName);
            this.Name = "page11_DXF";
            this.Size = new System.Drawing.Size(645, 240);
            this.Tag = "_page11_";
            this.Load += new System.EventHandler(this.page11_DXF_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonSelectFile;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btShowOriginalImage;
        public System.Windows.Forms.TextBox textBoxFileName;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox textBoxInfo;

        public int NextPage { get; set; }
        public Bitmap pageImageIN { get; set; }
        public Bitmap pageImageNOW { get; set; }
        public List<GroupPoint> pageVectorIN { get; set; }
        public List<GroupPoint> pageVectorNOW { get; set; }
    }
}
