// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Windows.Forms;

namespace ToolsGenGkode.pages
{
    public partial class page02_EnterText : UserControl, PageInterface
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

        public page02_EnterText()
        {
            InitializeComponent();

            PageName = @"Ввод текста (2)";
            LastPage = 1;
            CurrPage = 2;
            NextPage = 6;

            if (pageImageIN != null) pageImageNOW = (Bitmap)pageImageIN.Clone();
            pageVectorNOW = GlobalFunctions.pageVectorClone(pageVectorIN);

            toolTips myToolTip1 = new toolTips();

            myToolTip1.Size = new Size(300, 200);
            myToolTip1.BackColor = Color.FromArgb(255, 255, 192);
            myToolTip1.ForeColor = Color.Navy;
            myToolTip1.BorderColor = Color.FromArgb(128, 128, 255);

            myToolTip1.SetToolTip(rbUseSystemFont, "Используется шрифт установленный в данной операционной системе.");
            myToolTip1.SetToolTip(rbFontFromFile, "Используется шрифт из файла, выбранного пользователем.");
            myToolTip1.SetToolTip(rbFontToVector, "Выбор данной опции позволит получить данные в виде набора векторов.");
            myToolTip1.SetToolTip(rbFontToImage, "Выбор данной опции позволит получить данные в виде рисунка.");
            myToolTip1.SetToolTip(textSize, "Указание размера в этом поле,\n влияет на размер получаемого рисунка, если выбран вариант 'В виде рисунка',\n или на качество траектории, если выбрано 'В виде отрезков'.");
            
        }

        private void SelectFont_Load(object sender, EventArgs e)
        {
            // Заполним списком шрифтов установленных в систему
            InstalledFontCollection installedFontCollection = new InstalledFontCollection();

            foreach (FontFamily fnt in installedFontCollection.Families)
            {
                comboBoxFont.Items.Add(fnt.Name);
            }

            installedFontCollection.Dispose();

            comboBoxFont.Text = comboBoxFont.Items[0].ToString();

            rbUseSystemFont.Checked = true;
            rbFontToVector.Checked = true;
        }

        private void UserActions()
        {
            if (rbUseSystemFont.Checked)
            {
                buttonSetFontFile.Visible = false;
                nameFontFile.Visible      = false;
                comboBoxFont.Visible      = true;
            }
            else
            {
                buttonSetFontFile.Visible = true;
                nameFontFile.Visible      = true;
                comboBoxFont.Visible      = false;
            }

            if (rbFontToVector.Checked)
            {
                NextPage = 6;
            }
            else
            {
                NextPage = 9;
            }

            if (rbUseSystemFont.Checked) //используем системный шрифт
            {
                pageVectorNOW = GetVectorFromText(textString.Text, comboBoxFont.Text, (float)textSize.Value);
                pageImageNOW = CreateBitmapFromText(textString.Text, comboBoxFont.Text, (float)textSize.Value);

            }
            else  //используем внешний файл шрифта
            {
                pageVectorNOW = GetVectorFromText(textString.Text, comboBoxFont.Text, (float)textSize.Value, nameFontFile.Text);
                pageImageNOW = CreateBitmapFromText(textString.Text, comboBoxFont.Text, (float)textSize.Value, nameFontFile.Text);
            }


            //и замкнем траектории
            foreach (Segment vVector in pageVectorNOW)
            {
                vVector.Points.Add(vVector.Points[0]);
            }


            if (rbFontToImage.Checked) pageVectorNOW = new List<Segment>();
            if (rbFontToVector.Checked) pageImageNOW = null;


            CreateEvent("RefreshVector_02");
            CreateEvent("RefreshImage_02");
        }

        private void buttonSetFontFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofDialog = new OpenFileDialog
            {
                CheckFileExists = true,
                Multiselect = false,
                Title = @"Выбор файла шрифта",
                Filter = @"Font files (*.ttf)|*.ttf",
                FilterIndex = 2,
                RestoreDirectory = true
            };
            if (ofDialog.ShowDialog() == DialogResult.OK)
            {
                nameFontFile.Text = ofDialog.FileName;
                UserActions();
            }
        }

        private void rbUseSystemFont_CheckedChanged(object sender, EventArgs e)
        {
            UserActions();
        }

        private void rbFontFromFile_CheckedChanged(object sender, EventArgs e)
        {
            UserActions();
        }

        private void comboBoxFont_SelectedIndexChanged(object sender, EventArgs e)
        {
            UserActions();
        }

        private void rbFontToVector_CheckedChanged(object sender, EventArgs e)
        {
            UserActions();
        }

        private void rbFontToImage_CheckedChanged(object sender, EventArgs e)
        {
            UserActions();
        }

        private void textSize_ValueChanged(object sender, EventArgs e)
        {
            UserActions();
        }

        private void textString_TextChanged(object sender, EventArgs e)
        {
            UserActions();
        }

        /// <summary>
        /// Функция возвращает массив отрезков, которые и составляют рисунок шрифта
        /// </summary>
        /// <param name="text">Текст кторый нужно преобразовать в вектора</param>
        /// <param name="fontName">Имя шрифта</param>
        /// <param name="fontSize">Размер шрифта</param>
        /// <param name="extFileFont">Имя внешненего файла (если используется не системный шрифт)</param>
        /// <returns>Набор отрезков</returns>
        private List<Segment> GetVectorFromText(string text, string fontName, float fontSize, string extFileFont = "")
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
                    return new List<Segment>();
                }

                pts = path.PathPoints;
                ptsType = path.PathTypes;
            }

            //для сбора информации о точках у отрезка
            List<Location> ListPoints = new List<Location>();
            // для сбора информации об отрезках
            List<Segment> Lines = new List<Segment>();

            int index = 0;
            foreach (PointF value in pts)
            {
                byte ptypePoint = ptsType[index]; //тип точки: 0-точка является началом отрезка, 1-точка является продолжением отрезка, 129-161 и прочее окончанием отрезка, причем необходимо добавлять линию соединяющую начальную точку и конечную

                // это первая точка
                if (ptypePoint == 0)
                {
                    ListPoints.Add(new Location((decimal)value.X, (decimal)value.Y));
                }

                //а это продолжение
                if (ptypePoint == 1)
                {
                    ListPoints.Add(new Location((decimal)value.X, (decimal)value.Y));
                }

                //окончание
                if (ptypePoint == 129)
                {
                    ListPoints.Add(new Location((decimal)value.X, (decimal)value.Y));
                    ListPoints.Add(ListPoints[0]); //иногда не нужна
                    Lines.Add(new Segment(ListPoints));
                    ListPoints = new List<Location>();
                }

                if (ptypePoint == 161)
                {
                    ListPoints.Add(new Location((decimal)value.X, (decimal)value.Y));
                    //ListPoints.Add(ListPoints[0]);
                    Lines.Add(new Segment(ListPoints));
                    ListPoints = new List<Location>();
                }

                index++;
            }


            Lines = VectorUtilities.Rotate(Lines);

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

        // Создание рисунка из текста
        private Bitmap CreateBitmapFromText(string text, string fontName, float fontSize, string extFileFont = "")
        {
            //string imageText = textString.Text;

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


        public Bitmap pageImageIN { get; set; }
        public Bitmap pageImageNOW { get; set; }
        public List<Segment> pageVectorIN { get; set; }
        public List<Segment> pageVectorNOW { get; set; }
        public List<Location> PagePoints { get; set; }


        public void actionBefore()
        {
            //throw new System.NotImplementedException();
        }

        public void actionAfter()
        {
            //throw new System.NotImplementedException();
        }
    }
}
