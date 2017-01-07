// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Xml.Serialization;
using BinarizationThinning;
using ToolsGenGkode.pages;

namespace ToolsGenGkode
{
    /// <summary>
    ///  Класс для обработки изображений
    /// </summary>
    static class ImageProcessing
    {

        /// <summary>
        /// Создание рисунка из текста
        /// </summary>
        /// <param name="text"> Текст</param>
        /// <param name="fontName">Имя шрифта</param>
        /// <param name="fontSize">Размер символов</param>
        /// <param name="extFileFont">Имя файла шрифта, если используется внешний файл шрифта</param>
        /// <returns></returns>
        public static Bitmap CreateBitmapFromText(string text, string fontName, float fontSize, string extFileFont = "")
        {
            if (text.Trim().Length == 0) text = " ";

            Bitmap bitmap = new Bitmap(1, 1, PixelFormat.Format24bppRgb);

            int width = 0;
            int height = 0;

            // Создаем объект Font для "рисования" им текста.
            Font font = new Font(fontName, (int)fontSize, FontStyle.Bold, GraphicsUnit.Pixel);

            if (extFileFont != "")
            {
                PrivateFontCollection customFontFromFile;
                customFontFromFile = new PrivateFontCollection();
                customFontFromFile.AddFontFile(extFileFont);

                if (customFontFromFile.Families.Length > 0)
                {
                    font = new Font(customFontFromFile.Families[0], (int)fontSize);
                }
                else
                {
                    MessageBox.Show(@"Ошибка загрузки шрифта из файла!!!");
                }

                customFontFromFile.Dispose();
            }

            // Создаем объект Graphics для вычисления высоты и ширины текста.
            Graphics graphics = Graphics.FromImage(bitmap);

            // Определение размеров изображения.
            width = (int)graphics.MeasureString(text, font).Width;
            height = (int)graphics.MeasureString(text, font).Height;

            // Пересоздаем объект Bitmap с откорректированными размерами под текст и шрифт.
            bitmap = new Bitmap(bitmap, new Size(width, height));

            // Пересоздаем объект Graphics
            graphics = Graphics.FromImage(bitmap);

            // Задаем цвет фона.
            graphics.Clear(Color.White);
            // Задаем параметры анти-алиасинга
            graphics.SmoothingMode = SmoothingMode.None;
            graphics.TextRenderingHint = TextRenderingHint.SingleBitPerPixel;
            // Пишем (рисуем) текст
            graphics.DrawString(text, font, new SolidBrush(Color.Black), 0, 0);

            graphics.Flush();

            font.Dispose();

            return (bitmap);
        }

        /// <summary>
        /// Функция получения 2-х цветного изображения
        /// </summary>
        /// <param name="sourceImage"> Изображение которое необходимо преобразовать</param>
        /// <param name="koeff">Коэффициент определения что будет черным а что белым</param>
        /// <param name="reversingColor"></param>
        /// <returns></returns>
        public static Bitmap GetBlackWhileImage(Bitmap sourceImage, int koeff, bool reversingColor = false)
        {


            Bitmap tempImage = sourceImage;

            BitmapData bmd = tempImage.LockBits(new Rectangle(0, 0, sourceImage.Width, sourceImage.Height), ImageLockMode.ReadWrite, sourceImage.PixelFormat);

            // Получаем адрес первой линии.
            IntPtr ptr = bmd.Scan0;
            // Вычисляем необходимый размер, для выделения памяти
            int numBytes = bmd.Stride * bmd.Height;

            // TODO: bmd.Stride - это общее выделенное количество байт для одной линии
            // TODO: bmd.Width - реальное количество байт занятых одной линией (сколько отводится под изображение)
            // TODO: при работе с байтами учитывать что bmd.Stride может быть больше bmd.Width

            // выделяем память
            byte[] dataImage = new byte[numBytes];

            // Копируем значения из памяти в массив.
            Marshal.Copy(ptr, dataImage, 0, numBytes);

            // Достигли конфа файла
            bool theEndData = false;

            // Координаты основного прохода
            int mainX = 0;
            int mainY = 0;

            int sizeOnePicsel = 1;

            switch (bmd.PixelFormat)
            {
                case PixelFormat.Format32bppArgb:
                    sizeOnePicsel = 4;
                    break;
                case PixelFormat.Format24bppRgb:
                    sizeOnePicsel = 3;
                    break;
                default:
                    MessageBox.Show(@"Формат изображения: " + bmd.PixelFormat.ToString() + @" пока не поддерживается!!!");
                    return null;
            }



            while (!theEndData)
            {
                int pos = (bmd.Stride * mainY) + (mainX * sizeOnePicsel);

                //в зависимости от размерности, тут может отличаться количество байт на пиксель
                byte currPixelR = dataImage[pos];
                byte currPixelG = dataImage[pos + 1];
                byte currPixelB = dataImage[pos + 2];

                int cR = currPixelR;
                int cG = currPixelG;
                int cB = currPixelB;

                int grayScale = cR + cG + cB;

                grayScale = grayScale / 3;




                if (grayScale > koeff) grayScale = 255;
                else grayScale = 0;


                if (reversingColor)
                {
                    if (grayScale == 0) grayScale = 255;
                    else grayScale = 0;
                }


                dataImage[pos] = (byte)grayScale;
                dataImage[pos + 1] = (byte)grayScale;
                dataImage[pos + 2] = (byte)grayScale;



                // дошли до конца линии
                if (mainX == (bmd.Width - 1))
                {
                    // проверим есть-ли ещё линия дальше
                    if (mainY == (bmd.Height - 1))
                    {
                        //это уже конец
                        theEndData = true;
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

            Marshal.Copy(dataImage, 0, ptr, numBytes);

            tempImage.UnlockBits(bmd);


 
            return tempImage;
        }




        private static Color GetPosColor(ref Bitmap bmp, int x, int y)
        {
            Color cReturn = Color.FromArgb(255, 255, 255, 255);

            if (x < 0) return cReturn;

            if (y < 0) return cReturn;

            if (x >= bmp.Width) return cReturn;

            if (y >= bmp.Height) return cReturn;

            return bmp.GetPixel(x, y);
        }



        // удаление закрашенных областей
        public static Bitmap BitmapDeleteContent(Bitmap tmpBmp)
        {

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


            return bbb;
        }



        public static Bitmap Skeletonization(Bitmap bmpSource)
        {

            int Threshold = 0;

            Byte[,] m_SourceImage = Thining.ToBinaryArray(bmpSource, out Threshold);

            Byte[,] m_DesImage = Thining.ThinPicture(m_SourceImage);

            return Thining.BinaryArrayToBinaryBitmap(m_DesImage);
        }






    }


    /// <summary>
    /// Класс обработки векторных данных
    /// </summary>
    static class VectorProcessing
    {

        /// <summary>
        /// Функция возвращает массив отрезков, которые и составляют рисунок шрифта
        /// </summary>
        /// <param name="text">Текст кторый нужно преобразовать в вектора</param>
        /// <param name="fontName">Имя шрифта</param>
        /// <param name="fontSize">Размер шрифта</param>
        /// <param name="extFileFont">Имя внешненего файла (если используется не системный шрифт)</param>
        /// <returns>Набор отрезков</returns>
        public static List<GroupPoint> GetVectorFromText(string text, string fontName, float fontSize, string extFileFont = "")
        {
            PointF[] pts = null; //список точек
            byte[] ptsType = null; //информация о начале/окончании отрезка

            using (GraphicsPath path = new GraphicsPath())
            {
                if (extFileFont != "")
                {
                    PrivateFontCollection customFontFromFile;
                    customFontFromFile = new PrivateFontCollection();
                    customFontFromFile.AddFontFile(extFileFont);
                    if (customFontFromFile.Families.Length > 0)
                    {
                        Font font = new Font(customFontFromFile.Families[0], (int)fontSize);
                        try
                        {
                            path.AddString(text, font.FontFamily, (int)FontStyle.Regular, fontSize, new PointF(0f, 0f), StringFormat.GenericDefault);
                        }
                        catch (Exception)
                        {
                            string ss = "Произошла ошибка №1: при получении данных из внешнего файла шрифта.";
                            MessageBox.Show(ss);
                            //throw;
                        }

                    }
                    else
                    {
                        MessageBox.Show(@"Ошибка загрузки шрифта из файла!!!");
                    }

                    customFontFromFile.Dispose();
                }
                else
                {
                    try
                    {
                        path.AddString(text, new FontFamily(fontName), (int)FontStyle.Regular, fontSize, new PointF(0f, 0f), StringFormat.GenericDefault);
                    }
                    catch (Exception)
                    {
                        string ss = "Произошла ошибка №2: при получении данных из системного файла шрифта.";
                        ss += "\n" + text;
                        ss += "\n" + (new FontFamily(fontName)).ToString();
                        ss += "\n" + ((int)FontStyle.Regular).ToString();
                        ss += "\n" + (fontSize).ToString();
                        ss += "\n" + (StringFormat.GenericDefault).ToString();

                        MessageBox.Show(ss);
                        throw;
                    }

                }

                path.Flatten();

                if (path.PointCount == 0)//нет отрезков
                {
                    return new List<GroupPoint>();
                }

                pts = path.PathPoints;
                ptsType = path.PathTypes;
            }

            //для сбора информации о точках у отрезка
            List<cncPoint> ListPoints = new List<cncPoint>();
            // для сбора информации об отрезках
            List<GroupPoint> Lines = new List<GroupPoint>();

            int index = 0;
            foreach (PointF value in pts)
            {
                byte ptypePoint = ptsType[index]; //тип точки: 0-точка является началом отрезка, 1-точка является продолжением отрезка, 129-161 и прочее окончанием отрезка, причем необходимо добавлять линию соединяющую начальную точку и конечную

                // это первая точка
                if (ptypePoint == 0)
                {
                    ListPoints.Add(new cncPoint(value.X, value.Y));
                }

                //а это продолжение
                if (ptypePoint == 1)
                {
                    ListPoints.Add(new cncPoint(value.X, value.Y));
                }

                //окончание
                if (ptypePoint == 129)
                {
                    ListPoints.Add(new cncPoint(value.X, value.Y));
                    ListPoints.Add(ListPoints[0]); //иногда не нужна
                    Lines.Add(new GroupPoint(ListPoints));
                    ListPoints = new List<cncPoint>();
                }

                if (ptypePoint == 161)
                {
                    ListPoints.Add(new cncPoint(value.X, value.Y));
                    //ListPoints.Add(ListPoints[0]);
                    Lines.Add(new GroupPoint(ListPoints));
                    ListPoints = new List<cncPoint>();
                }

                index++;
            }


            Lines = VectorProcessing.Rotate(Lines);

            //// получим границы изображения
            //decimal minX = 99999;
            //decimal maxX = -99999;
            //decimal minY = 99999;
            //decimal maxY = -99999;

            //foreach (cVector vector in Lines)
            //{
            //    foreach (cPoint point in vector.Points)
            //    {
            //        if (minX > point.X) minX = point.X;

            //        if (maxX < point.X) maxX = point.X;

            //        if (minY > point.Y) minY = point.Y;

            //        if (maxY < point.Y) maxY = point.Y;
            //    }
            //}



            //if (Property.Orientation == 1)
            //{
            //    //реверс по оси Y


            //    List<cVector> tmp = new List<cVector>();

            //    //вычислим дельту, для смещения векторов
            //    decimal delta = minY + maxY;

            //    List<cVector> vectors = new List<cVector>();
            //    List<cPoint> points = new List<cPoint>();

            //    foreach (cVector vector in Lines)
            //    {
            //        points = new List<cPoint>();

            //        foreach (cPoint point in vector.Points)
            //        {
            //            points.Add(new cPoint(point.X, (-point.Y) + delta, point.Selected));
            //        }
            //        tmp.Add(new cVector(points, vector.Selected));
            //        points = new List<cPoint>();
            //    }

            //    Lines = tmp;

            //    vectors = new List<cVector>();
            //    points = new List<cPoint>();

            //}


            //if (Property.Orientation == 2)
            //{
            //    //ничего не делаем



            //}


            //if (Property.Orientation == 3)
            //{
            //    //реверс по оси X


            //    List<cVector> tmp = new List<cVector>();

            //    //вычислим дельту, для смещения векторов
            //    decimal deltaX = minX + maxX;

            //    List<cVector> vectors = new List<cVector>();
            //    List<cPoint> points = new List<cPoint>();

            //    foreach (cVector vector in Lines)
            //    {
            //        points = new List<cPoint>();

            //        foreach (cPoint point in vector.Points)
            //        {
            //            points.Add(new cPoint((-point.X) + deltaX, point.Y, point.Selected));
            //        }
            //        tmp.Add(new cVector(points, vector.Selected));
            //        points = new List<cPoint>();
            //    }

            //    Lines = tmp;

            //    vectors = new List<cVector>();
            //    points = new List<cPoint>();
            //}


            //if (Property.Orientation == 4)
            //{
            //    //реверс по обоим осям

            //    List<cVector> tmp = new List<cVector>();

            //    //вычислим дельту, для смещения векторов
            //    decimal deltaY = minY + maxY;
            //    decimal deltaX = minX + maxX;

            //    List<cVector> vectors = new List<cVector>();
            //    List<cPoint> points = new List<cPoint>();

            //    foreach (cVector vector in Lines)
            //    {
            //        points = new List<cPoint>();

            //        foreach (cPoint point in vector.Points)
            //        {
            //            points.Add(new cPoint((-point.X) + deltaX, (-point.Y) + deltaY, point.Selected));
            //        }
            //        tmp.Add(new cVector(points, vector.Selected));
            //        points = new List<cPoint>();
            //    }

            //    Lines = tmp;

            //    vectors = new List<cVector>();
            //    points = new List<cPoint>();
            //}





            //

            //if (Property.Orientation == 1)
            //{
            //    foreach (cVector vector in Lines)
            //    {
            //        foreach (cPoint point in vector.Points)
            //        {
            //            if (min > point.Y) min = point.Y;

            //            if (max < point.Y) max = point.Y;
            //        }
            //    }

            //    //вычислим дельту, для смещения векторов
            //    decimal delta = min + max;

            //    List<cVector> vectors = new List<cVector>();
            //    List<cPoint> points = new List<cPoint>();

            //    foreach (cVector vector in Lines)
            //    {
            //        points = new List<cPoint>();

            //        foreach (cPoint point in vector.Points)
            //        {
            //            points.Add(new cPoint(point.X, (-point.Y) + delta, point.Selected));
            //        }
            //        tmp.Add(new cVector(points, vector.Selected));
            //        points = new List<cPoint>();
            //    }

            //    Lines = tmp;

            //    vectors = new List<cVector>();
            //    points = new List<cPoint>();

            //}



            return Lines;

        }

        /// <summary>
        /// Создание дубликата, списка групп точек
        /// </summary>
        /// <param name="source">Список групп точек для клонирования</param>
        /// <returns>Новый список групп точек</returns>
        public static List<GroupPoint> ListGroupPointClone(List<GroupPoint> source)
        {
            List<GroupPoint> tmp = new List<GroupPoint>();

            if (source == null) return tmp;

            foreach (GroupPoint vector in source)
            {
                tmp.Add(vector.Clone());
            }
            return tmp;
        }


        public static List<GroupPoint> Rotate(List<GroupPoint> dataCVectors)
        {
            List<GroupPoint> returnValue = new List<GroupPoint>();

            // получим границы изображения
            double minX = 99999;
            double maxX = -99999;
            double minY = 99999;
            double maxY = -99999;

            foreach (GroupPoint vector in dataCVectors)
            {
                foreach (cncPoint point in vector.Points)
                {
                    if (minX > point.X) minX = point.X;

                    if (maxX < point.X) maxX = point.X;

                    if (minY > point.Y) minY = point.Y;

                    if (maxY < point.Y) maxY = point.Y;
                }
            }



            if (Properties.Settings.Default.page01AxisVariant == 1)
            {
                //реверс по оси Y


                List<GroupPoint> tmp = new List<GroupPoint>();

                //вычислим дельту, для смещения векторов
                double delta = minY + maxY;

                List<GroupPoint> vectors = new List<GroupPoint>();
                List<cncPoint> points = new List<cncPoint>();

                foreach (GroupPoint vector in dataCVectors)
                {
                    points = new List<cncPoint>();

                    foreach (cncPoint point in vector.Points)
                    {
                        points.Add(new cncPoint(point.X, (-point.Y) + delta, point.Svalue, point.Fvalue, point.Pvalue, point.Selected,point.Bright));
                    }
                    tmp.Add(new GroupPoint(points, vector.Selected,vector.Dirrect,vector.IndividualPoints));
                    points = new List<cncPoint>();
                }

                returnValue = tmp;

                vectors = new List<GroupPoint>();
                points = new List<cncPoint>();

            }


            if (Properties.Settings.Default.page01AxisVariant == 2)
            {
                //ничего не делаем
                foreach (GroupPoint VARIABLE in dataCVectors)
                {
                    returnValue.Add(VARIABLE.Clone());
                }
            }


            if (Properties.Settings.Default.page01AxisVariant == 3)
            {
                //реверс по оси X


                List<GroupPoint> tmp = new List<GroupPoint>();

                //вычислим дельту, для смещения векторов
                double deltaX = minX + maxX;

                List<GroupPoint> vectors = new List<GroupPoint>();
                List<cncPoint> points = new List<cncPoint>();

                foreach (GroupPoint vector in dataCVectors)
                {
                    points = new List<cncPoint>();

                    foreach (cncPoint point in vector.Points)
                    {
                        points.Add(new cncPoint((-point.X) + deltaX, point.Y, 0, 0, 0, point.Selected));
                    }
                    tmp.Add(new GroupPoint(points, vector.Selected));
                    points = new List<cncPoint>();
                }

                returnValue = tmp;

                vectors = new List<GroupPoint>();
                points = new List<cncPoint>();
            }


            if (Properties.Settings.Default.page01AxisVariant == 4)
            {
                //реверс по обоим осям

                List<GroupPoint> tmp = new List<GroupPoint>();

                //вычислим дельту, для смещения векторов
                double deltaY = minY + maxY;
                double deltaX = minX + maxX;

                List<GroupPoint> vectors = new List<GroupPoint>();
                List<cncPoint> points = new List<cncPoint>();

                foreach (GroupPoint vector in dataCVectors)
                {
                    points = new List<cncPoint>();

                    foreach (cncPoint point in vector.Points)
                    {
                        points.Add(new cncPoint((-point.X) + deltaX, (-point.Y) + deltaY, 0, 0, 0, point.Selected));
                    }
                    tmp.Add(new GroupPoint(points, vector.Selected));
                    points = new List<cncPoint>();
                }

                returnValue = tmp;

                vectors = new List<GroupPoint>();
                points = new List<cncPoint>();
            }

            return returnValue;
        }

        public static List<cncPoint> Rotate(List<cncPoint> dataCPoints)
        {
            List<cncPoint> returnValue = new List<cncPoint>();

            // получим границы изображения
            double minX = 99999;
            double maxX = -99999;
            double minY = 99999;
            double maxY = -99999;

            foreach (cncPoint point in dataCPoints)
            {
                if (minX > point.X) minX = point.X;

                if (maxX < point.X) maxX = point.X;

                if (minY > point.Y) minY = point.Y;

                if (maxY < point.Y) maxY = point.Y;
            }
    



            if (Properties.Settings.Default.page01AxisVariant == 1)
            {
                //реверс по оси Y
                //вычислим дельту, для смещения векторов
                double delta = minY + maxY;

                List<cncPoint> points = new List<cncPoint>();

                foreach (cncPoint point in dataCPoints)
                {
                    points.Add(new cncPoint(point.X, (-point.Y) + delta, 0, 0, 0, point.Selected));
                }
                returnValue = points;

                points = new List<cncPoint>();
            }


            if (Properties.Settings.Default.page01AxisVariant == 2)
            {
                //ничего не делаем
            }

            if (Properties.Settings.Default.page01AxisVariant == 3)
            {
                //реверс по оси X

                //вычислим дельту, для смещения векторов
                double deltaX = minX + maxX;

                List<cncPoint> points = new List<cncPoint>();

                foreach (cncPoint point in dataCPoints)
                {
                    points.Add(new cncPoint((-point.X) + deltaX, point.Y, 0, 0, 0, point.Selected));
                }
                

                returnValue = points;

                points = new List<cncPoint>();
            }


            if (Properties.Settings.Default.page01AxisVariant == 4)
            {
                //реверс по обоим осям
                //вычислим дельту, для смещения векторов
                double deltaY = minY + maxY;
                double deltaX = minX + maxX;

                List<cncPoint> points = new List<cncPoint>();

                foreach (cncPoint point in dataCPoints)
                {
                    points.Add(new cncPoint((-point.X) + deltaX, (-point.Y) + deltaY, 0, 0, 0, point.Selected));
                }
                returnValue = points;
                points = new List<cncPoint>();
            }

            return returnValue;
        }



        /// <summary>
        /// Вычисление номера байта в массиве байт, содержащее изображение
        /// </summary>
        /// <param name="pf">Формат изображения</param>
        /// <param name="stride">Длина строки байт одной линии</param>
        /// <param name="X">Координата</param>
        /// <param name="Y">Координата</param>
        /// <returns></returns>
        private static int GetPositionPointInImage(PixelFormat pf, int stride, int X, int Y)
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




        private static PlacePoint[] GetAreaPoint(ref byte[] dataImage, int _x, int _y, BitmapData bitdata, int _startDirection)
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



        private static GroupPoint GetTraectory(ref byte[] _DataImage, int _x, int _y, BitmapData bitdata)
        {
            // TODO: Stride - это общее выделенное количество байт для одной линии
            // TODO: Width - реальное количество байт занятых одной линией
            // TODO: при работе с байтами учитывать что bmd.Stride может быть больше bmd.Width

            GroupPoint tmpVector = new GroupPoint();

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
                tmpVector.Points.Add(new cncPoint(CurrX, CurrY));
                _DataImage[pos] = 150; //и сделаем светлее
                _DataImage[pos + 1] = 150; //и сделаем светлее
                _DataImage[pos + 2] = 150; //и сделаем светлее
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
                        _DataImage[ipos + 1] = 150;
                        _DataImage[ipos + 2] = 150;

                        tmpVector.Points.Add(new cncPoint(CurrX, CurrY));

                        break; // остановим цикл
                    }
                }

                if (!finded) needContinue = false;
            }








            return tmpVector;
        }



        /// <summary>
        /// Получение векторов из рисунка
        /// </summary>
        /// <param name="image">Рисунок для анализа</param>
        /// <returns>Список точек</returns>
        public static List<GroupPoint> GetVectorFromImage(Bitmap _image)
        {
            // для сбора информации об отрезках
            List<GroupPoint> Vectors = new List<GroupPoint>();

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
                    GroupPoint tmpVector1 = GetTraectory(ref DataImage, mainX, mainY, bmd);

                    GroupPoint tmpVector2 = GetTraectory(ref DataImage, mainX, mainY, bmd);

                    // а тут повтороно ищем с места старта

                    if (tmpVector2.Points.Count > 0)
                    {
                        // значения из 2-го вектора в первый скопируем, но наизнанку!!!
                        foreach (cncPoint pnt in tmpVector2.Points)
                        {
                            tmpVector1.Points.Insert(0, pnt);
                        }
                    }

                    if (tmpVector1.Points.Count > 1) Vectors.Add(tmpVector1);
                }

                // дошли до конца линии
                if (mainX == (bmd.Width - 1))
                {
                    // проверим есть-ли ещё линия дальше
                    if (mainY == (bmd.Height - 1))
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







            Vectors = VectorProcessing.Rotate(Vectors);










            return Vectors;
        }




        public static List<GroupPoint> GetVectorFromPLT(string FileNAME)
        {
            // для сбора информации о точках у отрезка
            List<cncPoint> ListPoints = new List<cncPoint>();
            // для сбора информации об отрезках
            List<GroupPoint> Lines = new List<GroupPoint>();

            //для смещения по оси в положительную сторону
            double minX = 0;
            double minY = 0;


            int numRow = 1;

            StreamReader fs = new StreamReader(FileNAME);
            string s = fs.ReadLine();

            while (s != null)
            {
                s = s.Trim();

                int pos1 = -1, pos2 = -1, pos3 = -1;

                //начальная точка
                if (s.Substring(0, 2) == "PU" && s.Length > 3)
                {
                    //это не самый первый сегмент, поэтому перед заполнением нового, предыдущий сохраним
                    if (ListPoints.Count > 0)
                    {
                        Lines.Add(new GroupPoint(ListPoints));
                        ListPoints = new List<cncPoint>();
                    }

                    pos1 = s.IndexOf('U');
                    pos2 = s.IndexOf(' ');
                    pos3 = s.IndexOf(';');
                }

                //продолжение
                if (s.Substring(0, 2) == "PD" && s.Length > 3)
                {
                    pos1 = s.IndexOf('D');
                    pos2 = s.IndexOf(' ');
                    pos3 = s.IndexOf(';');
                }

                // завершение
                if (s.Substring(0, 3) == "SP0")
                {
                    Lines.Add(new GroupPoint(ListPoints));
                    ListPoints = new List<cncPoint>();
                    s = fs.ReadLine();
                    numRow++;
                    continue;
                }

                if (pos1 == -1 || pos2 == -1 || pos3 == -1)//какая-то ненужная пока строка
                {
                    s = fs.ReadLine();
                    numRow++;
                    continue;
                }

                double posX, posY;

                if (!Double.TryParse(s.Substring(pos1 + 1, pos2 - pos1 - 1), out posX))
                {
                    MessageBox.Show(@"Ошибка преобразования координаты X в строке № " + numRow.ToString());
                    break;
                }

                if (!Double.TryParse(s.Substring(pos2 + 1, pos3 - pos2 - 1), out posY))
                {
                    MessageBox.Show(@"Ошибка преобразования координаты Y в строке № " + numRow.ToString());
                    break;
                }

                // Пересчет в милиметры
                posX = posX / 40;
                posY = posY / 40;


                if (posX < minX) minX = posX;

                if (posY < minY) minY = posY;

                //if (posX > xmax) xmax = posX;

                //if (posX < xmin) xmin = posX;

                //if (posY > ymax) ymax = posY;

                //if (posY < ymin) ymin = posY;


                ListPoints.Add(new cncPoint(posX, posY));



                s = fs.ReadLine();
                numRow++;
            }

            fs = null;


            List<GroupPoint> ListLines;

            List<cncPoint> ListPoint;

            ListLines = new List<GroupPoint>();

            foreach (GroupPoint pline in Lines)
            {
                ListPoint = new List<cncPoint>();

                foreach (cncPoint ppoint in pline.Points)
                {

                    double fX = (ppoint.X + (-minX)) * 10;
                    double fY = (ppoint.Y + (-minY)) * 10;

                    ListPoint.Add(new cncPoint(fX, fY));
                }
                ListLines.Add(new GroupPoint(ListPoint));
            }

            return ListLines;
        }


        /// <summary>
        /// Uses the Douglas Peucker algorithim to reduce the number of points.
        /// </summary>
        /// <param name="Points">The points.</param>
        /// <param name="Tolerance">The tolerance.</param>
        /// <returns></returns>
        public static List<cncPoint> DouglasPeuckerReduction(List<cncPoint> Points, Double Tolerance)
        {

            //if (Points == null || Points.Count < 3)
            //    return Points;

            Int32 firstPoint = 0;
            Int32 lastPoint = Points.Count - 1;
            List<Int32> pointIndexsToKeep = new List<Int32>();

            //Add the first and last index to the keepers
            pointIndexsToKeep.Add(firstPoint);
            pointIndexsToKeep.Add(lastPoint);


            //The first and the last point can not be the same
            while (Points[firstPoint].Equals(Points[lastPoint]))
            {
                lastPoint--;
            }

            DouglasPeuckerReduction(Points, firstPoint, lastPoint, Tolerance, ref pointIndexsToKeep);

            List<cncPoint> returnPoints = new List<cncPoint>();
            pointIndexsToKeep.Sort();
            foreach (Int32 index in pointIndexsToKeep)
            {
                returnPoints.Add(Points[index]);
            }

            return returnPoints;
        }

        /// <summary>
        /// Douglases the peucker reduction.
        /// </summary>
        /// <param name="points">The points.</param>
        /// <param name="firstPoint">The first point.</param>
        /// <param name="lastPoint">The last point.</param>
        /// <param name="tolerance">The tolerance.</param>
        /// <param name="pointIndexsToKeep">The point indexs to keep.</param>
        private static void DouglasPeuckerReduction(List<cncPoint> points, Int32 firstPoint, Int32 lastPoint, Double tolerance, ref List<Int32> pointIndexsToKeep)
        {
            Double maxDistance = 0;
            Int32 indexFarthest = 0;

            for (Int32 index = firstPoint; index < lastPoint; index++)
            {
                Double distance = PerpendicularDistance(points[firstPoint], points[lastPoint], points[index]);
                if (distance > maxDistance)
                {
                    maxDistance = distance;
                    indexFarthest = index;
                }
            }

            if (maxDistance > tolerance && indexFarthest != 0)
            {
                //Add the largest point that exceeds the tolerance
                pointIndexsToKeep.Add(indexFarthest);

                DouglasPeuckerReduction(points, firstPoint, indexFarthest, tolerance, ref pointIndexsToKeep);
                DouglasPeuckerReduction(points, indexFarthest, lastPoint, tolerance, ref pointIndexsToKeep);
            }
        }

        /// <summary>
        /// The distance of a point from a line made from point1 and point2.
        /// </summary>
        /// <param name="pt1">The PT1.</param>
        /// <param name="pt2">The PT2.</param>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        private static Double PerpendicularDistance(cncPoint Point1, cncPoint Point2, cncPoint Point)
        {
            //Area = |(1/2)(x1y2 + x2y3 + x3y1 - x2y1 - x3y2 - x1y3)|   *Area of triangle
            //Base = √((x1-x2)²+(x1-x2)²)                               *Base of Triangle*
            //Area = .5*Base*H                                          *Solve for height
            //Height = Area/.5/Base

            Double area = Math.Abs(.5 * ((double)Point1.X * (double)Point2.Y + (double)Point2.X * (double)Point.Y + (double)Point.X * (double)Point1.Y - (double)Point2.X * (double)Point1.Y - (double)Point.X * (double)Point2.Y - (double)Point1.X * (double)Point.Y));
            Double bottom = Math.Sqrt(Math.Pow((double)Point1.X - (double)Point2.X, 2) + Math.Pow((double)Point1.Y - (double)Point2.Y, 2));
            Double height = area / bottom * 2;

            return height;

            //Another option
            //Double A = Point.X - Point1.X;
            //Double B = Point.Y - Point1.Y;
            //Double C = Point2.X - Point1.X;
            //Double D = Point2.Y - Point1.Y;

            //Double dot = A * C + B * D;
            //Double len_sq = C * C + D * D;
            //Double param = dot / len_sq;

            //Double xx, yy;

            //if (param < 0)
            //{
            //    xx = Point1.X;
            //    yy = Point1.Y;
            //}
            //else if (param > 1)
            //{
            //    xx = Point2.X;
            //    yy = Point2.Y;
            //}
            //else
            //{
            //    xx = Point1.X + param * C;
            //    yy = Point1.Y + param * D;
            //}

            //Double d = DistanceBetweenOn2DPlane(Point, new Point(xx, yy));

        }
    }



    /// <summary>
    /// Класс работы с XML данными
    /// </summary>
    static class XMLProcessing
    {
        




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



/// <summary>
/// Для алгоритма получения скелета/утоньшения контуров рисунка
/// </summary>
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

            Byte[,] m_DesImage = ThinPicture(m_SourceImage);

            Bitmap bmpThin = BinaryArrayToBinaryBitmap(m_DesImage);

            bmpThin.Save(imageDestPath, ImageFormat.Jpeg);
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