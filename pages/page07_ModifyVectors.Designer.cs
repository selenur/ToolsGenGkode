using System.Collections.Generic;
using System.Drawing;

namespace ToolsGenGkode.pages
{
    partial class page07_ModifyVectors
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
            this.numRotate = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.cbKeepAspectRatio = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btCalcZoom = new System.Windows.Forms.Button();
            this.numYAfter = new System.Windows.Forms.NumericUpDown();
            this.numXAfter = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.numYbefore = new System.Windows.Forms.NumericUpDown();
            this.numXbefore = new System.Windows.Forms.NumericUpDown();
            this.label8 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.btMirrorX = new System.Windows.Forms.Button();
            this.btMirrorY = new System.Windows.Forms.Button();
            this.btRotate = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btoffset1 = new System.Windows.Forms.Button();
            this.btMoveToZero = new System.Windows.Forms.Button();
            this.label10 = new System.Windows.Forms.Label();
            this.numDeltaY = new System.Windows.Forms.NumericUpDown();
            this.numDeltaX = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.cbAddPadding = new System.Windows.Forms.CheckBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.numRotate)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numYAfter)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numXAfter)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numYbefore)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numXbefore)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numDeltaY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numDeltaX)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // numRotate
            // 
            this.numRotate.Location = new System.Drawing.Point(82, 19);
            this.numRotate.Maximum = new decimal(new int[] {
            360,
            0,
            0,
            0});
            this.numRotate.Minimum = new decimal(new int[] {
            360,
            0,
            0,
            -2147483648});
            this.numRotate.Name = "numRotate";
            this.numRotate.Size = new System.Drawing.Size(47, 20);
            this.numRotate.TabIndex = 0;
            this.numRotate.ValueChanged += new System.EventHandler(this.numRotate_ValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(135, 24);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 13);
            this.label2.TabIndex = 1;
            this.label2.Tag = "_deg_";
            this.label2.Text = "градусов";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 24);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(70, 13);
            this.label3.TabIndex = 6;
            this.label3.Tag = "_rotate_";
            this.label3.Text = "Вращать на:";
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(6, 55);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(102, 16);
            this.label5.TabIndex = 3;
            this.label5.Tag = "_newSize_";
            this.label5.Text = "Новый размер:";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(6, 34);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(102, 16);
            this.label4.TabIndex = 2;
            this.label4.Tag = "_currentSize_";
            this.label4.Text = "Исходный размер:";
            // 
            // cbKeepAspectRatio
            // 
            this.cbKeepAspectRatio.AutoSize = true;
            this.cbKeepAspectRatio.Checked = true;
            this.cbKeepAspectRatio.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbKeepAspectRatio.Location = new System.Drawing.Point(136, 76);
            this.cbKeepAspectRatio.Name = "cbKeepAspectRatio";
            this.cbKeepAspectRatio.Size = new System.Drawing.Size(136, 17);
            this.cbKeepAspectRatio.TabIndex = 1;
            this.cbKeepAspectRatio.Tag = "_UseRatio_";
            this.cbKeepAspectRatio.Text = "Сохранять пропорции";
            this.cbKeepAspectRatio.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btCalcZoom);
            this.groupBox2.Controls.Add(this.numYAfter);
            this.groupBox2.Controls.Add(this.numXAfter);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.numYbefore);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.numXbefore);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.cbKeepAspectRatio);
            this.groupBox2.Location = new System.Drawing.Point(245, 5);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(278, 141);
            this.groupBox2.TabIndex = 8;
            this.groupBox2.TabStop = false;
            this.groupBox2.Tag = "_changeSize_";
            this.groupBox2.Text = "Масштабирование в мм.";
            // 
            // btCalcZoom
            // 
            this.btCalcZoom.Location = new System.Drawing.Point(60, 97);
            this.btCalcZoom.Name = "btCalcZoom";
            this.btCalcZoom.Size = new System.Drawing.Size(212, 38);
            this.btCalcZoom.TabIndex = 16;
            this.btCalcZoom.Tag = "_RunChangeSize_";
            this.btCalcZoom.Text = "Выполнить масштабирование";
            this.btCalcZoom.UseVisualStyleBackColor = true;
            this.btCalcZoom.Click += new System.EventHandler(this.btCalcZoom_Click);
            // 
            // numYAfter
            // 
            this.numYAfter.DecimalPlaces = 3;
            this.numYAfter.ForeColor = System.Drawing.Color.Green;
            this.numYAfter.Location = new System.Drawing.Point(195, 53);
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
            this.numXAfter.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.numXAfter.Location = new System.Drawing.Point(112, 53);
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
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.ForeColor = System.Drawing.Color.Green;
            this.label7.Location = new System.Drawing.Point(214, 14);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(17, 13);
            this.label7.TabIndex = 11;
            this.label7.Text = "Y:";
            // 
            // numYbefore
            // 
            this.numYbefore.DecimalPlaces = 3;
            this.numYbefore.ForeColor = System.Drawing.Color.Green;
            this.numYbefore.Location = new System.Drawing.Point(195, 30);
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
            // numXbefore
            // 
            this.numXbefore.DecimalPlaces = 3;
            this.numXbefore.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.numXbefore.Location = new System.Drawing.Point(112, 30);
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
            this.label8.Location = new System.Drawing.Point(151, 14);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(17, 13);
            this.label8.TabIndex = 8;
            this.label8.Text = "X:";
            // 
            // button1
            // 
            this.button1.Image = global::ToolsGenGkode.Properties.Resources.arrow_refresh;
            this.button1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button1.Location = new System.Drawing.Point(9, 5);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(230, 50);
            this.button1.TabIndex = 9;
            this.button1.Tag = "_resetAllActions_";
            this.button1.Text = "            Сбросить все изменения внесенные на данной странице";
            this.button1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // btMirrorX
            // 
            this.btMirrorX.Location = new System.Drawing.Point(246, 152);
            this.btMirrorX.Name = "btMirrorX";
            this.btMirrorX.Size = new System.Drawing.Size(188, 31);
            this.btMirrorX.TabIndex = 10;
            this.btMirrorX.Tag = "_mirrorX_";
            this.btMirrorX.Text = "Отразить зеркально по оси X";
            this.btMirrorX.UseVisualStyleBackColor = true;
            this.btMirrorX.Click += new System.EventHandler(this.btMirrorX_Click);
            // 
            // btMirrorY
            // 
            this.btMirrorY.Location = new System.Drawing.Point(440, 152);
            this.btMirrorY.Name = "btMirrorY";
            this.btMirrorY.Size = new System.Drawing.Size(188, 31);
            this.btMirrorY.TabIndex = 11;
            this.btMirrorY.Tag = "_mirrorY_";
            this.btMirrorY.Text = "Отразить зеркально по оси Y";
            this.btMirrorY.UseVisualStyleBackColor = true;
            this.btMirrorY.Click += new System.EventHandler(this.btMirrorY_Click);
            // 
            // btRotate
            // 
            this.btRotate.Location = new System.Drawing.Point(6, 94);
            this.btRotate.Name = "btRotate";
            this.btRotate.Size = new System.Drawing.Size(218, 31);
            this.btRotate.TabIndex = 12;
            this.btRotate.Tag = "_RunRotate_";
            this.btRotate.Text = "Выполнить вращение";
            this.btRotate.UseVisualStyleBackColor = true;
            this.btRotate.Click += new System.EventHandler(this.btRotate_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btoffset1);
            this.groupBox1.Controls.Add(this.btMoveToZero);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.numDeltaY);
            this.groupBox1.Controls.Add(this.numDeltaX);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(529, 5);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(112, 141);
            this.groupBox1.TabIndex = 13;
            this.groupBox1.TabStop = false;
            this.groupBox1.Tag = "";
            this.groupBox1.Text = "Смещение";
            // 
            // btoffset1
            // 
            this.btoffset1.Location = new System.Drawing.Point(6, 60);
            this.btoffset1.Name = "btoffset1";
            this.btoffset1.Size = new System.Drawing.Size(98, 33);
            this.btoffset1.TabIndex = 15;
            this.btoffset1.Text = "Сместить";
            this.btoffset1.UseVisualStyleBackColor = true;
            this.btoffset1.Click += new System.EventHandler(this.btoffset1_Click);
            // 
            // btMoveToZero
            // 
            this.btMoveToZero.Location = new System.Drawing.Point(6, 97);
            this.btMoveToZero.Name = "btMoveToZero";
            this.btMoveToZero.Size = new System.Drawing.Size(98, 38);
            this.btMoveToZero.TabIndex = 14;
            this.btMoveToZero.Tag = "";
            this.btMoveToZero.Text = "Сместить к началу";
            this.btMoveToZero.UseVisualStyleBackColor = true;
            this.btMoveToZero.Click += new System.EventHandler(this.btMoveToZero_Click);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.ForeColor = System.Drawing.Color.Green;
            this.label10.Location = new System.Drawing.Point(7, 39);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(17, 13);
            this.label10.TabIndex = 13;
            this.label10.Text = "Y:";
            // 
            // numDeltaY
            // 
            this.numDeltaY.DecimalPlaces = 3;
            this.numDeltaY.ForeColor = System.Drawing.Color.Green;
            this.numDeltaY.Location = new System.Drawing.Point(27, 37);
            this.numDeltaY.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            0});
            this.numDeltaY.Minimum = new decimal(new int[] {
            99999,
            0,
            0,
            -2147483648});
            this.numDeltaY.Name = "numDeltaY";
            this.numDeltaY.Size = new System.Drawing.Size(77, 20);
            this.numDeltaY.TabIndex = 12;
            // 
            // numDeltaX
            // 
            this.numDeltaX.DecimalPlaces = 3;
            this.numDeltaX.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.numDeltaX.Location = new System.Drawing.Point(27, 14);
            this.numDeltaX.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            0});
            this.numDeltaX.Minimum = new decimal(new int[] {
            99999,
            0,
            0,
            -2147483648});
            this.numDeltaX.Name = "numDeltaX";
            this.numDeltaX.Size = new System.Drawing.Size(77, 20);
            this.numDeltaX.TabIndex = 11;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.label1.Location = new System.Drawing.Point(7, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(17, 13);
            this.label1.TabIndex = 10;
            this.label1.Text = "X:";
            // 
            // cbAddPadding
            // 
            this.cbAddPadding.AutoSize = true;
            this.cbAddPadding.Enabled = false;
            this.cbAddPadding.Location = new System.Drawing.Point(3, 220);
            this.cbAddPadding.Name = "cbAddPadding";
            this.cbAddPadding.Size = new System.Drawing.Size(301, 17);
            this.cbAddPadding.TabIndex = 14;
            this.cbAddPadding.Text = "Добавление отступов, от первоначальной траектории";
            this.cbAddPadding.UseVisualStyleBackColor = true;
            this.cbAddPadding.CheckedChanged += new System.EventHandler(this.cbAddPadding_CheckedChanged);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.comboBox1);
            this.groupBox3.Controls.Add(this.btRotate);
            this.groupBox3.Controls.Add(this.numRotate);
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Location = new System.Drawing.Point(10, 58);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(230, 133);
            this.groupBox3.TabIndex = 15;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Вращение";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(11, 48);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(166, 13);
            this.label6.TabIndex = 14;
            this.label6.Text = "Вращение относительно точки:";
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "1 - Центр",
            "2 - Лев.нижний",
            "3 - Лев.врхний",
            "4 - Прав.верхний",
            "5 - Прав. нижний",
            "6 - Начало координат"});
            this.comboBox1.Location = new System.Drawing.Point(6, 67);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(218, 21);
            this.comboBox1.TabIndex = 13;
            // 
            // page07_ModifyVectors
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.cbAddPadding);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btMirrorY);
            this.Controls.Add(this.btMirrorX);
            this.Controls.Add(this.groupBox2);
            this.Name = "page07_ModifyVectors";
            this.Size = new System.Drawing.Size(645, 240);
            this.Tag = "_page07_";
            this.Load += new System.EventHandler(this.page08_ModifyVectors_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numRotate)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numYAfter)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numXAfter)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numYbefore)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numXbefore)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numDeltaY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numDeltaX)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NumericUpDown numRotate;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox cbKeepAspectRatio;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button btMirrorX;
        private System.Windows.Forms.Button btMirrorY;
        private System.Windows.Forms.Button btRotate;
        private System.Windows.Forms.Button btCalcZoom;
        private System.Windows.Forms.NumericUpDown numYAfter;
        private System.Windows.Forms.NumericUpDown numXAfter;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.NumericUpDown numYbefore;
        private System.Windows.Forms.NumericUpDown numXbefore;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.NumericUpDown numDeltaY;
        private System.Windows.Forms.NumericUpDown numDeltaX;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btMoveToZero;
        private System.Windows.Forms.CheckBox cbAddPadding;
        private System.Windows.Forms.Button btoffset1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox comboBox1;

        public int NextPage { get; set; }
        public Bitmap pageImageIN { get; set; }
        public Bitmap pageImageNOW { get; set; }
        public List<GroupPoint> pageVectorIN { get; set; }
        public List<GroupPoint> pageVectorNOW { get; set; }



    }
}
