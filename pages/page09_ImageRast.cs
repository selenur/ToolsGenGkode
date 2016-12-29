// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.Windows.Forms;
using AForge.Imaging.Filters;

namespace ToolsGenGkode.pages
{
    // ReSharper disable once InconsistentNaming
    public partial class page09_ImageRast : UserControl, PageInterface
    {
        /// <summary>
        /// Событие при изменении параметров на данной форме
        /// </summary>
        public event EventHandler IsChange;

        void CreateEvent(string message)
        {
            MyEventArgs e = new MyEventArgs();
            e.ActionRun = message;

            EventHandler handler = IsChange;
            if (handler != null) IsChange?.Invoke(this, e);
        }

        public page09_ImageRast()
        {
            InitializeComponent();

            PageName = @"Подготовка изображения растра (9)";
            LastPage = 0;
            CurrPage = 9;
            NextPage = 10;

            pageImageNOW = null;
            pageVectorNOW = new List<Segment>();

            useFilter.Items.Clear();
            useFilter.Items.Add("01 - метод Флойда-Стеинберга (<распыление >)"); //FloydSteinberg Dithering
            useFilter.Items.Add("02 - метод Байера (<матрица>)");              //Bayer Dithering
            useFilter.Items.Add("03 - Получение оттенков серого (bright)");
        }

        private void page09_SelectImage_Load(object sender, EventArgs e)
        {
            decimal p1 = 1; //размер точки прожига
            decimal p2 = 1000; //длительность лазера
            decimal p3 = 100;  //процент от исходного размера
            decimal p4 = 1;     //размер по X 
            decimal p5 = 1;     //размер по Y

            string sp1 = IniParser.GetSetting("page09", "SizePoint");
            string sp2 = IniParser.GetSetting("page09", "LaserTimeOut");
            string sp3 = IniParser.GetSetting("page09", "PercentValue");
            string sp4 = IniParser.GetSetting("page09", "sizeDestX");
            string sp5 = IniParser.GetSetting("page09", "sizeDestY");
            string sp6 = IniParser.GetSetting("page09", "UserUseRation"); //флажок поддерживать пропорций
            string sp7 = IniParser.GetSetting("page09", "Variant"); //текущий выбранный вариант

            if (sp1 != null) decimal.TryParse(sp1, out p1);
            if (sp2 != null) decimal.TryParse(sp2, out p2);
            if (sp3 != null) decimal.TryParse(sp3, out p3);
            if (sp4 != null) decimal.TryParse(sp4, out p4);
            if (sp5 != null) decimal.TryParse(sp5, out p5);

            if (sp6 != null)
            {
                cbKeepAspectRatio.Checked = (sp6 == "1");
            }
            else
            {
                cbKeepAspectRatio.Checked = true;
            }

            if (p1 == 0) p1 = 1; 

            numSizePoint.Value = p1;
            LaserTimeOut.Value = p2;
            numericUpDownPercent.Value = p3;

            _changeIsUser = false;
                if (p4 == 0) p4 = 100;
                if (p5 == 0) p5 = 100;
                numXAfter.Value = p4;
                numYAfter.Value = p5;
            _changeIsUser = true;


            if (sp7 != null)
            {
                switch (sp7)
                {
                    case "1":
                        radioButtonDiametrSizePoint.Checked = true;
                        break;

                    case "2":
                        radioButtonSizePoint.Checked = true;
                        break;

                    case "3":
                        radioButtonPerent.Checked = true;
                        break;

                    case "4":
                        radioButtonUserSize.Checked = true;
                        break;
                }
            }

            RecalculateSize();
        }


        public string PageName { get; set; }
        public int LastPage { get; set; }
        public int CurrPage { get; set; }
        public int NextPage { get; set; }
        public Bitmap pageImageIN { get; set; }
        public Bitmap pageImageNOW { get; set; }
        public List<Segment> pageVectorIN { get; set; }
        public List<Segment> pageVectorNOW { get; set; }
        //public List<Location> PagePoints { get; set; }

        public void actionBefore()
        {
            pageVectorNOW = new List<Segment>();

            if (pageImageIN == null) return;

            pageImageNOW = (Bitmap)pageImageIN.Clone(); 

            GetInfoSize();

            CreateEvent("RefreshImage_09");

            RecalculateSize();
        }

        public void actionAfter()
        {
           // throw new NotImplementedException();
        }


        private void GetInfoSize()
        {
            if (pageImageNOW == null) return;

            numXbefore.Value = pageImageNOW.Width;
            numYbefore.Value = pageImageNOW.Height;
        }

        public static Bitmap ConvertToGrayScale(Bitmap me)
        {
            if (me == null)
                return null;

            // first convert to a grey scale image
            var filterGreyScale = new Grayscale(0.2125, 0.7154, 0.0721);

            me = filterGreyScale.Apply(me);
            return me;
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
            pageVectorNOW = new List<Segment>();
            Bitmap newBitmap = ConvertToGrayScale(pageImageNOW);

            int Xsize = (int)(numXAfter.Value / numSizePoint.Value);
            int Ysize = (int)(numYAfter.Value / numSizePoint.Value);

            ResizeBicubic filterResize = new ResizeBicubic(Xsize, Ysize);
            newBitmap = filterResize.Apply(newBitmap);



            if (useFilter.Text.StartsWith("01")) //FloydSteinbergDithering"
            {
                FloydSteinbergDithering filter = new FloydSteinbergDithering();
                filter.ApplyInPlace(newBitmap);
            }

            if (useFilter.Text.StartsWith("02")) //@"BayerDithering"
            {
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
            decimal sizeOnePoint = numSizePoint.Value;

            GlobalFunctions.LaserTimeOut = (int)LaserTimeOut.Value;

            pageVectorNOW = new List<Segment>();

            Bitmap bb = pageImageNOW;
            BitmapData data = bb.LockBits(new Rectangle(0, 0, bb.Width, bb.Height), ImageLockMode.ReadOnly, bb.PixelFormat);  // make sure you check the pixel format as you will be looking directly at memory

            //направление движения
            DirrectionSegment dir = DirrectionSegment.RIGHT;

            unsafe
            {
                byte* ptrSrc = (byte*)data.Scan0;

                int diff = data.Stride - data.Width;

                // example assumes 24bpp image.  You need to verify your pixel depth
                for (int y = 0; y < data.Height; ++y)
                {
                    List<Location> tmp = new List<Location>();

                    //во временный массив поместим линию с точками
                    for (int x = 0; x < data.Width; ++x)
                    {
                        // windows stores images in BGR pixel order
                        byte r = ptrSrc[0]; //тут получили нужный цвет

                        if (r == 0) tmp.Add(new Location((x * sizeOnePoint) + deltaX.Value, (y * sizeOnePoint) + deltaY.Value, 0, 0, (int)LaserTimeOut.Value));

                        ptrSrc += 1;
                    }

                    // а теперь временный массив скопируем в основной, но с определенным направлением
                    if (dir == DirrectionSegment.RIGHT)
                    {
                        dir = DirrectionSegment.LEFT;
                    }
                    else
                    {
                        tmp.Reverse();
                        dir = DirrectionSegment.RIGHT;
                    }

                    if (tmp.Count != 0) pageVectorNOW.Add(new Segment(tmp, false, false, dir, true));

                    // ReSharper disable once RedundantAssignment
                    tmp = new List<Location>();

                    ptrSrc += diff;
                }
            }

            bb.UnlockBits(data);

            // тут нужно сделать преворот согластно текущй ориентации осей
            pageVectorNOW = VectorUtilities.Rotate(pageVectorNOW);

        }

        private void GenVar3()
        {
            decimal sizeOnePoint = numSizePoint.Value;

            Bitmap bb = pageImageNOW;
            BitmapData data = bb.LockBits(new Rectangle(0, 0, bb.Width, bb.Height), ImageLockMode.ReadOnly, bb.PixelFormat);  // make sure you check the pixel format as you will be looking directly at memory

            //направление движения
            DirrectionSegment dir = DirrectionSegment.RIGHT;

            unsafe
            {
                byte* ptrSrc = (byte*)data.Scan0;

                int diff = data.Stride - data.Width;
                
                pageVectorNOW = new List<Segment>();

                for (int y = 0; y < data.Height; ++y) //проход по линии
                {
                    List<Location> tmp = new List<Location>();

                    byte lastValueColor = 0;
                    bool firstPoint = true;
                    bool lastPoint = false;


                    for (int x = 0; x < data.Width; ++x)//проход по точкам
                    {
                        lastPoint = (x == data.Width - 1); //будем знать последняя ли это точка линии по которой идем
                        
                        // windows stores images in BGR pixel order
                        byte r = ptrSrc[0]; //тут получили нужный цвет

                        if (firstPoint || lastPoint) //первую и последнюю точку добавим в любом случае
                        {
                            firstPoint = false;

                            Location lk = new Location((x * sizeOnePoint) + deltaX.Value, (y * sizeOnePoint) + deltaY.Value,0,0,0,false,false,(int)r);

                            tmp.Add(lk);
                            
                            lastValueColor = r;
                        }
                        else
                        {
                            if (lastValueColor != r)
                            {
                                Location lk = new Location((x * sizeOnePoint) + deltaX.Value, (y * sizeOnePoint) + deltaY.Value, 0, 0, 0, false, false, (int)r);

                                tmp.Add(lk);

                                lastValueColor = r;
                            }
                        }
                        ptrSrc += 1;
                    }


                    // а теперь временный массив скопируем в основной, но с определенным направлением
                    if (dir == DirrectionSegment.RIGHT)
                    {
                        dir = DirrectionSegment.LEFT;
                    }
                    else
                    {
                        tmp.Reverse();
                        dir = DirrectionSegment.RIGHT;
                    }

                    pageVectorNOW.Add(new Segment(tmp, false, false, dir, true));

                    // ReSharper disable once RedundantAssignment
                    tmp = new List<Location>();

                    ptrSrc += diff;
                }
            }

            bb.UnlockBits(data);

            // тут нужно сделать преворот согластно текущй ориентации осей
            pageVectorNOW = VectorUtilities.Rotate(pageVectorNOW);
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

            CreateEvent("RefreshVector_09");
            CreateEvent("RefreshImage_09");
        }

        // generate data
        private void btCalcTraectory_Click(object sender, EventArgs e)
        {
            if (useFilter.Text.Trim().Length == 0) return;

            Cursor.Current = Cursors.WaitCursor;

            preparationImage();

            GenerateData();

            Cursor.Current = Cursors.Default;

            CreateEvent("RefreshVector_09");
            CreateEvent("RefreshImage_09");
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

            IniParser.AddSetting("page09", "sizeDestX", numXAfter.Value.ToString(CultureInfo.InvariantCulture));
            IniParser.AddSetting("page09", "sizeDestY", numYAfter.Value.ToString(CultureInfo.InvariantCulture));

            IniParser.SaveSettings();
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

            IniParser.AddSetting("page09", "sizeDestX", numXAfter.Value.ToString(CultureInfo.InvariantCulture));
            IniParser.AddSetting("page09", "sizeDestY", numYAfter.Value.ToString(CultureInfo.InvariantCulture));
            IniParser.SaveSettings();
        }


        private void LaserTimeOut_ValueChanged(object sender, EventArgs e)
        {
            IniParser.AddSetting("page09", "LaserTimeOut", LaserTimeOut.Value.ToString(CultureInfo.InvariantCulture));
            IniParser.SaveSettings();
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
            if (cbKeepAspectRatio.Checked)
            {
                IniParser.AddSetting("page09", "UserUseRation", "1"); // сохранять-ли пропорции
            }
            else
            {
                IniParser.AddSetting("page09", "UserUseRation", "0"); // сохранять-ли пропорции
            }


            IniParser.SaveSettings();
        }

        private void numSizePoint_ValueChanged(object sender, EventArgs e)
        {
            IniParser.AddSetting("page09", "SizePoint", numSizePoint.Value.ToString(CultureInfo.InvariantCulture));
            IniParser.SaveSettings();

            RecalculateSize();
        }

        private void radioButtonDiametrSizePoint_CheckedChanged(object sender, EventArgs e)
        {
            IniParser.AddSetting("page09", "Variant", "1");
            IniParser.SaveSettings();

            RecalculateSize();
        }

        private void radioButtonSizePoint_CheckedChanged(object sender, EventArgs e)
        {
            IniParser.AddSetting("page09", "Variant", "2");
            IniParser.SaveSettings();
            RecalculateSize();
        }

        private void radioButtonPerent_CheckedChanged(object sender, EventArgs e)
        {
            IniParser.AddSetting("page09", "Variant", "3");
            IniParser.SaveSettings();
            RecalculateSize();
        }

        private void numericUpDownPercent_ValueChanged(object sender, EventArgs e)
        {
            IniParser.AddSetting("page09", "PercentValue", numericUpDownPercent.Value.ToString(CultureInfo.InvariantCulture));
            IniParser.SaveSettings();
            RecalculateSize();
        }

        private void radioButtonUserSize_CheckedChanged(object sender, EventArgs e)
        {
            IniParser.AddSetting("page09", "Variant", "4");       // пользовательский вариант

            if (cbKeepAspectRatio.Checked)
            {
                IniParser.AddSetting("page09", "UserUseRation", "1"); // сохранять-ли пропорции
            }
            else
            {
                IniParser.AddSetting("page09", "UserUseRation", "0"); // сохранять-ли пропорции
            }


            IniParser.SaveSettings();
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


    }
}
