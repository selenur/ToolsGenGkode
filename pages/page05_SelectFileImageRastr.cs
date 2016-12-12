// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace ToolsGenGkode.pages
{
    // ReSharper disable once InconsistentNaming
    public partial class page05_SelectFileImageRastr : UserControl, PageInterface
    {
        /// <summary>
        /// Событие при изменении параметров на данной форме
        /// </summary>
        public event EventHandler IsChange;

        /// <summary>
        /// Посылка события главной форме
        /// </summary>
        /// <param name="message"></param>
        void CreateEvent(string message)
        {
            MyEventArgs e = new MyEventArgs();
            e.ActionRun = message;

            EventHandler handler = IsChange;
            if (handler != null) IsChange?.Invoke(this, e);
        }


        public page05_SelectFileImageRastr()
        {
            InitializeComponent();

            PageName = @"Выбор файла рисунка (растр) (5)";
            LastPage = 1;
            CurrPage = 5;
            NextPage = 9;

            pageImageNOW = null;
            pageVectorNOW = new List<Segment>();
        }

        private void buttonSelectFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.CheckFileExists = true;
            openFileDialog1.Multiselect = false;
            openFileDialog1.Title = @"Выбор рисунка";
            //openFileDialog1.InitialDirectory = "c:\\";
            openFileDialog1.Filter = @"Файлы рисунков(*.jpg; *.jpeg; *.gif; *.bmp;*.png)|*.jpg; *.jpeg; *.gif; *.bmp;*.png";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textBoxFileName.Text = openFileDialog1.FileName;

                IniParser.AddSetting("page05", "selectedFile", textBoxFileName.Text);
                IniParser.SaveSettings();
            }


            LoadData();
        }

        private void btShowOriginalImage_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        public string PageName { get; set; }
        public int LastPage { get; set; }
        public int CurrPage { get; set; }
        public int NextPage { get; set; }
        public Bitmap pageImageIN { get; set; }
        public Bitmap pageImageNOW { get; set; }
        public List<Segment> pageVectorIN { get; set; }
        public List<Segment> pageVectorNOW { get; set; }


        public void actionBefore()
        {
            string sp1 = IniParser.GetSetting("page05", "selectedFile");

            if (sp1 != null)
            {
                textBoxFileName.Text = sp1;
            }
        }

        public void actionAfter()
        {
            LoadData();
        }

        private void page05_SelectFileImageRastr_Load(object sender, EventArgs e)
        {

        }

        private void LoadData()
        {

            if (!File.Exists(textBoxFileName.Text)) return;

            Bitmap tmp = new Bitmap(textBoxFileName.Text);
            // параметры расположения координатной оси
            if (Property.Orientation == 2) tmp.RotateFlip(RotateFlipType.RotateNoneFlipY);

            pageImageIN = tmp;
            pageImageNOW = (Bitmap)pageImageIN.Clone();
            pageVectorNOW = new List<Segment>();


            CreateEvent("RefreshVector_05");
            CreateEvent("RefreshImage_05");

        }
    }
}
