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

        private MainForm MAIN;

        public page02_EnterText(MainForm mf)
        {
            InitializeComponent();

            MAIN = mf;


            PageName = @"Ввод текста (2)";
            LastPage = 1;
            CurrPage = 2;
            NextPage = 6;

            if (pageImageIN != null) pageImageNOW = (Bitmap)pageImageIN.Clone();
            pageVectorNOW = VectorProcessing.ListGroupPointClone(pageVectorIN);

            toolTips myToolTip1 = new toolTips();

            myToolTip1.Size = new Size(300, 200);
            myToolTip1.BackColor = Color.FromArgb(255, 255, 192);
            myToolTip1.ForeColor = Color.Navy;
            myToolTip1.BorderColor = Color.FromArgb(128, 128, 255);

            myToolTip1.SetToolTip(rbUseSystemFont, "Используется шрифт установленный в данной операционной системе.");
            myToolTip1.SetToolTip(rbFontFromFile, "Используется шрифт из файла, выбранного пользователем.");
            myToolTip1.SetToolTip(rbFontToVector, "Выбор данной опции позволит получить данные в виде набора векторов.");
            myToolTip1.SetToolTip(rbFontToImage, "Выбор данной опции позволит получить данные в виде рисунка.");
            myToolTip1.SetToolTip(textSize, "Указание размера в этом поле,\n влияет на размер получаемого рисунка, если выбран вариант 'В виде рисунка',\n или на качество траектории, если выбрано 'В виде отрезков'.");
            
        }

        private void SelectFont_Load(object sender, EventArgs e)
        {
            // Заполним списком шрифтов установленных в систему
            InstalledFontCollection installedFontCollection = new InstalledFontCollection();

            foreach (FontFamily fnt in installedFontCollection.Families)
            {
                comboBoxFont.Items.Add(fnt.Name);
            }

            installedFontCollection.Dispose();

            comboBoxFont.Text = comboBoxFont.Items[0].ToString();

            rbUseSystemFont.Checked = true;
            rbFontToVector.Checked = true;
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
                MAIN.PreviewImage(null);
                MAIN.PreviewVectors(pageVectorNOW);
            }
            else
            {
                MAIN.PreviewImage(pageImageNOW);
                MAIN.PreviewVectors(new List<GroupPoint>());
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
        public List<cncPoint> PagePoints { get; set; }


        public void actionBefore()
        {
            //throw new System.NotImplementedException();
        }

        public void actionAfter()
        {
            //throw new System.NotImplementedException();
        }
    }
}
