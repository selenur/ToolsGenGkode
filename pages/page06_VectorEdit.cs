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
            //MyEventArgs e = new MyEventArgs();
            //e.ActionRun = message;

            //EventHandler handler = IsChange;
            //if (handler != null) IsChange?.Invoke(this, e);

            MAIN.PreviewImage(pageImageNOW);
            MAIN.PreviewVectors(pageVectorNOW);
        }


        private MainForm MAIN;

        public page06_VectorEdit(MainForm mf)
        {
            InitializeComponent();

            PageName = @"Ручная корректировка векторов (6)";
            LastPage = 0;
            CurrPage = 6;
            NextPage = 7;

            MAIN = mf;
        }

        private void page06_VectorEdit_Load(object sender, EventArgs e)
        {
            if (Setting.GenaMode)
            {
                ForGena.Visible = true;
            }
        }

        //public List<cncPoint> PagePoints { get; set; }

        public void actionBefore()
        {
            pageVectorNOW = VectorProcessing.ListGroupPointClone(pageVectorIN);
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

            foreach (GroupPoint line in pageVectorNOW)
            {
                TreeNode tn = treeViewVectors.Nodes.Add("Точек: " + line.Points.Count);

                foreach (cncPoint point in line.Points)
                {
                    tn.Nodes.Add(@"x: " + point.X + @" y: " + point.Y);

                    countpoints++;
                }
                countLine++;
            }

            labelTraectoryInfo.Text = @"Траектория состоит из:" + Environment.NewLine + countLine.ToString() + @" отрезков," + Environment.NewLine + countpoints.ToString() + @" точек.";

            pageImageNOW = null;

            CreateEvent("");
            //CreateEvent("RefreshImage_06");

        }

        private void btLoadVectors_Click(object sender, EventArgs e)
        {
            pageVectorNOW = VectorProcessing.ListGroupPointClone(pageVectorIN);
            pageImageNOW = null;


            RefreshTree();
        }

        private void treeViewVectors_AfterSelect(object sender, TreeViewEventArgs e)
        {
            foreach (GroupPoint vector in pageVectorNOW)
            {
                vector.Selected = false;

                foreach (cncPoint VARIABLE in vector.Points)
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

            CreateEvent("");
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
        public List<GroupPoint> pageVectorIN { get; set; }
        public List<GroupPoint> pageVectorNOW { get; set; }


        private void btOptimize1_Click(object sender, EventArgs e)
        {
            // Нужно отсортировать отрезки, для минимизации холостого хода

            if(pageVectorNOW.Count<2) return; //нет смысла сортировать

            // создаем временный список отрезков
            List<GroupPoint> destCVectors = new List<GroupPoint>();
            
            //начинаем поиск отрезка, который ближе всего к координатам 0,0

            GroupPoint tmpVector = new GroupPoint();
            decimal minDist = 99999;
            bool needReversVector = false;


            foreach (GroupPoint vector  in pageVectorNOW)
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
                cncPoint currPoint =
                    destCVectors[destCVectors.Count-1].Points[destCVectors[destCVectors.Count-1].Points.Count-1];

                tmpVector = new GroupPoint();
                minDist = 99999;
                needReversVector = false;

                foreach (GroupPoint vector in pageVectorNOW)
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

            pageVectorNOW = new List<GroupPoint>(destCVectors);
            destCVectors.Clear();

            RefreshTree();

        }

        private void btOptimize2_Click(object sender, EventArgs e)
        {
            if (pageVectorNOW.Count < 2) return; //нет смысла сортировать

            // создаем временный список отрезков
            List<GroupPoint> destCVectors = new List<GroupPoint>();

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

                GroupPoint lastGroupPoint = destCVectors[destCVectors.Count - 1];

                cncPoint lastCncPoint = lastGroupPoint.Points[lastGroupPoint.Points.Count - 1];

                cncPoint currPoint = pageVectorNOW[0].Points[0];

                if (currPoint.X == lastCncPoint.X && currPoint.Y == lastCncPoint.Y )
                {
                    //скопируем точки, и удалим отрезок
                    foreach (cncPoint point in pageVectorNOW[0].Points)
                    {
                        lastGroupPoint.Points.Add(point);
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

            pageVectorNOW = new List<GroupPoint>(destCVectors);
            destCVectors.Clear();

            RefreshTree();
        }




        private void TestNewAlgoritm()
        {
            //все траектории у которых начальная и конечная точка совпадают, должны быть разъеденены
            foreach (GroupPoint vector in pageVectorNOW)
            {
                if (vector.Points.Count < 3) continue;

                cncPoint startPoint = vector.Points[0];
                cncPoint endPoint = vector.Points[vector.Points.Count - 1];

                if (startPoint.X == endPoint.X && startPoint.Y == endPoint.Y ) endPoint.X += 0.01;


            }



            List<GroupPoint>tmp = new List<GroupPoint>();

            foreach (GroupPoint vector in pageVectorNOW)
            {

                List<cncPoint> result = VectorProcessing.DouglasPeuckerReduction(vector.Points, (double) numericUpDown1.Value);

                if (result.Count > 2) tmp.Add(new GroupPoint(result));
            }
            pageVectorNOW = tmp;

            RefreshTree();

        }




        private void button1_Click(object sender, EventArgs e)
        {

            TestNewAlgoritm();

        }















        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            TestNewAlgoritm();

        }

        private void btCloseTraectory_Click(object sender, EventArgs e)
        {

            List<GroupPoint> destCVectors = new List<GroupPoint>();

            foreach (GroupPoint vector in pageVectorNOW)
            {
                GroupPoint newGroupPoint = new GroupPoint();

                foreach (cncPoint pp in vector.Points)
                {
                    newGroupPoint.Points.Add(new cncPoint(pp.X,pp.Y));
                }

                if (newGroupPoint.Points.Count > 2)
                {
                    cncPoint p1 = newGroupPoint.Points[0];
                    cncPoint p2 = newGroupPoint.Points[newGroupPoint.Points.Count - 1];

                    if (p1.X == p2.X && p1.Y == p2.Y)
                    {
                        //not need
                        
                    }
                    else
                    {
                        newGroupPoint.Points.Add(new cncPoint(p1.X, p1.Y));
                    }
                }

                destCVectors.Add(newGroupPoint);
                newGroupPoint = new GroupPoint();
            }

            pageVectorNOW = destCVectors;

            RefreshTree();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            List<GroupPoint> destCVectors = new List<GroupPoint>();

            foreach (GroupPoint vector in pageVectorNOW)
            {

                double minX =  9999;
                double maxX = -9999;
                double minY =  9999;
                double maxY = -9999;


                //получим максимум и минимум точек отрезка
                foreach (cncPoint pp in vector.Points)
                {
                    if (minX > pp.X) minX = pp.X;

                    if (minY > pp.Y) minY = pp.Y;

                    if (maxX < pp.X) maxX = pp.X;

                    if (maxY < pp.Y) maxY = pp.Y;
                }

                double radius = (((maxX - minX) + (maxY - minY))/2)/2;

                if (checkBoxGena.Checked) radius = (double)numericGena.Value;

                double centrX = ((maxX - minX)/2)+minX;
                double centrY = ((maxY - minY)/2)+minY;

                cncPoint centrCircle = new cncPoint(centrX, centrY);

                GroupPoint newGroupPoint = new GroupPoint();



                int segmentsCount = (int)numericUpDown2.Value;


                for (int i = 0; i < segmentsCount; i++)
                {
                    float rx = (float)radius * (float)Math.Cos(2 * (float)Math.PI / segmentsCount * i);
                    float ry = (float)radius * (float)Math.Sin(2 * (float)Math.PI / segmentsCount * i);

                    newGroupPoint.Points.Add(new cncPoint(centrX + (double)rx, centrY + (double)ry));

                }

                newGroupPoint.Points.Add(new cncPoint(newGroupPoint.Points[0].X, newGroupPoint.Points[0].Y));



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

                destCVectors.Add(newGroupPoint);
                newGroupPoint = new GroupPoint();
            }

            pageVectorNOW = destCVectors;

            RefreshTree();
        }
    }
}
