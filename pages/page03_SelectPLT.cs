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
        private MainForm MAIN;

        public page03_SelectPLT(MainForm mf)
        {
            
            InitializeComponent();

            MAIN = mf;

            pageImageIN = null;
            pageImageNOW = null;
            pageVectorIN = new List<GroupPoint>();
            pageVectorNOW = new List<GroupPoint>();

            NextPage = 6;
        }

        public void actionBefore()
        {
            MAIN.PageName.Text = @"Выбор PLT файла (3)";
            MAIN.PageName.Tag = Tag;

            UserActions();
        }

        public void actionAfter()
        {
            UserActions();
        }

        private void SelectPLT_Load(object sender, EventArgs e)
        {

        }

        private void UserActions()
        {
            if (!File.Exists(textBoxFileName.Text)) return;

            pageVectorNOW = VectorProcessing.GetVectorFromPLT(textBoxFileName.Text);

            MAIN.PreviewDada(null, pageVectorNOW);

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


            UserActions();

        }

        private void btShowOriginalImage_Click(object sender, EventArgs e)
        {
            UserActions();
        }



 
    }
}
