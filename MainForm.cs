// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using MultiLanguage;
using ToolsGenGkode.pages;

namespace ToolsGenGkode
{
    public partial class MainForm : Form
    {

        public MainForm(string[] args)
        {
            InitializeComponent();

            foreach (string VARIABLE in args)
            {
                if (VARIABLE.ToUpper().Trim() == "-GENA") Setting.GenaMode = true;

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

        readonly List<PageInterface> _pageList = new List<PageInterface>();

        #region События формы

        private Translater _translater;

        private void Form_Load(object sender, EventArgs e)
        {
            //выберем профайл по умолчеию
            string catalog = string.Format("{0}\\", Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));

            IniParser.Init(catalog + @"\setting.ini");

            //string value = INI_Parser.GetSetting("main", "currentProfile");

            _page01Start                = new page01_start();
            _page02EnterText            = new page02_EnterText();
            _page03SelectPlt            = new page03_SelectPLT();
            _page04ImageToVector        = new page04_ImageToVector();
            _page05SelectFileImageRastr = new page05_SelectFileImageRastr();
            _page06VectorEdit           = new page06_VectorEdit();
            _page07ModifyVectors        = new page07_ModifyVectors();
            _page08AddPadding           = new page08_AddPadding();
            _page09SelectImage          = new page09_ImageRast();
            _page10GenerateGkode        = new page10_generateGkode();
            _page11DXF                  = new page11_DXF();

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

            // Подключим события ко всем страницам
            _page01Start.IsChange                += EventFromUc;
            _page02EnterText.IsChange            += EventFromUc;
            _page03SelectPlt.IsChange            += EventFromUc;
            _page04ImageToVector.IsChange        += EventFromUc;
            _page05SelectFileImageRastr.IsChange += EventFromUc;
            _page06VectorEdit.IsChange           += EventFromUc;
            _page07ModifyVectors.IsChange        += EventFromUc;
            _page08AddPadding.IsChange           += EventFromUc;
            _page09SelectImage.IsChange          += EventFromUc;
            _page10GenerateGkode.IsChange        += EventFromUc;
            _page11DXF.IsChange                  += EventFromUc;

            _translater = new Translater();
            _translater.Init(); //подгрузка данных из файла
            
            //Заполним списком языков меню
            foreach (LanguagePosition languageString in _translater.GetAvaibleLanguages())
            {
                ToolStripItem tsi = languageCurrent.DropDownItems.Add(languageString.Name, languageString.img);
                tsi.Tag = "_language_";
                tsi.Name = languageString.Key;
                tsi.Click += LangClick;

            }

            ToolStripItem tsi2 = languageCurrent.DropDownItems.Add(@"Edit translate dialog");
            tsi2.Tag = "_DialogTranslate_";
            tsi2.Name = @"DialogTranslate";
            tsi2.Click += DialogTranslateClick;

            string sKeyLang = IniParser.GetSetting("MAIN", "Language");

            if (sKeyLang != null)
            {
                LanguagePosition ls = _translater.GetFromKey(sKeyLang);

                languageCurrent.Text = ls.Name;
                languageCurrent.Image = ls.img;
                Property.Language = ls;
                Tranlate();
            }




        }

        private void DialogTranslateClick(object sender, EventArgs eventArgs)
        {
            _translater.ShowDialogEditTranslate(this.Controls);
        }

        private void LangClick(object sender, EventArgs eventArgs)
        {
            string sKey = ((ToolStripItem) sender).Name;

            LanguagePosition ls = _translater.GetFromKey(sKey);

            languageCurrent.Text = ls.Name;
            languageCurrent.Image = ls.img;

            Property.Language = ls;
            IniParser.AddSetting("MAIN", "Language", sKey);
            IniParser.SaveSettings();

            Tranlate();
        }

        private void Tranlate()
        {
            //заголовка программы
            Text = _translater.Translate((string)Tag, Property.Language) + @" (" + Application.ProductVersion + @")";

            //перевод всех элементов на форме
            _translater.Translate(this.Controls, Property.Language);

            //перевод элементов главного меню
            _translater.Translate(MainMenu, Property.Language);

            _translater.Translate(statusStrip1, Property.Language);
            

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
            Tranlate();
        }

        private void btFORWARD_Click(object sender, EventArgs e)
        {
            SetPage(DirectionPage.Forward);
            Tranlate();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Process currentProc = Process.GetCurrentProcess();
            toolStripStatusLabel2.Text = (currentProc.PrivateMemorySize64 / 1000000).ToString();
        }

        private void testLaserToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TestLaser tsl = new TestLaser();
            tsl.Show();
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


                // 2) Скопируем вектора на новую страницу
                //todo: попытка избавиться от ссылки
                List<Segment> tmpCVectors = new List<Segment>();
                //List<Location> tmpCPoints = new List<Location>();

                if (controlPageNow.pageVectorNOW != null)
                {
                    foreach (Segment vector in controlPageNow.pageVectorNOW)
                    {
                        tmpCVectors.Add(vector.Clone());
                        //tmpCPoints = new List<Location>();

                        //foreach (Location point in vector.Points)
                        //{
                        //    tmpCPoints.Add(new Location(point.X, point.Y, point.Svalue, point.Fvalue, point.Pvalue, point.SpindelOn, point.Selected));
                        //}
                        //tmpCVectors.Add(new Segment(tmpCPoints, vector.Selected,vector.TempTraectory,vector.Dirrect,vector.IndividualPoints));
                    }
                }
                controlPageNext.pageVectorIN = tmpCVectors;



                // 3) Скопируем точки
                //controlPageNext.PagePoints = controlPageNow.PagePoints;
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


                PageInterface currPI = (PageInterface)MainPanel.Controls[0];

                _pagePreviewImage.SetImage(currPI.pageImageIN);

                _pagePreviewVectors.SetNewData(currPI.pageVectorIN);


                _pagePreviewVectors.SetNewData(new List<Location>());

                //if (currPI.PagePoints != null)
                //{
                //    List<Location> ccc = new List<Location>();

                //    foreach (Location ppp in currPI.PagePoints)
                //    {
                //        ccc.Add(new Location(ppp.X, ppp.Y));

                //    }
                //    _pagePreviewVectors.SetNewData(ccc);
                //    ccc = new List<Location>();
                //}

            }



            //_pagePreviewVectors.SetNewData(tmpCVectors);


            //tmpCVectors = new List<cVector>();
            //tmpCPoints = new List<cPoint>();
            //controlPageNext.pageVector = new List<cVector>(controlPageNow.pageVector);

            //controlPageNext.pageVector = controlPageNow.pageVector.Select(i => i.Clone()).ToList();
            //  }
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

            // В окно предпросмотра выведем данные с текущей страницы
            if (strMessage.IndexOf("RefreshVector_", StringComparison.Ordinal) >= 0)
            {
                int iTmp;

                int.TryParse(strMessage.Substring(14, 2), out iTmp);

                if (iTmp == 0) return;

                _pagePreviewVectors.SetNewData(GlobalFunctions.pageVectorClone(_pageList[iTmp].pageVectorNOW));
            }

            //// В окно предпросмотра выведем данные с текущей страницы
            //if (strMessage.IndexOf("RefreshPoints_", StringComparison.Ordinal) >= 0)
            //{
            //    int iTmp;

            //    int.TryParse(strMessage.Substring(14, 2), out iTmp);

            //    if (iTmp == 0) return;

            //    _pagePreviewVectors.SetNewData(new List<Location>());

            //    //if (_pageList[iTmp].PagePoints != null)
            //    //{
            //    //    List<Location> ccc = new List<Location>();

            //    //    foreach (Location ppp in _pageList[iTmp].PagePoints)
            //    //    {
            //    //        ccc.Add(new Location(ppp.X, ppp.Y));
                        
            //    //    }
            //    //    _pagePreviewVectors.SetNewData(ccc);
            //    //    ccc = new List<Location>();
            //    //}
            //}

            // В окно предпросмотра выведем данные с текущей страницы
            if (strMessage.IndexOf("RefreshImage_", StringComparison.Ordinal) >= 0)
            {
                int iTmp;

                int.TryParse(strMessage.Substring(13, 2), out iTmp);

                if (iTmp == 0) return;

                if (_pageList[iTmp].pageImageNOW == null)
                {
                    _pagePreviewImage.SetImage(null);
                    return;
                }
                _pagePreviewImage.SetImage(_pageList[iTmp].pageImageNOW);
            }


            radioButton1.Enabled = _pagePreviewImage.containsData;
            radioButton2.Enabled = _pagePreviewVectors.containsData;





            //      


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
                    lb.Font = new Font("Microsoft Sans Serif", 15.75F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(204)));
                    lb.ForeColor = Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
                    lb.Location = new Point(117, 76);
                    lb.Name = "label2";
                    lb.Size = new Size(334, 25);
                    lb.TabIndex = 0;
                    lb.Text = "Нет данных для отображения";

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




            //else
            //{




            //    // выбраны векторы

            //    if (_pagePreviewVectors.containsData)
            //    {
            //        radioButton2.Checked = true;
            //        panelPreview.Controls.Clear();
            //        panelPreview.Controls.Add(_pagePreviewVectors);
            //        panelPreview.Controls[0].Dock = DockStyle.Fill;
            //    }
            //    else
            //    {


            //    }

            //}







        }
    } // public partial class MainForm : Form


    public static class Property
    {
        public static int Orientation = 1; 
        // 1- лев. нижний, 2- лев. верхний

        public static LanguagePosition Language = null;
    }
    

    // Производный класс от EventArgs
    class MyEventArgs : EventArgs
    {
        public string ActionRun;
    }


    /// <summary>
    /// Класс с различными функциями, которые могут вызываться с разных страниц
    /// </summary>
    public static class GlobalFunctions
    {
        /// <summary>
        /// Функция получения 2-х цветного изображения
        /// </summary>
        /// <param name="sourceImage"> Изображение которое необходимо преобразовать</param>
        /// <param name="koeff">Коэффициент определения что будет черным а что белым</param>
        /// <param name="reversingColor"></param>
        /// <returns></returns>
        public static Bitmap GetBlackWhileImage(Bitmap sourceImage, int koeff, bool reversingColor = false)
        {
            // Set cursor as hourglass
            Cursor.Current = Cursors.WaitCursor;


            Bitmap tempImage = sourceImage;

            BitmapData bmd = tempImage.LockBits(new Rectangle(0, 0, sourceImage.Width, sourceImage.Height), ImageLockMode.ReadWrite, sourceImage.PixelFormat);

            // Получаем адрес первой линии.
            IntPtr ptr = bmd.Scan0;
            // Вычисляем необходимый размер, для выделения памяти
            int numBytes = bmd.Stride * bmd.Height;

            // TODO: bmd.Stride - это общее выделенное количество байт для одной линии
            // TODO: bmd.Width - реальное количество байт занятых одной линией (сколько отводится под изображение)
            // TODO: при работе с байтами учитывать что bmd.Stride может быть больше bmd.Width

            // выделяем память
            byte[] dataImage = new byte[numBytes];

            // Копируем значения из памяти в массив.
            Marshal.Copy(ptr, dataImage, 0, numBytes);

            // Достигли конфа файла
            bool theEndData = false;

            // Координаты основного прохода
            int mainX = 0;
            int mainY = 0;

            int sizeOnePicsel = 1;

            switch (bmd.PixelFormat)
            {
                case PixelFormat.Format32bppArgb:
                        sizeOnePicsel = 4;
                        break;
                case PixelFormat.Format24bppRgb:
                    sizeOnePicsel = 3;
                    break;
                default:
                    MessageBox.Show(@"Формат изображения: " + bmd.PixelFormat.ToString() + @" пока не поддерживается!!!");
                    return null;
            }



            while (!theEndData)
            {
                int pos = (bmd.Stride * mainY) + (mainX*sizeOnePicsel);

                //в зависимости от размерности, тут может отличаться количество байт на пиксель
                byte currPixelR = dataImage[pos];
                byte currPixelG = dataImage[pos+1];
                byte currPixelB = dataImage[pos+2];

                int cR = currPixelR;
                int cG = currPixelG;
                int cB = currPixelB;

                int grayScale = cR + cG + cB;

                grayScale = grayScale / 3;




                if (grayScale > koeff) grayScale = 255;
                else grayScale = 0;


                if (reversingColor)
                {
                    if (grayScale == 0) grayScale = 255;
                    else grayScale = 0;
                }


                dataImage[pos] = (byte)grayScale;
                dataImage[pos + 1] = (byte)grayScale;
                dataImage[pos + 2] = (byte)grayScale;



                // дошли до конца линии
                if (mainX == (bmd.Width - 1))
                {
                    // проверим есть-ли ещё линия дальше
                    if (mainY == (bmd.Height - 1))
                    {
                        //это уже конец
                        theEndData = true;
                    }
                    else
                    {
                        //переходим на следующую строку
                        mainX = 0;
                        mainY++;
                    }
                }
                else
                {
                    mainX++;
                }
            }

            Marshal.Copy(dataImage, 0, ptr, numBytes);

            tempImage.UnlockBits(bmd);


            // Set cursor as default arrow
            Cursor.Current = Cursors.Default;

            return tempImage;
        }



        public static List<Segment> pageVectorClone(List<Segment> source)
        {
            List<Segment> tmp = new List<Segment>();

            if (source == null) return tmp;


            foreach (Segment vector in source)
            {
                tmp.Add(new Segment(vector));
            }


            return tmp;

        }


        public static int LaserTimeOut = 1000;
        //public static bool IsLaserPoint = false;

    }

} //namespace ToolsGenGkode





        //public static Bitmap GetGrayScaleImage(Bitmap sourceImage, bool ReversingColor = false)
        //{
        //    Bitmap tempImage = sourceImage;

        //    BitmapData bmd = tempImage.LockBits(new Rectangle(0, 0, sourceImage.Width, sourceImage.Height), ImageLockMode.ReadWrite, sourceImage.PixelFormat);

        //    // Получаем адрес первой линии.
        //    IntPtr ptr = bmd.Scan0;
        //    // Вычисляем необходимый размер, для выделения памяти
        //    int numBytes = bmd.Stride * bmd.Height;

        //    // TODO: bmd.Stride - это общее выделенное количество байт для одной линии
        //    // TODO: bmd.Width - реальное количество байт занятых одной линией (сколько отводится под изображение)
        //    // TODO: при работе с байтами учитывать что bmd.Stride может быть больше bmd.Width

        //    // выделяем память
        //    byte[] DataImage = new byte[numBytes];

        //    // Копируем значения из памяти в массив.
        //    Marshal.Copy(ptr, DataImage, 0, numBytes);

        //    // Достигли конфа файла
        //    bool TheEndData = false;

        //    // Координаты основного прохода
        //    int mainX = 0;
        //    int mainY = 0;

        //    int sizeOnePicsel = 1;

        //    switch (bmd.PixelFormat)
        //    {
        //        case PixelFormat.Format32bppArgb:
        //            sizeOnePicsel = 4;
        //            break;
        //        case PixelFormat.Format24bppRgb:
        //            sizeOnePicsel = 3;
        //            break;
        //        default:
        //            MessageBox.Show("Формат изображения: " + bmd.PixelFormat.ToString() + " пока не поддерживается!!!");
        //            return null;
        //            //break;
        //    }



        //    while (!TheEndData)
        //    {
        //        int pos = (bmd.Stride * mainY) + (mainX * sizeOnePicsel);

        //        //в зависимости от размерности, тут может отличаться количество байт на пиксель
        //        byte CurrPixelR = DataImage[pos];
        //        byte CurrPixelG = DataImage[pos + 1];
        //        byte CurrPixelB = DataImage[pos + 2];

        //        int cR = (int)CurrPixelR;
        //        int cG = (int)CurrPixelG;
        //        int cB = (int)CurrPixelB;

        //        int grayScale = (int)(cR + cG + cB);

        //        grayScale = grayScale / 3;

        //        if (ReversingColor)
        //        {
        //            grayScale = 255 - grayScale;

        //        }

        //        DataImage[pos] = (byte)grayScale;
        //        DataImage[pos + 1] = (byte)grayScale;
        //        DataImage[pos + 2] = (byte)grayScale;

        //        // дошли до конца линии
        //        if (mainX == (bmd.Width - 1))
        //        {
        //            // проверим есть-ли ещё линия дальше
        //            if (mainY == (bmd.Height - 1))
        //            {
        //                //это уже конец
        //                TheEndData = true;
        //            }
        //            else
        //            {
        //                //переходим на следующую строку
        //                mainX = 0;
        //                mainY++;
        //            }
        //        }
        //        else
        //        {
        //            mainX++;
        //        }
        //    }

        //    Marshal.Copy(DataImage, 0, ptr, numBytes);

        //    tempImage.UnlockBits(bmd);

        //    return tempImage;

        //}





//private Bitmap GetTraectory(List<cVector> lines)
//{
//    if (lines.Count == 0) return new Bitmap(100, 100, PixelFormat.Format24bppRgb); ;

//    decimal minX = 999999;
//    decimal maxX = -999999;
//    decimal minY = 999999;
//    decimal maxY = -999999;

//    foreach (cVector line in lines)
//    {
//        foreach (cPoint point in line.Points)
//        {
//            if (minX > point.X) minX = point.X;
//            if (maxX < point.X) maxX = point.X;
//            if (minY > point.Y) minY = point.Y;
//            if (maxY < point.Y) maxY = point.Y;
//        }
//    }

//    Bitmap bitmap = new Bitmap((int)maxX + 4, (int)maxY + 4, PixelFormat.Format24bppRgb);
//    // Создаем объект Graphics для вычисления высоты и ширины текста.
//    Graphics graphics = Graphics.FromImage(bitmap);
//    // Задаем цвет фона.
//    graphics.Clear(Color.White);
//    // Задаем параметры анти-алиасинга
//    graphics.SmoothingMode = SmoothingMode.None;
//    graphics.TextRenderingHint = TextRenderingHint.SingleBitPerPixel;

//    //********************************************************

//    Pen blackPen = new Pen(Color.BlueViolet, 1);
//    Pen redPen = new Pen(Color.Red, 2);
//    Pen greenPen = new Pen(Color.DarkGreen, 2);
//    Pen pointPen = new Pen(Color.DarkOrange, 4);

//    foreach (cVector line in lines)
//    {
//        if (line.Selected) graphics.DrawLines(redPen, line.GetArray());
//        else graphics.DrawLines(blackPen,line.GetArray());

//        line.Selected = false;

//        if (line.TempTraectory) graphics.DrawLines(greenPen, line.GetArray());

//        line.TempTraectory = false;

//        foreach (cPoint point in line.Points)
//        {

//            if (point.Selected) graphics.DrawEllipse(pointPen, (int)point.X  - 2, (int)point.Y  - 2, 4, 4);

//            point.Selected = false;
//        }
//    }

//    //*******************************************

//    graphics.Flush();

//    return bitmap;

//}