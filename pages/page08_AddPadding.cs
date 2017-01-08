// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using ClipperLib;

namespace ToolsGenGkode.pages
{

    using Polygon = List<IntPoint>;
    using Polygons = List<List<IntPoint>>;


    public partial class page08_AddPadding : UserControl, PageInterface
    {


        void CreateEvent(string message = "")
        {

        }

        private MainForm MAIN;

        public page08_AddPadding(MainForm mf)
        {
            InitializeComponent();

            PageName = @"Добавление отступов (8)";
            LastPage = 0;
            CurrPage = 8;
            NextPage = 10;

            MAIN = mf;

            pageImageNOW = null;
            pageVectorNOW = new List<GroupPoint>();
        }

        private void page12_AddPadding_Load(object sender, EventArgs e)
        {

        }

        public string PageName { get; set; }
        public int LastPage { get; set; }
        public int CurrPage { get; set; }
        public int NextPage { get; set; }
        public Bitmap pageImageIN { get; set; }
        public Bitmap pageImageNOW { get; set; }
        public List<GroupPoint> pageVectorIN { get; set; }
        public List<GroupPoint> pageVectorNOW { get; set; }
        public List<cncPoint> PagePoints { get; set; }


        public void actionBefore()
        {
            // заполним таблицу списком векторов
            dataGridView1.Rows.Clear();
            

            foreach (GroupPoint varVector in pageVectorNOW)
            {
                DataGridViewRow dgvr = new DataGridViewRow();
                
                int indx = dataGridView1.Rows.Add(dgvr);

                dataGridView1.Rows[indx].Cells[0].Value = indx;
                dataGridView1.Rows[indx].Cells[1].Value = "Траектория";
                dataGridView1.Rows[indx].Cells[2].Value = 0;

            }
        }

        public void actionAfter()
        {
            //throw new NotImplementedException();
        }

        private int CurrentSelectedRow = -1;

        private void dataGridView1_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            int selectedRow = e.RowIndex;

            if (selectedRow == -1)
            {
                labelNumTraectory.Text = "траектория не выбрана!";
                numericDiff.Value = 0;
                CurrentSelectedRow = -1;
                return;
            }
            CurrentSelectedRow = selectedRow;

            labelNumTraectory.Text = "№ траектории: " + selectedRow.ToString();

            decimal decvalue = 0;

            if (dataGridView1.Rows[selectedRow].Cells[2].Value == null) return;

            decimal.TryParse(dataGridView1.Rows[selectedRow].Cells[2].Value.ToString(), out decvalue);

            numericDiff.Value = decvalue;

            pageVectorNOW[selectedRow].Selected = true;

            CreateEvent();
        }

        private void numericDiff_ValueChanged(object sender, EventArgs e)
        {
            dataGridView1.Rows[CurrentSelectedRow].Cells[2].Value = numericDiff.Value;
            RefreshPrewievData();
        }

        //вызывается для отрисовки
        private void RefreshPrewievData()
        {
            CreateEvent("ReloadData_12"); //переполучим данные с предыдущей страицы

            //уже имеет оригинальные данные, теперь добавим новые траектории

            Polygons pSource = new Polygons();
            Polygons pDestin = new Polygons();

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                int diff = 0;

                int.TryParse(row.Cells[2].Value.ToString(), out diff);

                if (diff == 0) continue;

                long scale = 1000; //что-бы оперировать целочисленными значениями

                List<IntPoint> lpoint = new Polygon();

                foreach (cncPoint varPoint in pageVectorNOW[row.Index].Points)
                {
                    lpoint.Add(new IntPoint((long)(varPoint.X * (int)scale), (long)(varPoint.Y * (int)scale)));
                }

                pSource.Add(lpoint);

                pDestin = GetOffsetPolugon(pSource, (double)diff * 1000);

                if (pDestin.Count == 0) continue;

                List<cncPoint> ttmCPoints = new List<cncPoint>();

                foreach (IntPoint VARpoint in pDestin[0])
                {
                    ttmCPoints.Add(new cncPoint(VARpoint.X/1000,VARpoint.Y/1000));
                }

                pageVectorNOW.Add(new GroupPoint(ttmCPoints,false,DirrectionGroupPoint.Left ,true));
            }

            CreateEvent(); //"RefreshVector_12"
        }


        static private PointF[] PolygonToPointFArray(Polygon pg, float scale)
        {
            PointF[] result = new PointF[pg.Count];
            for (int i = 0; i < pg.Count; ++i)
            {
                result[i].X = (float)pg[i].X / scale;
                result[i].Y = (float)pg[i].Y / scale;
            }
            return result;
        }

        ClipType GetClipType()
        {

            //return ClipType.ctIntersection;
            return ClipType.ctUnion;
            //return ClipType.ctDifference;
            //return ClipType.ctXor;
        }

        PolyFillType GetPolyFillType()
        {
            return PolyFillType.pftNonZero;
            //return PolyFillType.pftEvenOdd;
        }



        private void AddPaddingTraectory()
        {
            //long scale = 1000;

            List<IntPoint> lpoint = new Polygon();

            //lpoint.Add(new IntPoint(10 * scale, 10 * scale));
            //lpoint.Add(new IntPoint(10 * scale, 20 * scale));
            //lpoint.Add(new IntPoint(20 * scale, 20 * scale));
            //lpoint.Add(new IntPoint(20 * scale, 10 * scale));
            //lpoint.Add(new IntPoint(16 * scale, 10 * scale));
            //lpoint.Add(new IntPoint(16 * scale, 15 * scale));
            //lpoint.Add(new IntPoint(14 * scale, 15 * scale));
            //lpoint.Add(new IntPoint(14 * scale, 10 * scale));




            Polygons pSource = new Polygons();
            Polygons pDestin = new Polygons();

            pSource.Add(lpoint);

            pDestin = GetOffsetPolugon(pSource, (double)numericDiff.Value * 10000);

            ////отобразим результат
            //Bitmap mybitmap = new Bitmap(pictureBox1.ClientRectangle.Width, pictureBox1.ClientRectangle.Height, PixelFormat.Format32bppArgb);


            //using (Graphics newgraphic = Graphics.FromImage(mybitmap))
            //{
            //    newgraphic.SmoothingMode = SmoothingMode.AntiAlias;
            //    newgraphic.Clear(Color.White);

            //    GraphicsPath path1 = new GraphicsPath();
            //    path1.FillMode = FillMode.Winding;

            //    GraphicsPath path2 = new GraphicsPath();
            //    path2.FillMode = FillMode.Winding;

            //    foreach (Polygon pg in pSource)
            //    {
            //        PointF[] pts = PolygonToPointFArray(pg, scale / 10);
            //        path1.AddPolygon(pts);
            //        pts = null;
            //    }

            //    foreach (Polygon pg in pDestin)
            //    {
            //        PointF[] pts = PolygonToPointFArray(pg, scale / 10);
            //        path2.AddPolygon(pts);
            //        pts = null;
            //    }

            //    newgraphic.DrawPath(new Pen(Color.Blue, 1.0f), path1);
            //    newgraphic.DrawPath(new Pen(Color.Red, 1.0f), path2);
            //}

            //pictureBox1.Image = mybitmap;


        }


        private Polygons GetOffsetPolugon(Polygons source, double offset)
        {
            if (offset != 0)
            {
                Polygons solution2 = new Polygons();

                ClipperOffset co = new ClipperOffset();
                co.AddPaths(source, JoinType.jtRound, EndType.etClosedPolygon);
                co.Execute(ref solution2, offset);
                return solution2;
            }

            return new Polygons(source);
        }

    }
}
