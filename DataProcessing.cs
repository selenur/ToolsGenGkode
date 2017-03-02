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
using AForge.Imaging;
using BinarizationThinning;
using ToolsGenGkode.pages;
using ToolsGenGkode.Properties;

namespace ToolsGenGkode
{
    /// <summary>
    ///  Класс для обработки изображений
    /// </summary>
    static class ImageProcessing
    {

        /// <summary>
        /// Создание 8-битного рисунка из текста
        /// </summary>
        /// <param name="text"> Текст</param>
        /// <param name="fontName">Имя шрифта</param>
        /// <param name="fontSize">Размер символов</param>
        /// <param name="extFileFont">Имя файла шрифта, если используется внешний файл шрифта</param>
        /// <returns></returns>
        public static Bitmap CreateBitmapFromText(string text, string fontName, float fontSize, string extFileFont = "")
        {
            if (text.Trim().Length == 0) text = " ";

            // по умолчанию рисунок 1х1 пиксель, и 24 бита на пиксель, т.к. сразу 8 бит использовать не получится, 
            // т.к. стандартные функции не работают...
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
            
            return CheckAndConvertImageto24bitPerPixel(bitmap);
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


            Bitmap tempImage = CheckAndConvertImageto24bitPerPixel(sourceImage);

            BitmapData bmd = tempImage.LockBits(new Rectangle(0, 0, tempImage.Width, tempImage.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

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

            Bitmap bbb = tmpBmp.Clone(new Rectangle(0, 0, tmpBmp.Width, tmpBmp.Height), PixelFormat.Format24bppRgb);




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





        static int MixColorToGreyScale(int R, int G, int B)
        {
            return (int)((R * 0.3) + (G * 0.59) + (B * 0.11));
        }

        public static Byte[,] ToByteArray(ref Bitmap sourceBMP)
        {
            // высота Y
            Int32 PixelHeight = sourceBMP.Height;
            // ширина X
            Int32 PixelWidth = sourceBMP.Width;
            // массив в котором будут хранится данные
            Byte[,] GrayArray = new Byte[PixelHeight, PixelWidth];

            BitmapData bmpData = sourceBMP.LockBits(new Rectangle(0, 0, PixelWidth, PixelHeight), ImageLockMode.ReadOnly, sourceBMP.PixelFormat);

            int bitPerPixel = 0;

            switch (sourceBMP.PixelFormat)
            {
                case PixelFormat.Format1bppIndexed:
                    bitPerPixel = 1;
                    break;
                case PixelFormat.Format4bppIndexed:
                    bitPerPixel = 4;
                    break;
                case PixelFormat.Format8bppIndexed:
                    bitPerPixel = 8;
                    break;
                case PixelFormat.Format24bppRgb:
                    bitPerPixel = 24;
                    break;
                case PixelFormat.Format32bppArgb:
                    bitPerPixel = 32;
                    break;
            }

            unsafe
            {
                // ссылка на область памяти
                byte* imagePointer1 = (byte*)bmpData.Scan0;

                for (int i = 0; i < bmpData.Height; i++)
                {
                    // для простых изображений
                    if (bitPerPixel == 24 || bitPerPixel == 32)
                    {
                        for (int j = 0; j < bmpData.Width; j++)
                        {
                            if (bitPerPixel == 24 || bitPerPixel == 32)
                            {
                                int grayScale = MixColorToGreyScale(imagePointer1[0], imagePointer1[1], imagePointer1[2]);

                                GrayArray[i, j] = (byte)(grayScale);
                            }
                            imagePointer1 += (bitPerPixel / 8);
                        }//end for j
                        imagePointer1 += bmpData.Stride - (bmpData.Width * (bitPerPixel / 8));
                    }

                    //для индексируемых
                    if (bitPerPixel == 1 || bitPerPixel == 4 || bitPerPixel == 8)
                    {
                        //получим палитру:
                        ColorPalette colPal = sourceBMP.Palette;

                        int leftByte = bmpData.Stride; // с каждым вычислением точек, будем уменьшать количество оставшихся байт
                        int leftPoint = PixelWidth;    // определение количества оставшихся точек
                        int leftBits = 8;              // определение количества оставщихся бит
                        int currJ = 0;



                        //******************************
                        // varian 1
                        //*******************************

                        int[] colorPoInts = new[] { 0, 0, 0, 0, 0, 0, 0, 0 };

                        while (leftPoint > 0)
                        {
                            int tmp = 0;

                            while (tmp < (8 / bitPerPixel))
                            {
                                int colorNumber = imagePointer1[0];

                                if (bitPerPixel == 1) colorNumber = (colorNumber & 128 / ((int)Math.Pow(2, tmp))) >> (7 - tmp);

                                if (bitPerPixel == 4 && tmp == 0) colorNumber = (colorNumber & 240) >> 4;
                                //    colorNumber1 = (colorNumber1 & 240)>>4; // применяем маску 1111 0000

                                if (bitPerPixel == 4 && tmp == 1) colorNumber = colorNumber & 15;
                                //    colorNumber1 = (colorNumber1 & 240)>>4; // применяем маску 1111 0000

                                if (bitPerPixel == 8) colorNumber = imagePointer1[0];


                                colorPoInts[tmp] = MixColorToGreyScale(colPal.Entries[colorNumber].R, colPal.Entries[colorNumber].G, colPal.Entries[colorNumber].B);

                                if (currJ < PixelWidth)
                                {
                                    GrayArray[i, currJ] = (byte)colorPoInts[tmp];
                                    currJ++;
                                    leftPoint--;
                                }


                                tmp++;
                            }

                            imagePointer1++;
                            leftByte--;





                            //************************************
                            //varian 2
                            //************************************


                            //if (bitPerPixel == 8)
                            //{
                            //    int colorNumber = imagePointer1[0];

                            //    int grayScale = MixColorToGreyScale(colPal.Entries[colorNumber].R, colPal.Entries[colorNumber].G, colPal.Entries[colorNumber].B);

                            //    GrayArray[i, currJ] = (byte)grayScale;

                            //    currJ++;
                            //    imagePointer1++;
                            //    leftPoint--;
                            //    leftByte--;
                            //}


                            //if (bitPerPixel == 4)
                            //{
                            //    int colorNumber1 = imagePointer1[0];
                            //    int colorNumber2 = imagePointer1[0];

                            //    colorNumber1 = (colorNumber1 & 240)>>4; // применяем маску 1111 0000
                            //    colorNumber2 = colorNumber2 & 15;  // применяем маску 0000 1111 


                            //    int Color1R = colPal.Entries[colorNumber1].R;
                            //    int Color1G = colPal.Entries[colorNumber1].G;
                            //    int Color1B = colPal.Entries[colorNumber1].B;

                            //    int Color2R = colPal.Entries[colorNumber2].R;
                            //    int Color2G = colPal.Entries[colorNumber2].G;
                            //    int Color2B = colPal.Entries[colorNumber2].B;


                            //    int grayScale1 =
                            //        (int)((Color1R * 0.3) + (Color1G * 0.59) + (Color1B * 0.11));

                            //    int grayScale2 =
                            //        (int)((Color2R * 0.3) + (Color2G * 0.59) + (Color2B * 0.11));

                            //    GrayArray[i, currJ] = (byte)grayScale1;
                            //    currJ ++;

                            //    if (currJ < PixelWidth) GrayArray[i, currJ] = (byte)grayScale2;

                            //    currJ++;
                            //    imagePointer1++;

                            //    leftPoint-=2;
                            //    leftByte--;
                            //}

                            //if (bitPerPixel == 1)
                            //{



                            //    int colorNumber1 = imagePointer1[0];
                            //    int colorNumber2 = imagePointer1[0];
                            //    int colorNumber3 = imagePointer1[0];
                            //    int colorNumber4 = imagePointer1[0];
                            //    int colorNumber5 = imagePointer1[0];
                            //    int colorNumber6 = imagePointer1[0];
                            //    int colorNumber7 = imagePointer1[0];
                            //    int colorNumber8 = imagePointer1[0];

                            //    colorNumber1 = (colorNumber1 & 128) >> 7; // применяем маску 1000 0000
                            //    colorNumber2 = (colorNumber2 &  64) >> 6; // применяем маску 0100 0000
                            //    colorNumber3 = (colorNumber3 &  32) >> 5; // применяем маску 0010 0000
                            //    colorNumber4 = (colorNumber4 &  16) >> 4; // применяем маску 0001 0000
                            //    colorNumber5 = (colorNumber5 &   8) >> 3; // применяем маску 0000 1000
                            //    colorNumber6 = (colorNumber6 &   4) >> 2; // применяем маску 0000 0100
                            //    colorNumber7 = (colorNumber7 &   2) >> 1; // применяем маску 0000 0010
                            //    colorNumber8 = (colorNumber8 &   1)     ; // применяем маску 0000 0001


                            //    int Color1R = colPal.Entries[colorNumber1].R;
                            //    int Color1G = colPal.Entries[colorNumber1].G;
                            //    int Color1B = colPal.Entries[colorNumber1].B;

                            //    int Color2R = colPal.Entries[colorNumber2].R;
                            //    int Color2G = colPal.Entries[colorNumber2].G;
                            //    int Color2B = colPal.Entries[colorNumber2].B;

                            //    int Color3R = colPal.Entries[colorNumber3].R;
                            //    int Color3G = colPal.Entries[colorNumber3].G;
                            //    int Color3B = colPal.Entries[colorNumber3].B;

                            //    int Color4R = colPal.Entries[colorNumber4].R;
                            //    int Color4G = colPal.Entries[colorNumber4].G;
                            //    int Color4B = colPal.Entries[colorNumber4].B;

                            //    int Color5R = colPal.Entries[colorNumber5].R;
                            //    int Color5G = colPal.Entries[colorNumber5].G;
                            //    int Color5B = colPal.Entries[colorNumber5].B;

                            //    int Color6R = colPal.Entries[colorNumber6].R;
                            //    int Color6G = colPal.Entries[colorNumber6].G;
                            //    int Color6B = colPal.Entries[colorNumber6].B;

                            //    int Color7R = colPal.Entries[colorNumber7].R;
                            //    int Color7G = colPal.Entries[colorNumber7].G;
                            //    int Color7B = colPal.Entries[colorNumber7].B;

                            //    int Color8R = colPal.Entries[colorNumber8].R;
                            //    int Color8G = colPal.Entries[colorNumber8].G;
                            //    int Color8B = colPal.Entries[colorNumber8].B;


                            //    int grayScale1 = (int)((Color1R * 0.3) + (Color1G * 0.59) + (Color1B * 0.11));
                            //    int grayScale2 = (int)((Color2R * 0.3) + (Color2G * 0.59) + (Color2B * 0.11));
                            //    int grayScale3 = (int)((Color3R * 0.3) + (Color3G * 0.59) + (Color3B * 0.11));
                            //    int grayScale4 = (int)((Color4R * 0.3) + (Color4G * 0.59) + (Color4B * 0.11));
                            //    int grayScale5 = (int)((Color5R * 0.3) + (Color5G * 0.59) + (Color5B * 0.11));
                            //    int grayScale6 = (int)((Color6R * 0.3) + (Color6G * 0.59) + (Color6B * 0.11));
                            //    int grayScale7 = (int)((Color7R * 0.3) + (Color7G * 0.59) + (Color7B * 0.11));
                            //    int grayScale8 = (int)((Color8R * 0.3) + (Color8G * 0.59) + (Color8B * 0.11));

                            //    if (currJ < PixelWidth)
                            //    {
                            //        GrayArray[i, currJ] = (byte)grayScale1;
                            //        currJ++;
                            //        leftPoint--;
                            //    }

                            //    if (currJ < PixelWidth)
                            //    {
                            //        GrayArray[i, currJ] = (byte)grayScale2;
                            //        currJ++;
                            //        leftPoint--;
                            //    }

                            //    if (currJ < PixelWidth)
                            //    {
                            //        GrayArray[i, currJ] = (byte)grayScale3;
                            //        currJ++;
                            //        leftPoint--;
                            //    }

                            //    if (currJ < PixelWidth)
                            //    {
                            //        GrayArray[i, currJ] = (byte)grayScale4;
                            //        currJ++;
                            //        leftPoint--;
                            //    }

                            //    if (currJ < PixelWidth)
                            //    {
                            //        GrayArray[i, currJ] = (byte)grayScale5;
                            //        currJ++;
                            //        leftPoint--;
                            //    }

                            //    if (currJ < PixelWidth)
                            //    {
                            //        GrayArray[i, currJ] = (byte)grayScale6;
                            //        currJ++;
                            //        leftPoint--;
                            //    }

                            //    if (currJ < PixelWidth)
                            //    {
                            //        GrayArray[i, currJ] = (byte)grayScale7;
                            //        currJ++;
                            //        leftPoint--;
                            //    }

                            //    if (currJ < PixelWidth)
                            //    {
                            //        GrayArray[i, currJ] = (byte)grayScale8;
                            //        currJ++;
                            //        leftPoint--;
                            //    }

                            //    imagePointer1++;
                            //    leftByte--;
                            //}





                        }
                        imagePointer1 += leftByte;
                    }



                }//end for i
            }//end unsafe
            sourceBMP.UnlockBits(bmpData);


            return GrayArray;
        }

        public static Bitmap BinaryArrayToBinaryBitmap(Byte[,] binaryArray)
        {

            Int32 PixelHeight = binaryArray.GetLength(0);
            Int32 PixelWidth = binaryArray.GetLength(1);

            Bitmap returnMap = new Bitmap(PixelWidth, PixelHeight, PixelFormat.Format24bppRgb);
            BitmapData bitmapData2 = returnMap.LockBits(new Rectangle(0, 0, returnMap.Width, returnMap.Height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);

            unsafe
            {
                byte* imagePointer2 = (byte*)bitmapData2.Scan0;
                for (int i = 0; i < PixelHeight; i++)
                {
                    for (int j = 0; j < PixelWidth; j++)
                    {

                        imagePointer2[0] = binaryArray[i, j];
                        imagePointer2[1] = binaryArray[i, j];
                        imagePointer2[2] = binaryArray[i, j];

                        imagePointer2 += 3;
                    }//end for j
                    imagePointer2 += bitmapData2.Stride - (bitmapData2.Width * 3);
                }//end for i
            }//end unsafe
            returnMap.UnlockBits(bitmapData2);

            return returnMap;
        }







        public static Bitmap Skeletonization(Bitmap bmpSource)
        {

            int Threshold = 0;

            Byte[,] m_SourceImage = ToByteArray(ref bmpSource);

            Byte[,] m_DesImage = Thining.ThinPicture(m_SourceImage);

            Bitmap ppp = BinaryArrayToBinaryBitmap(m_DesImage);

            //Bitmap ppp = Thining.BinaryArrayToBinaryBitmap(m_DesImage);


            //Bitmap bit8 = CopyToBpp(ppp, 8); //в начале сконвертируем в 8 битное

            //Bitmap returnMap = new Bitmap(ppp.Width, ppp.Height, PixelFormat.Format24bppRgb);

            //Convert(ppp, returnMap);

            //return returnMap;

            return ppp;
        }


        /// <summary>
        /// http://www.wischik.com/lu/programmer/1bpp.html
        /// Copies a bitmap into a 1bpp/8bpp bitmap of the same dimensions, fast
        /// </summary>
        /// <param name="b">original bitmap</param>
        /// <param name="bpp">1 or 8, target bpp</param>
        /// <returns>a 1bpp copy of the bitmap</returns>
        static System.Drawing.Bitmap CopyToBpp(System.Drawing.Bitmap b, int bpp)
        {
            if (bpp != 1 && bpp != 8) throw new System.ArgumentException("1 or 8", "bpp");

            // Plan: built into Windows GDI is the ability to convert
            // bitmaps from one format to another. Most of the time, this
            // job is actually done by the graphics hardware accelerator card
            // and so is extremely fast. The rest of the time, the job is done by
            // very fast native code.
            // We will call into this GDI functionality from C#. Our plan:
            // (1) Convert our Bitmap into a GDI hbitmap (ie. copy unmanaged->managed)
            // (2) Create a GDI monochrome hbitmap
            // (3) Use GDI "BitBlt" function to copy from hbitmap into monochrome (as above)
            // (4) Convert the monochrone hbitmap into a Bitmap (ie. copy unmanaged->managed)

            int w = b.Width, h = b.Height;
            IntPtr hbm = b.GetHbitmap(); // this is step (1)
                                         //
                                         // Step (2): create the monochrome bitmap.
                                         // "BITMAPINFO" is an interop-struct which we define below.
                                         // In GDI terms, it's a BITMAPHEADERINFO followed by an array of two RGBQUADs
            BITMAPINFO bmi = new BITMAPINFO();
            bmi.biSize = 40;  // the size of the BITMAPHEADERINFO struct
            bmi.biWidth = w;
            bmi.biHeight = h;
            bmi.biPlanes = 1; // "planes" are confusing. We always use just 1. Read MSDN for more info.
            bmi.biBitCount = (short)bpp; // ie. 1bpp or 8bpp
            bmi.biCompression = BI_RGB; // ie. the pixels in our RGBQUAD table are stored as RGBs, not palette indexes
            bmi.biSizeImage = (uint)(((w + 7) & 0xFFFFFFF8) * h / 8);
            bmi.biXPelsPerMeter = 1000000; // not really important
            bmi.biYPelsPerMeter = 1000000; // not really important
                                           // Now for the colour table.
            uint ncols = (uint)1 << bpp; // 2 colours for 1bpp; 256 colours for 8bpp
            bmi.biClrUsed = ncols;
            bmi.biClrImportant = ncols;
            bmi.cols = new uint[256]; // The structure always has fixed size 256, even if we end up using fewer colours
            if (bpp == 1) { bmi.cols[0] = MAKERGB(0, 0, 0); bmi.cols[1] = MAKERGB(255, 255, 255); }
            else { for (int i = 0; i < ncols; i++) bmi.cols[i] = MAKERGB(i, i, i); }
            // For 8bpp we've created an palette with just greyscale colours.
            // You can set up any palette you want here. Here are some possibilities:
            // greyscale: for (int i=0; i<256; i++) bmi.cols[i]=MAKERGB(i,i,i);
            // rainbow: bmi.biClrUsed=216; bmi.biClrImportant=216; int[] colv=new int[6]{0,51,102,153,204,255};
            //          for (int i=0; i<216; i++) bmi.cols[i]=MAKERGB(colv[i/36],colv[(i/6)%6],colv[i%6]);
            // optimal: a difficult topic: http://en.wikipedia.org/wiki/Color_quantization
            // 
            // Now create the indexed bitmap "hbm0"
            IntPtr bits0; // not used for our purposes. It returns a pointer to the raw bits that make up the bitmap.
            IntPtr hbm0 = CreateDIBSection(IntPtr.Zero, ref bmi, DIB_RGB_COLORS, out bits0, IntPtr.Zero, 0);
            //
            // Step (3): use GDI's BitBlt function to copy from original hbitmap into monocrhome bitmap
            // GDI programming is kind of confusing... nb. The GDI equivalent of "Graphics" is called a "DC".
            IntPtr sdc = GetDC(IntPtr.Zero);       // First we obtain the DC for the screen
                                                   // Next, create a DC for the original hbitmap
            IntPtr hdc = CreateCompatibleDC(sdc); SelectObject(hdc, hbm);
            // and create a DC for the monochrome hbitmap
            IntPtr hdc0 = CreateCompatibleDC(sdc); SelectObject(hdc0, hbm0);
            // Now we can do the BitBlt:
            BitBlt(hdc0, 0, 0, w, h, hdc, 0, 0, SRCCOPY);
            // Step (4): convert this monochrome hbitmap back into a Bitmap:
            System.Drawing.Bitmap b0 = System.Drawing.Bitmap.FromHbitmap(hbm0);
            //
            // Finally some cleanup.
            DeleteDC(hdc);
            DeleteDC(hdc0);
            ReleaseDC(IntPtr.Zero, sdc);
            DeleteObject(hbm);
            DeleteObject(hbm0);
            //
            return b0;
        }

        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr hObject);

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern int InvalidateRect(IntPtr hwnd, IntPtr rect, int bErase);

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern IntPtr GetDC(IntPtr hwnd);

        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        public static extern IntPtr CreateCompatibleDC(IntPtr hdc);

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern int ReleaseDC(IntPtr hwnd, IntPtr hdc);

        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        public static extern int DeleteDC(IntPtr hdc);

        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        public static extern IntPtr SelectObject(IntPtr hdc, IntPtr hgdiobj);

        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        public static extern int BitBlt(IntPtr hdcDst, int xDst, int yDst, int w, int h, IntPtr hdcSrc, int xSrc, int ySrc, int rop);
        static int SRCCOPY = 0x00CC0020;

        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        static extern IntPtr CreateDIBSection(IntPtr hdc, ref BITMAPINFO bmi, uint Usage, out IntPtr bits, IntPtr hSection, uint dwOffset);
        static uint BI_RGB = 0;
        static uint DIB_RGB_COLORS = 0;
        [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
        public struct BITMAPINFO
        {
            public uint biSize;
            public int biWidth, biHeight;
            public short biPlanes, biBitCount;
            public uint biCompression, biSizeImage;
            public int biXPelsPerMeter, biYPelsPerMeter;
            public uint biClrUsed, biClrImportant;
            [System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst = 256)]
            public uint[] cols;
        }

        static uint MAKERGB(int r, int g, int b)
        {
            return ((uint)(b & 255)) | ((uint)((r & 255) << 8)) | ((uint)((g & 255) << 16));
        }



        private static unsafe void Convert(Bitmap src, Bitmap conv)
        {
            // Lock source and destination in memory for unsafe access
            var bmbo = src.LockBits(new Rectangle(0, 0, src.Width, src.Height), ImageLockMode.ReadOnly,
                                     src.PixelFormat);
            var bmdn = conv.LockBits(new Rectangle(0, 0, conv.Width, conv.Height), ImageLockMode.ReadWrite,
                                     conv.PixelFormat);

            var srcScan0 = bmbo.Scan0;
            var convScan0 = bmdn.Scan0;

            var srcStride = bmbo.Stride;
            var convStride = bmdn.Stride;

            byte* sourcePixels = (byte*)(void*)srcScan0;
            byte* destPixels = (byte*)(void*)convScan0;

            var srcLineIdx = 0;
            var convLineIdx = 0;
            var hmax = src.Height - 1;
            var wmax = src.Width - 1;
            for (int y = 0; y < hmax; y++)
            {
                // find indexes for source/destination lines

                // use addition, not multiplication?
                srcLineIdx += srcStride;
                convLineIdx += convStride;

                var srcIdx = srcLineIdx;
                for (int x = 0; x < wmax; x++)
                {
                    // index for source pixel (32bbp, rgba format)
                    srcIdx += 4;
                    //var r = pixel[2];
                    //var g = pixel[1];
                    //var b = pixel[0];

                    // could just check directly?
                    //if (Color.FromArgb(r,g,b).GetBrightness() > 0.01f)
                    if (!(sourcePixels[srcIdx] == 0 && sourcePixels[srcIdx + 1] == 0 && sourcePixels[srcIdx + 2] == 0))
                    {
                        // destination byte for pixel (1bpp, ie 8pixels per byte)
                        var idx = convLineIdx + (x >> 3);
                        // mask out pixel bit in destination byte
                        destPixels[idx] |= (byte)(0x80 >> (x & 0x7));
                    }
                }
            }
            src.UnlockBits(bmbo);
            conv.UnlockBits(bmdn);
        }


        /// <summary>
        /// Resizes an image to a certain height
        /// </summary>
        /// <param name="Image">Image to resize</param>
        /// <param name="Width">New width for the final image</param>
        /// <param name="Height">New height for the final image</param>
        /// <param name="Quality">Quality of the resizing</param>
        /// <returns>A bitmap object of the resized image</returns>
        public static Bitmap ResizeImage(Bitmap Image, int Width, int Height, bool Quality = false)
        {
            Bitmap NewBitmap = new Bitmap(Width, Height,PixelFormat.Format24bppRgb);
            using (Graphics NewGraphics = Graphics.FromImage(NewBitmap))
            {
                if (Quality)
                {
                    NewGraphics.CompositingQuality = CompositingQuality.HighQuality;
                    NewGraphics.SmoothingMode = SmoothingMode.HighQuality;
                    NewGraphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                }
                else
                {
                    NewGraphics.CompositingQuality = CompositingQuality.HighSpeed;
                    NewGraphics.SmoothingMode = SmoothingMode.HighSpeed;
                    NewGraphics.InterpolationMode = InterpolationMode.NearestNeighbor;
                }
                NewGraphics.DrawImage(Image, new System.Drawing.Rectangle(0, 0, Width, Height));
            }
            return CheckAndConvertImageto24bitPerPixel(NewBitmap);
        }

        public static Bitmap ConvertToGrayScale(Bitmap Image)
        {
            if (Image.PixelFormat != PixelFormat.Format24bppRgb)
            {
                throw new UnsupportedImageFormatException("Оппа! Не поддерживаемый формат изображения!!! (001)");
            }


            Bitmap returnMap = new Bitmap(Image.Width, Image.Height, PixelFormat.Format24bppRgb);
            BitmapData bitmapData1 = Image.LockBits(new Rectangle(0, 0, Image.Width, Image.Height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
            BitmapData bitmapData2 = returnMap.LockBits(new Rectangle(0, 0, returnMap.Width, returnMap.Height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
            int a = 0;
            unsafe
            {
                byte* imagePointer1 = (byte*)bitmapData1.Scan0;
                byte* imagePointer2 = (byte*)bitmapData2.Scan0;
                for (int i = 0; i < bitmapData1.Height; i++)
                {
                    for (int j = 0; j < bitmapData1.Width; j++)
                    {
                        // write the logic implementation here
                        a = (imagePointer1[0] + imagePointer1[1] + imagePointer1[2]) / 3;
                        imagePointer2[0] = (byte)a;
                        imagePointer2[1] = (byte)a;
                        imagePointer2[2] = (byte)a;

                        imagePointer1 += 3;
                        imagePointer2 += 3;
                    }//end for j
                    imagePointer1 += bitmapData1.Stride - (bitmapData1.Width * 3);
                    imagePointer2 += bitmapData1.Stride - (bitmapData1.Width * 3);
                }//end for i
            }//end unsafe
            returnMap.UnlockBits(bitmapData2);
            Image.UnlockBits(bitmapData1);
            return returnMap;
        }





        public static Bitmap CheckAndConvertImageto24bitPerPixel(Bitmap sourceBMP)
        {
            // Работать будем с 24-битным изображением
            if (sourceBMP.PixelFormat == PixelFormat.Format24bppRgb) return sourceBMP;

            // Необходимо сконвертировать в необходимый формат
            Bitmap returnMap = new Bitmap(sourceBMP.Width, sourceBMP.Height, PixelFormat.Format24bppRgb);

            int bytePerPixel = 0;

            if (sourceBMP.PixelFormat == PixelFormat.Format32bppArgb) bytePerPixel = 4;

            if (sourceBMP.PixelFormat == PixelFormat.Format8bppIndexed) bytePerPixel = 1;

            //todo: add exceptions
            if (bytePerPixel == 0)
            {
                using (Bitmap bmp1 = sourceBMP)
                {
                    BitmapData data = bmp1.LockBits(new Rectangle(0, 0, bmp1.Width, bmp1.Height),
                        System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                    returnMap = new Bitmap(bmp1.Width, bmp1.Height, data.Stride, data.PixelFormat, data.Scan0);

                    bmp1.UnlockBits(data);
                    return returnMap;
                }


                throw new UnsupportedImageFormatException("Не поддерживаемый формат изображения!!!.");



                return null;
            }

            // исходное изображение
            BitmapData bitmapData1 = sourceBMP.LockBits(new Rectangle(0, 0, sourceBMP.Width, sourceBMP.Height), ImageLockMode.ReadOnly, sourceBMP.PixelFormat);
            // итоговое изображение
            BitmapData bitmapData2 = returnMap.LockBits(new Rectangle(0, 0, returnMap.Width, returnMap.Height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);

            //int a = 0;

            unsafe
            {
                // ссылка на область памяти
                byte* imagePointer1 = (byte*)bitmapData1.Scan0;
                // ссылка на область памяти
                byte* imagePointer2 = (byte*)bitmapData2.Scan0;

                for (int i = 0; i < bitmapData1.Height; i++)
                {
                    for (int j = 0; j < bitmapData1.Width; j++)
                    {
                        if (bytePerPixel == 1)
                        {
                            imagePointer2[0] = imagePointer1[0];
                            imagePointer2[1] = imagePointer1[0];
                            imagePointer2[2] = imagePointer1[0];
                            //imagePointer2[3] = imagePointer1[3]; //для 32-х битного
                            imagePointer1 += 1;
                            imagePointer2 += 3;
                        }

                        if (bytePerPixel == 4)
                        {
                            imagePointer2[0] = imagePointer1[0];
                            imagePointer2[1] = imagePointer1[1];
                            imagePointer2[2] = imagePointer1[2];
                            //imagePointer2[3] = imagePointer1[3]; //для 32-х битного
                            imagePointer1 += 4;
                            imagePointer2 += 3;

                        }
                    }//end for j

                    if (bytePerPixel == 1)
                    {
                        imagePointer1 += bitmapData1.Stride - (bitmapData1.Width * 1);
                        imagePointer2 += bitmapData2.Stride - (bitmapData1.Width * 3);
                    }

                    if (bytePerPixel == 4)
                    {
                        imagePointer1 += bitmapData1.Stride - (bitmapData1.Width * 4);
                        imagePointer2 += bitmapData2.Stride - (bitmapData1.Width * 3);
                    }
                }//end for i
            }//end unsafe
            returnMap.UnlockBits(bitmapData2);
            sourceBMP.UnlockBits(bitmapData1);
            return returnMap;
        }


        public static Bitmap ConvertTo8Bit(Bitmap sourceBMP)
        {
            // нет смысла конвертировать
            if (sourceBMP.PixelFormat == PixelFormat.Format8bppIndexed) return sourceBMP;

            // Необходимо сконвертировать в необходимый формат
            Bitmap returnMap = new Bitmap(sourceBMP.Width, sourceBMP.Height, PixelFormat.Format8bppIndexed);

            //todo: add exceptions
            if (!(sourceBMP.PixelFormat == PixelFormat.Format24bppRgb || sourceBMP.PixelFormat == PixelFormat.Format32bppRgb || sourceBMP.PixelFormat == PixelFormat.Format32bppArgb))
            {
                throw new UnsupportedImageFormatException("На текущий момент изображение имеющее pixelFormat: " + sourceBMP.PixelFormat.ToString() + @" не поддерживатется!");
                return null;
            }

            // исходное изображение
            BitmapData bitmapData1 = sourceBMP.LockBits(new Rectangle(0, 0, sourceBMP.Width, sourceBMP.Height), ImageLockMode.ReadOnly, sourceBMP.PixelFormat);
            // итоговое изображение
            BitmapData bitmapData2 = returnMap.LockBits(new Rectangle(0, 0, returnMap.Width, returnMap.Height), ImageLockMode.ReadOnly, PixelFormat.Format8bppIndexed);

            unsafe
            {
                // ссылка на область памяти
                byte* imagePointer1 = (byte*)bitmapData1.Scan0;
                // ссылка на область памяти
                byte* imagePointer2 = (byte*)bitmapData2.Scan0;

                for (int i = 0; i < bitmapData1.Height; i++)
                {
                    for (int j = 0; j < bitmapData1.Width; j++)
                    {

                        if (sourceBMP.PixelFormat == PixelFormat.Format24bppRgb)
                        {
                            imagePointer2[0] = (byte)((imagePointer1[0] + imagePointer1[1] + imagePointer1[2]) / 3);
                            imagePointer1 += 3;
                        }

                        if (sourceBMP.PixelFormat == PixelFormat.Format32bppRgb)
                        {
                            imagePointer2[0] = (byte)((imagePointer1[0] + imagePointer1[1] + imagePointer1[2]) / 3);
                            imagePointer1 += 4;
                        }

                        if (sourceBMP.PixelFormat == PixelFormat.Format32bppArgb)
                        {
                            imagePointer2[0] = (byte)((imagePointer1[0] + imagePointer1[1] + imagePointer1[2]) / 3);
                            imagePointer1 += 4;
                        }

                        imagePointer2 += 1;

                    }//end for j

                    if (sourceBMP.PixelFormat == PixelFormat.Format24bppRgb) imagePointer1 += bitmapData1.Stride - (bitmapData1.Width * 3);
                    if (sourceBMP.PixelFormat == PixelFormat.Format32bppRgb) imagePointer1 += bitmapData1.Stride - (bitmapData1.Width * 4);
                    if (sourceBMP.PixelFormat == PixelFormat.Format32bppArgb) imagePointer1 += bitmapData1.Stride - (bitmapData1.Width * 4);

                    imagePointer2 += bitmapData2.Stride - (bitmapData1.Width * 1);
                    
                }//end for i
            }//end unsafe
            returnMap.UnlockBits(bitmapData2);
            sourceBMP.UnlockBits(bitmapData1);
            return returnMap;
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
                        //throw;
                        return new List<GroupPoint>();
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


            Lines = Rotate(Lines);

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


        public static List<cncPoint> ListCncPointClone(List<cncPoint> source)
        {
            List<cncPoint> tmp = new List<cncPoint>();

            if (source == null) return tmp;

            foreach (cncPoint VARIABLE in source)
            {
                tmp.Add(VARIABLE.Clone());
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



            if (Settings.Default.page01AxisVariant == 1)
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
                        points.Add(new cncPoint(point.X, (-point.Y) + delta, point.Svalue, point.Fvalue, point.Pvalue, point.Selected,point.Bright,point.Gvalue));
                    }
                    tmp.Add(new GroupPoint(points, vector.Selected,vector.Dirrect,vector.IndividualPoints));
                    points = new List<cncPoint>();
                }

                returnValue = tmp;

                vectors = new List<GroupPoint>();
                points = new List<cncPoint>();

            }


            if (Settings.Default.page01AxisVariant == 2)
            {
                //ничего не делаем
                foreach (GroupPoint VARIABLE in dataCVectors)
                {
                    returnValue.Add(VARIABLE.Clone());
                }
            }


            if (Settings.Default.page01AxisVariant == 3)
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


            if (Settings.Default.page01AxisVariant == 4)
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
    



            if (Settings.Default.page01AxisVariant == 1)
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


            if (Settings.Default.page01AxisVariant == 2)
            {
                //ничего не делаем
            }

            if (Settings.Default.page01AxisVariant == 3)
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


            if (Settings.Default.page01AxisVariant == 4)
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







            Vectors = Rotate(Vectors);










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