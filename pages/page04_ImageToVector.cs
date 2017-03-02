// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using ToolsGenGkode.Properties;

namespace ToolsGenGkode.pages
{
    public partial class page04_ImageToVector : UserControl, PageInterface
    {
        private MainForm MAIN;

        private int ShowStep = -1; //для возможности просмотра разных стадий
        /* step 1 = показ исходного изображения
         * step 2 = инверсия цветов + преобразование по коэффициенту
         * step 3 = итоговый результат
         */

        private bool LastPage2 = false;



        public page04_ImageToVector(MainForm mf)
        {
            InitializeComponent();

            MAIN = mf;

            pageImageIN = null;
            pageImageNOW = null;
            pageVectorIN = new List<GroupPoint>();
            pageVectorNOW = new List<GroupPoint>();

            NextPage = 6;
        }

        private void page03_ImageModification_Load(object sender, EventArgs e)
        {

        }

        public void actionBefore()
        {
            MAIN.PageName.Text = @"Получение контуров (4)";
            MAIN.PageName.Tag = Tag;

            LastPage2 = (((Control) MAIN.CurrentPage.Previous.Value).Tag.ToString() == @"_page02_");

            //если предыдущая страница ввод текста, то выбор имени файла не нужно
            if (LastPage2)
            {
                label6.Enabled = false;
                textBoxFileName.Enabled = false;
                buttonSelectFile.Enabled = false;
            }
            else
            {
                label6.Enabled = true;
                textBoxFileName.Enabled = true;
                buttonSelectFile.Enabled = true;
            }

            ShowStep = 1;
            UserActions();
        }

        public void actionAfter()
        {
            ShowStep = 3;
            UserActions();
        }

        public bool IsReady()
        {
            return true;
        }

        private void UserActions()
        {
            //if (pageImageIN == null) return;

            Cursor.Current = Cursors.WaitCursor;

            pageVectorNOW = new List<GroupPoint>();

            if (ShowStep > 0)
            {
                if (LastPage2)
                {
                    pageImageNOW = ImageProcessing.CheckAndConvertImageto24bitPerPixel((Bitmap)pageImageIN.Clone());
                }
                else
                {
                    if (File.Exists(textBoxFileName.Text))
                    {
                        Bitmap tmp = ImageProcessing.CheckAndConvertImageto24bitPerPixel(new Bitmap(textBoxFileName.Text));

                        //TODO: Вот тут обратить внимание!!!!! на ориентацию оей

                        // параметры расположения координатной оси
                        if (Settings.Default.page01AxisVariant == 2) tmp.RotateFlip(RotateFlipType.RotateNoneFlipY);

                        pageImageIN = tmp;
                        pageImageNOW = (Bitmap)pageImageIN.Clone();
                        pageVectorNOW = new List<GroupPoint>();                       
                    }
                    else
                    {
                        pageImageNOW = null;
                    }
                }
            }

            if (ShowStep > 1)
            {
                pageImageNOW = ImageProcessing.GetBlackWhileImage(pageImageNOW, (int)numericUpDownKoefPalitra.Value, cbNegative.Checked);
            }

            if (ShowStep > 2)
            {
                if (SkeletonizationFilter.Checked)
                {
                    pageImageNOW = ImageProcessing.Skeletonization(pageImageNOW);
                }
                pageImageNOW = ImageProcessing.BitmapDeleteContent(pageImageNOW);
                pageVectorNOW = VectorProcessing.GetVectorFromImage(pageImageNOW);
            }

            Cursor.Current = Cursors.Default;

            MAIN.PreviewDada(pageImageNOW,pageVectorNOW);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ShowStep = 2;
            UserActions();
        }

        private void checkBoxUseFilter1_CheckedChanged(object sender, EventArgs e)
        {
            ShowStep = 2;
            UserActions();
        }

        private void numericUpDownKoefPalitra_ValueChanged(object sender, EventArgs e)
        {
            ShowStep = 2;
            UserActions();
        }

        private void GetResult_Click(object sender, EventArgs e)
        {
            ShowStep = 3;
            UserActions();

            //Cursor.Current = Cursors.WaitCursor;

            //pageImageNOW = (Bitmap)pageImageIN.Clone();
            //pageImageNOW = ImageProcessing.GetBlackWhileImage(pageImageNOW, (int)numericUpDownKoefPalitra.Value, cbNegative.Checked);

            //if (SkeletonizationFilter.Checked)
            //{

            //    int Threshold = 0;

            //    Byte[,] m_SourceImage = Thining.ToBinaryArray(pageImageNOW, out Threshold);

            //    Byte[,] m_DesImage = Thining.ThinPicture(m_SourceImage);

            //    Bitmap bmpThin = Thining.BinaryArrayToBinaryBitmap(m_DesImage);

            //    pageImageNOW = bmpThin;

            //}

            //pageImageNOW = ImageProcessing.BitmapDeleteContent(pageImageNOW);
            //pageVectorNOW = VectorProcessing.GetVectorFromImage(pageImageNOW);

            //Cursor.Current = Cursors.Default;
        }

        private void buttonSelectFile_Click(object sender, EventArgs e)
        {
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

            ShowStep = 1;
            UserActions();
        }

        private void btShowOriginalImage_Click(object sender, EventArgs e)
        {
            ShowStep = 1;
            UserActions();
        }

        private void SkeletonizationFilter_CheckedChanged(object sender, EventArgs e)
        {
            ShowStep = 3;
        }
    }



}

