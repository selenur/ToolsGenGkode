using System.Collections.Generic;
using System.Drawing;

namespace ToolsGenGkode.pages
{
    partial class page05_SelectFileImageRastr
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
            this.SuspendLayout();
            // 
            // buttonSelectFile
            // 
            this.buttonSelectFile.Location = new System.Drawing.Point(426, 72);
            this.buttonSelectFile.Name = "buttonSelectFile";
            this.buttonSelectFile.Size = new System.Drawing.Size(39, 21);
            this.buttonSelectFile.TabIndex = 10;
            this.buttonSelectFile.Text = "...";
            this.buttonSelectFile.UseVisualStyleBackColor = true;
            this.buttonSelectFile.Click += new System.EventHandler(this.buttonSelectFile_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(124, 57);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(78, 13);
            this.label6.TabIndex = 11;
            this.label6.Tag = "_SelectFile_";
            this.label6.Text = "Выбор файла:";
            // 
            // btShowOriginalImage
            // 
            this.btShowOriginalImage.Location = new System.Drawing.Point(118, 99);
            this.btShowOriginalImage.Name = "btShowOriginalImage";
            this.btShowOriginalImage.Size = new System.Drawing.Size(252, 37);
            this.btShowOriginalImage.TabIndex = 9;
            this.btShowOriginalImage.Tag = "_showImage_";
            this.btShowOriginalImage.Text = "Показать рисунок";
            this.btShowOriginalImage.UseVisualStyleBackColor = true;
            this.btShowOriginalImage.Click += new System.EventHandler(this.btShowOriginalImage_Click);
            // 
            // textBoxFileName
            // 
            this.textBoxFileName.Location = new System.Drawing.Point(118, 73);
            this.textBoxFileName.Name = "textBoxFileName";
            this.textBoxFileName.Size = new System.Drawing.Size(302, 20);
            this.textBoxFileName.TabIndex = 8;
            // 
            // page05_SelectFileImageRastr
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.buttonSelectFile);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.btShowOriginalImage);
            this.Controls.Add(this.textBoxFileName);
            this.Name = "page05_SelectFileImageRastr";
            this.Size = new System.Drawing.Size(604, 219);
            this.Tag = "_page05_";
            this.Load += new System.EventHandler(this.page05_SelectFileImageRastr_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonSelectFile;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btShowOriginalImage;
        public System.Windows.Forms.TextBox textBoxFileName;

        public int NextPage { get; set; }
        public Bitmap pageImageIN { get; set; }
        public Bitmap pageImageNOW { get; set; }
        public List<GroupPoint> pageVectorIN { get; set; }
        public List<GroupPoint> pageVectorNOW { get; set; }
    }
}
