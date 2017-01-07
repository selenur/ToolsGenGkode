// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using BinarizationThinning;

namespace ToolsGenGkode.pages
{
    public partial class page04_ImageToVector : UserControl, PageInterface
    {
        /// <summary>
        /// Событие при изменении параметров на данной форме
        /// </summary>
        public event EventHandler IsChange;

        private MainForm MAIN;

        /// <summary>
        /// Посылка события главной форме
        /// </summary>
        /// <param name="message"></param>
        void CreateEvent(string message)
        {
            //MyEventArgs e = new MyEventArgs();
            //e.ActionRun = message;

            //EventHandler handler = IsChange;
            //if (handler != null) IsChange?.Invoke(this, e);

            MAIN.PreviewImage(pageImageNOW);
            MAIN.PreviewVectors(pageVectorNOW);



        }

        public page04_ImageToVector(MainForm mf)
        {
            InitializeComponent();

            PageName = @"Получение контуров (4)";
            LastPage = 0;
            CurrPage = 4;
            NextPage = 6;

            MAIN = mf;

            pageImageNOW = null;
            pageVectorNOW = new List<GroupPoint>();

        }

        private void page03_ImageModification_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
                pageImageNOW = (Bitmap)pageImageIN.Clone();
                pageImageNOW = ImageProcessing.GetBlackWhileImage(pageImageNOW, (int)numericUpDownKoefPalitra.Value, cbNegative.Checked);

                pageVectorNOW = new List<GroupPoint>();
                CreateEvent("");
            Cursor.Current = Cursors.Default;
        }

        private void checkBoxUseFilter1_CheckedChanged(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
                pageImageNOW = (Bitmap)pageImageIN.Clone();
                pageImageNOW = ImageProcessing.GetBlackWhileImage(pageImageNOW, (int)numericUpDownKoefPalitra.Value, cbNegative.Checked);

                pageVectorNOW = new List<GroupPoint>();
                CreateEvent("");
            Cursor.Current = Cursors.Default;
        }

        private void numericUpDownKoefPalitra_ValueChanged(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
                pageImageNOW = (Bitmap)pageImageIN.Clone();
                pageImageNOW = ImageProcessing.GetBlackWhileImage(pageImageNOW, (int)numericUpDownKoefPalitra.Value, cbNegative.Checked);

                pageVectorNOW = new List<GroupPoint>();
                CreateEvent("");
            Cursor.Current = Cursors.Default;
        }




        private void button2_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

                pageImageNOW = (Bitmap)pageImageIN.Clone();
                pageImageNOW = ImageProcessing.GetBlackWhileImage(pageImageNOW, (int)numericUpDownKoefPalitra.Value, cbNegative.Checked);


                if (SkeletonizationFilter.Checked)
                {

                    int Threshold = 0;

                    Byte[,] m_SourceImage = Thining.ToBinaryArray(pageImageNOW, out Threshold);

                    Byte[,] m_DesImage = Thining.ThinPicture(m_SourceImage);

                    Bitmap bmpThin = Thining.BinaryArrayToBinaryBitmap(m_DesImage);

                    pageImageNOW = bmpThin;

                }

                pageImageNOW = ImageProcessing.BitmapDeleteContent(pageImageNOW);
                pageVectorNOW = VectorProcessing.GetVectorFromImage(pageImageNOW);

                CreateEvent("");

            Cursor.Current = Cursors.Default;

        }

        public Bitmap pageImageIN { get; set; }
        public Bitmap pageImageNOW { get; set; }
        public List<GroupPoint> pageVectorIN { get; set; }
        public List<GroupPoint> pageVectorNOW { get; set; }
        public List<cncPoint> PagePoints { get; set; }

        public void actionBefore()
        {
            //throw new NotImplementedException();
            if (LastPage == 2)
            {
                label6.Visible = false;
                textBoxFileName.Visible = false;
                buttonSelectFile.Visible = false;


            }
            else
            {
                label6.Visible = true;
                textBoxFileName.Visible = true;
                buttonSelectFile.Visible = true;
            }
        }

        public void actionAfter()
        {
            if (pageImageIN == null) return;

            Cursor.Current = Cursors.WaitCursor;

                pageImageNOW = (Bitmap)pageImageIN.Clone();
                pageImageNOW = ImageProcessing.GetBlackWhileImage(pageImageNOW, (int)numericUpDownKoefPalitra.Value, cbNegative.Checked);

                if (SkeletonizationFilter.Checked)
                {
                    pageImageNOW = ImageProcessing.Skeletonization(pageImageNOW);
                }

                pageImageNOW = ImageProcessing.BitmapDeleteContent(pageImageNOW);
                pageVectorNOW = VectorProcessing.GetVectorFromImage(pageImageNOW);
            Cursor.Current = Cursors.Default;
        }

        private void buttonSelectFile_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.CheckFileExists = true;
            openFileDialog1.Multiselect = false;
            openFileDialog1.Title = @"Выбор рисунка";
            //openFileDialog1.InitialDirectory = "c:\\";
            openFileDialog1.Filter = @"Файлы рисунков(*.jpg; *.jpeg; *.gif; *.bmp; *.png)|*.jpg; *.jpeg; *.gif; *.bmp; *.png";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textBoxFileName.Text = openFileDialog1.FileName;
            }


            if (!File.Exists(textBoxFileName.Text)) return;

            Bitmap tmp = new Bitmap(textBoxFileName.Text);

            //TODO: Вот тут обратить внимание!!!!! на ориентацию оей


            // параметры расположения координатной оси
            if (Properties.Settings.Default.page01AxisVariant == 2) tmp.RotateFlip(RotateFlipType.RotateNoneFlipY);

            pageImageIN = tmp;
            pageImageNOW = (Bitmap)pageImageIN.Clone();
            pageVectorNOW = new List<GroupPoint>();


            CreateEvent("");
            Cursor.Current = Cursors.Default;
        }

        private void btShowOriginalImage_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            pageImageNOW = (Bitmap)pageImageIN.Clone();
            pageVectorNOW = new List<GroupPoint>();
            CreateEvent("");

            Cursor.Current = Cursors.Default;
        }

        private void SkeletonizationFilter_CheckedChanged(object sender, EventArgs e)
        {

        }
    }



}

