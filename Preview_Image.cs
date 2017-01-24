// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

using System;
using System.Drawing;
using System.Windows.Forms;
using Cyotek.Windows.Forms;

namespace ToolsGenGkode
{
    public partial class Preview_Image : UserControl
    {

        public bool containsData;

        public Preview_Image()
        {
            InitializeComponent();
        }

        private void btZoom100_Click(object sender, EventArgs e)
        {
            pictureBoxPreview.Zoom = 100;
        }

        private void radioButton_Strech_CheckedChanged(object sender, EventArgs e)
        {
            pictureBoxPreview.SizeMode = ImageBoxSizeMode.Fit;
        }

        private void radioButton_FullSize_CheckedChanged(object sender, EventArgs e)
        {
            pictureBoxPreview.SizeMode = ImageBoxSizeMode.Normal;
        }

        private void pictureBoxPreview_ZoomChanged(object sender, EventArgs e)
        {
            RefreshInfo();
        }

        private void pictureBoxPreview_ImageChanged(object sender, EventArgs e)
        {
            RefreshInfo();
        }


        private void RefreshInfo()
        {

            labelZoomSize.Text = @"Масштаб " + pictureBoxPreview.Zoom + @"%";

            if (pictureBoxPreview.Image != null)
            {
                labelZoomSize.Text += @" размер: " + pictureBoxPreview.Image.Width + @"x" + pictureBoxPreview.Image.Height + @" пикселей";
            }
        }



        /// <summary>
        /// Функция дорисовывает оси
        /// </summary>
        /// <param name="bmp"></param>
        /// <returns></returns>
        private Bitmap DrawAxes(Bitmap bmp)
        {
           

            // Количество дополнительных пикселей, для рисования осей
            int sizeAxis = 30;

            // создаем новое изображение
            Bitmap image = new Bitmap(bmp.Width + sizeAxis, bmp.Height + sizeAxis);

            //теперь в него нужно скопировать данные

            using (Graphics grD = Graphics.FromImage(image))
            {
                Rectangle srcRegion = new Rectangle(0, 0, bmp.Width, bmp.Height);
                //переопределим позже
                Rectangle destRegion = new Rectangle();

                switch (Properties.Settings.Default.page01AxisVariant)
                {
                    case 1:
                        destRegion = new Rectangle(6, 0, bmp.Width, bmp.Height);
                        break;

                    case 2:
                        destRegion = new Rectangle(6, 6, bmp.Width, bmp.Height);
                        break;
                    case 3:
                        destRegion = new Rectangle(0, 6, bmp.Width, bmp.Height);
                        break;
                    case 4:
                        destRegion = new Rectangle(0, 0, bmp.Width, bmp.Height);
                        break;
                    default:
                        break;
                }

                // В новое изображение вставим переданное изображение, но со смещением, что-бы было место для рисования осей.
                grD.DrawImage(bmp, destRegion, srcRegion, GraphicsUnit.Pixel);

                int sizeX = bmp.Width + sizeAxis;
                int sizeY = bmp.Height + sizeAxis;

                //grD.DrawString("X", this.Font, Brushes.Black, bmp.Width+ 10, 6);
                //grD.DrawString("Y", this.Font, Brushes.Black, 6, bmp.Height + 10);

                switch (Properties.Settings.Default.page01AxisVariant)
                {
                    case 1:
                        grD.FillRectangle(Brushes.Beige, new Rectangle(0, sizeY - 5, sizeX, sizeY)); //полоска оси
                        grD.FillRectangle(Brushes.Beige, new Rectangle(0, 0, 5, sizeY));
                        grD.DrawLine(new Pen(Color.Blue), new Point(5, sizeY - 5), new Point(sizeX, sizeY - 5));
                        grD.DrawLine(new Pen(Color.Blue), new Point(5, sizeY - 5), new Point(5, 0));
                        grD.DrawLine(new Pen(Color.Blue), new Point(sizeX, sizeY - 5), new Point(sizeX - 8, sizeY - 5 + 2));
                        grD.DrawLine(new Pen(Color.Blue), new Point(5, 0), new Point(5 - 2, 0 + 8));

                        break;

                    case 2:
                        grD.FillRectangle(Brushes.Beige, new Rectangle(0, 0, sizeX, 5)); //полоска оси
                        grD.FillRectangle(Brushes.Beige, new Rectangle(0, 0, 5, sizeY));
                        grD.DrawLine(new Pen(Color.Blue), new Point(5, 5), new Point(sizeX, 5));
                        grD.DrawLine(new Pen(Color.Blue), new Point(5, 5), new Point(5, sizeY));
                        grD.DrawLine(new Pen(Color.Blue), new Point(sizeX, 5), new Point(sizeX - 8, 5 - 2));
                        grD.DrawLine(new Pen(Color.Blue), new Point(5, sizeY), new Point(5 - 2, sizeY - 8));

                        break;
                    case 3:
                        grD.FillRectangle(Brushes.Beige, new Rectangle(0, 0, sizeX, 5)); //полоска оси
                        grD.FillRectangle(Brushes.Beige, new Rectangle(sizeX - 5, 0, sizeX, sizeY));
                        grD.DrawLine(new Pen(Color.Blue), new Point(sizeX - 5, 5), new Point(0, 5));
                        grD.DrawLine(new Pen(Color.Blue), new Point(sizeX - 5, 5), new Point(sizeX - 5, sizeY));
                        grD.DrawLine(new Pen(Color.Blue), new Point(0, 5), new Point(0 + 8, 5 - 2));
                        grD.DrawLine(new Pen(Color.Blue), new Point(sizeX - 5, sizeY), new Point(sizeX - 5 + 2, sizeY - 8));

                        break;
                    case 4:
                        grD.FillRectangle(Brushes.Beige, new Rectangle(0, sizeY - 5, sizeX, sizeY)); //полоска оси
                        grD.FillRectangle(Brushes.Beige, new Rectangle(sizeX - 5, 0, sizeX, sizeY));
                        grD.DrawLine(new Pen(Color.Blue), new Point(sizeX - 5, sizeY - 5), new Point(0, sizeY - 5));
                        grD.DrawLine(new Pen(Color.Blue), new Point(sizeX - 5, sizeY - 5), new Point(sizeX - 5, 0));
                        grD.DrawLine(new Pen(Color.Blue), new Point(0, sizeY - 5), new Point(0 + 8, sizeY - 5 + 2));
                        grD.DrawLine(new Pen(Color.Blue), new Point(sizeX - 5, 0), new Point(sizeX - 5 + 2, 0 + 8));

                        break;

                    default:
                        break;
                }

                // FOR XXX
                if (Properties.Settings.Default.page01AxisVariant == 1 || Properties.Settings.Default.page01AxisVariant == 2)
                {
                    int st0 = 0;
                    int st1 = 5;
                    int st2 = 3;

                    if (Properties.Settings.Default.page01AxisVariant == 1)
                    {
                        st0 = sizeY;
                        st1 = sizeY - 5;
                        st2 = sizeY - 3;
                    }

                    for (int i = 5; i < sizeX; i += 10)
                    {
                        grD.DrawLine(new Pen(Color.Blue), new Point(i, st1), new Point(i, st2));
                    }

                    for (int i = 105; i < sizeX; i += 100)
                    {
                        grD.DrawLine(new Pen(Color.Chocolate), new Point(i, st0), new Point(i, st1));
                    }
                }

                if (Properties.Settings.Default.page01AxisVariant == 3 || Properties.Settings.Default.page01AxisVariant == 4)
                {
                    int st0 = 0;

                    int st1 = 5;
                    int st2 = 3;

                    if (Properties.Settings.Default.page01AxisVariant == 4)
                    {
                        st0 = sizeY;
                        st1 = sizeY - 5;
                        st2 = sizeY - 3;
                    }

                    for (int i = sizeX - 5; i > 0; i -= 10)
                    {
                        grD.DrawLine(new Pen(Color.Blue), new Point(i, st1), new Point(i, st2));
                    }

                    for (int i = sizeX - 105; i > 0; i -= 100)
                    {
                        grD.DrawLine(new Pen(Color.Chocolate), new Point(i, st0), new Point(i, st1));
                    }
                }

                //FOR YYYY
                if (Properties.Settings.Default.page01AxisVariant == 1 || Properties.Settings.Default.page01AxisVariant == 4)
                {
                    int st0 = 0;
                    int st1 = 5;
                    int st2 = 3;

                    if (Properties.Settings.Default.page01AxisVariant == 4)
                    {
                        st0 = sizeX;
                        st1 = sizeX - 5;
                        st2 = sizeX - 3;
                    }

                    for (int i = sizeY - 5; i > 0; i -= 10)
                    {
                        grD.DrawLine(new Pen(Color.Blue), new Point(st1, i), new Point(st2, i));
                    }

                    for (int i = sizeY - 105; i > 0; i -= 100)
                    {
                        grD.DrawLine(new Pen(Color.Chocolate), new Point(st0, i), new Point(st1, i));
                    }
                }

                if (Properties.Settings.Default.page01AxisVariant == 2 || Properties.Settings.Default.page01AxisVariant == 3)
                {
                    int st0 = 0;

                    int st1 = 5;
                    int st2 = 3;

                    if (Properties.Settings.Default.page01AxisVariant == 3)
                    {
                        st0 = sizeX;
                        st1 = sizeX - 5;
                        st2 = sizeX - 3;
                    }

                    for (int i = 5; i < sizeY; i += 10)
                    {
                        grD.DrawLine(new Pen(Color.Blue), new Point(st1, i), new Point(st2, i));
                    }

                    for (int i = 105; i < sizeY; i += 100)
                    {
                        grD.DrawLine(new Pen(Color.Chocolate), new Point(st0, i), new Point(st1, i));
                    }
                }
            }

            return image;
        }

        private void Preview_Image_Load(object sender, EventArgs e)
        {
            // подключение обработчика, колесика мышки
            //pictureBoxPreview.Image = Resources.no_picture;
            containsData = (pictureBoxPreview.Image != null);
        }

        public void SetImage(Bitmap bmp)
        {
            if (bmp == null)
            {
                containsData = false;
                return;
            }

            //pictureBoxPreview.Image = DrawAxes(bmp);
            pictureBoxPreview.Image = bmp;
            containsData = true;
        }
    }
}
