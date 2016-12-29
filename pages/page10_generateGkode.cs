// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using ELW.Library.Math;
using ELW.Library.Math.Exceptions;
using ELW.Library.Math.Expressions;
using ELW.Library.Math.Tools;

namespace ToolsGenGkode.pages
{
    public partial class page10_generateGkode : UserControl, PageInterface
    {
        /// <summary>
        /// Событие при изменении параметров на данной форме
        /// </summary>
        public event EventHandler IsChange;


        public string TextStart = "M3 " + Environment.NewLine + " Z10" + Environment.NewLine;

        public string TextEnds = "M5" + Environment.NewLine + "Z10" + Environment.NewLine;

        public string TextVectorStart = @"Z0" + Environment.NewLine;

        public string TextVectorEnds = @"Z10" + Environment.NewLine;


        void CreateEvent(string Action)
        {
            MyEventArgs e = new MyEventArgs();
            e.ActionRun = Action;
            //вызовем событие
            EventHandler handler = IsChange;
            if (handler != null)
                IsChange?.Invoke(this, e);
        }


        public page10_generateGkode()
        {
            InitializeComponent();

            PageName = @"Генерация G-кода (10)";
            LastPage = 0;
            CurrPage = 10;
            NextPage = 0;

            pageImageNOW = null;
            pageVectorNOW = new List<Segment>();




            toolTips myToolTip1 = new toolTips();

            myToolTip1.Size = new Size(300, 200);
            myToolTip1.BackColor = Color.FromArgb(255, 255, 192);
            myToolTip1.ForeColor = Color.Navy;
            myToolTip1.BorderColor = Color.FromArgb(128, 128, 255);

            myToolTip1.SetToolTip(buttonAddNewProfile, "Создать новый профиль");
            myToolTip1.SetToolTip(btEditProfile, "Редактировать существующий профиль");

        }

        private void page09_generateGkode_Load(object sender, EventArgs e)
        {
            //cbProfile.Text = cbProfile.Items[0].ToString();
        }

        public Bitmap pageImageIN { get; set; }
        public Bitmap pageImageNOW { get; set; }
        public List<Segment> pageVectorIN { get; set; }
        public List<Segment> pageVectorNOW { get; set; }
        public List<Location> PagePoints { get; set; }

        public void actionBefore()
        {
            pageVectorNOW = GlobalFunctions.pageVectorClone(pageVectorIN);
            pageImageNOW = null;
        }

        public void actionAfter()
        {

        }

        public void ChangeValueVariable(ref List<VariableValue>  _variableDataValues, string _name, double _value)
        {
            bool finded = false;

            foreach (VariableValue VARIABLE in _variableDataValues)
            {
                if (VARIABLE.VariableName == _name)
                {
                    finded = true;
                    VARIABLE.value = _value;
                }
            }

            if (!finded) _variableDataValues.Add(new VariableValue(_value, _name));
        }


        private void btGenerateCode_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            // сюда будем писать текст
            StringBuilder sb = new StringBuilder();

            decimal maxX = -999999;
            decimal minX =  999999;
            decimal maxY = -999999;
            decimal minY =  999999;

            foreach (Segment _segment in pageVectorNOW)
            {
                foreach (Location point in _segment.Points)
                {
                    if (maxX < point.X) maxX = point.X;

                    if (maxY < point.Y) maxY = point.Y;

                    if (minX > point.X) minX = point.X;

                    if (minY > point.Y) minY = point.Y;
                }
            }

            // переменнные для алгоритма генерации
            List<VariableValue> variableDataValues = new List<VariableValue>();
            variableDataValues.Add(new VariableValue((double)maxX, "maxX"));
            variableDataValues.Add(new VariableValue((double)maxY, "maxY"));
            variableDataValues.Add(new VariableValue((double)minX, "minX"));
            variableDataValues.Add(new VariableValue((double)minY, "minY"));

            CurrProfile.GetHead(ref sb, ref variableDataValues);

            foreach (Segment _segment in pageVectorNOW)
            {
                CurrProfile.GetBeforeSegment(ref sb, _segment, ref variableDataValues);

                bool FirstPoint = true; //для возможности проскочить первую точку, если это будет необходимо

                foreach (Location point in _segment.Points)
                {
                    //TODO:


                    ChangeValueVariable(ref variableDataValues, "P", point.Pvalue);
                    ChangeValueVariable(ref variableDataValues, "S", point.Svalue);
                    ChangeValueVariable(ref variableDataValues, "F", point.Fvalue);
                    ChangeValueVariable(ref variableDataValues, "bright", point.Bright);

                    CurrProfile.GetPoint(ref sb, point, FirstPoint, ref variableDataValues);
                    FirstPoint = false;
                }
                CurrProfile.GetAfterSegment(ref sb, _segment, ref variableDataValues);
            }
            CurrProfile.GetTail(ref sb, ref variableDataValues);

            string sss = sb.ToString().Replace("\"", "");
            //DivSymbol
            //узнаем текущий разделитель
            string curSymb = ((decimal) 1/2).ToString().Replace("0", "").Replace("5", "");

            textBoxGkod.Text = sss.Replace(curSymb,CurrProfile.DivSymbol);

            labelCountRow.Text = @"Размер: " + sb.Length + @" байт";

            Cursor.Current = Cursors.Default;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Determine if any text is selected in the TextBox control.
            if (textBoxGkod.SelectionLength == 0)
                // Select all text in the text box.
                textBoxGkod.SelectAll();

            // Copy the contents of the control to the Clipboard.
            textBoxGkod.Copy();
        }

        private void cbProfile_TextChanged(object sender, EventArgs e)
        {

        }

        private List<profile> profiles = new List<profile>(); 

        private void cbProfile_DropDown(object sender, EventArgs e)
        {
            cbProfile.Items.Clear();
            profiles.Clear();

            //просканируем каталог на наличие файлов.

            string Catalog = string.Format("{0}\\profiles", Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));

            if (!Directory.Exists(Catalog)) return;

            string[] MyFiles = Directory.GetFiles(Catalog, @"*.*", SearchOption.AllDirectories);

            //parse finded files
            foreach (string filename in MyFiles)
            {
                profile tmp = new profile(filename);

                profiles.Add(tmp);
            }

            //add elements to droplist
            foreach (profile VARIABLE in profiles)
            {
                cbProfile.Items.Add(VARIABLE.Name);
            }

        }

        private profile CurrProfile = null;

        private void cbProfile_SelectedIndexChanged(object sender, EventArgs e)
        {
            int curr = cbProfile.SelectedIndex;
            
            if (curr == -1) return;

            CurrProfile = profiles[curr];

            //if (CurrProfile != null) myToolTip1.SetToolTip(btEditProfile, CurrProfile.Text);
        }

        private void buttonSaveToFile_Click(object sender, EventArgs e)
        {
            SaveFileDialog save = new SaveFileDialog();
            save.FileName = "g-kode.nc";
            save.Filter = "Файл G-кода | *.nc";
            if (save.ShowDialog() == DialogResult.OK)
            {
                StreamWriter writer = new StreamWriter(save.OpenFile());

                writer.Write(textBoxGkod.Text);

                writer.Dispose();
                writer.Close();
            }
        }

        private void btOpenProfile_Click(object sender, EventArgs e)
        {
            int curr = cbProfile.SelectedIndex;

            if (curr == -1) return;

            CurrProfile = profiles[curr];

            ProfileEditor pfEditor = new ProfileEditor();
            pfEditor.textBoxFileProfile.Text = CurrProfile.FileName;
            pfEditor.Show();
            pfEditor.LoadDataFromFile();
        }
    }


    public static class MathEx
    {
        public static List<String> Errors = new List<string>();



        /// <summary>
        /// Матиматическое вычисление выражения
        /// </summary>
        /// <param name="variables">Список параметров</param>
        /// <param name="Expression">Текст выражения</param>
        /// <returns>Результат вычисления</returns>
        public static double Calc(List<VariableValue> variables, string Expression)
        {
            double result = 0;


            try
            {
                // Compiling an expression
                PreparedExpression preparedExpression = ToolsHelper.Parser.Parse(Expression);
                CompiledExpression compiledExpression = ToolsHelper.Compiler.Compile(preparedExpression);
                // Optimizing an expression
                CompiledExpression optimizedExpression = ToolsHelper.Optimizer.Optimize(compiledExpression);
                // Creating list of variables specified
                //    List<VariableValue> variables = new List<VariableValue>();

                //    double firstValue;
                //    if (!Double.TryParse(textBoxValue1.Text, out firstValue))
                //    {
                //        MessageBox.Show("Error converting first variable value.");
                //        return;
                //    }
                //    variables.Add(new VariableValue(firstValue, textBoxVariable1.Text));

                //    double secondValue;
                //    if (!Double.TryParse(textBoxValue2.Text, out secondValue))
                //    {
                //        MessageBox.Show("Error converting second variable value.");
                //        return;
                //    }
                //    variables.Add(new VariableValue(secondValue, textBoxVariable2.Text));

                // Do it !
                result = ToolsHelper.Calculator.Calculate(compiledExpression, variables);
                // Show the result.
                //    MessageBox.Show(String.Format("Result: {0}\nOptimized: {1}", res, ToolsHelper.Decompiler.Decompile(optimizedExpression)));
            }
            catch (CompilerSyntaxException ex)
            {
                Errors.Add("Ошибка компиляции выражения:" + ex.Message);
            }
            catch (MathProcessorException ex)
            {
                Errors.Add("Ошибка:" + ex.Message);
            }
            catch (ArgumentException)
            {
                Errors.Add("Ошибка переданных параметров");
            }
            catch (Exception)
            {
                Errors.Add("Неизвестная ошибка");
                throw;
            }


            return result;
        }


    }


    //////sb.AppendLine("%");
    //////if (TextStart.Trim() != "") sb.AppendLine(TextStart);

    //////#region for laser

    //////if (GlobalFunctions.IsLaserPoint)
    //////{
    //////    decimal LastX = -999999;
    //////    decimal LastY = -999999;

    //////    foreach (Location ppPoint in PagePoints)
    //////    {
    //////        decimal x = (ppPoint.X);
    //////        decimal y = (ppPoint.Y);

    //////        string ssrt = "M5 ";

    //////        if (LastX != x)
    //////        {
    //////            //нужно вывести значение по оси Х
    //////            LastX = x;

    //////            ssrt += " X" + x.ToString("#0.###");
    //////        }

    //////        if (LastY != y)
    //////        {
    //////            //нужно вывести значение по оси Х
    //////            LastY = y;
    //////            ssrt += " Y" + y.ToString("#0.###");
    //////        }

    //////        sb.AppendLine(ssrt.Trim());
    //////        sb.AppendLine("M3 G4 P" + (((decimal)GlobalFunctions.LaserTimeOut / 1000).ToString("###0.####")));

    //////    }
    //////    sb.AppendLine("M5");          
    //////}

    //////#endregion

    //////#region for frezer

    //////if (!GlobalFunctions.IsLaserPoint)
    //////{
    //////    foreach (Segment vector in pageVectorNOW)
    //////    {
    //////        decimal x = (vector.Points[0].X);
    //////        decimal y = (vector.Points[0].Y);

    //////        // подход к началу вектора, на безопастной высоте
    //////        sb.AppendLine("g0 x" + x.ToString("#0.###") + " y" + y.ToString("#0.###"));

    //////        if (TextVectorStart.Trim() != "") sb.AppendLine(TextVectorStart);
    //////        foreach (Location point in vector.Points)
    //////        {
    //////            x = (point.X);
    //////            y = (point.Y);
    //////            sb.AppendLine("g1 x" + x.ToString("#0.###") + " y" + y.ToString("#0.###"));
    //////        }
    //////        if (TextVectorEnds.Trim() != "") sb.AppendLine(TextVectorEnds);
    //////        x = (vector.Points[vector.Points.Count - 1].X);
    //////        y = (vector.Points[vector.Points.Count - 1].Y);
    //////    }
    //////}
    //////#endregion
    //////if (TextEnds.Trim() != "") sb.AppendLine(TextEnds);
    //////sb.AppendLine("%");



    public class profile
    {
        public string FileName;
        public string Name;
        public string Text;
        public string DivSymbol = ".";

        private List<string> _Head;
        private List<string> _tail;
        private List<string> _beforeSegment;
        private List<string> _beforeSegmentL;
        private List<string> _beforeSegmentR;
        private List<string> _afterSegment;
        private List<string> _afterSegmentL;
        private List<string> _afterSegmentR;
        private List<string> _spoint;

        /// <summary>
        /// Constryctor
        /// </summary>
        /// <param name="fileName">Имя файла</param>
        public profile(string _fileName)
        {
            _Head           = new List<string>();
            _tail           = new List<string>();
            _beforeSegment  = new List<string>();
            _beforeSegmentL = new List<string>();
            _beforeSegmentR = new List<string>();
            _afterSegment   = new List<string>();
            _afterSegmentL  = new List<string>();
            _afterSegmentR  = new List<string>();
            _spoint = new List<string>();

            FileName = _fileName;

            StreamReader fs = new StreamReader(FileName, Encoding.GetEncoding(1251));

            // Читаем строку из файла во временную переменную.
            string temp = fs.ReadLine();

            // Если достигнут конец файла, прерываем считывание.
            if (temp == null)
            {
                Name = @"Файл: " + FileName + " неверной структуры!";
                Text = "";
                return;
            }
            
            Name = temp.Replace("#", "");

            temp = fs.ReadLine();

            DivSymbol = temp.Replace("$", "").Trim();


            string strTT = "UP"; // or DOWN
            string strCurPosition = "HEAD";

            string strRead = fs.ReadLine();

            while (strRead != null)
            {
                strRead = strRead.Trim().Replace("\t", "");

                if (strRead == "")
                {
                    strRead = fs.ReadLine();
                    continue;
                }

                //комментарии пропустим 
                if (strRead.IndexOf(@"//", StringComparison.Ordinal) == 0)
                {
                    strRead = fs.ReadLine();
                    continue; 
                }

                //если вложенные данные
                if (strRead == "LINES" || strRead == "POINTS" || strRead == "TOLEFT" || strRead == "TORIGHT" || strRead == "FROMLEFT" || strRead == "FROMRIGHT")
                {

                    strCurPosition = strRead;
                    Text += strRead + Environment.NewLine;
                    strRead = fs.ReadLine(); 

                    continue;
                }

                if (strRead == "{")
                {
                    strTT = "UP";

                    Text += strRead + Environment.NewLine;
                    strRead = fs.ReadLine(); 
                    continue;
                }

                if (strRead == "}")
                {
                    strTT = "DOWN";

                    //а вот тут немного сложнее
                    if (strCurPosition == "LINES") strCurPosition = "HEAD";

                    if (strCurPosition == "POINTS") strCurPosition = "LINES";

                    if (strCurPosition == "TOLEFT")
                    {
                        strTT = "UP";
                        strCurPosition = "LINES";
                    }

                    if (strCurPosition == "TORIGHT")
                    {
                        strTT = "UP";
                        strCurPosition = "LINES";
                    }

                    if (strCurPosition == "FROMLEFT")
                    {
                        strCurPosition = "LINES";
                    }

                    if (strCurPosition == "FROMRIGHT")
                    {
                        strCurPosition = "LINES";
                    }


                    //strCurPosition = "";
                    Text += strRead + Environment.NewLine;
                    strRead = fs.ReadLine();
                    continue;
                }

                Text += strRead + Environment.NewLine;


                if (strCurPosition == "HEAD" && strTT == "UP") _Head.Add(strRead);

                if (strCurPosition == "LINES" && strTT == "UP") _beforeSegment.Add(strRead);

                if (strCurPosition == "TOLEFT" && strTT == "UP") _beforeSegmentL.Add(strRead);

                if (strCurPosition == "TORIGHT" && strTT == "UP") _beforeSegmentR.Add(strRead);

                if (strCurPosition == "POINTS") _spoint.Add(strRead);

                if (strCurPosition == "LINES" && strTT == "DOWN") _afterSegment.Add(strRead);

                if (strCurPosition == "FROMLEFT" && strTT == "UP") _afterSegmentL.Add(strRead);

                if (strCurPosition == "FROMRIGHT" && strTT == "UP") _afterSegmentR.Add(strRead);

                if (strCurPosition == "HEAD" && strTT == "DOWN") _tail.Add(strRead);

                strRead = fs.ReadLine();

            }
        }

        private static List<string> ParseStringToSybString(string value)
        {
            List<string> returnValue = new List<string>();

            string tmpString = value.Trim();


            if (tmpString == "") return returnValue;

            string buffStr = "";

            bool OpenCollect = false;
            string symOpen = "";


            foreach (char symb in tmpString)
            {
                buffStr += symb;

                if (symb == '[' && OpenCollect == false)
                {
                    OpenCollect = true;
                    symOpen = "[";
                    continue;
                }


                if (symb == '\"' && OpenCollect == false)
                {
                    OpenCollect = true;
                    symOpen = "\"";
                    continue;
                }


                if (symb == ']' && OpenCollect == true)
                {
                    OpenCollect = false;
                    symOpen = "";

                    if (buffStr.Trim() != "") returnValue.Add(buffStr);
                    buffStr = "";
                    continue;
                }


                if (symb == '\"' && OpenCollect == true)
                {
                    OpenCollect = false;
                    symOpen = "";
                    if (buffStr.Trim() != "") returnValue.Add(buffStr);
                    buffStr = "";
                    continue;
                }
            }

            return returnValue;
        }

        /// <summary>
        /// Разбивка строки на отдельные команды
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static List<string> ParseStringToListString(string value)
        {
            List<string> returnValue = new List<string>();

            string tmpString = value.Trim();

            if (tmpString.Length == 1) return returnValue;

            //все что после скобки отбросим, дальше не будем анализировать
            int i = tmpString.IndexOf(@"(", StringComparison.Ordinal);
            if (i != -1) return returnValue;

            //все что после точки с запятой отбросим, дальше не будем анализировать
            i = tmpString.IndexOf(@";", StringComparison.Ordinal);
            if (i != -1) return returnValue;

            //все что после точки с запятой отбросим, дальше не будем анализировать
            i = tmpString.IndexOf(@"%", StringComparison.Ordinal);
            if (i != -1) return returnValue;

            //все что после двух косых отбросим, дальше не будем анализировать
            i = tmpString.IndexOf(@"//", StringComparison.Ordinal);
            if (i != -1) return returnValue;

            // ещё раз обрежем
            tmpString = tmpString.Trim();

            if (tmpString.Length < 2) return returnValue;

            // распарсим строку на отдельные строки с параметрами
            int inx = 0;

            bool collectCommand = false;

            foreach (char symb in tmpString)
            {
                if (symb > 0x40 && symb < 0x5B)  //символы от A до Z
                {
                    if (collectCommand)
                    {
                        inx++;
                    }

                    collectCommand = true;
                    returnValue.Add("");
                }

                if (collectCommand && symb != ' ') returnValue[inx] += symb.ToString();
            }

            return returnValue;
        }

        private void FindAndFillData(string _ss, ref List<VariableValue> _variableDataValues)
        {

            // Все неликвидные команды будем отбрасывать
            List<string> LCommand = ParseStringToListString(_ss);

            if (LCommand.Count == 0) return;

            //получим символ разделения дробной и целой части.
            string symbSeparatorDec = CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator;

            char csourse = '.';
            char cdestination = ',';

            if (symbSeparatorDec == ".")
            {
                csourse = ',';
                cdestination = '.';
            }

            // Необходимость прервать цикл парсинга
            bool breakLoop = false;

            // по циклу начинаем перебирать команды
            foreach (string sLine in LCommand)
            {
                string parseLine = sLine.Trim().ToUpper();

                if (parseLine.Length < 2) continue;

                string sCommand = parseLine.Substring(0, 1); //буква команды
                string sValue = parseLine.Substring(1).Replace(csourse, cdestination).Replace("\"", ""); //номер, или значение команды

                #region F
                //if (sCommand == "F")
                //{
                //    float F_value = 0;

                //    float.TryParse(sValue, out F_value);


                //    foreach (VariableValue VARIABLE in _variableDataValues)
                //    {
                //        if (VARIABLE.VariableName == "F") _variableDataValues.Remove(VARIABLE);
                //        break;
                //    }

                //    _variableDataValues.Add(new VariableValue((double)F_value,"F"));

                //}
                #endregion

                #region S
                //if (sCommand == "S")
                //{
                //    float S_value = 0;

                //    float.TryParse(sValue, out S_value);

                //    foreach (VariableValue VARIABLE in _variableDataValues)
                //    {
                //        if (VARIABLE.VariableName == "S") _variableDataValues.Remove(VARIABLE);
                //        break;
                //    }

                //    _variableDataValues.Add(new VariableValue((double)S_value, "S"));

                //}
                #endregion

                #region P
                //if (sCommand == "P")
                //{
                //    float P_value = 0;

                //    float.TryParse(sValue, out P_value);

                //    foreach (VariableValue VARIABLE in _variableDataValues)
                //    {
                //        if (VARIABLE.VariableName == "P") _variableDataValues.Remove(VARIABLE);
                //        break;
                //    }

                //    _variableDataValues.Add(new VariableValue((double)P_value, "P"));

                //}
                #endregion

                #region X
                if (sCommand == "X")
                {
                    decimal X_value = 0;

                    decimal.TryParse(sValue, out X_value);

                    //foreach (VariableValue VARIABLE in _variableDataValues)
                    //{
                    //    if (VARIABLE.VariableName == "X") _variableDataValues.Remove(VARIABLE);
                    //    break;
                    //}

                    //_variableDataValues.Add(new VariableValue((double)X_value, "X"));
                }
                #endregion

                #region Y
                if (sCommand == "Y")
                {
                    decimal Y_value = 0;

                    decimal.TryParse(sValue, out Y_value);

                    //foreach (VariableValue VARIABLE in _variableDataValues)
                    //{
                    //    if (VARIABLE.VariableName == "Y") _variableDataValues.Remove(VARIABLE);
                    //    break;
                    //}

                    //_variableDataValues.Add(new VariableValue((double)Y_value, "Y"));

                }
                #endregion

                #region Z
                if (sCommand == "Z")
                {
                    decimal Z_value = 0;

                    decimal.TryParse(sValue, out Z_value);

                    //foreach (VariableValue VARIABLE in _variableDataValues)
                    //{
                    //    if (VARIABLE.VariableName == "Z") _variableDataValues.Remove(VARIABLE);
                    //    break;
                    //}

                    //_variableDataValues.Add(new VariableValue((double)Z_value, "Z"));

                }
                #endregion

            }
        }

        // сюда передается с трока с тектом и параметрами
        private void FillStringLine(string INString, ref StringBuilder _sb, ref List<VariableValue> _variableDataValues, Segment _segment = null, Location _point = null)
        {

            //распарсим строку на части
            List<string> tst = ParseStringToSybString(INString);

            string buffStr = "";

            foreach (string ss in tst)
            {
                if (ss.Trim().IndexOf("\"") != -1)
                {
                    //string data
                    buffStr += " " + ss;

                    //парсим строку
                    FindAndFillData(ss, ref _variableDataValues);
                }


                if (ss.Trim().IndexOf("[") != -1)
                {
                    string newString = "";

                    //избавимся от скобок
                    string ttmString = ss.Trim().Replace("[", "").Replace("]", "");

                    //узнаем есть ли параметр означающий не выводить данные
                    //тоторые не изменились с помледнего раза
                    bool NotChanged = false;
                    if (ttmString.IndexOf("!") != -1)
                    {
                        NotChanged = true;
                        //удалим лишний символ
                        ttmString = ttmString.Replace("!", "");
                    }

                    //получим строку форматирования значения типа такой: (#0.###)

                    string strFormatValue = "";

                    if (ttmString.IndexOf("'") != -1)
                    {
                        int firstSymb = ttmString.IndexOf("'");
                        int secondSymb = ttmString.IndexOf("'",firstSymb+1);

                        strFormatValue = ttmString.Substring(firstSymb + 1, secondSymb - firstSymb - 1);
                        ttmString      = ttmString.Substring(0, firstSymb);
                    }


                    //обновим данные в переменных


                    if (_segment != null) //Если это сегмент из точек
                    {
                            if (_segment.Points.Count > 0)
                            {


                                foreach (VariableValue VARIABLE in _variableDataValues)
                                {
                                    if (VARIABLE.VariableName == "X")
                                    {
                                        _variableDataValues.Remove(VARIABLE);
                                        break;

                                    }
                                }

                                _variableDataValues.Add(new VariableValue((double)_segment.Points[0].X, "X"));


                                foreach (VariableValue VARIABLE in _variableDataValues)
                                {
                                    if (VARIABLE.VariableName == "Y")
                                    {
                                        _variableDataValues.Remove(VARIABLE);
                                        break;

                                    }
                                }

                                _variableDataValues.Add(new VariableValue((double)_segment.Points[0].Y, "Y"));
                        }
                    }

                    if (_point != null) // если это конкретная точка
                    {


                        foreach (VariableValue VARIABLE in _variableDataValues)
                        {
                            if (VARIABLE.VariableName == "X")
                            {
                                _variableDataValues.Remove(VARIABLE);
                               break;

                            }
                        }

                        _variableDataValues.Add(new VariableValue((double)_point.X, "X"));


                        foreach (VariableValue VARIABLE in _variableDataValues)
                        {
                            if (VARIABLE.VariableName == "Y")
                            {
                                _variableDataValues.Remove(VARIABLE);
                               break;

                            }
                        }

                        _variableDataValues.Add(new VariableValue((double)_point.Y, "Y"));
                    }






                    double resultic = MathEx.Calc(_variableDataValues, ttmString);

                    newString = resultic.ToString(strFormatValue);


                    //узнаем есть ли символ разделения дробной части

                    //string symbDiv = null;
                    //if (ttmString.IndexOf(".") != -1)
                    //{
                    //    symbDiv = ".";
                    //    //удалим лишний символ
                    //    ttmString = ttmString.Replace(".", "");
                    //}

                    //if (ttmString.IndexOf(",") != -1)
                    //{
                    //    symbDiv = ",";
                    //    //удалим лишний символ
                    //    ttmString = ttmString.Replace(",", "");
                    //}





                    //а вот уже имеем букву X Y Z S F P




                    //

                    //// скопируем переменные что сверху прилетели
                    //if (variables != null)
                    //{
                    //    foreach (VariableValue VARIABLE in variables)
                    //    {
                    //        tmpvar.Add(VARIABLE);
                    //    }
                    //}

                    ////добавим переменную свою
                    //




                    //if (ttmString == "X" || ttmString == "Y")
                    //{

 


                    //    if (symbDiv != null)
                    //    {
                    //        //установим нужный нам символ разделителя дробной, и целой части

                    //        //получим символ разделения дробной и целой части.
                    //        string symbSeparatorDec = CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator;

                    //        newString = newString.Replace(symbSeparatorDec, symbDiv);
                    //    }
                    //}

                    //if (ttmString == "Z")
                    //{
                    //    if (_segment != null)
                    //    {
                    //        if (_segment.Points.Count > 0)
                    //        {
                    //            newString = "Z" + _LVC.Zpos.ToString("#0.###");
                    //        }
                    //    }

                    //    if (_point != null)
                    //    {
                    //        newString = "Z" + _LVC.Ypos.ToString("#0.###");
                    //    }
                    //}







                    buffStr += newString;

                }
                
            }

                    _sb.AppendLine(buffStr);



            //    forReturn += sHead.Replace('"', ' ').Trim() + Environment.NewLine;


            //decimal x = (vector.Points[0].X);
            //decimal y = (vector.Points[0].Y);

            //// подход к началу вектора, на безопастной высоте
            //sb.AppendLine("g0 x" + x.ToString("#0.###") + " y" + y.ToString("#0.###"));

            //if (TextVectorStart.Trim() != "") sb.AppendLine(TextVectorStart);

            //if (TextVectorEnds.Trim() != "") sb.AppendLine(TextVectorEnds);

            //x = (vector.Points[vector.Points.Count - 1].X);
            //y = (vector.Points[vector.Points.Count - 1].Y);


            //x = (point.X);
            //y = (point.Y);

            //sb.AppendLine("g1 x" + x.ToString("#0.###") + " y" + y.ToString("#0.###"));


        }



        /// <summary>
        /// Получить заголовок G-текста
        /// </summary>
        public void GetHead(ref StringBuilder _sb, ref List<VariableValue> _variableDataValues)
        {
            foreach (string str in _Head)
            {
                string ss = str.Replace('\t', ' ').Trim();

                if (ss == "") continue;

                FillStringLine(ss, ref _sb, ref _variableDataValues);
            }
        }

        /// <summary>
        /// Получить подвал G-текста
        /// </summary>
        public void GetTail(ref StringBuilder _sb, ref List<VariableValue> _variableDataValues)
        {
            foreach (string str in _tail)
            {
                string ss = str.Replace('\t', ' ').Trim();

                if (ss == "") continue;

                FillStringLine(ss, ref _sb,ref _variableDataValues);
            }
        }

        public void GetBeforeSegment(ref StringBuilder _sb,Segment _segment, ref List<VariableValue> _variableDataValues)
        {
            foreach (string str in _beforeSegment)
            {
                string ss = str.Replace('\t', ' ').Trim();

                if (ss == "") continue;

                FillStringLine(ss, ref _sb, ref _variableDataValues, _segment);
            }

            foreach (string str in _beforeSegmentL)
            {
                string ss = str.Replace('\t', ' ').Trim();

                if (ss == "") continue;

                FillStringLine(ss, ref _sb, ref _variableDataValues, _segment);
            }
            foreach (string str in _beforeSegmentR)
            {
                string ss = str.Replace('\t', ' ').Trim();

                if (ss == "") continue;

                FillStringLine(ss, ref _sb, ref _variableDataValues, _segment);
            }


        }

        public void GetAfterSegment(ref StringBuilder _sb, Segment _segment, ref List<VariableValue> _variableDataValues)
        {
            foreach (string str in _afterSegment)
            {
                string ss = str.Replace('\t', ' ').Trim();

                if (ss == "") continue;

                FillStringLine(ss, ref _sb, ref _variableDataValues, _segment);
            }

            foreach (string str in _afterSegmentL)
            {
                string ss = str.Replace('\t', ' ').Trim();

                if (ss == "") continue;

                FillStringLine(ss, ref _sb, ref _variableDataValues, _segment);
            }

            foreach (string str in _afterSegmentR)
            {
                string ss = str.Replace('\t', ' ').Trim();

                if (ss == "") continue;

                FillStringLine(ss, ref _sb, ref _variableDataValues, _segment);
            }
        }



        public void GetPoint(ref StringBuilder _sb, Location _point, bool _firstPoint, ref List<VariableValue> _variableDataValues)
        {
            foreach (string str in _spoint)
            {
                string ss = str.Replace('\t', ' ').Trim();

                if (ss == "") continue;

                //TODO: тут следим за [SKIP_FIRST_POINT]
                if (ss.IndexOf("[SKIP_FIRST_POINT]") != -1)
                {
                    if (_firstPoint) return;
                    continue;
                }

                FillStringLine(ss, ref _sb, ref _variableDataValues,null, _point);
            }
        }
    }


    //public class LastValueCommand
    //{
    //    public decimal X = 0;
    //    public decimal Y = 0;
    //    public decimal Z = 0;

    //    public int F = 0;
    //    public int S = 0;
    //    public int P = 0;

    //    public LastValueCommand()
    //    {
    //        decimal X = 0;
    //        decimal Y = 0;
    //        decimal Z = 0;

    //        int F = 0;
    //        int S = 0;
    //        int P = 0;
    //    }

    //    // Для доступа к полям по текстовому имени
    //    public object this[string key]
    //    {
    //        get
    //        {
    //            if (key == "X") return X;

    //            if (key == "Y") return Y;

    //            return null;
    //        }
    //        set
    //        {
    //            if (key == "X") X = (decimal)value;

    //            if (key == "Y") Y = (decimal)value;

    //        }
    //    }
    //}
}
