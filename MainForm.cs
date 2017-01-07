// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using CalibrateLaserTools;
using MultiLanguage;
using ToolsGenGkode.pages;
using ToolsGenGkode.Properties;

namespace ToolsGenGkode
{
    public partial class MainForm : Form
    {

        public MainForm(string[] args)
        {
            InitializeComponent();

            // Для друга дополнительная фишка в программе
            foreach (string variable in args)
            {
                if (variable.ToUpper().Trim() == "-GENA") Setting.GenaMode = true;

            }
        }

        // СПИСОК ВОЗМОЖНЫХ СТРАНИЦ ДЛЯ ОТОБРАЖЕНИЯ 
        private page01_start                _page01Start;
        private page02_EnterText            _page02EnterText;
        private page03_SelectPLT            _page03SelectPlt;
        private page04_ImageToVector        _page04ImageToVector;
        private page05_SelectFileImageRastr _page05SelectFileImageRastr;
        private page06_VectorEdit           _page06VectorEdit;
        private page07_ModifyVectors        _page07ModifyVectors;
        private page08_AddPadding           _page08AddPadding;
        private page09_ImageRast            _page09SelectImage;
        private page10_generateGkode        _page10GenerateGkode;
        private page11_DXF                  _page11DXF;

        private Preview_Image               _pagePreviewImage;
        private Preview_Vectors             _pagePreviewVectors;

        //старый список страниц
        readonly List<PageInterface> _pageList = new List<PageInterface>();

        // Перевод интерфейса
        private Translater TranslateInterface;

        #region События формы
        
        private void Form_Load(object sender, EventArgs e)
        {
            _page01Start                = new page01_start(this);
            _page02EnterText            = new page02_EnterText(this);
            _page03SelectPlt            = new page03_SelectPLT(this);
            _page04ImageToVector        = new page04_ImageToVector(this);
            _page05SelectFileImageRastr = new page05_SelectFileImageRastr(this);
            _page06VectorEdit           = new page06_VectorEdit(this);
            _page07ModifyVectors        = new page07_ModifyVectors(this);
            _page08AddPadding           = new page08_AddPadding(this);
            _page09SelectImage          = new page09_ImageRast(this);
            _page10GenerateGkode        = new page10_generateGkode(this);
            _page11DXF                  = new page11_DXF(this);

            _pagePreviewImage           = new Preview_Image();
            _pagePreviewVectors         = new Preview_Vectors();

            // Принудительная установка первой страницы
            MainPanel.Controls.Add(_page01Start);
            PageName.Text = _page01Start.PageName;
            PageName.Tag = _page01Start.Tag;

            // Список доступных страниц для отображения
            _pageList.Add(null); //для красоты нумирации, что-бы не пользоваться нулем
            _pageList.Add(_page01Start);
            _pageList.Add(_page02EnterText);
            _pageList.Add(_page03SelectPlt);
            _pageList.Add(_page04ImageToVector);
            _pageList.Add(_page05SelectFileImageRastr);
            _pageList.Add(_page06VectorEdit);
            _pageList.Add(_page07ModifyVectors);
            _pageList.Add(_page08AddPadding);
            _pageList.Add(_page09SelectImage);
            _pageList.Add(_page10GenerateGkode);
            _pageList.Add(_page11DXF);


            // Перевод интерфейса
            TranslateInterface = new Translater();
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

            // В меню добавим вызов окна перевода данных
            ToolStripItem tsi2 = languageCurrent.DropDownItems.Add(@"RESTART MODULE");
            tsi2.Tag = "_RestartTranslater_";
            tsi2.Name = @"RESTARTMODULE";
            tsi2.Text = @"RESTART MODULE";
            tsi2.Click += TranslateReset;

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

        private void TranslateReset(object sender, EventArgs e)
        {
            TranslateRestart();
        }


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
            //TestLaser tsl = new TestLaser();
            //tsl.Show();

            Form1 frrm = new Form1();
            frrm.Show();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            // и теперь установим новую страницу
            panelPreview.Controls.Clear();
            panelPreview.Controls.Add(_pagePreviewImage);
            panelPreview.Controls[0].Dock = DockStyle.Fill;
            //_currPreview = @"image";
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            // и теперь установим новую страницу
            panelPreview.Controls.Clear();
            panelPreview.Controls.Add(_pagePreviewVectors);
            panelPreview.Controls[0].Dock = DockStyle.Fill;
            //_currPreview = @"vector";
        }
        #endregion


        #region Необходимость перерисовки предпросмотра

        public void PreviewImage(Bitmap sourceBitmap)
        {
            _pagePreviewImage.SetImage(sourceBitmap);

            SetActivityChech();

            if (sourceBitmap != null)
            {
                radioButton1.Checked = true;
            }

        }

        public void PreviewVectors(List<GroupPoint> sourceListGroupPoint)
        {
            _pagePreviewVectors.SetNewData(VectorProcessing.ListGroupPointClone(sourceListGroupPoint));

            SetActivityChech();

            if (sourceListGroupPoint.Count > 0)
            {
                radioButton2.Checked = true;
            }
        }

        public void SetActivityChech()
        {
            radioButton1.Enabled = _pagePreviewImage.containsData;

            radioButton2.Enabled = _pagePreviewVectors.containsData;

            string currElement = "";


            //узнаем имя текущего элемента
            if (panelPreview.Controls.Count != 0) currElement = panelPreview.Controls[0].Name;

            if (radioButton1.Checked && radioButton1.Enabled)
            {
                //проверим что текущий элемент это вывод рисунка

                if (currElement != @"Preview_Image")
                {
                    panelPreview.Controls.Clear();
                    panelPreview.Controls.Add(_pagePreviewImage);
                    panelPreview.Controls[0].Dock = DockStyle.Fill;
                }
            }

            if (radioButton2.Checked && radioButton2.Enabled)
            {
                //проверим что текущий элемент это вывод вектора

                if (currElement != @"Preview_Vectors")
                {
                    panelPreview.Controls.Clear();
                    panelPreview.Controls.Add(_pagePreviewVectors);
                    panelPreview.Controls[0].Dock = DockStyle.Fill;
                }
            }

            if (!radioButton1.Enabled && !radioButton2.Enabled)
            {
                if (currElement != @"label2")
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

                    panelPreview.Controls.Clear();
                    panelPreview.Controls.Add(lb);
                }
            }


            if (radioButton1.Checked && !radioButton1.Enabled)
            {
                //флаг стоит на изображении, но изображения нет, попробуем переключиться на вектор, если возможно

                if (radioButton2.Enabled) radioButton2.Checked = true;
            }

            if (radioButton2.Checked && !radioButton2.Enabled)
            {
                //флаг стоит на векторах, но векторов нет, попробуем переключиться на рисунок, если возможно

                if (radioButton1.Enabled) radioButton1.Checked = true;
            }

        }



        #endregion


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
            if (MainPanel.Controls.Count == 0) return; 

            // Текущая отображаемая страница
            PageInterface controlPageNow = (PageInterface)MainPanel.Controls[0];

            // Следующая страница для отображения
            PageInterface controlPageNext;

            if (dir == DirectionPage.Back)
            {
                controlPageNext = _pageList[controlPageNow.LastPage];

               // Debug.WriteLine("Направление НАЗАД - след страница " + controlPageNext.CurrPage);
            }
            else
            {
                controlPageNext = _pageList[controlPageNow.NextPage];
               // Debug.WriteLine("Направление ВПЕРЕД - след страница " + controlPageNext.CurrPage);
            }

            // Нет нужной страницы
            if (controlPageNext == null) return;

            // На текущей странице вызовем пересчет всех данных (если еэто необходимо)
            controlPageNow.actionAfter();

            if (dir == DirectionPage.Forward)
            {

                // 1) Скопируем изображение с текущей страницы на следующую, если оно доступно
                if (controlPageNow.pageImageNOW != null)
                {
                    controlPageNext.pageImageIN = (Bitmap) controlPageNow.pageImageNOW.Clone();
                }

                controlPageNext.pageVectorIN = VectorProcessing.ListGroupPointClone(controlPageNow.pageVectorNOW);
            }


            // установим нумерацию маршрута
            if (dir == DirectionPage.Back)
            {
                controlPageNext.NextPage = controlPageNow.CurrPage;
            }
            else
            {
                controlPageNext.LastPage = controlPageNow.CurrPage;
            }

            // На следующей странице запустим если необходимо подготовку перед открытием страницы
            controlPageNext.actionBefore();

            // и теперь установим новую страницу
            MainPanel.Controls.Clear();
            MainPanel.Controls.Add((Control)controlPageNext);
            PageName.Text = controlPageNext.PageName;
            PageName.Tag = ((Control)controlPageNext).Tag;

            btBACK.Enabled = true;
            btFORWARD.Enabled = true;

            if (controlPageNext.LastPage == 0)
            {
                btBACK.Enabled = false;
            }

            if (controlPageNext.NextPage == 0)
            {
                btFORWARD.Enabled = false;
            }

            if (dir == DirectionPage.Back)
            {
                //при обратном шаге установим начальные значения установленной предыдущей страницы
                PageInterface currPi = (PageInterface)MainPanel.Controls[0];

                _pagePreviewImage.SetImage(currPi.pageImageIN);

                _pagePreviewVectors.SetNewData(currPi.pageVectorIN);

                _pagePreviewVectors.SetNewData(new List<cncPoint>());
            }
        }

        // обработчик событий от страниц
        private void EventFromUc(object sender, EventArgs eventArgs)
        {
            string strMessage = (((MyEventArgs) (eventArgs))).ActionRun;

            if (strMessage.Length == 0) return;

            if (strMessage.Substring(0, 1) == "!")
            {
                return;
            }

            //// В окно предпросмотра выведем данные с текущей страницы
            //if (strMessage.IndexOf("RefreshVector_", StringComparison.Ordinal) >= 0)
            //{
            //    int iTmp;

            //    int.TryParse(strMessage.Substring(14, 2), out iTmp);

            //    if (iTmp == 0) return;

            //    _pagePreviewVectors.SetNewData(VectorProcessing.ListGroupPointClone(_pageList[iTmp].pageVectorNOW));
            //}

            //// В окно предпросмотра выведем данные с текущей страницы
            //if (strMessage.IndexOf("RefreshImage_", StringComparison.Ordinal) >= 0)
            //{
            //    int iTmp;

            //    int.TryParse(strMessage.Substring(13, 2), out iTmp);

            //    if (iTmp == 0) return;

            //    if (_pageList[iTmp].pageImageNOW == null)
            //    {
            //        _pagePreviewImage.SetImage(null);
            //        return;
            //    }
            //    _pagePreviewImage.SetImage(_pageList[iTmp].pageImageNOW);
            //}

            //radioButton1.Enabled = _pagePreviewImage.containsData;
            //radioButton2.Enabled = _pagePreviewVectors.containsData;

            //string currElement = "";

            ////узнаем имя текущего элемента
            //if (panelPreview.Controls.Count != 0) currElement = panelPreview.Controls[0].Name;

            //if (radioButton1.Checked && radioButton1.Enabled)
            //{
            //    //проверим что текущий элемент это вывод рисунка

            //    if (currElement != @"Preview_Image")
            //    {
            //        panelPreview.Controls.Clear();
            //        panelPreview.Controls.Add(_pagePreviewImage);
            //        panelPreview.Controls[0].Dock = DockStyle.Fill;
            //    }
            //}

            //if (radioButton2.Checked && radioButton2.Enabled)
            //{
            //    //проверим что текущий элемент это вывод вектора

            //    if (currElement != @"Preview_Vectors")
            //    {
            //        panelPreview.Controls.Clear();
            //        panelPreview.Controls.Add(_pagePreviewVectors);
            //        panelPreview.Controls[0].Dock = DockStyle.Fill;
            //    }
            //}

            //if (!radioButton1.Enabled && !radioButton2.Enabled)
            //{
            //    if (currElement != @"label2")
            //    {
            //        Label lb = new Label();
            //        lb.AutoSize = true;
            //        lb.Font = new Font(@"Microsoft Sans Serif", 15.75F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(204)));
            //        lb.ForeColor = Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            //        lb.Location = new Point(117, 76);
            //        lb.Name = @"label2";
            //        lb.Size = new Size(334, 25);
            //        lb.TabIndex = 0;
            //        lb.Text = @"Нет данных для отображения";

            //        panelPreview.Controls.Clear();
            //        panelPreview.Controls.Add(lb);            
            //    }
            //}


            //if (radioButton1.Checked && !radioButton1.Enabled)
            //{
            //    //флаг стоит на изображении, но изображения нет, попробуем переключиться на вектор, если возможно

            //    if (radioButton2.Enabled) radioButton2.Checked = true;
            //}

            //if (radioButton2.Checked && !radioButton2.Enabled)
            //{
            //    //флаг стоит на векторах, но векторов нет, попробуем переключиться на рисунок, если возможно

            //    if (radioButton1.Enabled) radioButton1.Checked = true;
            //}
        }
    } // public partial class MainForm : Form


   

    // Производный класс от EventArgs
    class MyEventArgs : EventArgs
    {
        public string ActionRun;
    }
} //namespace ToolsGenGkode