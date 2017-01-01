// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using AForge.Imaging.Filters;

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


            if (SkeletonizationFilter.Checked)
            {

                int Threshold = 0;

                Byte[,] m_SourceImage = BinarizationThinning.Thining.ToBinaryArray(pageImageNOW, out Threshold);

                Byte[,] m_DesImage = BinarizationThinning.Thining.ThinPicture(m_SourceImage);

                Bitmap bmpThin = BinarizationThinning.Thining.BinaryArrayToBinaryBitmap(m_DesImage);

                pageImageNOW = bmpThin;

            }

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

            if (SkeletonizationFilter.Checked)
            {

                int Threshold = 0;

                Byte[,] m_SourceImage = BinarizationThinning.Thining.ToBinaryArray(pageImageNOW, out Threshold);

                Byte[,] m_DesImage = BinarizationThinning.Thining.ThinPicture(m_SourceImage);

                Bitmap bmpThin = BinarizationThinning.Thining.BinaryArrayToBinaryBitmap(m_DesImage);

                pageImageNOW = bmpThin;

            }

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


namespace BinarizationThinning
{
    public static class Thining
    {
        //调用此函数即可实现提取图像骨架
        public static void getThinPicture(string imageSrcPath, string imageDestPath)
        {
            Bitmap bmp = new Bitmap(imageSrcPath);

            int Threshold = 0;

            Byte[,] m_SourceImage = ToBinaryArray(bmp, out Threshold);

            Byte[,] m_DesImage = BinarizationThinning.Thining.ThinPicture(m_SourceImage);

            Bitmap bmpThin = BinaryArrayToBinaryBitmap(m_DesImage);

            bmpThin.Save(imageDestPath, System.Drawing.Imaging.ImageFormat.Jpeg);
        }

        public static int B(Byte[,] picture, int x, int y)
        {
            return picture[x, y - 1] + picture[x + 1, y - 1] + picture[x + 1, y] + picture[x + 1, y + 1] +
                   picture[x, y + 1] + picture[x - 1, y + 1] + picture[x - 1, y] + picture[x - 1, y - 1];
        }

        public static int A(Byte[,] picture, int x, int y)
        {
            int counter = 0;
            if ((picture[x, y - 1] == 0) && (picture[x + 1, y - 1] == 1))
            {
                counter++;
            }
            if ((picture[x + 1, y - 1] == 0) && (picture[x + 1, y] == 1))
            {
                counter++;
            }
            if ((picture[x + 1, y] == 0) && (picture[x + 1, y + 1] == 1))
            {
                counter++;
            }
            if ((picture[x + 1, y + 1] == 0) && (picture[x, y + 1] == 1))
            {
                counter++;
            }
            if ((picture[x, y + 1] == 0) && (picture[x - 1, y + 1] == 1))
            {
                counter++;
            }
            if ((picture[x - 1, y + 1] == 0) && (picture[x - 1, y] == 1))
            {
                counter++;
            }
            if ((picture[x - 1, y] == 0) && (picture[x - 1, y - 1] == 1))
            {
                counter++;
            }
            if ((picture[x - 1, y - 1] == 0) && (picture[x, y - 1] == 1))
            {
                counter++;
            }
            return counter;
        }



        public static Byte[,] ThinPicture(Byte[,] newPicture)
        {

            Byte[,] picture = new Byte[newPicture.GetLength(0) + 2, newPicture.GetLength(1) + 2];
            Byte[,] pictureToRemove = new Byte[newPicture.GetLength(0) + 2, newPicture.GetLength(1) + 2];
            bool hasChanged;
            for (int i = 0; i < picture.GetLength(1); i++)
            {
                for (int j = 0; j < picture.GetLength(0); j++)
                {
                    picture[j, i] = 255;
                    pictureToRemove[j, i] = 0;
                }
            }

            for (int i = 0; i < newPicture.GetLength(1); i++)
            {
                for (int j = 0; j < newPicture.GetLength(0); j++)
                {
                    picture[j + 1, i + 1] = newPicture[j, i];
                }
            }

            for (int i = 0; i < picture.GetLength(1); i++)
            {
                for (int j = 0; j < picture.GetLength(0); j++)
                {
                    picture[j, i] = picture[j, i] == 0 ? picture[j, i] = 1 : picture[j, i] = 0;
                }
            }
            do
            {
                hasChanged = false;
                for (int i = 0; i < newPicture.GetLength(1); i++)
                {
                    for (int j = 0; j < newPicture.GetLength(0); j++)
                    {
                        if ((picture[j, i] == 1) && (2 <= B(picture, j, i)) && (B(picture, j, i) <= 6) && (A(picture, j, i) == 1) &&
                            (picture[j, i - 1] * picture[j + 1, i] * picture[j, i + 1] == 0) &&
                            (picture[j + 1, i] * picture[j, i + 1] * picture[j - 1, i] == 0))
                        {
                            pictureToRemove[j, i] = 1;
                            hasChanged = true;
                        }
                    }
                }
                for (int i = 0; i < newPicture.GetLength(1); i++)
                {
                    for (int j = 0; j < newPicture.GetLength(0); j++)
                    {
                        if (pictureToRemove[j, i] == 1)
                        {
                            picture[j, i] = 0;
                            pictureToRemove[j, i] = 0;
                        }
                    }
                }
                for (int i = 0; i < newPicture.GetLength(1); i++)
                {
                    for (int j = 0; j < newPicture.GetLength(0); j++)
                    {
                        if ((picture[j, i] == 1) && (2 <= B(picture, j, i)) && (B(picture, j, i) <= 6) &&
                            (A(picture, j, i) == 1) &&
                            (picture[j, i - 1] * picture[j + 1, i] * picture[j - 1, i] == 0) &&
                            (picture[j, i - 1] * picture[j, i + 1] * picture[j - 1, i] == 0))
                        {
                            pictureToRemove[j, i] = 1;
                            hasChanged = true;
                        }
                    }
                }

                for (int i = 0; i < newPicture.GetLength(1); i++)
                {
                    for (int j = 0; j < newPicture.GetLength(0); j++)
                    {
                        if (pictureToRemove[j, i] == 1)
                        {
                            picture[j, i] = 0;
                            pictureToRemove[j, i] = 0;
                        }
                    }
                }
            } while (hasChanged);

            for (int i = 0; i < newPicture.GetLength(1); i++)
            {
                for (int j = 0; j < newPicture.GetLength(0); j++)
                {
                    if ((picture[j, i] == 1) &&
                        (((picture[j, i - 1] * picture[j + 1, i] == 1) && (picture[j - 1, i + 1] != 1)) || ((picture[j + 1, i] * picture[j, i + 1] == 1) && (picture[j - 1, i - 1] != 1)) ||      //Небольшая модификцаия алгоритма для ещё большего утоньшения
                        ((picture[j, i + 1] * picture[j - 1, i] == 1) && (picture[j + 1, i - 1] != 1)) || ((picture[j, i - 1] * picture[j - 1, i] == 1) && (picture[j + 1, i + 1] != 1))))
                    {
                        picture[j, i] = 0;
                    }
                }
            }

            for (int i = 0; i < picture.GetLength(1); i++)
            {
                for (int j = 0; j < picture.GetLength(0); j++)
                {
                    // picture[j, i] = picture[j, i] == 0 ? 255 : 0;      
                    if (0 == picture[j, i])
                    {
                        picture[j, i] = 255;
                    }
                    else
                    {
                        picture[j, i] = 0;
                    }
                }
            }

            Byte[,] outPicture = new Byte[newPicture.GetLength(0), newPicture.GetLength(1)];

            for (int i = 0; i < newPicture.GetLength(1); i++)
            {
                for (int j = 0; j < newPicture.GetLength(0); j++)
                {
                    outPicture[j, i] = picture[j + 1, i + 1];
                }
            }
            return outPicture;
        }

        /// <summary>
        /// 全局阈值图像二值化
        /// </summary>
        /// <param name="bmp">原始图像</param>
        /// <param name="method">二值化方法</param>
        /// <param name="threshold">输出：全局阈值</param>
        /// <returns>二值化后的图像数组</returns>       
        public static Byte[,] ToBinaryArray(this Bitmap bmp, out Int32 threshold)
        {   // 位图转换为灰度数组
            Byte[,] GrayArray = bmp.ToGrayArray();

            // 计算全局阈值
            threshold = OtsuThreshold(GrayArray);

            // 根据阈值进行二值化
            Int32 PixelHeight = bmp.Height;
            Int32 PixelWidth = bmp.Width;
            Byte[,] BinaryArray = new Byte[PixelHeight, PixelWidth];
            for (Int32 i = 0; i < PixelHeight; i++)
            {
                for (Int32 j = 0; j < PixelWidth; j++)
                {
                    BinaryArray[i, j] = Convert.ToByte((GrayArray[i, j] > threshold) ? 255 : 0);
                }
            }

            return BinaryArray;
        }

        /// <summary>
        /// 将位图转换为灰度数组（256级灰度）
        /// </summary>
        /// <param name="bmp">原始位图</param>
        /// <returns>灰度数组</returns>
        public static Byte[,] ToGrayArray(this Bitmap bmp)
        {
            Int32 PixelHeight = bmp.Height; // 图像高度
            Int32 PixelWidth = bmp.Width;   // 图像宽度
            Int32 Stride = ((PixelWidth * 3 + 3) >> 2) << 2;    // 跨距宽度
            Byte[] Pixels = new Byte[PixelHeight * Stride];

            // 锁定位图到系统内存
            BitmapData bmpData = bmp.LockBits(new Rectangle(0, 0, PixelWidth, PixelHeight), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
            Marshal.Copy(bmpData.Scan0, Pixels, 0, Pixels.Length);  // 从非托管内存拷贝数据到托管内存
            bmp.UnlockBits(bmpData);    // 从系统内存解锁位图

            // 将像素数据转换为灰度数组
            Byte[,] GrayArray = new Byte[PixelHeight, PixelWidth];
            for (Int32 i = 0; i < PixelHeight; i++)
            {
                Int32 Index = i * Stride;
                for (Int32 j = 0; j < PixelWidth; j++)
                {
                    GrayArray[i, j] = Convert.ToByte((Pixels[Index + 2] * 19595 + Pixels[Index + 1] * 38469 + Pixels[Index] * 7471 + 32768) >> 16);
                    Index += 3;
                }
            }

            return GrayArray;
        }

        /// <summary>
        /// 大津法计算阈值
        /// </summary>
        /// <param name="grayArray">灰度数组</param>
        /// <returns>二值化阈值</returns>
        public static Int32 OtsuThreshold(Byte[,] grayArray)
        {   // 建立统计直方图
            Int32[] Histogram = new Int32[256];
            Array.Clear(Histogram, 0, 256);     // 初始化
            foreach (Byte b in grayArray)
            {
                Histogram[b]++;                 // 统计直方图
            }

            // 总的质量矩和图像点数
            Int32 SumC = grayArray.Length;    // 总的图像点数
            Double SumU = 0;                  // 双精度避免方差运算中数据溢出
            for (Int32 i = 1; i < 256; i++)
            {
                SumU += i * Histogram[i];     // 总的质量矩               
            }

            // 灰度区间
            Int32 MinGrayLevel = Array.FindIndex(Histogram, NonZero);       // 最小灰度值
            Int32 MaxGrayLevel = Array.FindLastIndex(Histogram, NonZero);   // 最大灰度值

            // 计算最大类间方差
            Int32 Threshold = MinGrayLevel;
            Double MaxVariance = 0.0;       // 初始最大方差
            Double U0 = 0;                  // 初始目标质量矩
            Int32 C0 = 0;                   // 初始目标点数
            for (Int32 i = MinGrayLevel; i < MaxGrayLevel; i++)
            {
                if (Histogram[i] == 0) continue;

                // 目标的质量矩和点数               
                U0 += i * Histogram[i];
                C0 += Histogram[i];

                // 计算目标和背景的类间方差
                Double Diference = U0 * SumC - SumU * C0;
                Double Variance = Diference * Diference / C0 / (SumC - C0); // 方差
                if (Variance > MaxVariance)
                {
                    MaxVariance = Variance;
                    Threshold = i;
                }
            }

            // 返回类间方差最大阈值
            return Threshold;
        }

        /// <summary>
        /// 检测非零值
        /// </summary>
        /// <param name="value">要检测的数值</param>
        /// <returns>
        ///     true：非零
        ///     false：零
        /// </returns>
        private static Boolean NonZero(Int32 value)
        {
            return (value != 0) ? true : false;
        }

        /// <summary>
        /// 将二值化数组转换为二值化图像
        /// </summary>
        /// <param name="binaryArray">二值化数组</param>
        /// <returns>二值化图像</returns>
        public static Bitmap BinaryArrayToBinaryBitmap(Byte[,] binaryArray)
        {   // 将二值化数组转换为二值化数据
            Int32 PixelHeight = binaryArray.GetLength(0);
            Int32 PixelWidth = binaryArray.GetLength(1);
            Int32 Stride = ((PixelWidth + 31) >> 5) << 2;
            Byte[] Pixels = new Byte[PixelHeight * Stride];
            for (Int32 i = 0; i < PixelHeight; i++)
            {
                Int32 Base = i * Stride;
                for (Int32 j = 0; j < PixelWidth; j++)
                {
                    if (binaryArray[i, j] != 0)
                    {
                        Pixels[Base + (j >> 3)] |= Convert.ToByte(0x80 >> (j & 0x7));
                    }
                }
            }

            // 创建黑白图像
            Bitmap BinaryBmp = new Bitmap(PixelWidth, PixelHeight, PixelFormat.Format1bppIndexed);

            // 设置调色表
            ColorPalette cp = BinaryBmp.Palette;
            cp.Entries[0] = Color.Black;    // 黑色
            cp.Entries[1] = Color.White;    // 白色
            BinaryBmp.Palette = cp;

            // 设置位图图像特性
            BitmapData BinaryBmpData = BinaryBmp.LockBits(new Rectangle(0, 0, PixelWidth, PixelHeight), ImageLockMode.WriteOnly, PixelFormat.Format1bppIndexed);
            Marshal.Copy(Pixels, 0, BinaryBmpData.Scan0, Pixels.Length);
            BinaryBmp.UnlockBits(BinaryBmpData);

            return BinaryBmp;
        }
    }
}