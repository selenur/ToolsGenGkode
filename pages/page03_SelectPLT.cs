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

        private MainForm MAIN;

        void CreateEvent(string message)
        {
            //MyEventArgs e = new MyEventArgs();
            //e.ActionRun = message;

            //EventHandler handler = IsChange;
            //if (handler != null) IsChange?.Invoke(this, e);


            MAIN.PreviewImage(null);
            MAIN.PreviewVectors(pageVectorNOW);



        }

        public page03_SelectPLT(MainForm mf)
        {
            
            InitializeComponent();

            PageName = @"Выбор PLT файла (3)";
            LastPage = 1;
            CurrPage = 3;
            NextPage = 6;

            MAIN = mf;

            pageImageNOW = null;
            pageVectorNOW = new List<GroupPoint>();
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


            if (!File.Exists(textBoxFileName.Text)) return;

            pageVectorNOW = VectorProcessing.GetVectorFromPLT(textBoxFileName.Text);

            CreateEvent("");
        }

        private void btShowOriginalImage_Click(object sender, EventArgs e)
        {
            CreateEvent("");
        }

        public Bitmap pageImageIN { get; set; }
        public Bitmap pageImageNOW { get; set; }
        public List<GroupPoint> pageVectorIN { get; set; }
        public List<GroupPoint> pageVectorNOW { get; set; }

 
    }
}
