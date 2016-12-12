// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace ToolsGenGkode.pages
{
    public partial class page06_VectorEdit : UserControl, PageInterface
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

        public page06_VectorEdit()
        {
            InitializeComponent();

            PageName = @"Ручная корректировка векторов (6)";
            LastPage = 0;
            CurrPage = 6;
            NextPage = 7;
        }

        private void page06_VectorEdit_Load(object sender, EventArgs e)
        {
            if (Setting.GenaMode)
            {
                ForGena.Visible = true;
            }
        }

        public List<Location> PagePoints { get; set; }

        public void actionBefore()
        {
            pageVectorNOW = GlobalFunctions.pageVectorClone(pageVectorIN);
            pageImageNOW = null;

            RefreshTree();
        }

        public void actionAfter()
        {
        }



        private void RefreshTree()
        {
            int countLine = 0;
            int countpoints = 0;
            
            treeViewVectors.Nodes.Clear();

            foreach (Segment line in pageVectorNOW)
            {
                TreeNode tn = treeViewVectors.Nodes.Add("Точек: " + line.Points.Count);

                foreach (Location point in line.Points)
                {
                    tn.Nodes.Add(@"x: " + point.X + @" y: " + point.Y);

                    countpoints++;
                }
                countLine++;
            }

            labelTraectoryInfo.Text = @"Траектория состоит из:" + Environment.NewLine + countLine.ToString() + @" отрезков," + Environment.NewLine + countpoints.ToString() + @" точек.";

            pageImageNOW = null;

            CreateEvent("RefreshVector_06");
            CreateEvent("RefreshImage_06");

        }

        private void btLoadVectors_Click(object sender, EventArgs e)
        {
            pageVectorNOW = GlobalFunctions.pageVectorClone(pageVectorIN);
            pageImageNOW = null;


            RefreshTree();
        }

        private void treeViewVectors_AfterSelect(object sender, TreeViewEventArgs e)
        {
            foreach (Segment vector in pageVectorNOW)
            {
                vector.Selected = false;

                foreach (Location VARIABLE in vector.Points)
                {
                    VARIABLE.Selected = false;
                }
            }


            //TODO: реализовать отрисовку


            int level = -1; //расположение 0-выбран узел линии, 1-точка линии
            int pos = -1;   // если это линия то её индекс, если точка то её индекс
            int posParent = -1; //если выбрана точка, то тут индекс линии


            if (treeViewVectors.SelectedNode != null)
            {
                level = treeViewVectors.SelectedNode.Level;
                pos = treeViewVectors.SelectedNode.Index;
                if (level > 0) posParent = treeViewVectors.SelectedNode.Parent.Index;

                if (level == 0)
                {
                    pageVectorNOW[pos].Selected = true;
                }

                if (level == 1)
                {
                    pageVectorNOW[posParent].Points[pos].Selected = true;
                }
            }

            CreateEvent("RefreshVector_06");
            CreateEvent("RefreshImage_06");
        }

        private void btDelVector_Click(object sender, EventArgs e)
        {
            int level = -1; //расположение 0-выбран узел линии, 1-точка линии
            int pos = -1;   // если это линия то её индекс, если точка то её индекс
            int posParent = -1; //если выбрана точка, то тут индекс линии

            if (treeViewVectors.SelectedNode == null) return;

            level = treeViewVectors.SelectedNode.Level;
            pos = treeViewVectors.SelectedNode.Index;

            if (level > 0) posParent = treeViewVectors.SelectedNode.Parent.Index;

            if (level == 0) pageVectorNOW.RemoveAt(pos);
            if (level == 1) pageVectorNOW[posParent].Points.RemoveRange(pos, 1);

            RefreshTree();
        }


        public Bitmap pageImageIN { get; set; }
        public Bitmap pageImageNOW { get; set; }
        public List<Segment> pageVectorIN { get; set; }
        public List<Segment> pageVectorNOW { get; set; }


        private void btOptimize1_Click(object sender, EventArgs e)
        {
            // Нужно отсортировать отрезки, для минимизации холостого хода

            if(pageVectorNOW.Count<2) return; //нет смысла сортировать

            // создаем временный список отрезков
            List<Segment> destCVectors = new List<Segment>();
            
            //начинаем поиск отрезка, который ближе всего к координатам 0,0

            Segment tmpVector = new Segment();
            decimal minDist = 99999;
            bool needReversVector = false;


            foreach (Segment vector  in pageVectorNOW)
            {
                //узнаем расстояние 
                decimal iS = (decimal) (Math.Sqrt(Math.Pow(Math.Abs((double)vector.Points[0].X), 2) + Math.Pow(Math.Abs((double)vector.Points[0].Y), 2)));
                //узнаем расстояние конечной точки этого-же отрезка
                decimal iE = (decimal)(Math.Sqrt(Math.Pow(Math.Abs((double)vector.Points[vector.Points.Count - 1].X), 2) + Math.Pow(Math.Abs((double)vector.Points[vector.Points.Count - 1].Y), 2)));

                if (iS < minDist || iE < minDist)
                {
                    //если оказались тут, осталось определить что ближе, начало отрезка или конец отрезка
                    if (iS < iE)
                    {
                        minDist = iS;
                        needReversVector = false;
                    }
                    else
                    {
                        minDist = iE;
                        needReversVector = true;
                    }
                    tmpVector = vector;
                }
            }

            //теперь самый ближний вектор, перенесем в новый список
            destCVectors.Add(tmpVector);
            // и данный вектор удалим из прежнего списка
            pageVectorNOW.Remove(tmpVector);

            if (needReversVector)  // нужно сменить порядок координат
            {
                destCVectors[destCVectors.Count-1].Points.Reverse();
            }

            



            //теперь зациклимся пока не отсортируем
            while (pageVectorNOW.Count > 0)
            {
                // из временного вектора берем последний отрезок, и в нем последнюю точку, с которой дальше
                // будем искать самую короткую дистанцию
                Location currPoint =
                    destCVectors[destCVectors.Count-1].Points[destCVectors[destCVectors.Count-1].Points.Count-1];

                tmpVector = new Segment();
                minDist = 99999;
                needReversVector = false;

                foreach (Segment vector in pageVectorNOW)
                {
                    //узнаем расстояние начальной точки отрезка 
                    decimal iS = (decimal)(Math.Sqrt(Math.Pow(Math.Abs((double)vector.Points[0].X - (double)currPoint.X), 2) + Math.Pow(Math.Abs((double)vector.Points[0].Y - (double)currPoint.Y), 2)));
                    //узнаем расстояние конечной точки этого-же отрезка
                    decimal iE = (decimal)(Math.Sqrt(Math.Pow(Math.Abs((double)vector.Points[vector.Points.Count - 1].X - (double)currPoint.X), 2) + Math.Pow(Math.Abs((double)vector.Points[vector.Points.Count - 1].Y - (double)currPoint.Y), 2)));


                    if (iS < minDist || iE < minDist)
                    {
                        //если оказались тут, осталось определить что ближе, начало отрезка или конец отрезка
                        if (iS < iE)
                        {
                            minDist = iS;
                            needReversVector = false;
                        }
                        else
                        {
                            minDist = iE;
                            needReversVector = true;
                        }
                        tmpVector = vector;
                    }
                }
                //теперь самый ближний вектор, перенесем в новый список
                destCVectors.Add(tmpVector);
                // и данный вектор удалим из прежнего списка
                pageVectorNOW.Remove(tmpVector);

                if (needReversVector)  // нужно сменить порядок координат
                {
                    destCVectors[destCVectors.Count - 1].Points.Reverse();
                }
            }

            pageVectorNOW = new List<Segment>(destCVectors);
            destCVectors.Clear();

            RefreshTree();

        }

        private void btOptimize2_Click(object sender, EventArgs e)
        {
            if (pageVectorNOW.Count < 2) return; //нет смысла сортировать

            // создаем временный список отрезков
            List<Segment> destCVectors = new List<Segment>();

            bool firstLoop = true;

            //теперь зациклимся пока не проанализируем все доступные отрезки
            while (pageVectorNOW.Count > 0)
            {

                if (firstLoop)
                {
                    destCVectors.Add(pageVectorNOW[0]);
                    pageVectorNOW.RemoveAt(0);
                    firstLoop = false;
                    continue;
                }


                //со втого круга проверяем отрезки

                Segment lastSegment = destCVectors[destCVectors.Count - 1];

                Location lastLocation = lastSegment.Points[lastSegment.Points.Count - 1];

                Location currPoint = pageVectorNOW[0].Points[0];

                if (currPoint.X == lastLocation.X && currPoint.Y == lastLocation.Y )
                {
                    //скопируем точки, и удалим отрезок
                    foreach (Location point in pageVectorNOW[0].Points)
                    {
                        lastSegment.Points.Add(point);
                        //destCVectors[destCVectors.Count - 1].Points[
                        //    destCVectors[destCVectors.Count - 1].Points.Add(new cPoint(point.X,point.Y,point.Selected));


                    }
                    pageVectorNOW.RemoveAt(0);


                }
                else
                {
                    //создадим новый отрезок
                    destCVectors.Add(pageVectorNOW[0]);
                    pageVectorNOW.RemoveAt(0);
                }
            }

            pageVectorNOW = new List<Segment>(destCVectors);
            destCVectors.Clear();

            RefreshTree();
        }




        private void TestNewAlgoritm()
        {
            //все траектории у которых начальная и конечная точка совпадают, должны быть разъеденены
            foreach (Segment vector in pageVectorNOW)
            {
                if (vector.Points.Count < 3) continue;

                Location startPoint = vector.Points[0];
                Location endPoint = vector.Points[vector.Points.Count - 1];

                if (startPoint.X == endPoint.X && startPoint.Y == endPoint.Y ) endPoint.X += (decimal)0.01;


            }


            //pageVectorNOW.Clear();

            List<Segment>tmp = new List<Segment>();

            foreach (Segment vector in pageVectorNOW)
            {

                List<Location> result = DouglasPeuckerReduction(vector.Points, (double) numericUpDown1.Value);

                if (result.Count > 2) tmp.Add(new Segment(result));
            }
            pageVectorNOW = tmp;

            RefreshTree();

        }




        private void button1_Click(object sender, EventArgs e)
        {

            TestNewAlgoritm();

        }
















        /// <summary>
        /// Uses the Douglas Peucker algorithim to reduce the number of points.
        /// </summary>
        /// <param name="Points">The points.</param>
        /// <param name="Tolerance">The tolerance.</param>
        /// <returns></returns>
        public static List<Location> DouglasPeuckerReduction(List<Location> Points, Double Tolerance)
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

            List<Location> returnPoints = new List<Location>();
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
        private static void DouglasPeuckerReduction(List<Location> points, Int32 firstPoint, Int32 lastPoint, Double tolerance, ref List<Int32> pointIndexsToKeep)
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
        public static Double PerpendicularDistance(Location Point1, Location Point2, Location Point)
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

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            TestNewAlgoritm();

        }

        private void btCloseTraectory_Click(object sender, EventArgs e)
        {

            List<Segment> destCVectors = new List<Segment>();

            foreach (Segment vector in pageVectorNOW)
            {
                Segment newSegment = new Segment();

                foreach (Location pp in vector.Points)
                {
                    newSegment.Points.Add(new Location(pp.X,pp.Y));
                }

                if (newSegment.Points.Count > 2)
                {
                    Location p1 = newSegment.Points[0];
                    Location p2 = newSegment.Points[newSegment.Points.Count - 1];

                    if (p1.X == p2.X && p1.Y == p2.Y)
                    {
                        //not need
                        
                    }
                    else
                    {
                        newSegment.Points.Add(new Location(p1.X, p1.Y));
                    }
                }

                destCVectors.Add(newSegment);
                newSegment = new Segment();
            }

            pageVectorNOW = destCVectors;

            RefreshTree();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            List<Segment> destCVectors = new List<Segment>();

            foreach (Segment vector in pageVectorNOW)
            {

                decimal minX =  9999;
                decimal maxX = -9999;
                decimal minY =  9999;
                decimal maxY = -9999;


                //получим максимум и минимум точек отрезка
                foreach (Location pp in vector.Points)
                {
                    if (minX > pp.X) minX = pp.X;

                    if (minY > pp.Y) minY = pp.Y;

                    if (maxX < pp.X) maxX = pp.X;

                    if (maxY < pp.Y) maxY = pp.Y;
                }

                decimal radius = (((maxX - minX) + (maxY - minY))/2)/2;

                if (checkBoxGena.Checked) radius = numericGena.Value;

                decimal centrX = ((maxX - minX)/2)+minX;
                decimal centrY = ((maxY - minY)/2)+minY;

                Location centrCircle = new Location(centrX, centrY);

                Segment newSegment = new Segment();



                int segmentsCount = (int)numericUpDown2.Value;


                for (int i = 0; i < segmentsCount; i++)
                {
                    float rx = (float)radius * (float)Math.Cos(2 * (float)Math.PI / segmentsCount * i);
                    float ry = (float)radius * (float)Math.Sin(2 * (float)Math.PI / segmentsCount * i);

                    newSegment.Points.Add(new Location(centrX + (decimal)rx, centrY + (decimal)ry));

                }

                newSegment.Points.Add(new Location(newSegment.Points[0].X, newSegment.Points[0].Y));



                //в цикле....
                //  newSegment.Points.Add(new Location(pp.X, pp.Y));

                //newSegment.Points.Add(new Location(centrX- radius, centrY- radius));
                //newSegment.Points.Add(new Location(centrX+ radius, centrY- radius));
                //newSegment.Points.Add(new Location(centrX+ radius, centrY+ radius));
                //newSegment.Points.Add(new Location(centrX- radius, centrY+ radius));
                //newSegment.Points.Add(new Location(centrX- radius, centrY- radius));

                //if (newSegment.Points.Count > 2)
                //{
                //    Location p1 = newSegment.Points[0];
                //    Location p2 = newSegment.Points[newSegment.Points.Count - 1];

                //    if (p1.X != p2.X && p1.Y != p2.Y)
                //    {
                //        newSegment.Points.Add(new Location(p1.X, p1.Y));
                //    }
                //}

                destCVectors.Add(newSegment);
                newSegment = new Segment();
            }

            pageVectorNOW = destCVectors;

            RefreshTree();
        }
    }
}
