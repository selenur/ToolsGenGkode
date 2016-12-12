// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace ToolsGenGkode.pages
{
    public partial class page03_SelectPLT : UserControl, PageInterface
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

        public page03_SelectPLT()
        {
            InitializeComponent();

            PageName = @"Выбор PLT файла (3)";
            LastPage = 1;
            CurrPage = 3;
            NextPage = 6;

            pageImageNOW = null;
            pageVectorNOW = new List<Segment>();
        }

        private void SelectPLT_Load(object sender, EventArgs e)
        {

        }

        private void buttonSelectFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.CheckFileExists = true;
            openFileDialog1.Multiselect = false;
            openFileDialog1.Title = @"Выбор PLT Corel Draw";
            //openFileDialog1.InitialDirectory = "c:\\";
            openFileDialog1.Filter = @"PLT Corel Draw (*.plt)|*.plt";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textBoxFileName.Text = openFileDialog1.FileName;
            }


            if (!System.IO.File.Exists(textBoxFileName.Text)) return;

            pageVectorNOW = GetVectorFromPLT(textBoxFileName.Text);

            CreateEvent("RefreshVector_03");
        }

        private void btShowOriginalImage_Click(object sender, EventArgs e)
        {
            CreateEvent("RefreshVector_03");
        }

        private List<Segment> GetVectorFromPLT(string FileNAME)
        {
            // для сбора информации о точках у отрезка
            List<Location> ListPoints = new List<Location>();
            // для сбора информации об отрезках
            List<Segment> Lines = new List<Segment>();

            //для смещения по оси в положительную сторону
            decimal minX = 0;
            decimal minY = 0;


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
                        Lines.Add(new Segment(ListPoints));
                        ListPoints = new List<Location>();
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
                    Lines.Add(new Segment(ListPoints));
                    ListPoints = new List<Location>();
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

                decimal posX, posY;

                if (!decimal.TryParse(s.Substring(pos1 + 1, pos2 - pos1 - 1), out posX))
                {
                    MessageBox.Show(@"Ошибка преобразования координаты X в строке № " + numRow.ToString());
                    break;
                }

                if (!decimal.TryParse(s.Substring(pos2 + 1, pos3 - pos2 - 1), out posY))
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


                ListPoints.Add(new Location(posX, posY));



                s = fs.ReadLine();
                numRow++;
            }

            fs = null;


            List<Segment> ListLines;

            List<Location> ListPoint;

            ListLines = new List<Segment>();

            foreach (Segment pline in Lines)
            {
                ListPoint = new List<Location>();

                foreach (Location ppoint in pline.Points)
                {

                    decimal fX = (ppoint.X + (-minX)) * 10;
                    decimal fY = (ppoint.Y + (-minY)) * 10;

                    ListPoint.Add(new Location(fX, fY));
                }
                ListLines.Add(new Segment(ListPoint));
            }

            return ListLines;
        }


        public Bitmap pageImageIN { get; set; }
        public Bitmap pageImageNOW { get; set; }
        public List<Segment> pageVectorIN { get; set; }
        public List<Segment> pageVectorNOW { get; set; }
        //public List<Location> PagePoints { get; set; }

 
    }
}
