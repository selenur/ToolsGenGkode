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
            //MyEventArgs e = new MyEventArgs();
            //e.ActionRun = message;

            //EventHandler handler = IsChange;
            //if (handler != null) IsChange?.Invoke(this, e);

            MAIN.PreviewImage(pageImageNOW);
            MAIN.PreviewVectors(pageVectorNOW);
        }

        private MainForm MAIN;

        public page05_SelectFileImageRastr(MainForm mf)
        {
            InitializeComponent();

            PageName = @"Выбор файла рисунка (растр) (5)";
            LastPage = 1;
            CurrPage = 5;
            NextPage = 9;

            MAIN = mf;

            pageImageNOW = null;
            pageVectorNOW = new List<GroupPoint>();
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

                Properties.Settings.Default.page05SelectedFile = textBoxFileName.Text;
                Properties.Settings.Default.Save();
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
        public List<GroupPoint> pageVectorIN { get; set; }
        public List<GroupPoint> pageVectorNOW { get; set; }


        public void actionBefore()
        {
            string sp1 = Properties.Settings.Default.page05SelectedFile;

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

            //TODO: обратить внимание на ориентацию осей

            // параметры расположения координатной оси
            if (Properties.Settings.Default.page01AxisVariant == 2) tmp.RotateFlip(RotateFlipType.RotateNoneFlipY);

            pageImageIN = tmp;
            pageImageNOW = (Bitmap)pageImageIN.Clone();
            pageVectorNOW = new List<GroupPoint>();


            CreateEvent("");
            //CreateEvent("RefreshImage_05");

        }
    }
}
