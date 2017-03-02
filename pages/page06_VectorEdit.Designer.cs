using System.Collections.Generic;
using System.Drawing;

namespace ToolsGenGkode.pages
{
    partial class page06_VectorEdit
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
            this.treeViewVectors = new System.Windows.Forms.TreeView();
            this.btDelVector = new System.Windows.Forms.Button();
            this.btLoadVectors = new System.Windows.Forms.Button();
            this.btOptimize1 = new System.Windows.Forms.Button();
            this.btOptimize2 = new System.Windows.Forms.Button();
            this.labelTraectoryInfo = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.btCloseTraectory = new System.Windows.Forms.Button();
            this.ForGena = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.numericUpDown2 = new System.Windows.Forms.NumericUpDown();
            this.numericGena = new System.Windows.Forms.NumericUpDown();
            this.checkBoxGena = new System.Windows.Forms.CheckBox();
            this.button2 = new System.Windows.Forms.Button();
            this.buttonSelectedUp = new System.Windows.Forms.Button();
            this.buttonSelectedDown = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.ForGena.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericGena)).BeginInit();
            this.SuspendLayout();
            // 
            // treeViewVectors
            // 
            this.treeViewVectors.Location = new System.Drawing.Point(25, 3);
            this.treeViewVectors.Name = "treeViewVectors";
            this.treeViewVectors.Size = new System.Drawing.Size(224, 234);
            this.treeViewVectors.TabIndex = 0;
            this.treeViewVectors.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeViewVectors_AfterSelect);
            // 
            // btDelVector
            // 
            this.btDelVector.Location = new System.Drawing.Point(481, 5);
            this.btDelVector.Name = "btDelVector";
            this.btDelVector.Size = new System.Drawing.Size(164, 41);
            this.btDelVector.TabIndex = 4;
            this.btDelVector.Tag = "_delSelected_";
            this.btDelVector.Text = "Удалить выбранный отрезок/точку";
            this.btDelVector.UseVisualStyleBackColor = true;
            this.btDelVector.Click += new System.EventHandler(this.btDelVector_Click);
            // 
            // btLoadVectors
            // 
            this.btLoadVectors.Image = global::ToolsGenGkode.Properties.Resources.arrow_refresh;
            this.btLoadVectors.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btLoadVectors.Location = new System.Drawing.Point(255, 3);
            this.btLoadVectors.Name = "btLoadVectors";
            this.btLoadVectors.Size = new System.Drawing.Size(217, 43);
            this.btLoadVectors.TabIndex = 7;
            this.btLoadVectors.Tag = "_resetAllActions_";
            this.btLoadVectors.Text = "Сбросить все изменения       внесенные на данной странице";
            this.btLoadVectors.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btLoadVectors.UseVisualStyleBackColor = true;
            this.btLoadVectors.Click += new System.EventHandler(this.btLoadVectors_Click);
            // 
            // btOptimize1
            // 
            this.btOptimize1.Location = new System.Drawing.Point(255, 52);
            this.btOptimize1.Name = "btOptimize1";
            this.btOptimize1.Size = new System.Drawing.Size(101, 43);
            this.btOptimize1.TabIndex = 10;
            this.btOptimize1.Tag = "_skipIdleMotion_";
            this.btOptimize1.Text = "Сокращение холостого хода";
            this.btOptimize1.UseVisualStyleBackColor = true;
            this.btOptimize1.Click += new System.EventHandler(this.btOptimize1_Click);
            // 
            // btOptimize2
            // 
            this.btOptimize2.Location = new System.Drawing.Point(362, 52);
            this.btOptimize2.Name = "btOptimize2";
            this.btOptimize2.Size = new System.Drawing.Size(174, 43);
            this.btOptimize2.TabIndex = 11;
            this.btOptimize2.Tag = "_CombineNearbyPoint_";
            this.btOptimize2.Text = "Объеденить линии с одинаковыми координатами";
            this.btOptimize2.UseVisualStyleBackColor = true;
            this.btOptimize2.Click += new System.EventHandler(this.btOptimize2_Click);
            // 
            // labelTraectoryInfo
            // 
            this.labelTraectoryInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelTraectoryInfo.Location = new System.Drawing.Point(454, 101);
            this.labelTraectoryInfo.Name = "labelTraectoryInfo";
            this.labelTraectoryInfo.Size = new System.Drawing.Size(188, 64);
            this.labelTraectoryInfo.TabIndex = 12;
            this.labelTraectoryInfo.Text = "инфо по траектории";
            this.labelTraectoryInfo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(255, 101);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(126, 43);
            this.button1.TabIndex = 13;
            this.button1.Tag = "_simplification_";
            this.button1.Text = "Вариант упрощения траектории";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.DecimalPlaces = 2;
            this.numericUpDown1.Increment = new decimal(new int[] {
            25,
            0,
            0,
            131072});
            this.numericUpDown1.Location = new System.Drawing.Point(387, 114);
            this.numericUpDown1.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(61, 20);
            this.numericUpDown1.TabIndex = 14;
            this.numericUpDown1.Value = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.numericUpDown1.ValueChanged += new System.EventHandler(this.numericUpDown1_ValueChanged);
            // 
            // btCloseTraectory
            // 
            this.btCloseTraectory.Location = new System.Drawing.Point(542, 52);
            this.btCloseTraectory.Name = "btCloseTraectory";
            this.btCloseTraectory.Size = new System.Drawing.Size(100, 43);
            this.btCloseTraectory.TabIndex = 15;
            this.btCloseTraectory.Tag = "_CloseLoop_";
            this.btCloseTraectory.Text = "Замкнуть траектории";
            this.btCloseTraectory.UseVisualStyleBackColor = true;
            this.btCloseTraectory.Click += new System.EventHandler(this.btCloseTraectory_Click);
            // 
            // ForGena
            // 
            this.ForGena.Controls.Add(this.label1);
            this.ForGena.Controls.Add(this.numericUpDown2);
            this.ForGena.Controls.Add(this.numericGena);
            this.ForGena.Controls.Add(this.checkBoxGena);
            this.ForGena.Controls.Add(this.button2);
            this.ForGena.Location = new System.Drawing.Point(255, 168);
            this.ForGena.Name = "ForGena";
            this.ForGena.Size = new System.Drawing.Size(387, 63);
            this.ForGena.TabIndex = 16;
            this.ForGena.TabStop = false;
            this.ForGena.Text = "Для Гены";
            this.ForGena.Visible = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(284, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(96, 13);
            this.label1.TabIndex = 17;
            this.label1.Text = "количество точек";
            // 
            // numericUpDown2
            // 
            this.numericUpDown2.Location = new System.Drawing.Point(309, 30);
            this.numericUpDown2.Minimum = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.numericUpDown2.Name = "numericUpDown2";
            this.numericUpDown2.Size = new System.Drawing.Size(61, 20);
            this.numericUpDown2.TabIndex = 16;
            this.numericUpDown2.Value = new decimal(new int[] {
            6,
            0,
            0,
            0});
            // 
            // numericGena
            // 
            this.numericGena.DecimalPlaces = 2;
            this.numericGena.Increment = new decimal(new int[] {
            25,
            0,
            0,
            131072});
            this.numericGena.Location = new System.Drawing.Point(179, 37);
            this.numericGena.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numericGena.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.numericGena.Name = "numericGena";
            this.numericGena.Size = new System.Drawing.Size(61, 20);
            this.numericGena.TabIndex = 15;
            this.numericGena.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // checkBoxGena
            // 
            this.checkBoxGena.AutoSize = true;
            this.checkBoxGena.Location = new System.Drawing.Point(166, 19);
            this.checkBoxGena.Name = "checkBoxGena";
            this.checkBoxGena.Size = new System.Drawing.Size(85, 17);
            this.checkBoxGena.TabIndex = 1;
            this.checkBoxGena.Text = "исп. радиус";
            this.checkBoxGena.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(6, 19);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(154, 38);
            this.button2.TabIndex = 0;
            this.button2.Text = "заменить все траектории на окружность";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // buttonSelectedUp
            // 
            this.buttonSelectedUp.Location = new System.Drawing.Point(3, 45);
            this.buttonSelectedUp.Name = "buttonSelectedUp";
            this.buttonSelectedUp.Size = new System.Drawing.Size(22, 49);
            this.buttonSelectedUp.TabIndex = 17;
            this.buttonSelectedUp.Text = "˄";
            this.buttonSelectedUp.UseVisualStyleBackColor = true;
            this.buttonSelectedUp.Click += new System.EventHandler(this.buttonSelectedUp_Click);
            // 
            // buttonSelectedDown
            // 
            this.buttonSelectedDown.Location = new System.Drawing.Point(3, 114);
            this.buttonSelectedDown.Name = "buttonSelectedDown";
            this.buttonSelectedDown.Size = new System.Drawing.Size(22, 49);
            this.buttonSelectedDown.TabIndex = 18;
            this.buttonSelectedDown.Text = "˅";
            this.buttonSelectedDown.UseVisualStyleBackColor = true;
            this.buttonSelectedDown.Click += new System.EventHandler(this.buttonSelectedDown_Click);
            // 
            // page06_VectorEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.buttonSelectedDown);
            this.Controls.Add(this.buttonSelectedUp);
            this.Controls.Add(this.ForGena);
            this.Controls.Add(this.btCloseTraectory);
            this.Controls.Add(this.numericUpDown1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.labelTraectoryInfo);
            this.Controls.Add(this.btOptimize2);
            this.Controls.Add(this.btOptimize1);
            this.Controls.Add(this.btLoadVectors);
            this.Controls.Add(this.btDelVector);
            this.Controls.Add(this.treeViewVectors);
            this.Name = "page06_VectorEdit";
            this.Size = new System.Drawing.Size(645, 240);
            this.Tag = "_page06_";
            this.Load += new System.EventHandler(this.page06_VectorEdit_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.ForGena.ResumeLayout(false);
            this.ForGena.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericGena)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView treeViewVectors;
        private System.Windows.Forms.Button btDelVector;
        private System.Windows.Forms.Button btLoadVectors;
        private System.Windows.Forms.Button btOptimize1;
        private System.Windows.Forms.Button btOptimize2;
        private System.Windows.Forms.Label labelTraectoryInfo;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.Button btCloseTraectory;
        private System.Windows.Forms.GroupBox ForGena;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.NumericUpDown numericGena;
        private System.Windows.Forms.CheckBox checkBoxGena;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown numericUpDown2;
        private System.Windows.Forms.Button buttonSelectedUp;
        private System.Windows.Forms.Button buttonSelectedDown;

        public int NextPage { get; set; }

        public Bitmap pageImageIN { get; set; }
        public Bitmap pageImageNOW { get; set; }
        public List<GroupPoint> pageVectorIN { get; set; }
        public List<GroupPoint> pageVectorNOW { get; set; }



    }
}
