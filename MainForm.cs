// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using MultiLanguage;
using ToolsGenGkode.pages;
using ToolsGenGkode.Properties;

namespace ToolsGenGkode
{
    public partial class MainForm : Form
    {
        /// <summary>
        /// Перевод интерфейса
        /// </summary>
        private Translater TranslateInterface;
        /// <summary>
        /// Последовательность страниц для отображения
        /// </summary>
        private LinkedList<PageInterface> PathPages;
        /// <summary>
        /// Текущая отображаемая страница
        /// </summary>
        public LinkedListNode<PageInterface> CurrentPage;

        private Preview_Image    _pagePreviewImage;
        private Preview_Vectors  _pagePreviewVectors;

        public MainForm(string[] args)
        {
            InitializeComponent();

            // Для друга дополнительная фишка в программе
            foreach (string variable in args)
            {
                if (variable.ToUpper().Trim() == "-GENA") Setting.GenaMode = true;

            }
        }

        #region События формы

        private void Form_Load(object sender, EventArgs e)
        {
            _pagePreviewImage = new Preview_Image();
            _pagePreviewVectors = new Preview_Vectors();

            PathPages = new LinkedList<PageInterface>();
            // добавляем в список первую страницу
            PathPages.AddFirst(new page01_start(this));
            // и позиционируемся на первой странице
            CurrentPage = PathPages.First;
            // добавляем её на форму
            MainPanel.Controls.Add((Control)CurrentPage.Value);
            // и вызываем первоначальную инициализацию
            CurrentPage.Value.actionBefore();
            // инициализация Перевода интерфейса
            TranslateInterface = new Translater();
            // заполнение списком доступных языков
            FillMenuLanguage();
            // И запустим перевод интерфейса
            Translate();
        }

        /// <summary>
        /// Повторная инициализация модуля
        /// Применяется при переводе интерфейса, без перезапуска программы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        private void TranslateRestart()
        {
            TranslateInterface.Init();
            FillMenuLanguage();
            Translate();
        }

        /// <summary>
        /// Выбор пункта меню с вариантами языка интерфейса
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        private void LangClick(object sender, EventArgs eventArgs)
        {
            string sKey = ((ToolStripItem) sender).Name;

            Settings.Default.LanguageCode = sKey;
            Settings.Default.Save();

            TranslateRestart();
        }
        
        /// <summary>
        /// Заполнение меню связанное с выбором языка
        /// </summary>
        private void FillMenuLanguage()
        {
            //очистим список элементов
            languageCurrent.DropDownItems.Clear();

            //В меню добавим языки, которые доступны в файле
            foreach (LanguageDescription languageString in TranslateInterface.GetAvaibleLanguages())
            {
                ToolStripItem tsi = languageCurrent.DropDownItems.Add(languageString.Name, languageString.img);
                tsi.Tag = "_language_";
                tsi.Name = languageString.Key;
                tsi.Click += LangClick;
            }

            //// В меню добавим вызов окна перевода данных
            //ToolStripItem tsi2 = languageCurrent.DropDownItems.Add(@"RESTART MODULE");
            //tsi2.Tag = "_RestartTranslater_";
            //tsi2.Name = @"RESTARTMODULE";
            //tsi2.Text = @"RESTART MODULE";
            //tsi2.Click += TranslateReset;

            // Узнаем какой язык по умолчанию использовать
            string sKeyLang = Settings.Default.LanguageCode;

            LanguageDescription ls = TranslateInterface.GetFromKey(sKeyLang);

            if (ls != null)
            {
                // только при условии что язык с указаным кодом существует
                languageCurrent.Text = ls.Name;
                languageCurrent.Image = ls.img;
            }
        }

        //private void TranslateReset(object sender, EventArgs e)
        //{
        //    TranslateRestart();
        //}

        private void Translate()
        {
            LanguageDescription ls = TranslateInterface.GetFromKey(Settings.Default.LanguageCode);

            if (ls == null) return;

            TranslateInterface.Translate(this, ls);

            // заголовок программы по своему сделаем
            Text = TranslateInterface.Translate((string)Tag, ls) + @" (" + Application.ProductVersion + @")";
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            About abfrm = new About();
            abfrm.ShowDialog();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btBACK_Click(object sender, EventArgs e)
        {
            SetPage(DirectionPage.Back);
            Translate();
        }

        private void btFORWARD_Click(object sender, EventArgs e)
        {
            SetPage(DirectionPage.Forward);
            Translate();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Process currentProc = Process.GetCurrentProcess();
            toolStripStatusLabel2.Text = (currentProc.PrivateMemorySize64 / 1000000).ToString();
        }

        private void testLaserToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            // и теперь установим новую страницу
            panelPreview.Controls.Clear();
            panelPreview.Controls.Add(_pagePreviewImage);
            panelPreview.Controls[0].Dock = DockStyle.Fill;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            // и теперь установим новую страницу
            panelPreview.Controls.Clear();
            panelPreview.Controls.Add(_pagePreviewVectors);
            panelPreview.Controls[0].Dock = DockStyle.Fill;
        }
        #endregion

        public void PreviewDada(Bitmap sourceBitmap, List<GroupPoint> sourceListGroupPoint)
        {
            // если вообще нет доступных данных
            if (sourceBitmap == null && sourceListGroupPoint.Count == 0)
            {
                Label lb = new Label();
                lb.AutoSize = true;
                lb.Font = new Font(@"Microsoft Sans Serif", 15.75F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(204)));
                lb.ForeColor = Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
                lb.Location = new Point(117, 76);
                lb.Name = @"label2";
                lb.Size = new Size(334, 25);
                lb.TabIndex = 0;
                lb.Text = @"Нет данных для отображения";
                lb.Tag = @"_notFoundData_";

                panelPreview.Controls.Clear();
                panelPreview.Controls.Add(lb);

                radioButton1.Enabled = false;
                radioButton2.Enabled = false;

                return; // нет смысла дальше отображать что либо
            }

            radioButton1.Enabled = (sourceBitmap != null);
            radioButton2.Enabled = (sourceListGroupPoint.Count != 0);

            // текущий вариант отображения на панели предпросмотра
            // 0 - нет данных
            // 1 - рисунок
            // 2 - вектрные данные

            int currVariant = 0;

            if (panelPreview.Controls.Count != 0)
            {
                if (panelPreview.Controls[0].GetType() == typeof (Label)) currVariant = 0;

                if (panelPreview.Controls[0].GetType() == typeof(Preview_Image)) currVariant = 1;

                if (panelPreview.Controls[0].GetType() == typeof(Preview_Vectors)) currVariant = 2;
            }

            if (sourceBitmap == null)
            {
                _pagePreviewImage.SetImage(new Bitmap(1,1));
            }
            else
            {
                _pagePreviewImage.SetImage(sourceBitmap);
            }

            _pagePreviewVectors.SetNewData(sourceListGroupPoint);

            // переключение на рисунок, если возможно
            if ((radioButton1.Enabled && currVariant == 0) || (radioButton1.Enabled && !radioButton2.Enabled && currVariant ==2))
            {
                radioButton1.Checked = true;
                panelPreview.Controls.Clear();
                panelPreview.Controls.Add(_pagePreviewImage);
                panelPreview.Controls[0].Dock = DockStyle.Fill;                
            }
            //переключение на вектор
            if ((radioButton2.Enabled && currVariant == 0) || (!radioButton1.Enabled && radioButton2.Enabled && currVariant == 1))
            {
                radioButton2.Checked = true;
                panelPreview.Controls.Clear();
                panelPreview.Controls.Add(_pagePreviewVectors);
                panelPreview.Controls[0].Dock = DockStyle.Fill;
            }

        }

        /// <summary>
        /// Направления получения страницы
        /// </summary>
        private enum DirectionPage
        {
            Back = 0,
            Forward = 1
        }

        /// <summary>
        /// Установка нужной страницы
        /// </summary>
        private void SetPage(DirectionPage dir)
        {

            if (dir == DirectionPage.Forward)
            {
                //добавим запрос у текущей страницы, о том возможно-ли переходить на следующий шаг
                //if (CurrentPage)


                if (CurrentPage.Value.NextPage != 0)
                {
                    PageInterface nextPageI = GetNextPage(CurrentPage.Value.NextPage);

                    if (nextPageI != null)
                    {
                        CurrentPage.Value.actionAfter();

                        PathPages.AddAfter(CurrentPage, nextPageI);

                        LinkedListNode<PageInterface> NextPage = CurrentPage.Next;

                        if (CurrentPage.Value.pageImageNOW == null)
                        {
                            NextPage.Value.pageImageIN = null;
                        }
                        else
                        {
                            NextPage.Value.pageImageIN = (Bitmap)CurrentPage.Value.pageImageNOW.Clone();
                        }

                        NextPage.Value.pageVectorIN = VectorProcessing.ListGroupPointClone(CurrentPage.Value.pageVectorNOW);

                        CurrentPage = NextPage;

                    } //if (nextPageI != null)
                } // if (CurrentPage.Value.NextPage != 0)
            } // if (dir == DirectionPage.Forward)

            if (dir == DirectionPage.Back)
            {
                if (CurrentPage.Previous == null)
                {
                    btBACK.Enabled = false;
                    return;
                }

                CurrentPage = CurrentPage.Previous;
                //прошлый узел удалим
                PathPages.RemoveLast();
            }

            // А тепрь страницу добавим на форму
            MainPanel.Controls.Clear();
            MainPanel.Controls.Add((Control)CurrentPage.Value);
            // и вызываем первоначальную инициализацию
            CurrentPage.Value.actionBefore();

            // И установим доступность кнопок, вперед назад
            if (CurrentPage.Value.NextPage == 0)
            {
                btFORWARD.Enabled = false;
            }
            else
            {
                btFORWARD.Enabled = true;
            }

            if (CurrentPage.Previous == null)
            {
                btBACK.Enabled = false;
            }
            else
            {
                btBACK.Enabled = true;
            }
        }


        private PageInterface GetNextPage(int numberPage)
        {
            PageInterface result = null;

            if (numberPage == 2) return new page02_EnterText(this);

            if (numberPage == 3) return new page03_SelectPLT(this);

            if (numberPage == 4) return new page04_ImageToVector(this);

            if (numberPage == 5) return new page05_SelectFileImageRastr(this);

            if (numberPage == 6) return new page06_VectorEdit(this);

            if (numberPage == 7) return new page07_ModifyVectors(this);

            if (numberPage == 8) return new page08_AddPadding(this);

            if (numberPage == 9) return new page09_ImageRast(this);

            if (numberPage == 10) return new page10_generateGkode(this);

            if (numberPage == 11) return new page11_DXF(this);

            return result;
        }

    } // public partial class MainForm : Form

} //namespace ToolsGenGkode