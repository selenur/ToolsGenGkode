// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.Windows.Forms;

namespace ToolsGenGkode.pages
{
    public partial class page02_EnterText : UserControl, PageInterface
    {

        private MainForm MAIN;

        public page02_EnterText(MainForm mf)
        {
            InitializeComponent();

            MAIN = mf;

            pageImageIN = null;
            pageImageNOW = null;
            pageVectorIN = new List<GroupPoint>();
            pageVectorNOW = new List<GroupPoint>();

            NextPage = 6;

            // Заполним списком шрифтов установленных в систему
            InstalledFontCollection installedFontCollection = new InstalledFontCollection();

            foreach (FontFamily fnt in installedFontCollection.Families)
            {
                comboBoxFont.Items.Add(fnt.Name);
            }

            installedFontCollection.Dispose();

            if (comboBoxFont.Items.Count > 0) comboBoxFont.Text = comboBoxFont.Items[0].ToString();

            rbUseSystemFont.Checked = true;
            rbFontToVector.Checked = true;

        }

        public void actionBefore()
        {
            MAIN.PageName.Text = @"Ввод текста (2)";
            MAIN.PageName.Tag = Tag;

            UserActions();

        }

        public void actionAfter()
        {

        }

        public bool IsReady()
        {
            return true;
        }


        private void SelectFont_Load(object sender, EventArgs e)
        {

        }

        private void UserActions()
        {
            if (rbUseSystemFont.Checked)
            {
                buttonSetFontFile.Visible = false;
                nameFontFile.Visible      = false;
                comboBoxFont.Visible      = true;
            }
            else
            {
                buttonSetFontFile.Visible = true;
                nameFontFile.Visible      = true;
                comboBoxFont.Visible      = false;
            }

            if (rbFontToVector.Checked)
            {
                NextPage = 6;
            }
            else
            {
                if (rbFontToImage.Checked) NextPage = 9;
                else
                {
                    NextPage = 4;
                }
            }

            if (rbUseSystemFont.Checked) //используем системный шрифт
            {
                pageVectorNOW = VectorProcessing.GetVectorFromText(textString.Text, comboBoxFont.Text, (float)textSize.Value);
                pageImageNOW  = ImageProcessing.CreateBitmapFromText(textString.Text, comboBoxFont.Text, (float)textSize.Value);
            }
            else  //используем внешний файл шрифта
            {
                pageVectorNOW = VectorProcessing.GetVectorFromText(textString.Text, comboBoxFont.Text, (float)textSize.Value, nameFontFile.Text);
                pageImageNOW  = ImageProcessing.CreateBitmapFromText(textString.Text, comboBoxFont.Text, (float)textSize.Value, nameFontFile.Text);
            }

            //и замкнем траектории
            foreach (GroupPoint vVector in pageVectorNOW)
            {
                vVector.Points.Add(vVector.Points[0]);
            }

            if (rbFontToImage.Checked) pageVectorNOW = new List<GroupPoint>();
            if (rbFontToVector.Checked) pageImageNOW = null;

            if (rbFontToVector.Checked)
            {
                MAIN.PreviewDada(null, VectorProcessing.ListGroupPointClone(pageVectorNOW));
            }
            else
            {
                MAIN.PreviewDada(pageImageNOW,new List<GroupPoint>());
            }
        }

        private void buttonSetFontFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofDialog = new OpenFileDialog
            {
                CheckFileExists = true,
                Multiselect = false,
                Title = @"Выбор файла шрифта",
                Filter = @"Font files (*.ttf)|*.ttf",
                FilterIndex = 2,
                RestoreDirectory = true
            };
            if (ofDialog.ShowDialog() == DialogResult.OK)
            {
                nameFontFile.Text = ofDialog.FileName;
                UserActions();
            }
        }

        private void rbUseSystemFont_CheckedChanged(object sender, EventArgs e)
        {
            UserActions();
        }

        private void rbFontFromFile_CheckedChanged(object sender, EventArgs e)
        {
            UserActions();
        }

        private void comboBoxFont_SelectedIndexChanged(object sender, EventArgs e)
        {
            UserActions();
        }

        private void rbFontToVector_CheckedChanged(object sender, EventArgs e)
        {
            UserActions();
        }

        private void rbFontToImage_CheckedChanged(object sender, EventArgs e)
        {
            UserActions();
        }

        private void textSize_ValueChanged(object sender, EventArgs e)
        {
            UserActions();
        }

        private void textString_TextChanged(object sender, EventArgs e)
        {
            UserActions();
        }


        public Bitmap pageImageIN { get; set; }
        public Bitmap pageImageNOW { get; set; }
        public List<GroupPoint> pageVectorIN { get; set; }
        public List<GroupPoint> pageVectorNOW { get; set; }

    }
}


//toolTips myToolTip1 = new toolTips();

//myToolTip1.Size = new Size(300, 200);
//myToolTip1.BackColor = Color.FromArgb(255, 255, 192);
//myToolTip1.ForeColor = Color.Navy;
//myToolTip1.BorderColor = Color.FromArgb(128, 128, 255);

//myToolTip1.SetToolTip(rbUseSystemFont, "Используется шрифт установленный в данной операционной системе.");
//myToolTip1.SetToolTip(rbFontFromFile, "Используется шрифт из файла, выбранного пользователем.");
//myToolTip1.SetToolTip(rbFontToVector, "Выбор данной опции позволит получить данные в виде набора векторов.");
//myToolTip1.SetToolTip(rbFontToImage, "Выбор данной опции позволит получить данные в виде рисунка.");
//myToolTip1.SetToolTip(textSize, "Указание размера в этом поле,\n влияет на размер получаемого рисунка, если выбран вариант 'В виде рисунка',\n или на качество траектории, если выбрано 'В виде отрезков'.");
