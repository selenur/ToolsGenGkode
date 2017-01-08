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
        private MainForm MAIN;

        public page07_ModifyVectors(MainForm mf)
        {
            InitializeComponent();

            MAIN = mf;

            pageImageIN = null;
            pageImageNOW = null;
            pageVectorIN = new List<GroupPoint>();
            pageVectorNOW = new List<GroupPoint>();

            NextPage = 10;
        }

        public void actionBefore()
        {
            MAIN.PageName.Text = @"Ручная корректировка векторов (7)";
            MAIN.PageName.Tag = Tag;

            pageVectorNOW = VectorProcessing.ListGroupPointClone(pageVectorIN);
            pageImageNOW = null;

            UserActions();
        }

        public void actionAfter()
        {

        }

        void UserActions()
        {
            getInfoSize();
            MAIN.PreviewDada(pageImageNOW,pageVectorNOW);
        }

        private void page08_ModifyVectors_Load(object sender, EventArgs e)
        {

        }

        private void MirrorX()
        {
            List<GroupPoint> tmp = new List<GroupPoint>();

            // получим границы изображения
            double min = 99999;
            double max = -99999;

            foreach (GroupPoint vector in pageVectorNOW)
            {
                foreach (cncPoint point in vector.Points)
                {
                    if (min > point.Y) min = point.Y;

                    if (max < point.Y) max = point.Y;
                }
            }

            //вычислим дельту, для смещения векторов
            double delta = min + max;

            List<GroupPoint> vectors = new List<GroupPoint>();
            List<cncPoint> points = new List<cncPoint>();

            foreach (GroupPoint vector in pageVectorNOW)
            {
                points = new List<cncPoint>();

                foreach (cncPoint point in vector.Points)
                {
                    points.Add(new cncPoint(point.X, (-point.Y)+delta, 0, 0, 0, point.Selected));
                }
                tmp.Add(new GroupPoint(points, vector.Selected));
                points = new List<cncPoint>();
            }

            pageVectorNOW = VectorProcessing.ListGroupPointClone(tmp);

            UserActions();
        }


        private void MirrorY()
        {
            List<GroupPoint> tmp = new List<GroupPoint>();

            // получим границы изображения
            double min = 99999;
            double max = -99999;

            foreach (GroupPoint vector in pageVectorNOW)
            {
                foreach (cncPoint point in vector.Points)
                {
                    if (min > point.X) min = point.X;

                    if (max < point.X) max = point.X;
                }
            }

            //вычислим дельту, для смещения векторов
            double delta = min + max;

            List<GroupPoint> vectors = new List<GroupPoint>();
            List<cncPoint> points = new List<cncPoint>();

            foreach (GroupPoint vector in pageVectorNOW)
            {
                points = new List<cncPoint>();

                foreach (cncPoint point in vector.Points)
                {
                    points.Add(new cncPoint((-point.X) + delta, point.Y, 0, 0, 0, point.Selected));
                    //point.X = (-point.X) + delta;
                }
                tmp.Add(new GroupPoint(points,vector.Selected));
                points = new List<cncPoint>();
            }

            pageVectorNOW = VectorProcessing.ListGroupPointClone(tmp);


            UserActions();
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
            // Для определения центра изображения
            // получим границы изображения
            float minX = 99999;
            float maxX = -99999;
            float minY = 99999;
            float maxY = -99999;

            foreach (GroupPoint vector in pageVectorNOW)
            {
                foreach (cncPoint point in vector.Points)
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

            List<GroupPoint> tmp = new List<GroupPoint>();

            List<GroupPoint> vectors = new List<GroupPoint>();
            List<cncPoint> points = new List<cncPoint>();

            foreach (GroupPoint vector in pageVectorNOW)
            {
                points = new List<cncPoint>();

                foreach (cncPoint point in vector.Points)
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
                    points.Add(new cncPoint((double)newPnt.X, (double)newPnt.Y));
                }
                tmp.Add(new GroupPoint(points, vector.Selected));
                points = new List<cncPoint>();
            }

            pageVectorNOW = VectorProcessing.ListGroupPointClone(tmp);




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

            UserActions();
            
        }


        private void getInfoSize()
        {
            if (pageVectorNOW == null) return;

            if (pageVectorNOW.Count == 0) return;

            double minX = 99999;
            double maxX = -99999;
            double minY = 99999;
            double maxY = -99999;

            foreach (GroupPoint vector in pageVectorNOW)
            {
                foreach (cncPoint point in vector.Points)
                {
                    if (minX > point.X) minX = point.X;

                    if (maxX < point.X) maxX = point.X;

                    if (minY > point.Y) minY = point.Y;

                    if (maxY < point.Y) maxY = point.Y;
                }
            }

            //вычислим размер
            double X0 = maxX - minX;
            double Y0 = maxY - minY;

            numXbefore.Value = (decimal)X0;
            numYbefore.Value = (decimal)Y0;

            numDeltaX.Value = (decimal)minX;
            numDeltaY.Value = (decimal)minY;


        }







        private void numRotate_ValueChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            pageVectorNOW = VectorProcessing.ListGroupPointClone(pageVectorIN);
            pageImageNOW = null;

            UserActions();
        }

        private void btMirrorX_Click(object sender, EventArgs e)
        {
            MirrorX();
            UserActions();

        }

        private void btMirrorY_Click(object sender, EventArgs e)
        {
            MirrorY();
        }

        private void btRotate_Click(object sender, EventArgs e)
        {
            Rotate();
        }

        private void btMoveToZero_Click(object sender, EventArgs e)
        {
            double minX = 99999;
            double maxX = -99999;
            double minY = 99999;
            double maxY = -99999;

            foreach (GroupPoint vector in pageVectorNOW)
            {
                foreach (cncPoint point in vector.Points)
                {
                    if (minX > point.X) minX = point.X;

                    if (maxX < point.X) maxX = point.X;

                    if (minY > point.Y) minY = point.Y;

                    if (maxY < point.Y) maxY = point.Y;
                }
            }


            foreach (GroupPoint vector in pageVectorNOW)
            {
                foreach (cncPoint point in vector.Points)
                {
                    point.X -= minX;
                    point.Y -= minY;
                }
            }

            UserActions();
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

            foreach (GroupPoint vector in pageVectorNOW)
            {
                foreach (cncPoint point in vector.Points)
                {
                    point.X = point.X * (double)deltaX;
                    point.Y = point.Y * (double)deltaY;
                }
            }

            UserActions();


        }

        private void cbAddPadding_CheckedChanged(object sender, EventArgs e)
        {
            if (cbAddPadding.Checked) NextPage = 8;
            else NextPage = 10;
        }
    }
}
