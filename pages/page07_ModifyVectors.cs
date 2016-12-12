// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace ToolsGenGkode.pages
{
    public partial class page07_ModifyVectors : UserControl, PageInterface
    {
        /// <summary>
        /// Событие при изменении параметров на данной форме
        /// </summary>
        public event EventHandler IsChange;

        void CreateEvent(string action)
        {
            MyEventArgs e = new MyEventArgs();
            e.ActionRun = action;
            //вызовем событие
            EventHandler handler = IsChange;
            if (handler != null) IsChange?.Invoke(this, e);
        }

        public page07_ModifyVectors()
        {
            InitializeComponent();

            PageName = @"Модификация векторов (7)";
            LastPage = 0;
            CurrPage = 7;
            NextPage = 10;

            pageImageNOW = null;
            pageVectorNOW = new List<Segment>();
        }

        private void page08_ModifyVectors_Load(object sender, EventArgs e)
        {

        }

        public Bitmap pageImageIN { get; set; }
        public Bitmap pageImageNOW { get; set; }
        public List<Segment> pageVectorIN { get; set; }
        public List<Segment> pageVectorNOW { get; set; }
        public List<Location> PagePoints { get; set; }

        public void actionBefore()
        {
            pageVectorNOW = GlobalFunctions.pageVectorClone(pageVectorIN);
            pageImageNOW = null;

            getInfoSize();

            //todo: перезаполнить разные поля на форме

            CreateEvent("RefreshVector_07");

        }

        public void actionAfter()
        {
            // throw new NotImplementedException();
            //GlobalFunctions.IsLaserPoint = false;
        }


        private void MirrorX()
        {
            List<Segment> tmp = new List<Segment>();

            // получим границы изображения
            decimal min = 99999;
            decimal max = -99999;

            foreach (Segment vector in pageVectorNOW)
            {
                foreach (Location point in vector.Points)
                {
                    if (min > point.Y) min = point.Y;

                    if (max < point.Y) max = point.Y;
                }
            }

            //вычислим дельту, для смещения векторов
            decimal delta = min + max;

            List<Segment> vectors = new List<Segment>();
            List<Location> points = new List<Location>();

            foreach (Segment vector in pageVectorNOW)
            {
                points = new List<Location>();

                foreach (Location point in vector.Points)
                {
                    points.Add(new Location(point.X, (-point.Y)+delta, 0, 0, 0, false, point.Selected));
                }
                tmp.Add(new Segment(points, vector.Selected));
                points = new List<Location>();
            }

            pageVectorNOW = tmp;

            vectors = new List<Segment>();
            points = new List<Location>();


        }


        private void MirrorY()
        {
            List<Segment> tmp = new List<Segment>();

            // получим границы изображения
            decimal min = 99999;
            decimal max = -99999;

            foreach (Segment vector in pageVectorNOW)
            {
                foreach (Location point in vector.Points)
                {
                    if (min > point.X) min = point.X;

                    if (max < point.X) max = point.X;
                }
            }

            //вычислим дельту, для смещения векторов
            decimal delta = min + max;

            List<Segment> vectors = new List<Segment>();
            List<Location> points = new List<Location>();

            foreach (Segment vector in pageVectorNOW)
            {
                points = new List<Location>();

                foreach (Location point in vector.Points)
                {
                    points.Add(new Location((-point.X) + delta, point.Y, 0, 0, 0, false, point.Selected));
                    //point.X = (-point.X) + delta;
                }
                tmp.Add(new Segment(points,vector.Selected));
                points = new List<Location>();
            }

            pageVectorNOW = tmp;

            vectors = new List<Segment>();
            points = new List<Location>();


        }


        public PointF Rotate(PointF point, PointF pivot, double angleDegree)
        {
            float angle = (float)angleDegree * (float)Math.PI / 180;
            float cos = (float)Math.Cos(angle);
            float sin = (float)Math.Sin(angle);
            float dx = (float)(point.X - pivot.X);
            float dy = (float)(point.Y - pivot.Y);
            float x = cos * dx - sin * dy + (float)pivot.X;
            float y = sin * dx + cos * dy + (float)pivot.X;


            //float xt=500+(x1-500.00)*cos(0.05)+(500.00-y1)*sin(0.05);
            //float yt = 500 + (x1 - 500.00) * sin(0.05) + (y1 - 500.00) * cos(0.05);
            //x1 = xt;
            //y1 = yt;

            PointF rotated = new PointF(x,y);

            //cPoint rotated = new cPoint((int)Math.Round(x), (int)Math.Round(y));
            return rotated;
        }


        private void Rotate()
        {
            CreateEvent("ReloadData_08");
            CreateEvent("RefreshVector_08");


            // Для определения центра изображения
            // получим границы изображения
            float minX = 99999;
            float maxX = -99999;
            float minY = 99999;
            float maxY = -99999;

            foreach (Segment vector in pageVectorNOW)
            {
                foreach (Location point in vector.Points)
                {
                    if (minX > (float)point.X) minX = (float)point.X;

                    if (maxX < (float)point.X) maxX = (float)point.X;

                    if (minY > (float)point.Y) minY = (float)point.Y;

                    if (maxY < (float)point.Y) maxY = (float)point.Y;
                }
            }

            //вычислим центр
            float X0 = ((maxX - minX)/2) + minX;
            float Y0 = ((maxY - minY)/2) + minY;







            List<Segment> tmp = new List<Segment>();


            List<Segment> vectors = new List<Segment>();
            List<Location> points = new List<Location>();

            foreach (Segment vector in pageVectorNOW)
            {
                points = new List<Location>();

                foreach (Location point in vector.Points)
                {

                    PointF newPnt = Rotate(new PointF((float)point.X, (float)point.Y), new PointF(X0, Y0), (double)numRotate.Value);

                    //////float rad = (float)numRotate.Value * (float)(Math.PI / 180);

                    ////////p'x = cos(theta) * (px) - sin(theta) * (py)
                    ////////p'y = sin(theta) * (px) + cos(theta) * (py)
                    //////newXpos = Math.Cos(rad)*(double)point.X - Math.Sin(rad)*(double)point.Y;
                    //////newYpos = Math.Sin(rad)*(double) point.X - Math.Cos(rad)*(double) point.Y;
                    
                    //double fW = Math.Abs((Math.Cos(rad) * bmpBU.Width)) + Math.Abs((Math.Sin(rad) * bmpBU.Height));
                    //double fH = Math.Abs((Math.Sin(rad) * bmpBU.Width)) + Math.Abs((Math.Cos(rad) * bmpBU.Height));

                    //TODO: вычислить новые координаты точки

                    //////points.Add(new cPoint((decimal)newXpos, (decimal)newYpos, point.Selected));
                    points.Add(new Location((decimal)newPnt.X, (decimal)newPnt.Y));
                }
                tmp.Add(new Segment(points, vector.Selected));
                points = new List<Location>();
            }

            pageVectorNOW = tmp;

            vectors = new List<Segment>();
            points = new List<Location>();



            //////TODO: выполнить смещение в плюсовую зону

            ////// получим границы изображения
            ////minX = 99999;
            ////maxX = -99999;
            ////minY = 99999;
            ////maxY = -99999;

            ////foreach (cVector vector in pageVector)
            ////{
            ////    foreach (cPoint point in vector.Points)
            ////    {
            ////        if (minX > point.X) minX = point.X;

            ////        if (maxX < point.X) maxX = point.X;

            ////        if (minY > point.Y) minY = point.Y;

            ////        if (maxY < point.Y) maxY = point.Y;
            ////    }
            ////}

            //////вычислим дельту, для смещения векторов
            ////decimal deltaX = -minX;
            ////decimal deltaY = -minY;

            ////if (deltaX < 0) deltaX = 0;

            ////if (deltaY < 0) deltaY = 0;

            //////deltaX = 0;
            //////deltaY = 0;


            ////foreach (cVector vector in pageVector)
            ////{
            ////    points = new List<cPoint>();

            ////    foreach (cPoint point in vector.Points)
            ////    {
            ////        points.Add(new cPoint(point.X + deltaX, point.Y+deltaY, point.Selected));
            ////    }
            ////}

            //pageVector = tmp;


            
        }


        private void getInfoSize()
        {
            if (pageVectorNOW == null) return;

            if (pageVectorNOW.Count == 0) return;

            decimal minX = 99999;
            decimal maxX = -99999;
            decimal minY = 99999;
            decimal maxY = -99999;

            foreach (Segment vector in pageVectorNOW)
            {
                foreach (Location point in vector.Points)
                {
                    if (minX > point.X) minX = point.X;

                    if (maxX < point.X) maxX = point.X;

                    if (minY > point.Y) minY = point.Y;

                    if (maxY < point.Y) maxY = point.Y;
                }
            }

            //вычислим размер
            decimal X0 = maxX - minX;
            decimal Y0 = maxY - minY;

            numXbefore.Value = X0;
            numYbefore.Value = Y0;

            numDeltaX.Value = minX;
            numDeltaY.Value = minY;


        }







        private void numRotate_ValueChanged(object sender, EventArgs e)
        {
            //Calculate();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            pageVectorNOW = GlobalFunctions.pageVectorClone(pageVectorIN);
            pageImageNOW = null;

            getInfoSize();
            CreateEvent("RefreshVector_07");
        }

        private void btMirrorX_Click(object sender, EventArgs e)
        {
            MirrorX();
            CreateEvent("RefreshVector_07");
            getInfoSize();

        }

        private void btMirrorY_Click(object sender, EventArgs e)
        {
            MirrorY();
            CreateEvent("RefreshVector_07");
            getInfoSize();
        }

        private void btRotate_Click(object sender, EventArgs e)
        {
            Rotate();
            CreateEvent("RefreshVector_07");
            getInfoSize();
        }

        private void btMoveToZero_Click(object sender, EventArgs e)
        {
            decimal minX = 99999;
            decimal maxX = -99999;
            decimal minY = 99999;
            decimal maxY = -99999;

            foreach (Segment vector in pageVectorNOW)
            {
                foreach (Location point in vector.Points)
                {
                    if (minX > point.X) minX = point.X;

                    if (maxX < point.X) maxX = point.X;

                    if (minY > point.Y) minY = point.Y;

                    if (maxY < point.Y) maxY = point.Y;
                }
            }


            foreach (Segment vector in pageVectorNOW)
            {
                foreach (Location point in vector.Points)
                {
                    point.X -= minX;
                    point.Y -= minY;
                }
            }


            CreateEvent("RefreshVector_07");
            getInfoSize();
        }


        /// <summary>
        /// Для определения того кто меняет значение, пользователь, или код программы
        /// </summary>
        private bool changeIsUser = true;

        private void numXAfter_ValueChanged(object sender, EventArgs e)
        {
            if (!changeIsUser) return;

            if (cbKeepAspectRatio.Checked)
            {
                decimal delta = (numYbefore.Value / numXbefore.Value) * numXAfter.Value;

                changeIsUser = false;
                numYAfter.Value = delta;
                changeIsUser = true;
            }

        }

        private void numYAfter_ValueChanged(object sender, EventArgs e)
        {
            if (!changeIsUser) return;

            if (cbKeepAspectRatio.Checked)
            {
                decimal delta = (numXbefore.Value / numYbefore.Value) * numYAfter.Value;
                changeIsUser = false;
                numXAfter.Value =  delta;
                changeIsUser = true;
            }

        }

        private void btCalcZoom_Click(object sender, EventArgs e)
        {
            decimal deltaX = numXAfter.Value / numXbefore.Value;
            decimal deltaY = numYAfter.Value / numYbefore.Value;

            foreach (Segment vector in pageVectorNOW)
            {
                foreach (Location point in vector.Points)
                {
                    point.X = point.X * deltaX;
                    point.Y = point.Y * deltaY;
                }
            }

            CreateEvent("RefreshVector_07");
            getInfoSize();

        }

        private void cbAddPadding_CheckedChanged(object sender, EventArgs e)
        {
            if (cbAddPadding.Checked) NextPage = 8;
            else NextPage = 10;
        }
    }
}
