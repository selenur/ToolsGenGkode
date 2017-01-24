// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using DXFLib;
using ToolsGenGkode.Properties;

namespace ToolsGenGkode.pages
{
    public partial class page11_DXF : UserControl, PageInterface
    {
        private MainForm MAIN;

        public page11_DXF(MainForm mf)
        {
            InitializeComponent();

            MAIN = mf;

            pageImageIN = null;
            pageImageNOW = null;
            pageVectorIN = new List<GroupPoint>();
            pageVectorNOW = new List<GroupPoint>();

            NextPage = 6;
        }

        private void page11_DXF_Load(object sender, EventArgs e)
        {
            //string s1 = IniParser.GetSetting("page11", "arcMaxLengLine");

            //if (s1 == null)// если нет такого параметра, то добавим
            //{
            //    //arcMaxLengLine = 0.1;
            //    //IniParser.AddSetting("page11", "arcMaxLengLine", arcMaxLengLine.ToString());
            //    //IniParser.SaveSettings();

            //    Properties.Settings.Default.page11arcMaxLengLine = arcMaxLengLine;
            //    Properties.Settings.Default.Save();
            //}
            //else
            //{
            //    double.TryParse(s1, out arcMaxLengLine);
            //}


            //toolTips myToolTip1 = new toolTips();

            //myToolTip1.Size = new Size(300, 200);
            //myToolTip1.BackColor = Color.FromArgb(255, 255, 192);
            //myToolTip1.ForeColor = Color.Navy;
            //myToolTip1.BorderColor = Color.FromArgb(128, 128, 255);

            //myToolTip1.SetToolTip(buttonSelectFile, "Выбор файла для загрузки данных.");
            //myToolTip1.SetToolTip(btShowOriginalImage, "Вызывается повторное чтение данных из файла.");
        }



        public void actionBefore()
        {
            //throw new NotImplementedException();
            MAIN.PageName.Text = @"Выбор DXF файла (11)";
            MAIN.PageName.Tag = Tag;

            pageVectorNOW = VectorProcessing.ListGroupPointClone(pageVectorIN);
            pageImageNOW = null;

            UserActions();
        }

        public void actionAfter()
        {
            //throw new NotImplementedException();
        }
















        void UserActions()
        {
            MAIN.PreviewDada(pageImageNOW,pageVectorNOW);
        }

        private void btShowOriginalImage_Click(object sender, EventArgs e)
        {
            pageVectorNOW = GetVectorDXF(textBoxFileName.Text);
            pageImageNOW = null;

            UserActions();
        }

        private void buttonSelectFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.CheckFileExists = true;
            openFileDialog1.Multiselect = false;
            openFileDialog1.Title = @"Выбор DXF файла";
            //openFileDialog1.InitialDirectory = "c:\\";
            openFileDialog1.Filter = @"DXF (*.dxf)|*.dxf";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textBoxFileName.Text = openFileDialog1.FileName;
            }

            if (!File.Exists(textBoxFileName.Text)) return;

            pageVectorNOW = GetVectorDXF(textBoxFileName.Text);
            pageImageNOW = null;

            UserActions();
        }

        // Вычисление угла, при котором дрина сегмента с учетом радиуса будет указанной длины
        private double GetAngleArcSegment(decimal Radius, decimal maxLenght)
        {
            //
            //
            //
            //       |\
            //       | \ 
            //    5  |  \   10
            //       |   \ 
            //       |   &\
            //        ------

            // & = asin(5/10) * 180/PI

            double result = Math.Asin(((double)maxLenght / 2) / (double)Radius) * 180 / Math.PI;

            return result;
        }


        private List<GroupPoint> GetVectorDXF(string FileNAME)
        {
            textBoxInfo.Text = "";

            DXFDocument doc = new DXFDocument();
            doc.Load(FileNAME);

            List<GroupPoint> ListLines;
            List<cncPoint> ListPoint;

            ListLines = new List<GroupPoint>();

            int count_DXFLWPolyLine = 0;
            int count_DXFPolyLine = 0;
            int count_DXFLine = 0;
            int count_DXFCircle = 0;
            int count_DXFSpline = 0;
            int count_DXFArc = 0;



            double minX = 99999;
            double maxX = -99999;
            double deltaX = 0;

            double minY = 99999;
            double maxY = -99999;
            double deltaY = 0;

            foreach (DXFEntity VARIABLE in doc.Entities)
            {
                bool typeDetected = false;

                if (VARIABLE.GetType() == typeof(DXFLWPolyLine))
                {
                    typeDetected = true;
                    count_DXFLWPolyLine++;
                    DXFLWPolyLine lp = (DXFLWPolyLine)VARIABLE;

                    ListPoint = new List<cncPoint>();

                    foreach (DXFLWPolyLine.Element pl_e in lp.Elements)
                    {
                        ListPoint.Add(new cncPoint((double)pl_e.Vertex.X, (double)pl_e.Vertex.Y));

                    }
                    ListLines.Add(new GroupPoint(ListPoint));
                    ListPoint = new List<cncPoint>();
                }

                if (VARIABLE.GetType() == typeof(DXFPolyLine))
                {
                    typeDetected = true;
                    count_DXFPolyLine++;
                    DXFPolyLine lp = (DXFPolyLine)VARIABLE;

                    ListPoint = new List<cncPoint>();

                    foreach (DXFVertex pl_e in lp.Children)
                    {
                        if (pl_e.GetType() == typeof(DXFVertex))
                            if (pl_e.Location.X != null && pl_e.Location.Y != null)
                                    ListPoint.Add(new cncPoint((double)pl_e.Location.X, (double)pl_e.Location.Y));
                    }
                    ListLines.Add(new GroupPoint( ListPoint));
                    ListPoint = new List<cncPoint>();
                }



                if (VARIABLE.GetType() == typeof(DXFLine))
                {
                    typeDetected = true;
                    count_DXFLine++;
                    DXFLine line = (DXFLine)VARIABLE;

                    ListPoint = new List<cncPoint>();
                    ListPoint.Add(new cncPoint((double)line.Start.X, (double)line.Start.Y));
                    ListPoint.Add(new cncPoint((double)line.End.X, (double)line.End.Y));
                    ListLines.Add(new GroupPoint(ListPoint));
                    ListPoint = new List<cncPoint>();
                }


                if (VARIABLE.GetType() == typeof (DXFSpline))
                {
                    typeDetected = true;
                    count_DXFSpline++;
                    DXFSpline spline = (DXFSpline)VARIABLE;

                    ListPoint = new List<cncPoint>();

                    foreach (DXFPoint ppoint in spline.ControlPoints)
                    {
                        ListPoint.Add(new cncPoint((double)ppoint.X, (double)ppoint.Y));

                    }

                    ListLines.Add(new GroupPoint(ListPoint));
                    ListPoint = new List<cncPoint>();
                }


                if (VARIABLE.GetType() == typeof(DXFCircle))
                {
                    typeDetected = true;
                    count_DXFCircle++;
                    DXFCircle circle = (DXFCircle)VARIABLE;

                    double X = (double)circle.Center.X;
                    double Y = (double)circle.Center.Y;
                    float R = (float) circle.Radius;



                    //тут определим необходимый угол
                    //и вычислим количество сегментов

                    double angle = GetAngleArcSegment((decimal)R, Settings.Default.page11arcMaxLengLine);

                    if (angle < 1) angle = 1;


                    int segmentsCount = 360/(int)angle;

                    ListPoint = new List<cncPoint>();

                    for (int i = 0; i < segmentsCount; i++)
                    {
                        float rx = R * (float)Math.Cos(2 * (float)Math.PI / segmentsCount * i);
                        float ry = R * (float)Math.Sin(2 * (float)Math.PI / segmentsCount * i);

                        ListPoint.Add(new cncPoint(X + (double)rx, Y + (double)ry));

                    }

                    ListPoint.Add(new cncPoint(ListPoint[0].X, ListPoint[0].Y));

                    ListLines.Add(new GroupPoint(ListPoint));
                    ListPoint = new List<cncPoint>();


                    //
                    //ListPoint.Add(new Location((decimal)line.Start.X, (decimal)line.Start.Y));
                    //ListPoint.Add(new Location((decimal)line.End.X, (decimal)line.End.Y));
                    //
                }


                if (VARIABLE.GetType() == typeof(DXFArc))
                {
                    typeDetected = true;
                    count_DXFArc++;
                    DXFArc arc = (DXFArc)VARIABLE;

                    double X = (double)arc.Center.X;
                    double Y = (double)arc.Center.Y;
                    double R = arc.Radius;
                    double startAngle = arc.StartAngle;
                    double endAngle = arc.EndAngle;

                    if (startAngle > endAngle) endAngle += 360;

                    //тут определим необходимый угол
                    //и вычислим количество сегментов

                    float StepAngle = (float)GetAngleArcSegment((decimal)R, Settings.Default.page11arcMaxLengLine);

                    ListPoint = new List<cncPoint>();

                    double currAngle = startAngle;

                    while (currAngle < endAngle)
                    {

                        double angle = currAngle * Math.PI / 180;

                        double rx =(double) (X + R * Math.Cos(angle));
                        double ry = (double)(Y + R * Math.Sin(angle));

                        ListPoint.Add(new cncPoint(rx, ry));

                        currAngle += StepAngle;

                        if (currAngle > endAngle)
                        {
                            //если перескочили конечный угол, то остановимся на конечном угле

                            double angle2 = endAngle * Math.PI / 180;

                            double rx2 = (double)(X + R * Math.Cos(angle2));
                            double ry2 = (double)(Y + R * Math.Sin(angle2));

                            ListPoint.Add(new cncPoint(rx2, ry2));

                        }

                    }


                   // int segmentsCount = 360 / (int)angle;


                    //for (float i = startAngle; i < endAngle; i+= (float)angle)
                    //{


                    //    

                    //}


                    ListLines.Add(new GroupPoint(ListPoint));
                    ListPoint = new List<cncPoint>();



                }



                if (!typeDetected)
                {
                    textBoxInfo.Text += "Не распознан тип данных: " + VARIABLE.GetType().ToString() +
                                        Environment.NewLine;
                }

      

            }

            //ListLines = new List<Segment>();

            textBoxInfo.Text += @"Обработано DXFLWPolyLine: " + count_DXFLWPolyLine.ToString() + Environment.NewLine;
            textBoxInfo.Text += @"Обработано DXFPolyLine: " + count_DXFPolyLine.ToString() + Environment.NewLine;
            textBoxInfo.Text += @"Обработано DXFLine: " + count_DXFLine.ToString() + Environment.NewLine;
            textBoxInfo.Text += @"Обработано DXFCircle: " + count_DXFCircle.ToString() + Environment.NewLine;

            textBoxInfo.Text += @"Обработано DXFSpline: " + count_DXFSpline.ToString() + Environment.NewLine;
            textBoxInfo.Text += @"Обработано DXFArc: " + count_DXFArc.ToString() + Environment.NewLine;


            foreach (GroupPoint vSegment in ListLines)
            {
                foreach (cncPoint vLocation in vSegment.Points)
                {
                    if (vLocation.X < minX) minX = vLocation.X;

                    if (vLocation.Y < minY) minY = vLocation.Y;

                    if (vLocation.X > maxX) maxX = vLocation.X;

                    if (vLocation.Y > maxY) maxY = vLocation.Y;

                }

            }

            deltaX = maxX - minX;
            deltaY = maxY - minY;

            textBoxInfo.Text += @"Размер по X: " + deltaX.ToString() + Environment.NewLine;
            textBoxInfo.Text += @"Размер по Y: " + deltaY.ToString() + Environment.NewLine;




            return ListLines;

        }

    }
}
