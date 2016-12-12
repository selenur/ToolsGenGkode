using System.Collections.Generic;
using System.Drawing;

namespace ToolsGenGkode.pages
{
    partial class page09_ImageRast
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
            this.label2 = new System.Windows.Forms.Label();
            this.LaserTimeOut = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.numSizePoint = new System.Windows.Forms.NumericUpDown();
            this.useFilter = new System.Windows.Forms.ComboBox();
            this.button3 = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this.deltaY = new System.Windows.Forms.NumericUpDown();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.numericUpDownPercent = new System.Windows.Forms.NumericUpDown();
            this.radioButtonPerent = new System.Windows.Forms.RadioButton();
            this.radioButtonSizePoint = new System.Windows.Forms.RadioButton();
            this.radioButtonUserSize = new System.Windows.Forms.RadioButton();
            this.radioButtonDiametrSizePoint = new System.Windows.Forms.RadioButton();
            this.deltaX = new System.Windows.Forms.NumericUpDown();
            this.btCalcTraectory = new System.Windows.Forms.Button();
            this.numYAfter = new System.Windows.Forms.NumericUpDown();
            this.numXAfter = new System.Windows.Forms.NumericUpDown();
            this.cbKeepAspectRatio = new System.Windows.Forms.CheckBox();
            this.label7 = new System.Windows.Forms.Label();
            this.numYbefore = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.numXbefore = new System.Windows.Forms.NumericUpDown();
            this.label8 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.LaserTimeOut)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSizePoint)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.deltaY)).BeginInit();
            this.groupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPercent)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.deltaX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numYAfter)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numXAfter)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numYbefore)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numXbefore)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.LaserTimeOut);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.numSizePoint);
            this.groupBox1.Location = new System.Drawing.Point(469, 2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(173, 77);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Размер прожигаемой точки";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 50);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(89, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Время прожига:";
            // 
            // LaserTimeOut
            // 
            this.LaserTimeOut.Increment = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.LaserTimeOut.Location = new System.Drawing.Point(101, 46);
            this.LaserTimeOut.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.LaserTimeOut.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.LaserTimeOut.Name = "LaserTimeOut";
            this.LaserTimeOut.Size = new System.Drawing.Size(66, 20);
            this.LaserTimeOut.TabIndex = 2;
            this.LaserTimeOut.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.LaserTimeOut.ValueChanged += new System.EventHandler(this.LaserTimeOut_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(81, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Диаметр (мм):";
            // 
            // numSizePoint
            // 
            this.numSizePoint.DecimalPlaces = 2;
            this.numSizePoint.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.numSizePoint.Location = new System.Drawing.Point(101, 19);
            this.numSizePoint.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.numSizePoint.Name = "numSizePoint";
            this.numSizePoint.Size = new System.Drawing.Size(66, 20);
            this.numSizePoint.TabIndex = 0;
            this.numSizePoint.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numSizePoint.ValueChanged += new System.EventHandler(this.numSizePoint_ValueChanged);
            // 
            // useFilter
            // 
            this.useFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.useFilter.FormattingEnabled = true;
            this.useFilter.Items.AddRange(new object[] {
            "FloydSteinbergDithering",
            "BayerDithering"});
            this.useFilter.Location = new System.Drawing.Point(6, 21);
            this.useFilter.Name = "useFilter";
            this.useFilter.Size = new System.Drawing.Size(448, 21);
            this.useFilter.TabIndex = 8;
            this.useFilter.SelectedIndexChanged += new System.EventHandler(this.useFilter_SelectedIndexChanged);
            // 
            // button3
            // 
            this.button3.Image = global::ToolsGenGkode.Properties.Resources.arrow_refresh16;
            this.button3.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button3.Location = new System.Drawing.Point(5, 46);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(449, 26);
            this.button3.TabIndex = 10;
            this.button3.Text = "Показать предварительный результат";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.deltaY);
            this.groupBox2.Controls.Add(this.groupBox5);
            this.groupBox2.Controls.Add(this.deltaX);
            this.groupBox2.Controls.Add(this.btCalcTraectory);
            this.groupBox2.Controls.Add(this.numYAfter);
            this.groupBox2.Controls.Add(this.numXAfter);
            this.groupBox2.Controls.Add(this.cbKeepAspectRatio);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.numYbefore);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.numXbefore);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Location = new System.Drawing.Point(3, 82);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(639, 153);
            this.groupBox2.TabIndex = 11;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Генерация данных:";
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(9, 132);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(162, 18);
            this.label6.TabIndex = 25;
            this.label6.Text = "Применить смещение (мм):";
            // 
            // deltaY
            // 
            this.deltaY.DecimalPlaces = 3;
            this.deltaY.ForeColor = System.Drawing.Color.Green;
            this.deltaY.Location = new System.Drawing.Point(260, 128);
            this.deltaY.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            0});
            this.deltaY.Minimum = new decimal(new int[] {
            99999,
            0,
            0,
            -2147483648});
            this.deltaY.Name = "deltaY";
            this.deltaY.Size = new System.Drawing.Size(77, 20);
            this.deltaY.TabIndex = 16;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.numericUpDownPercent);
            this.groupBox5.Controls.Add(this.radioButtonPerent);
            this.groupBox5.Controls.Add(this.radioButtonSizePoint);
            this.groupBox5.Controls.Add(this.radioButtonUserSize);
            this.groupBox5.Controls.Add(this.radioButtonDiametrSizePoint);
            this.groupBox5.Location = new System.Drawing.Point(6, 16);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(627, 44);
            this.groupBox5.TabIndex = 24;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Результирующий размер вычисляется как:";
            // 
            // numericUpDownPercent
            // 
            this.numericUpDownPercent.DecimalPlaces = 2;
            this.numericUpDownPercent.ForeColor = System.Drawing.SystemColors.ControlText;
            this.numericUpDownPercent.Location = new System.Drawing.Point(346, 18);
            this.numericUpDownPercent.Maximum = new decimal(new int[] {
            900000,
            0,
            0,
            0});
            this.numericUpDownPercent.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.numericUpDownPercent.Name = "numericUpDownPercent";
            this.numericUpDownPercent.Size = new System.Drawing.Size(56, 20);
            this.numericUpDownPercent.TabIndex = 27;
            this.numericUpDownPercent.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.numericUpDownPercent.ValueChanged += new System.EventHandler(this.numericUpDownPercent_ValueChanged);
            // 
            // radioButtonPerent
            // 
            this.radioButtonPerent.AutoSize = true;
            this.radioButtonPerent.ForeColor = System.Drawing.SystemColors.ControlText;
            this.radioButtonPerent.Location = new System.Drawing.Point(307, 19);
            this.radioButtonPerent.Name = "radioButtonPerent";
            this.radioButtonPerent.Size = new System.Drawing.Size(33, 17);
            this.radioButtonPerent.TabIndex = 26;
            this.radioButtonPerent.TabStop = true;
            this.radioButtonPerent.Text = "%";
            this.radioButtonPerent.UseVisualStyleBackColor = true;
            this.radioButtonPerent.CheckedChanged += new System.EventHandler(this.radioButtonPerent_CheckedChanged);
            // 
            // radioButtonSizePoint
            // 
            this.radioButtonSizePoint.AutoSize = true;
            this.radioButtonSizePoint.ForeColor = System.Drawing.SystemColors.ControlText;
            this.radioButtonSizePoint.Location = new System.Drawing.Point(196, 19);
            this.radioButtonSizePoint.Name = "radioButtonSizePoint";
            this.radioButtonSizePoint.Size = new System.Drawing.Size(94, 17);
            this.radioButtonSizePoint.TabIndex = 25;
            this.radioButtonSizePoint.TabStop = true;
            this.radioButtonSizePoint.Text = "кол.пикселей";
            this.radioButtonSizePoint.UseVisualStyleBackColor = true;
            this.radioButtonSizePoint.CheckedChanged += new System.EventHandler(this.radioButtonSizePoint_CheckedChanged);
            // 
            // radioButtonUserSize
            // 
            this.radioButtonUserSize.AutoSize = true;
            this.radioButtonUserSize.ForeColor = System.Drawing.SystemColors.ControlText;
            this.radioButtonUserSize.Location = new System.Drawing.Point(422, 19);
            this.radioButtonUserSize.Name = "radioButtonUserSize";
            this.radioButtonUserSize.Size = new System.Drawing.Size(199, 17);
            this.radioButtonUserSize.TabIndex = 24;
            this.radioButtonUserSize.TabStop = true;
            this.radioButtonUserSize.Text = "пользователь сам укажет размер";
            this.radioButtonUserSize.UseVisualStyleBackColor = true;
            this.radioButtonUserSize.CheckedChanged += new System.EventHandler(this.radioButtonUserSize_CheckedChanged);
            // 
            // radioButtonDiametrSizePoint
            // 
            this.radioButtonDiametrSizePoint.AutoSize = true;
            this.radioButtonDiametrSizePoint.Checked = true;
            this.radioButtonDiametrSizePoint.ForeColor = System.Drawing.SystemColors.ControlText;
            this.radioButtonDiametrSizePoint.Location = new System.Drawing.Point(6, 19);
            this.radioButtonDiametrSizePoint.Name = "radioButtonDiametrSizePoint";
            this.radioButtonDiametrSizePoint.Size = new System.Drawing.Size(184, 17);
            this.radioButtonDiametrSizePoint.TabIndex = 23;
            this.radioButtonDiametrSizePoint.TabStop = true;
            this.radioButtonDiametrSizePoint.Text = "диаметр точки * кол.  пикселей";
            this.radioButtonDiametrSizePoint.UseVisualStyleBackColor = true;
            this.radioButtonDiametrSizePoint.CheckedChanged += new System.EventHandler(this.radioButtonDiametrSizePoint_CheckedChanged);
            // 
            // deltaX
            // 
            this.deltaX.DecimalPlaces = 3;
            this.deltaX.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.deltaX.Location = new System.Drawing.Point(177, 128);
            this.deltaX.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            0});
            this.deltaX.Minimum = new decimal(new int[] {
            99999,
            0,
            0,
            -2147483648});
            this.deltaX.Name = "deltaX";
            this.deltaX.Size = new System.Drawing.Size(77, 20);
            this.deltaX.TabIndex = 15;
            // 
            // btCalcTraectory
            // 
            this.btCalcTraectory.Image = global::ToolsGenGkode.Properties.Resources.control_play_blue;
            this.btCalcTraectory.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btCalcTraectory.Location = new System.Drawing.Point(353, 103);
            this.btCalcTraectory.Name = "btCalcTraectory";
            this.btCalcTraectory.Size = new System.Drawing.Size(279, 42);
            this.btCalcTraectory.TabIndex = 16;
            this.btCalcTraectory.Text = "         Вычислить траекторию";
            this.btCalcTraectory.UseVisualStyleBackColor = true;
            this.btCalcTraectory.Click += new System.EventHandler(this.btCalcTraectory_Click);
            // 
            // numYAfter
            // 
            this.numYAfter.DecimalPlaces = 3;
            this.numYAfter.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.numYAfter.ForeColor = System.Drawing.Color.Green;
            this.numYAfter.Location = new System.Drawing.Point(260, 106);
            this.numYAfter.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            0});
            this.numYAfter.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            196608});
            this.numYAfter.Name = "numYAfter";
            this.numYAfter.Size = new System.Drawing.Size(77, 20);
            this.numYAfter.TabIndex = 14;
            this.numYAfter.Value = new decimal(new int[] {
            1,
            0,
            0,
            196608});
            this.numYAfter.ValueChanged += new System.EventHandler(this.numYAfter_ValueChanged);
            // 
            // numXAfter
            // 
            this.numXAfter.DecimalPlaces = 3;
            this.numXAfter.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.numXAfter.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.numXAfter.Location = new System.Drawing.Point(177, 106);
            this.numXAfter.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            0});
            this.numXAfter.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            196608});
            this.numXAfter.Name = "numXAfter";
            this.numXAfter.Size = new System.Drawing.Size(77, 20);
            this.numXAfter.TabIndex = 13;
            this.numXAfter.Value = new decimal(new int[] {
            1,
            0,
            0,
            196608});
            this.numXAfter.ValueChanged += new System.EventHandler(this.numXAfter_ValueChanged);
            // 
            // cbKeepAspectRatio
            // 
            this.cbKeepAspectRatio.Checked = true;
            this.cbKeepAspectRatio.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbKeepAspectRatio.ForeColor = System.Drawing.Color.Purple;
            this.cbKeepAspectRatio.Location = new System.Drawing.Point(385, 66);
            this.cbKeepAspectRatio.Name = "cbKeepAspectRatio";
            this.cbKeepAspectRatio.Size = new System.Drawing.Size(242, 34);
            this.cbKeepAspectRatio.TabIndex = 1;
            this.cbKeepAspectRatio.Text = "Сохранять пропорции при изменении пользователем, итоговых размеров";
            this.cbKeepAspectRatio.UseVisualStyleBackColor = true;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.ForeColor = System.Drawing.Color.Green;
            this.label7.Location = new System.Drawing.Point(289, 64);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(17, 13);
            this.label7.TabIndex = 11;
            this.label7.Text = "Y:";
            // 
            // numYbefore
            // 
            this.numYbefore.DecimalPlaces = 3;
            this.numYbefore.ForeColor = System.Drawing.Color.Green;
            this.numYbefore.Location = new System.Drawing.Point(260, 80);
            this.numYbefore.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            0});
            this.numYbefore.Minimum = new decimal(new int[] {
            99999,
            0,
            0,
            -2147483648});
            this.numYbefore.Name = "numYbefore";
            this.numYbefore.ReadOnly = true;
            this.numYbefore.Size = new System.Drawing.Size(77, 20);
            this.numYbefore.TabIndex = 10;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label5.Location = new System.Drawing.Point(13, 109);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(155, 13);
            this.label5.TabIndex = 3;
            this.label5.Text = "Получится размер (мм.):";
            // 
            // numXbefore
            // 
            this.numXbefore.DecimalPlaces = 3;
            this.numXbefore.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.numXbefore.Location = new System.Drawing.Point(177, 80);
            this.numXbefore.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            0});
            this.numXbefore.Minimum = new decimal(new int[] {
            99999,
            0,
            0,
            -2147483648});
            this.numXbefore.Name = "numXbefore";
            this.numXbefore.ReadOnly = true;
            this.numXbefore.Size = new System.Drawing.Size(77, 20);
            this.numXbefore.TabIndex = 9;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.label8.Location = new System.Drawing.Point(210, 64);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(17, 13);
            this.label8.TabIndex = 8;
            this.label8.Text = "X:";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(9, 82);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(162, 18);
            this.label4.TabIndex = 2;
            this.label4.Text = "Исходный размер в пикселях:";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.useFilter);
            this.groupBox3.Controls.Add(this.button3);
            this.groupBox3.Location = new System.Drawing.Point(3, 2);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(460, 77);
            this.groupBox3.TabIndex = 12;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Для генерации данных, будем использовать фильтр:";
            // 
            // page09_ImageRast
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "page09_ImageRast";
            this.Size = new System.Drawing.Size(645, 240);
            this.Tag = "_page09_";
            this.Load += new System.EventHandler(this.page09_SelectImage_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.LaserTimeOut)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSizePoint)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.deltaY)).EndInit();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPercent)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.deltaX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numYAfter)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numXAfter)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numYbefore)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numXbefore)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown LaserTimeOut;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown numSizePoint;
        private System.Windows.Forms.ComboBox useFilter;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btCalcTraectory;
        private System.Windows.Forms.NumericUpDown numYAfter;
        private System.Windows.Forms.NumericUpDown numXAfter;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.NumericUpDown numYbefore;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown numXbefore;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.CheckBox cbKeepAspectRatio;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.NumericUpDown deltaY;
        private System.Windows.Forms.NumericUpDown deltaX;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.RadioButton radioButtonSizePoint;
        private System.Windows.Forms.RadioButton radioButtonUserSize;
        private System.Windows.Forms.RadioButton radioButtonDiametrSizePoint;
        private System.Windows.Forms.NumericUpDown numericUpDownPercent;
        private System.Windows.Forms.RadioButton radioButtonPerent;
        private System.Windows.Forms.Label label6;
    }
}
