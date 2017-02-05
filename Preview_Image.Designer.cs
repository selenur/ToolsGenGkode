// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
namespace ToolsGenGkode
{
    partial class Preview_Image
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
            this.labelZoomSize = new System.Windows.Forms.Label();
            this.pictureBoxPreview = new Cyotek.Windows.Forms.ImageBox();
            this.btZoom100 = new System.Windows.Forms.Button();
            this.radioButton_Strech = new System.Windows.Forms.RadioButton();
            this.radioButton_FullSize = new System.Windows.Forms.RadioButton();
            this.SuspendLayout();
            // 
            // labelZoomSize
            // 
            this.labelZoomSize.Location = new System.Drawing.Point(3, 28);
            this.labelZoomSize.Name = "labelZoomSize";
            this.labelZoomSize.Size = new System.Drawing.Size(413, 21);
            this.labelZoomSize.TabIndex = 24;
            this.labelZoomSize.Text = "Текущий масштаб: 100%";
            // 
            // pictureBoxPreview
            // 
            this.pictureBoxPreview.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBoxPreview.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBoxPreview.Location = new System.Drawing.Point(6, 50);
            this.pictureBoxPreview.Name = "pictureBoxPreview";
            this.pictureBoxPreview.ShowPixelGrid = true;
            this.pictureBoxPreview.Size = new System.Drawing.Size(408, 151);
            this.pictureBoxPreview.TabIndex = 23;
            this.pictureBoxPreview.ImageChanged += new System.EventHandler(this.pictureBoxPreview_ImageChanged);
            this.pictureBoxPreview.ZoomChanged += new System.EventHandler(this.pictureBoxPreview_ZoomChanged);
            // 
            // btZoom100
            // 
            this.btZoom100.Image = global::ToolsGenGkode.Properties.Resources.resizeActual;
            this.btZoom100.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btZoom100.Location = new System.Drawing.Point(278, 5);
            this.btZoom100.Name = "btZoom100";
            this.btZoom100.Size = new System.Drawing.Size(138, 23);
            this.btZoom100.TabIndex = 25;
            this.btZoom100.Tag = "_SetScale100_";
            this.btZoom100.Text = "Уст. масштаб 100%";
            this.btZoom100.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btZoom100.UseVisualStyleBackColor = true;
            this.btZoom100.Click += new System.EventHandler(this.btZoom100_Click);
            // 
            // radioButton_Strech
            // 
            this.radioButton_Strech.AutoSize = true;
            this.radioButton_Strech.Location = new System.Drawing.Point(132, 8);
            this.radioButton_Strech.Name = "radioButton_Strech";
            this.radioButton_Strech.Size = new System.Drawing.Size(140, 17);
            this.radioButton_Strech.TabIndex = 21;
            this.radioButton_Strech.Tag = "_strech_";
            this.radioButton_Strech.Text = "Растянуть на всё окно";
            this.radioButton_Strech.UseVisualStyleBackColor = true;
            this.radioButton_Strech.CheckedChanged += new System.EventHandler(this.radioButton_Strech_CheckedChanged);
            // 
            // radioButton_FullSize
            // 
            this.radioButton_FullSize.AutoSize = true;
            this.radioButton_FullSize.Checked = true;
            this.radioButton_FullSize.Location = new System.Drawing.Point(9, 8);
            this.radioButton_FullSize.Name = "radioButton_FullSize";
            this.radioButton_FullSize.Size = new System.Drawing.Size(117, 17);
            this.radioButton_FullSize.TabIndex = 22;
            this.radioButton_FullSize.TabStop = true;
            this.radioButton_FullSize.Tag = "_realSize_";
            this.radioButton_FullSize.Text = "Реальный размер";
            this.radioButton_FullSize.UseVisualStyleBackColor = true;
            this.radioButton_FullSize.CheckedChanged += new System.EventHandler(this.radioButton_FullSize_CheckedChanged);
            // 
            // Preview_Image
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.labelZoomSize);
            this.Controls.Add(this.pictureBoxPreview);
            this.Controls.Add(this.btZoom100);
            this.Controls.Add(this.radioButton_Strech);
            this.Controls.Add(this.radioButton_FullSize);
            this.Name = "Preview_Image";
            this.Size = new System.Drawing.Size(419, 204);
            this.Load += new System.EventHandler(this.Preview_Image_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelZoomSize;
        private Cyotek.Windows.Forms.ImageBox pictureBoxPreview;
        private System.Windows.Forms.Button btZoom100;
        private System.Windows.Forms.RadioButton radioButton_Strech;
        private System.Windows.Forms.RadioButton radioButton_FullSize;
    }
}
