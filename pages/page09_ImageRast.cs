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
        }

        private void page09_SelectImage_Load(object sender, EventArgs e)
        {
            decimal p1 = 1;
            decimal p2 = 1000;
            decimal p3 = 100;
            decimal p4 = 1;
            decimal p5 = 1;

            string sp1 = IniParser.GetSetting("page09", "SizePoint");
            string sp2 = IniParser.GetSetting("page09", "LaserTimeOut");
            string sp3 = IniParser.GetSetting("page09", "PercentValue");
            string sp4 = IniParser.GetSetting("page09", "sizeDestX");
            string sp5 = IniParser.GetSetting("page09", "sizeDestY");

            if (sp1 != null) decimal.TryParse(sp1, out p1);
            if (sp2 != null) decimal.TryParse(sp2, out p2);
            if (sp3 != null) decimal.TryParse(sp3, out p3);
            if (sp4 != null) decimal.TryParse(sp4, out p4);
            if (sp5 != null) decimal.TryParse(sp5, out p5);

            if (p1 == 0) p1 = 1; 

            numSizePoint.Value = p1;
            LaserTimeOut.Value = p2;
            numericUpDownPercent.Value = p3;

            _changeIsUser = false;
            numXAfter.Value = p4;
            numYAfter.Value = p5;
            _changeIsUser = true;





            string sp6 = IniParser.GetSetting("page09", "Variant");
            if (sp6 != null)
            {
                switch (sp6)
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

        private void button3_Click(object sender, EventArgs e)
        {
            if (useFilter.Text.Trim().Length == 0) return;

            pageImageNOW = (Bitmap)pageImageIN.Clone();
            pageVectorNOW = new List<Segment>();
            Bitmap newBitmap = ConvertToGrayScale(pageImageNOW);

            if (useFilter.Text == @"FloydSteinbergDithering")
            {
                FloydSteinbergDithering filter = new FloydSteinbergDithering();
                filter.ApplyInPlace(newBitmap);
            }

            if (useFilter.Text == @"BayerDithering")
            {
                BayerDithering filter = new BayerDithering();
                filter.ApplyInPlace(newBitmap);
            }

            pageImageNOW = newBitmap;

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

        private void btCalcTraectory_Click(object sender, EventArgs e)
        {
            decimal newSizeX = numXAfter.Value / numSizePoint.Value;
            decimal newSizeY = numYAfter.Value / numSizePoint.Value;

            if (newSizeX < 1 || newSizeY < 1)
            {
                MessageBox.Show(@"Не указан желаемый размер, вычисление невозможно!");

                return;
            }

            GlobalFunctions.LaserTimeOut = (int)LaserTimeOut.Value;
            //GlobalFunctions.IsLaserPoint = true;


            pageImageNOW = (Bitmap)pageImageIN.Clone();
            // create filter
            ResizeBilinear filter1 = new ResizeBilinear((int)newSizeX, (int)newSizeY);
            // apply the filter
            Bitmap newImage = filter1.Apply(pageImageNOW);

            Bitmap newBitmap = ConvertToGrayScale(newImage);

            if (useFilter.Text == @"FloydSteinbergDithering")
            {
                FloydSteinbergDithering filter2 = new FloydSteinbergDithering();
                filter2.ApplyInPlace(newBitmap);
            }

            if (useFilter.Text == @"BayerDithering")
            {
                BayerDithering filter3 = new BayerDithering();
                filter3.ApplyInPlace(newBitmap);
            }

            pageImageNOW = newBitmap;

            pageVectorNOW = new List<Segment>();

            Bitmap bb = newBitmap;
            BitmapData data = bb.LockBits(new Rectangle(0, 0, bb.Width, bb.Height), ImageLockMode.ReadOnly, bb.PixelFormat);  // make sure you check the pixel format as you will be looking directly at memory

            //направление движения
            DirrectionSegment dir = DirrectionSegment.RIGHT;


            unsafe
            {
                byte* ptrSrc = (byte*)data.Scan0;

                int diff = data.Stride - data.Width;

                // example assumes 24bpp image.  You need to verify your pixel depth
                // loop by row for better data locality
                for (int y = 0; y < data.Height; ++y)
                {
                    List <Location> tmp = new List<Location>();

                    //во временный массив поместим линию с точками
                    for (int x = 0; x < data.Width; ++x)
                    {
                        // windows stores images in BGR pixel order
                        byte r = ptrSrc[0];
                        if (r == 0) tmp.Add(new Location((x * numSizePoint.Value) + deltaX.Value, (y * numSizePoint.Value)+deltaY.Value,0,0,(int)LaserTimeOut.Value));
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

                    pageVectorNOW.Add(new Segment(tmp,false,false,dir,true));

                    // ReSharper disable once RedundantAssignment
                    tmp = new List<Location>();

                    //foreach (Location ppPoint in tmp)
                    //{
                    //    //PagePoints.Add(new Location(ppPoint));
                    //}

                    ptrSrc += diff;
                }
                //pageVectorNOW.Add(new Segment());
            }

            bb.UnlockBits(data);


            // тут нужно сделать преворот
            pageVectorNOW = VectorUtilities.Rotate(pageVectorNOW);

            CreateEvent("RefreshVector_09");
            CreateEvent("RefreshImage_09");
        }



        private void LaserTimeOut_ValueChanged(object sender, EventArgs e)
        {
            IniParser.AddSetting("page09", "LaserTimeOut", LaserTimeOut.Value.ToString(CultureInfo.InvariantCulture));
            IniParser.SaveSettings();
        }

        private void useFilter_SelectedIndexChanged(object sender, EventArgs e)
        {

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

            _changeIsUser = true;
        
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
            IniParser.AddSetting("page09", "Variant", "4");
            IniParser.SaveSettings();
            RecalculateSize();
        }
    }
}
