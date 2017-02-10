// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using ToolsGenGkode.Properties;

namespace ToolsGenGkode.pages
{
    public partial class page01_start : UserControl, PageInterface
    {
        // ссылка на основную форму
        private MainForm MAIN;

        public page01_start(MainForm mf)
        {
            InitializeComponent();

            MAIN = mf;

            pageImageIN = null;
            pageImageNOW = null;
            pageVectorIN = new List<GroupPoint>();
            pageVectorNOW = new List<GroupPoint>();

            //Страница по умолчанию для перехода
            NextPage = Properties.Settings.Default.page01NextPage;

            switch (NextPage)
            {
                case 2:
                    radioButtonTypeSourceText.Checked = true;
                    break;

                case 3:
                    radioButtonTypeSourcePLT.Checked = true;
                    break;

                case 4:
                    radioButtonTypeSourcePicture1.Checked = true;
                    break;

                case 5:
                    radioButtonTypeSourcePicture2.Checked = true;
                    break;
                case 11:
                    radioButtonTypeSourceDXF.Checked = true;
                    break;
                default:
                    radioButtonTypeSourceText.Checked = true;
                    break;
            }

            int sAxesPos = Properties.Settings.Default.page01AxisVariant;

            SetOrientation(sAxesPos);

            switch (sAxesPos)
            {
                case 1:
                    OrientationVar1.Checked = true;
                    break;

                case 2:
                    OrientationVar2.Checked = true;
                    break;
                case 3:
                    OrientationVar3.Checked = true;
                    break;
                case 4:
                    OrientationVar4.Checked = true;
                    break;
                default:
                    OrientationVar1.Checked = true;
                    break;
            }
        }

        public void actionBefore()
        {
            // В заголовке первой страницы установим наименование по умолчанию
            MAIN.PageName.Text = @"Выбор ориентации осей, и источника данных (1)";
            MAIN.PageName.Tag = Tag;
            // передадим пустые данные
            MAIN.PreviewDada(null,new List<GroupPoint>());
        }

        public void actionAfter()
        {

        }

        private void page01_Load(object sender, EventArgs e)
        {

        }

        private void SetOrientation(int value)
        {
            switch (value)
            {
                case 1:
                    pictureBoxAxes.Image = Resources.axes_var1;

                    break;

                case 2:
                    pictureBoxAxes.Image = Resources.axes_var2;

                    break;
                case 3:
                    pictureBoxAxes.Image = Resources.axes_var3;

                    break;
                case 4:
                    pictureBoxAxes.Image = Resources.axes_var4;

                    break;
                default:
                    pictureBoxAxes.Image = Resources.axes_var1;
                    break;
            }

            Properties.Settings.Default.page01AxisVariant = value;
            Properties.Settings.Default.Save();
        }

        private void SetNextPage(int number)
        {
            NextPage = number;
            Properties.Settings.Default.page01NextPage = number;
            Properties.Settings.Default.Save();
        }

        private void OrientationVar1_CheckedChanged(object sender, EventArgs e)
        {
            SetOrientation(1);
        }

        private void OrientationVar2_CheckedChanged(object sender, EventArgs e)
        {
            SetOrientation(2);
        }

        private void OrientationVar3_CheckedChanged(object sender, EventArgs e)
        {
            SetOrientation(3);
        }

        private void OrientationVar4_CheckedChanged(object sender, EventArgs e)
        {
            SetOrientation(4);
        }

        private void radioButtonTypeSourceText_CheckedChanged(object sender, EventArgs e)
        {
            SetNextPage(2);
        }

        private void radioButtonTypeSourcePLT_CheckedChanged(object sender, EventArgs e)
        {
            SetNextPage(3);
        }

        private void radioButtonTypeSourcePicture_CheckedChanged(object sender, EventArgs e)
        {
            SetNextPage(4);
        }

        private void radioButtonTypeSourcePicture2_CheckedChanged(object sender, EventArgs e)
        {
            SetNextPage(5);
        }

        private void radioButtonTypeSourceDXF_CheckedChanged(object sender, EventArgs e)
        {
            SetNextPage(11);
        }
    }

    public interface PageInterface
    {
        /// <summary>
        /// Номер следующей страницы
        /// </summary>
        int NextPage { get; set; }    // свойство

        Bitmap pageImageIN { get; set; } //входной рисунок
        Bitmap pageImageNOW { get; set; }// текущий рисунок

        List<GroupPoint> pageVectorIN { get; set; } //траектория полученная с предыдущей страницы
        List<GroupPoint> pageVectorNOW { get; set; } //скорректированная траектория на данной странице

        /// <summary>
        /// Вызов полного пересчета данных, на текущей странице, при перекючении на неё
        /// </summary>
        void actionBefore(); // метод

        /// <summary>
        /// Вызов полного пересчета данных, на текущей странице, перед переключением на слеющую
        /// </summary>
        void actionAfter(); // метод
    }

    /// <summary>
    /// Класс описания группы точек
    /// </summary>
    public class GroupPoint
    {
        /// <summary>
        /// Набор точек
        /// </summary>
        public List<cncPoint> Points { get; set; }

        /// <summary>
        /// Направление траектории, применяется для лазерной гравировки
        /// </summary>
        public DirrectionGroupPoint Dirrect { get; set; }

        /// <summary>
        /// Выделен ли данный сегмент в пользовательском диалоге (применяется для 3D вуализации)
        /// </summary>
        public bool Selected { get; set; }

        /// <summary>
        /// Являются-ли точки в сегменте, самостоятельными, а не кривой состоящей из точек (применяется для 3D вуализации)
        /// </summary>
        public bool IndividualPoints { get; set; }

        //TODO: траектория для предварительного просмотра смещения контура
        //public bool TempTraectory { get; set; }

        // Создание отдельной копии
        public GroupPoint Clone()
        {
            GroupPoint segReturn = new GroupPoint();

            segReturn.Selected = Selected;
            //segReturn.TempTraectory = TempTraectory;
            segReturn.Dirrect = Dirrect;
            segReturn.IndividualPoints = IndividualPoints;

            segReturn.Points = new List<cncPoint>();

            foreach (cncPoint VARIABLE in Points)
            {
                segReturn.Points.Add(VARIABLE.Clone());
            }

            return segReturn;
        }

        public GroupPoint()
        {
            Points = new List<cncPoint>();
            Selected = false;
            //TempTraectory = false;
            Dirrect = DirrectionGroupPoint.Left;
            IndividualPoints = false;
        }

        public GroupPoint(List<cncPoint> _points, bool _select = false, DirrectionGroupPoint _dir = DirrectionGroupPoint.Left, bool _individ = false )
        {
            Points = _points;
            Selected = _select;
            //TempTraectory = _tempTraectory;
            Dirrect = _dir;
            IndividualPoints = _individ;
        }

        public PointF[] GetArray()
        {
            PointF[] arr = new PointF[Points.Count];

            int ind = 0;

            foreach (cncPoint VARIABLE in Points)
            {
                arr[ind++] = new PointF((float)VARIABLE.X, (float)VARIABLE.Y);
            }

            return arr;
        }

        //public GroupPoint(GroupPoint _source)
        //{
        //    Selected = _source.Selected;
        //    //TempTraectory = _source.TempTraectory;
        //    Points = new List<cncPoint>();
        //    Dirrect = _source.Dirrect;
        //    IndividualPoints = _source.IndividualPoints;

        //    foreach (cncPoint VARIABLE in _source.Points)
        //    {
        //        Points.Add(new cncPoint(VARIABLE));
        //    }
        //}
    }

    /// <summary>
    /// Класс описания положения координаты
    /// </summary>
    public class cncPoint
    {
        public double X { get; set; }
        public double Y { get; set; }
        public int Gvalue { get; set; }    // режим перемещения
        public int Svalue { get; set; }    // мощность
        public int Fvalue { get; set; }    // скорость
        public int Pvalue { get; set; }    // задержка
        public int Bright { get; set; }    // яркость точки
        public bool Selected { get; set; } // для визуального отображения

        public cncPoint Clone()
        {
            cncPoint locReturn = new cncPoint();

            locReturn.X = X;
            locReturn.Y = Y;
            locReturn.Gvalue = Gvalue;
            locReturn.Svalue = Svalue;
            locReturn.Fvalue = Fvalue;
            locReturn.Pvalue = Pvalue;
            locReturn.Selected = Selected;
            locReturn.Bright = Bright;

            return locReturn;
        }

        /// <summary>
        /// Конструктор класса
        /// </summary>
        public cncPoint(double _x = 0, double _y = 0, int _s = 0, int _f = 0, int _p = 0, bool _select = false, int _Bright = 255, int _g = 1)
        {
            X         = _x;
            Y         = _y;
            Gvalue    = _g;
            Svalue    = _s;
            Fvalue    = _f;
            Pvalue    = _p;
            Selected  = _select;
            Bright = _Bright;
        }

        /// <summary>
        /// Конструктор на основании себя
        /// </summary>
        /// <param name="_source"></param>
        public cncPoint(cncPoint _source)
        {
            X         = _source.X;
            Y         = _source.Y;
            Gvalue = _source.Gvalue;
            Svalue    = _source.Svalue;
            Fvalue    = _source.Fvalue;
            Pvalue    = _source.Pvalue;
            Selected  = _source.Selected;
            Bright = _source.Bright;
        }

        // Для доступа к полям по текстовому имени
        public object this[string key]
        {
            get
            {
                if (key.ToUpper() == "X") return X;

                if (key.ToUpper() == "Y") return Y;

                return null;
            }
            set
            {
                if (key.ToUpper() == "X")
                {
                    try
                    {
                        X = (double)value;
                    }
                    catch (Exception)
                    {
                        X = 0;
                    }
                }

                if (key.ToUpper() == "Y")
                {
                    try
                    {
                        Y = (double)value;
                    }
                    catch (Exception)
                    {
                        Y = 0;
                    }
                }

            }
        }
    }

    // направление сегмента
    public enum DirrectionGroupPoint
    {
        Left = 0,
        Right = 1
    }
}
////toolTips myToolTip1 = new toolTips();

//////myToolTip1.Size = new Size(300,200);
//////myToolTip1.BackColor = Color.FromArgb(255, 255, 192);
//////myToolTip1.ForeColor = Color.Navy;
//////myToolTip1.BorderColor = Color.FromArgb(128, 128, 255);

////myToolTip1.SetToolTip(radioButtonTypeSourceText, "Выбор данного пункта, предназначен для формирования траектории,\n на основании текста, введенного пользователем.");
////myToolTip1.SetToolTip(radioButtonTypeSourcePLT, "Выбор данного пункта, предназначен для формирования траектории,\n из файла PLT сформированного из програмы COREL DRAW.");
////myToolTip1.SetToolTip(radioButtonTypeSourcePicture1, "Выбор данного пункта, предназначен для формирования траектории,\n из рисунка, причем будет получен лишь контур рисунка.");
////myToolTip1.SetToolTip(radioButtonTypeSourcePicture2, "Выбор данного пункта, предназначен для формирования траектории,\n для лазерной гравировки изображения.");

//////myToolTip1.SetToolTip(OrientationVar1, "Выбор данного пункта,\n определяет расположение начала координат, относительно стола станка.\n \n \n \n \n \n");
//////myToolTip1.SetToolTip(OrientationVar2, "Выбор данного пункта,\n определяет расположение начала координат, относительно стола станка.\n \n \n \n \n \n");

////// rbvar1.Tag = Resources.axesVar1;
////// rbvar2.Tag = Resources.axesVar2;
