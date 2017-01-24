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
        private MainForm MAIN;

        public page05_SelectFileImageRastr(MainForm mf)
        {
            InitializeComponent();

            MAIN = mf;

            pageImageIN = null;
            pageImageNOW = null;
            pageVectorIN = new List<GroupPoint>();
            pageVectorNOW = new List<GroupPoint>();

            NextPage = 9;

            string sp1 = Properties.Settings.Default.page05SelectedFile;

            if (sp1 != null)
            {
                textBoxFileName.Text = sp1;
            }

        }

        public void actionBefore()
        {
            MAIN.PageName.Text = @"Выбор файла рисунка (растр) (5)";
            MAIN.PageName.Tag = Tag;

            UserActions();
        }

        public void actionAfter()
        {
            UserActions();
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


            UserActions();
        }

        private void btShowOriginalImage_Click(object sender, EventArgs e)
        {
            UserActions();
        }





        private void page05_SelectFileImageRastr_Load(object sender, EventArgs e)
        {

        }

        private void UserActions()
        {

            if (!File.Exists(textBoxFileName.Text)) return;

            Bitmap tmp = ImageProcessing.CheckAndConvertImageto24bitPerPixel(new Bitmap(textBoxFileName.Text));

            //TODO: обратить внимание на ориентацию осей

            // параметры расположения координатной оси
            if (Properties.Settings.Default.page01AxisVariant == 2) tmp.RotateFlip(RotateFlipType.RotateNoneFlipY);

            pageImageIN = tmp;
            pageImageNOW = (Bitmap)pageImageIN.Clone();
            pageVectorNOW = new List<GroupPoint>();

            MAIN.PreviewDada(pageImageNOW, pageVectorNOW);

        }
    }
}
