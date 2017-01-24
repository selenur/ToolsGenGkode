// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using AForge.Imaging.Filters;
using ToolsGenGkode.Properties;

namespace ToolsGenGkode.pages
{
    // ReSharper disable once InconsistentNaming
    public partial class page09_ImageRast : UserControl, PageInterface
    {
        private MainForm MAIN;

        public page09_ImageRast(MainForm mf)
        {
            InitializeComponent();

            MAIN = mf;

            pageImageIN = null;
            pageImageNOW = null;
            pageVectorIN = new List<GroupPoint>();
            pageVectorNOW = new List<GroupPoint>();

            NextPage = 10;

            useFilter.Items.Clear();
            useFilter.Items.Add("01 - метод Флойда-Стеинберга (<распыление>)"); //FloydSteinberg Dithering
            useFilter.Items.Add("02 - метод Байера (<матрица>)");              //Bayer Dithering
            useFilter.Items.Add("03 - Получение оттенков серого (bright)");

            try
            {
                cbKeepAspectRatio.Checked = Settings.Default.page09UserUseRation;
                numSizePoint.Value = Settings.Default.page09SizePoint;
                LaserTimeOut.Value = Settings.Default.page09LaserTimeOut;
                numericUpDownPercent.Value = Settings.Default.page09PercentValue;

                _changeIsUser = false;
                numXAfter.Value = Settings.Default.page09sizeDestX;
                numYAfter.Value = Settings.Default.page09sizeDestY;
                _changeIsUser = true;
            }
            catch (Exception ex)
            {
                cbKeepAspectRatio.Checked = true;
                numSizePoint.Value = 1;
                LaserTimeOut.Value = 1000;
                numericUpDownPercent.Value = 100;

                _changeIsUser = false;
                numXAfter.Value = 100;
                numYAfter.Value = 100;
                _changeIsUser = true;
            }

            switch (Settings.Default.page09VariantSize)
            {
                case 1:
                    radioButtonDiametrSizePoint.Checked = true;
                    break;

                case 2:
                    radioButtonSizePoint.Checked = true;
                    break;

                case 3:
                    radioButtonPerent.Checked = true;
                    break;

                case 4:
                    radioButtonUserSize.Checked = true;
                    break;
                default:
                    radioButtonSizePoint.Checked = true;
                    break;
            }
        }

        private void page09_SelectImage_Load(object sender, EventArgs e)
        {

        }

        void UserActions()
        {
            MAIN.PreviewDada(pageImageNOW, pageVectorNOW);
        }

        public void actionBefore()
        {
            MAIN.PageName.Text = @"Подготовка изображения растра (9)";
            MAIN.PageName.Tag = Tag;

            pageVectorNOW = new List<GroupPoint>();
            if (pageImageIN == null) return;

            pageImageNOW = (Bitmap)pageImageIN.Clone(); 

            GetInfoSize();
            
            UserActions();

            RecalculateSize();
        }

        public void actionAfter()
        {

        }

        private void GetInfoSize()
        {
            if (pageImageNOW == null) return;

            numXbefore.Value = pageImageNOW.Width;
            numYbefore.Value = pageImageNOW.Height;
        }

        private void preparationImage()
        {
            //decimal newSizeX = numXAfter.Value / numSizePoint.Value;
            //decimal newSizeY = numYAfter.Value / numSizePoint.Value;

            //if (newSizeX < 1 || newSizeY < 1)
            //{
            //    MessageBox.Show(@"Не указан желаемый размер, вычисление невозможно!");

            //    return;
            //}

            pageImageNOW = (Bitmap)pageImageIN.Clone();
            pageVectorNOW = new List<GroupPoint>();
            Bitmap newBitmap = ImageProcessing.ConvertToGrayScale(pageImageNOW);

            int Xsize = (int)(numXAfter.Value / numSizePoint.Value);
            int Ysize = (int)(numYAfter.Value / numSizePoint.Value);

            newBitmap = ImageProcessing.ResizeImage(newBitmap,Xsize,  Ysize);

            if (useFilter.Text.StartsWith("01")) //FloydSteinbergDithering"
            {
                newBitmap = ImageProcessing.ConvertTo8Bit(newBitmap);

                FloydSteinbergDithering filter = new FloydSteinbergDithering();
                filter.ApplyInPlace(newBitmap);
            }

            if (useFilter.Text.StartsWith("02")) //@"BayerDithering"
            {
                newBitmap = ImageProcessing.ConvertTo8Bit(newBitmap);

                BayerDithering filter = new BayerDithering();
                filter.ApplyInPlace(newBitmap);
            }

            if (useFilter.Text.StartsWith("03")) //@"получение оттенков серого"
            {
                //получим новое перемасштабированное изображение
                //тут с изображением уже ничего не делаем
            }

            pageImageNOW = newBitmap;

        }


        private void GenVar1And2()
        {
            double sizeOnePoint = (double)numSizePoint.Value;

            //GlobalFunctions.LaserTimeOut = (int)LaserTimeOut.Value;

            pageVectorNOW = new List<GroupPoint>();

            Bitmap bb = pageImageNOW;
            BitmapData data = bb.LockBits(new Rectangle(0, 0, bb.Width, bb.Height), ImageLockMode.ReadOnly, bb.PixelFormat);  // make sure you check the pixel format as you will be looking directly at memory

            //направление движения
            DirrectionGroupPoint dir = DirrectionGroupPoint.Right;

            unsafe
            {
                byte* ptrSrc = (byte*)data.Scan0;

                int diff = data.Stride - data.Width;

                // example assumes 24bpp image.  You need to verify your pixel depth
                for (int y = 0; y < data.Height; ++y)
                {
                    List<cncPoint> tmp = new List<cncPoint>();

                    //во временный массив поместим линию с точками
                    for (int x = 0; x < data.Width; ++x)
                    {
                        // windows stores images in BGR pixel order
                        byte r = ptrSrc[0]; //тут получили нужный цвет

                        if (r == 0) tmp.Add(new cncPoint((x * sizeOnePoint) + (double)deltaX.Value, (y * sizeOnePoint) + (double)deltaY.Value, 0, 0, (int)LaserTimeOut.Value));

                        ptrSrc += 1;
                    }

                    // а теперь временный массив скопируем в основной, но с определенным направлением
                    if (dir == DirrectionGroupPoint.Right)
                    {
                        dir = DirrectionGroupPoint.Left;
                    }
                    else
                    {
                        tmp.Reverse();
                        dir = DirrectionGroupPoint.Right;
                    }

                    if (tmp.Count != 0) pageVectorNOW.Add(new GroupPoint(tmp, false, dir, true));

                    // ReSharper disable once RedundantAssignment
                    tmp = new List<cncPoint>();

                    ptrSrc += diff;
                }
            }

            bb.UnlockBits(data);

            // тут нужно сделать преворот согластно текущй ориентации осей
            pageVectorNOW = VectorProcessing.Rotate(pageVectorNOW);

        }


        // тут зигзагом будем идти по файлу, и получать цвет
        private void GenVar3()
        {
            // работа заточена только под 24-х битный пиксель
            if (pageImageNOW.PixelFormat != PixelFormat.Format24bppRgb)
            {
                //throw new UnsupportedImageFormatException("Оппа! Не поддерживаемый формат изображения!!!");
                MessageBox.Show(
                    "Для генерации данных, требуется изображение имеющее 24 бита на пиксель, а получилось иначе, очень подозрительно....!!");
                return;
            }

            BitmapData bitmapData1 = pageImageNOW.LockBits(new Rectangle(0, 0, pageImageNOW.Width, pageImageNOW.Height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);

            //значение текущего цвета
            int lastColor = -1; // прошлый цвет
            int currColor = -1; // текущий цвет

            pageVectorNOW = new List<GroupPoint>();

            

            double sizeOnePoint = (double)numSizePoint.Value;

            bool isFirst = true;
            bool isLast = false;

            unsafe
            {
                byte* imagePointer1 = (byte*)bitmapData1.Scan0;
                for (int i = 0; i < bitmapData1.Height; i++)
                {
                    //TODO: Доделать............
                    isFirst = true;
                    isLast = false;

                    GroupPoint tmpGroup = new GroupPoint();

                    for (int j = 0; j < bitmapData1.Width; j++)
                    {
                        // дошли до последней строки в текущей линии
                        if (j == (bitmapData1.Width - 1)) isLast = true;

                        // получим текущий цвет
                        currColor = (imagePointer1[0] + imagePointer1[1] + imagePointer1[2]) / 3;

                        if (isFirst || isLast)
                        {
                           lastColor = currColor;
                           tmpGroup.Points.Add(new cncPoint((j * sizeOnePoint) + (double)deltaX.Value, (i * sizeOnePoint) + (double)deltaY.Value, 0, 0, 0, false, currColor));

                            if (isLast) pageVectorNOW.Add(tmpGroup.Clone());

                            isFirst = false;
                        }
                        else
                        {
                            if (lastColor != currColor)
                            {
                                tmpGroup.Points.Add(new cncPoint((j * sizeOnePoint) + (double)deltaX.Value, (i * sizeOnePoint) + (double)deltaY.Value, 0, 0, 0, false, lastColor));
                                lastColor = currColor;
                                tmpGroup.Points.Add(new cncPoint((j * sizeOnePoint) + (double)deltaX.Value, (i * sizeOnePoint) + (double)deltaY.Value, 0,0,0,false,currColor));


                            }
                        }

                        imagePointer1 += 3;
                    }//end for j
                    imagePointer1 += bitmapData1.Stride - (bitmapData1.Width * 3);

                }//end for i
            }//end unsafe
            pageImageNOW.UnlockBits(bitmapData1);

            //теперь у нечентных линий нужно сменить направление
            bool needRevers = false;

            foreach (GroupPoint gpGroupPoint in pageVectorNOW)
            {
                if (needRevers) gpGroupPoint.Points.Reverse();

                needRevers = !needRevers;
            }

            pageVectorNOW.Reverse();


            ////******************************

            ////направление движения
            ////DirrectionGroupPoint dir = DirrectionGroupPoint.Right;

            //        //if (dir == DirrectionGroupPoint.Left) dir = DirrectionGroupPoint.Right;
            //        //else dir = DirrectionGroupPoint.Left;


            //Bitmap bb = pageImageNOW;
            //BitmapData data = bb.LockBits(new Rectangle(0, 0, bb.Width, bb.Height), ImageLockMode.ReadOnly, bb.PixelFormat);  // make sure you check the pixel format as you will be looking directly at memory

            ////направление движения
            ////DirrectionGroupPoint dir = DirrectionGroupPoint.Right;

            //unsafe
            //{
            //    byte* ptrSrc = (byte*)data.Scan0;

            //    int diff = data.Stride - data.Width;

            //    pageVectorNOW = new List<GroupPoint>();

            //    for (int y = 0; y < data.Height; ++y) //проход по линии
            //    {
            //        List<cncPoint> tmp = new List<cncPoint>();

            //        byte lastValueColor = 0;
            //        bool firstPoint = true;
            //        bool lastPoint = false;


            //        for (int x = 0; x < data.Width; ++x)//проход по точкам
            //        {
            //            lastPoint = (x == data.Width - 1); //будем знать последняя ли это точка линии по которой идем

            //            // windows stores images in BGR pixel order
            //            byte r = ptrSrc[0]; //тут получили нужный цвет

            //            if (firstPoint || lastPoint) //первую и последнюю точку добавим в любом случае
            //            {
            //                firstPoint = false;

            //                cncPoint lk = new cncPoint((x * sizeOnePoint) + (double)deltaX.Value, (y * sizeOnePoint) + (double)deltaY.Value,0,0,0,false,(int)r);

            //                tmp.Add(lk);

            //                lastValueColor = r;
            //            }
            //            else
            //            {
            //                if (lastValueColor != r)
            //                {
            //                    cncPoint lk = new cncPoint((x * sizeOnePoint) + (double)deltaX.Value, (y * sizeOnePoint) + (double)deltaY.Value, 0, 0, 0, false, (int)r);

            //                    tmp.Add(lk);

            //                    lastValueColor = r;
            //                }
            //            }
            //            ptrSrc += 1;
            //        }


            //        // а теперь временный массив скопируем в основной, но с определенным направлением
            //        if (dir == DirrectionGroupPoint.Right)
            //        {
            //            dir = DirrectionGroupPoint.Left;
            //        }
            //        else
            //        {
            //            tmp.Reverse();
            //            dir = DirrectionGroupPoint.Right;
            //        }

            //        pageVectorNOW.Add(new GroupPoint(tmp, false, dir, true));

            //        // ReSharper disable once RedundantAssignment
            //        tmp = new List<cncPoint>();

            //        ptrSrc += diff;
            //    }
            //}

            //bb.UnlockBits(data);

            //// тут нужно сделать преворот согластно текущй ориентации осей
            pageVectorNOW = VectorProcessing.Rotate(pageVectorNOW);
        }


        private void GenerateData()
        {
            if (useFilter.Text.StartsWith("01") || useFilter.Text.StartsWith("02"))
            {
                GenVar1And2();
            }

            if (useFilter.Text.StartsWith("03"))
            {
                GenVar3();
            }
        }


        // preview
        private void PreviewButton_Click(object sender, EventArgs e)
        {
            if (useFilter.Text.Trim().Length == 0) return;

            Cursor.Current = Cursors.WaitCursor;

            preparationImage();

            Cursor.Current = Cursors.Default;

            UserActions();
            //CreateEvent("RefreshImage_09");
        }

        // generate data
        private void btCalcTraectory_Click(object sender, EventArgs e)
        {
            if (useFilter.Text.Trim().Length == 0) return;

            Cursor.Current = Cursors.WaitCursor;

            preparationImage();
            GenerateData();

            UserActions();


            Cursor.Current = Cursors.Default;

            //UserActions();
            //CreateEvent("RefreshImage_09");
        }




        /// <summary>
        /// Для определения того кто меняет значение, пользователь, или код программы
        /// </summary>
        private bool _changeIsUser = true;

        private void numXAfter_ValueChanged(object sender, EventArgs e)
        {
            if (!_changeIsUser) return;

            if (cbKeepAspectRatio.Checked)
            {
                decimal delta = (numYbefore.Value / numXbefore.Value) * numXAfter.Value;

                _changeIsUser = false;
                numYAfter.Value = delta;
                _changeIsUser = true;
            }

            Settings.Default.page09sizeDestX = numXAfter.Value;
            Settings.Default.page09sizeDestY = numYAfter.Value;
            Settings.Default.Save();
        }

        private void numYAfter_ValueChanged(object sender, EventArgs e)
        {
            if (!_changeIsUser) return;

            if (cbKeepAspectRatio.Checked)
            {
                decimal delta = (numXbefore.Value / numYbefore.Value) * numYAfter.Value;
                _changeIsUser = false;
                numXAfter.Value = delta;
                _changeIsUser = true;
            }

            Settings.Default.page09sizeDestX = numXAfter.Value;
            Settings.Default.page09sizeDestY = numYAfter.Value;
            Settings.Default.Save();
        }


        private void LaserTimeOut_ValueChanged(object sender, EventArgs e)
        {
            Settings.Default.page09LaserTimeOut = LaserTimeOut.Value;
            Settings.Default.Save();
        }

        private void useFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            //тут появится доступность некоторых из элементов формы
        }

        private void RecalculateSize()
        {

            _changeIsUser = false;

            if (radioButtonDiametrSizePoint.Checked)
            {
                numXAfter.Value = numSizePoint.Value * numXbefore.Value;
                numYAfter.Value = numSizePoint.Value * numYbefore.Value;
            }

            if (radioButtonSizePoint.Checked)
            {
                numXAfter.Value = numXbefore.Value;
                numYAfter.Value = numYbefore.Value;
            }

            if (radioButtonPerent.Checked)
            {
                numXAfter.Value = numXbefore.Value * (numericUpDownPercent.Value / 100);
                numYAfter.Value = numYbefore.Value * (numericUpDownPercent.Value / 100);
            }

            if (radioButtonUserSize.Checked)
            {
                if (cbKeepAspectRatio.Checked)
                {
                    decimal delta = (numYbefore.Value / numXbefore.Value) * numXAfter.Value;

                    numYAfter.Value = delta;

                }
                else
                {
                    //оставляем значачения как есть
                }
            }

            _changeIsUser = true;
        
        }

        private void cbKeepAspectRatio_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.page09UserUseRation = cbKeepAspectRatio.Checked;
            Settings.Default.Save();
        }

        private void numSizePoint_ValueChanged(object sender, EventArgs e)
        {
            Settings.Default.page09SizePoint = numSizePoint.Value;
            Settings.Default.Save();
            RecalculateSize();
        }

        private void radioButtonDiametrSizePoint_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.page09VariantSize = 1;
            Settings.Default.Save();
            RecalculateSize();
        }

        private void radioButtonSizePoint_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.page09VariantSize = 2;
            Settings.Default.Save();
            RecalculateSize();
        }

        private void radioButtonPerent_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.page09VariantSize = 3;
            Settings.Default.Save();
            RecalculateSize();
        }

        private void numericUpDownPercent_ValueChanged(object sender, EventArgs e)
        {
            Settings.Default.page09PercentValue = numericUpDownPercent.Value;
            Settings.Default.Save();
            RecalculateSize();
        }

        private void radioButtonUserSize_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.page09VariantSize = 4;
            Settings.Default.page09UserUseRation = cbKeepAspectRatio.Checked;
            Settings.Default.Save();
            RecalculateSize();
        }

        private void buttonHelpInfo_Click(object sender, EventArgs e)
        {
            string textInfo = "<html><body>Информация о выбранном фильтре<hr/>фильтр не выбран</body></html>";

            helpText hlp = new helpText();
            



            if (useFilter.Text.StartsWith("01")) //FloydSteinbergDithering"
            {
                textInfo = "<html><body>Информация о выбранном фильтре<hr/>Данный фильтр формирует всего один набор точек, которые и представляют собой рисунок, где точки сконцентрированны в большем количестве, в темных участках изображения, и в меньшем количестве, на более светлых участках.</body></html>";

            }

            if (useFilter.Text.StartsWith("02")) //@"BayerDithering"
            {
                textInfo = "<html><body>Информация о выбранном фильтре<hr/>Данный фильтр формирует всего один набор точек, которые и представляют собой рисунок, где точки сконцентрированны в большем количестве, в темных участках изображения, и в меньшем количестве, на более светлых участках.</body></html>";

            }


            if (useFilter.Text.StartsWith("03")) //@"получение оттенков серого"
            {
                textInfo = "<html><body>Информация о выбранном фильтре<hr/>Даный фильтр получает наборы точек, где набор представляет собой последовательность точек с направлением в левую или правую сторону, у каждой точки доступен параметр 'bright' содержащий значение от 0 до 255, значение 0 означает что это черный цвет, 255 - белый. </body></html>";


            }


            hlp.webBrowser1.DocumentText = textInfo;

            hlp.ShowDialog();
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }
    }
}
