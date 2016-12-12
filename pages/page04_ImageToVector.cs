// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace ToolsGenGkode.pages
{
    public partial class page04_ImageToVector : UserControl, PageInterface
    {
        /// <summary>
        /// Событие при изменении параметров на данной форме
        /// </summary>
        public event EventHandler IsChange;

        /// <summary>
        /// Посылка события главной форме
        /// </summary>
        /// <param name="message"></param>
        void CreateEvent(string message)
        {
            MyEventArgs e = new MyEventArgs();
            e.ActionRun = message;

            EventHandler handler = IsChange;
            if (handler != null) IsChange?.Invoke(this, e);
        }

        public page04_ImageToVector()
        {
            InitializeComponent();

            PageName = @"Получение контуров (4)";
            LastPage = 0;
            CurrPage = 4;
            NextPage = 6;

            pageImageNOW = null;
            pageVectorNOW = new List<Segment>();

        }

        private void page03_ImageModification_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            pageImageNOW = (Bitmap)pageImageIN.Clone();
            pageImageNOW = GlobalFunctions.GetBlackWhileImage(pageImageNOW, (int)numericUpDownKoefPalitra.Value, cbNegative.Checked);

            pageVectorNOW = new List<Segment>();
            CreateEvent("RefreshVector_04");
            CreateEvent("RefreshImage_04");
        }

        private void checkBoxUseFilter1_CheckedChanged(object sender, EventArgs e)
        {
            pageImageNOW = (Bitmap)pageImageIN.Clone();
            pageImageNOW = GlobalFunctions.GetBlackWhileImage(pageImageNOW, (int)numericUpDownKoefPalitra.Value, cbNegative.Checked);

            pageVectorNOW = new List<Segment>();
            CreateEvent("RefreshVector_04");
            CreateEvent("RefreshImage_04");
        }

        private void numericUpDownKoefPalitra_ValueChanged(object sender, EventArgs e)
        {
            pageImageNOW = (Bitmap)pageImageIN.Clone();
            pageImageNOW = GlobalFunctions.GetBlackWhileImage(pageImageNOW, (int)numericUpDownKoefPalitra.Value, cbNegative.Checked);

            pageVectorNOW = new List<Segment>();
            CreateEvent("RefreshVector_04");
            CreateEvent("RefreshImage_04");
        }



        private void button2_Click(object sender, EventArgs e)
        {
            pageImageNOW = (Bitmap)pageImageIN.Clone();
            pageImageNOW = GlobalFunctions.GetBlackWhileImage(pageImageNOW, (int)numericUpDownKoefPalitra.Value, cbNegative.Checked);

            pageImageNOW = BitmapDeleteContent(pageImageNOW);
            pageVectorNOW = GetVectorFromImage(pageImageNOW);

            CreateEvent("RefreshVector_04");
            CreateEvent("RefreshImage_04");

        }

        public Bitmap pageImageIN { get; set; }
        public Bitmap pageImageNOW { get; set; }
        public List<Segment> pageVectorIN { get; set; }
        public List<Segment> pageVectorNOW { get; set; }
        public List<Location> PagePoints { get; set; }

        public void actionBefore()
        {
            //throw new NotImplementedException();
        }

        public void actionAfter()
        {
            if (pageImageIN == null) return;

            pageImageNOW = (Bitmap)pageImageIN.Clone();
            pageImageNOW = GlobalFunctions.GetBlackWhileImage(pageImageNOW, (int)numericUpDownKoefPalitra.Value, cbNegative.Checked);

            pageImageNOW = BitmapDeleteContent(pageImageNOW);
            pageVectorNOW = GetVectorFromImage(pageImageNOW);

        }

   




        // удаление закрашенных областей
        private Bitmap BitmapDeleteContent(Bitmap tmpBmp)
        {
            // Set cursor as hourglass
            Cursor.Current = Cursors.WaitCursor;

            Bitmap bbb = tmpBmp.Clone(new Rectangle(0, 0, tmpBmp.Width, tmpBmp.Height), PixelFormat.Format24bppRgb); ;


             

            for (int pY = 0; pY < tmpBmp.Height; pY++)
            {
                for (int pX = 0; pX < tmpBmp.Width; pX++)
                {

                    Color pColor = GetPosColor(ref bbb, pX, pY);

                    if (pColor == Color.White) continue; //если это белая точка нет необходимости осматриваться


                    //а тут осмотримся:
                    Color pColorUp = GetPosColor(ref bbb, pX, pY - 1);
                    Color pColorDown = GetPosColor(ref bbb, pX, pY + 1);
                    Color pColorLeft = GetPosColor(ref bbb, pX - 1, pY);
                    Color pColorRight = GetPosColor(ref bbb, pX + 1, pY);

                    Color compareColor = Color.FromArgb(255, 255, 255, 255);

                    if (pColorUp != compareColor && pColorDown != compareColor && pColorLeft != compareColor && pColorRight != compareColor)
                    {
                        bbb.SetPixel(pX, pY, Color.FromArgb(245, 245, 245, 245));
                        //Set8bppIndexedPixel(ref tmpBmp, pX, pY, Color.FromArgb(245, 245, 245, 245));
                    }
                }
            }

            // Set cursor as default arrow
            Cursor.Current = Cursors.Default;

            return bbb;
        }

        private Color GetPosColor(ref Bitmap bmp, int x, int y)
        {
            Color cReturn = Color.FromArgb(255, 255, 255, 255);

            if (x < 0) return cReturn;

            if (y < 0) return cReturn;

            if (x >= bmp.Width) return cReturn;

            if (y >= bmp.Height) return cReturn;

            return bmp.GetPixel(x, y);
        }


        private PlacePoint[] GetAreaPoint(ref byte[] dataImage, int _x, int _y, BitmapData bitdata, int _startDirection)
        {
            PlacePoint[] ppp = new PlacePoint[8];
            int tmpDir = _startDirection;
            for (int i = 0; i < 8; i++)
            {
                ppp[i] = new PlacePoint(tmpDir, false);

                if (tmpDir == 1) //-x,-y
                {
                    ppp[i].DeltaX = -1;
                    ppp[i].DeltaY = -1;
                }

                if (tmpDir == 2)//x,-y
                {
                    ppp[i].DeltaX = 0;
                    ppp[i].DeltaY = -1;
                }

                if (tmpDir == 3)//+x,-y
                {
                    ppp[i].DeltaX = +1;
                    ppp[i].DeltaY = -1;
                }

                if (tmpDir == 4)//+x,y
                {
                    ppp[i].DeltaX = +1;
                    ppp[i].DeltaY = 0;
                }

                if (tmpDir == 5)//+x,+y
                {
                    ppp[i].DeltaX = +1;
                    ppp[i].DeltaY = +1;
                }

                if (tmpDir == 6)//x,+y
                {
                    ppp[i].DeltaX = 0;
                    ppp[i].DeltaY = +1;
                }

                if (tmpDir == 7)//-x,+y
                {
                    ppp[i].DeltaX = -1;
                    ppp[i].DeltaY = +1;
                }

                if (tmpDir == 8)//-x,y
                {
                    ppp[i].DeltaX = -1;
                    ppp[i].DeltaY = 0;
                }

                tmpDir++;

                if (tmpDir == 9) tmpDir = 1;

                if ((_x + ppp[i].DeltaX) < 0) continue; // т.к. не сможем получить за пределами

                if ((_y + ppp[i].DeltaY) < 0) continue; // т.к. не сможем получить за пределами


                if ((_x + ppp[i].DeltaX) == bitdata.Width) continue; // т.к. не сможем получить за пределами

                if ((_y + ppp[i].DeltaY) == bitdata.Height) continue; // т.к. не сможем получить за пределами

                int pos = GetPositionPointInImage(bitdata.PixelFormat, bitdata.Stride, _x + ppp[i].DeltaX, _y + ppp[i].DeltaY);
                byte CurrPixel = dataImage[pos];

                if (CurrPixel == 0) ppp[i].isBlack = true;
            }

            return ppp;
        }

        private Segment GetTraectory(ref byte[] _DataImage, int _x, int _y, BitmapData bitdata)
        {
            // TODO: Stride - это общее выделенное количество байт для одной линии
            // TODO: Width - реальное количество байт занятых одной линией
            // TODO: при работе с байтами учитывать что bmd.Stride может быть больше bmd.Width

            Segment tmpVector = new Segment();

            int CurrX = _x;
            int CurrY = _y;

            int pos = GetPositionPointInImage(bitdata.PixelFormat, bitdata.Stride, CurrX, CurrY);

            byte CurrPixelR = _DataImage[pos];
            byte CurrPixelG = _DataImage[pos + 1];
            byte CurrPixelB = _DataImage[pos + 2];

            int cR = (int)CurrPixelR;
            int cG = (int)CurrPixelG;
            int cB = (int)CurrPixelB;

            int grayScale = (int)(cR + cG + cB);


            //если начальная точка черная то её добавим
            if (grayScale == 0)
            {
                tmpVector.Points.Add(new Location(CurrX, CurrY));
                _DataImage[pos] = 150; //и сделаем светлее
                _DataImage[pos+1] = 150; //и сделаем светлее
                _DataImage[pos+2] = 150; //и сделаем светлее
            }

            bool needContinue = true;
            int direction = 4; // направление начала сканирования
            
            while (needContinue)
            {
                // получаем список окружения точки
                PlacePoint[] ppp = GetAreaPoint(ref _DataImage, CurrX, CurrY, bitdata, direction);

                bool finded = false;

                foreach (PlacePoint VARIABLE in ppp)
                {
                    if (VARIABLE.isBlack)
                    {
                        direction = VARIABLE.Direction;

                        finded = true;

                        CurrX += VARIABLE.DeltaX;
                        CurrY += VARIABLE.DeltaY;

                        //очистим точку в указанном направлении, и добавим точку в массив
                        int ipos = GetPositionPointInImage(bitdata.PixelFormat, bitdata.Stride, CurrX, CurrY);
                        _DataImage[ipos] = 150;
                        _DataImage[ipos+1] = 150;
                        _DataImage[ipos+2] = 150;

                        tmpVector.Points.Add(new Location(CurrX, CurrY));

                        break; // остановим цикл
                    }
                }

                if (!finded) needContinue = false;
            }






         

            return tmpVector;
        }

        /// <summary>
        /// Вычисление номера байта в массиве байт, содержащее изображение
        /// </summary>
        /// <param name="pf">Формат изображения</param>
        /// <param name="stride">Длина строки байт одной линии</param>
        /// <param name="X">Координата</param>
        /// <param name="Y">Координата</param>
        /// <returns></returns>
        private int GetPositionPointInImage(PixelFormat pf, int stride, int X, int Y)
        {
            int sizeOnePicsel = 0;

            switch (pf)
            {
                case PixelFormat.Format32bppArgb:
                    sizeOnePicsel = 4;
                    break;
                case PixelFormat.Format24bppRgb:
                    sizeOnePicsel = 3;
                    break;
                default:
                    MessageBox.Show("Формат изображения: " + pf.ToString() + " пока не поддерживается!!!");
                    break;
            }

            int pos = (stride * Y) + (X * sizeOnePicsel);

            return pos;
        }

        /// <summary>
        /// Получение векторов из рисунка
        /// </summary>
        /// <param name="image">Рисунок для анализа</param>
        /// <returns>Список точек</returns>
        private List<Segment> GetVectorFromImage(Bitmap _image)
        {
            // для сбора информации об отрезках
            List<Segment> Vectors = new List<Segment>();

            Bitmap sourceImage = null;

            sourceImage = _image.Clone(new Rectangle(0, 0, _image.Width, _image.Height), PixelFormat.Format24bppRgb); ;
           


            BitmapData bmd = sourceImage.LockBits(new Rectangle(0, 0, sourceImage.Width, sourceImage.Height), ImageLockMode.ReadWrite, sourceImage.PixelFormat);

            // Получаем адрес первой линии.
            IntPtr ptr = bmd.Scan0;
            // Вычисляем необходимый размер, для выделения памяти
            int numBytes = bmd.Stride * bmd.Height;

            // TODO: bmd.Stride - это общее выделенное количество байт для одной линии
            // TODO: bmd.Width - реальное количество байт занятых одной линией
            // TODO: при работе с байтами учитывать что bmd.Stride может быть больше bmd.Width

            // выделяем память
            byte[] DataImage = new byte[numBytes];

            // Копируем значения из памяти в массив.
            Marshal.Copy(ptr, DataImage, 0, numBytes);

            // Достигли конфа файла
            bool TheEndData = false;

            // Координаты основного прохода
            int mainX = 0;
            int mainY = 0;

            while (!TheEndData)
            {
                int pos = GetPositionPointInImage(bmd.PixelFormat, bmd.Stride, mainX, mainY);

                byte CurrPixelR = DataImage[pos];
                byte CurrPixelG = DataImage[pos + 1];
                byte CurrPixelB = DataImage[pos + 2];

                int cR = (int)CurrPixelR;
                int cG = (int)CurrPixelG;
                int cB = (int)CurrPixelB;

                int grayScale = (int)(cR + cG + cB);

                if (grayScale == 0) // нашли черную точку
                {
                    Segment tmpVector1 = GetTraectory(ref DataImage, mainX, mainY, bmd);

                    Segment tmpVector2 = GetTraectory(ref DataImage, mainX, mainY, bmd);

                    // а тут повтороно ищем с места старта

                    if (tmpVector2.Points.Count > 0)
                    {
                        // значения из 2-го вектора в первый скопируем, но наизнанку!!!
                        foreach (Location pnt in tmpVector2.Points)
                        {
                            tmpVector1.Points.Insert(0,pnt);
                        }
                    }

                    if (tmpVector1.Points.Count > 1) Vectors.Add(tmpVector1);
                }

                // дошли до конца линии
                if (mainX == (bmd.Width-1))
                {
                    // проверим есть-ли ещё линия дальше
                    if (mainY == (bmd.Height-1))
                    {
                        //это уже конец
                        TheEndData = true;
                    }
                    else
                    {
                        //переходим на следующую строку
                        mainX = 0;
                        mainY++;
                    }
                }
                else
                {
                    mainX++;
                }
                

            }

            Marshal.Copy(DataImage, 0, ptr, numBytes);

            sourceImage.UnlockBits(bmd);







            Vectors = VectorUtilities.Rotate(Vectors);










            return Vectors;
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


            if (!System.IO.File.Exists(textBoxFileName.Text)) return;

            Bitmap tmp = new Bitmap(textBoxFileName.Text);
            // параметры расположения координатной оси
            if (Property.Orientation == 2) tmp.RotateFlip(RotateFlipType.RotateNoneFlipY);

            pageImageIN = tmp;
            pageImageNOW = (Bitmap)pageImageIN.Clone();
            pageVectorNOW = new List<Segment>();


            CreateEvent("RefreshVector_04");
            CreateEvent("RefreshImage_04");
        }

        private void btShowOriginalImage_Click(object sender, EventArgs e)
        {
            pageImageNOW = (Bitmap)pageImageIN.Clone();
            pageVectorNOW = new List<Segment>();
            CreateEvent("RefreshVector_04");
            CreateEvent("RefreshImage_04");
        }
    }

    /// <summary>
    /// Для удобного пользования поиска направления
    /// </summary>
    public class PlacePoint
    {
        public int Direction;
        public bool isBlack;
        public int DeltaX;
        public int DeltaY;

        public PlacePoint(int _dir = 1, bool _isBlack = false, int _DeltaX = 0, int _DeltaY = 0)
        {
            Direction = _dir;
            isBlack = _isBlack;
            DeltaX = _DeltaX;
            DeltaY = _DeltaY;
        }

    }
}
