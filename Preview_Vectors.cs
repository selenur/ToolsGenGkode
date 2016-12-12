// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SharpGL;
using SharpGL.Enumerations;
using ToolsGenGkode.pages;

namespace ToolsGenGkode
{
    public partial class Preview_Vectors : UserControl
    {
        public bool containsData;

        List<Segment> DrawVectors = new List<Segment>();

        public void SetNewData(List<Segment> _DrawVectors)
        {
            DrawVectors = _DrawVectors;


            containsData = (DrawVectors.Count > 0);


            openGLControl1.Refresh();

        }
        public void SetNewData(List<Location> _DrawPoints)
        {
            containsData = (DrawVectors.Count > 0);

            openGLControl1.Refresh();
        }



        public Preview_Vectors()
        {
            InitializeComponent();
        }

        private void openGLControl1_OpenGLDraw(object sender, SharpGL.RenderEventArgs args)
        {
            if (Property.Orientation == 1)
            {
                Setting.PosAngleX = 0;
                Setting.PosAngleY = 0;
                Setting.PosAngleZ = 0;
            }

            if (Property.Orientation == 2)
            {
                Setting.PosAngleX = 180;
                Setting.PosAngleY = 0;
                Setting.PosAngleZ = 0;
            }

            if (Property.Orientation == 3)
            {
                Setting.PosAngleX = -180;
                Setting.PosAngleY = -180;
                Setting.PosAngleZ = 0;
            }

            if (Property.Orientation == 4)
            {
                Setting.PosAngleX = 0;
                Setting.PosAngleY = -180;
                Setting.PosAngleZ = 0;
            }

            OpenGL gl = openGLControl1.OpenGL;

            gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT); // очистка буфера цвета и буфера глубины 
            gl.ClearColor(0.5f, 0.5f, 0.5f, 1);
            //gl.ClearColor(0.7f, 0.7f, 0.7f, 1);

            gl.LoadIdentity();                                         // очищение текущей матрицы

            gl.PushMatrix();                                           // помещаем состояние матрицы в стек матриц 

            // перемещаем камеру для более хорошего обзора объекта 
            gl.Translate(Setting.PosX / 1000.0, Setting.PosY / 1000.0, Setting.PosZ / 1000.0);

            ////угловое вращение
            gl.Rotate(Setting.PosAngleX, 1, 0, 0);//PreviewSetting.PosAngleX
            gl.Rotate(Setting.PosAngleY, 0, 1, 0);//PreviewSetting.PosAngleY
            gl.Rotate(Setting.PosAngleZ, 0, 0, 1);//PreviewSetting.PosAngleZ


            float aspect = openGLControl1.Width / (float)openGLControl1.Height;

            double scaleX = Setting.zoomSize / (1000.0 * aspect);
            double scaleY = Setting.zoomSize / (1000.0 * aspect);
            double scaleZ = Setting.zoomSize / (1000.0 * aspect);
            //TODO: тут с пропорциями разобраться, сейчас только XY плоскость нормально отображается
            gl.Scale(scaleX, scaleY, scaleZ);

            //// помещаем состояние матрицы в стек матриц 
            gl.PushMatrix();

            gl.Enable(OpenGL.GL_BLEND);


            gl.Hint(OpenGL.GL_PERSPECTIVE_CORRECTION_HINT, OpenGL.GL_NICEST);			// Really Nice Perspective Calculations
            gl.Hint(OpenGL.GL_POINT_SMOOTH_HINT, OpenGL.GL_NICEST);


            ////включение сглаживания линий
            //gl.Enable(OpenGL.GL_LINE_SMOOTH);
            //gl.Hint(OpenGL.GL_LINE_SMOOTH_HINT, OpenGL.GL_NICEST);


            #region Оси

            gl.LineWidth(2);

            //ось х
            gl.Color(0.0f, 1f, 0.0f);
            gl.Begin(OpenGL.GL_LINES); //РИСОВАНИЕ ОБЫЧНОЙ ЛИНИИ
            gl.Vertex(0, 0, 0);
            gl.Vertex(10, 0, 0);
            gl.Vertex(10, 0, 0);
            gl.Vertex(9, 1, 0);
            gl.Vertex(10, 0, 0);
            gl.Vertex(9, -1, 0);
            gl.End();

            //ось y
            gl.Color(1.0F, 0, 0.0F);
            gl.Begin(OpenGL.GL_LINES);
            gl.Vertex(0, 0, 0);
            gl.Vertex(0, 10, 0);
            gl.Vertex(0, 10, 0);
            gl.Vertex(1, 9, 0);
            gl.Vertex(0, 10, 0);
            gl.Vertex(-1, 9, 0);
            gl.End();

            ////ось z
            //gl.Color(0.0F, 0, 1.0F);
            //gl.Begin(OpenGL.GL_LINES);
            //gl.Vertex(0, 0, 0);
            //gl.Vertex(0, 0, 10);
            //gl.Vertex(0, 0, 10);
            //gl.Vertex(1, 1, 9);
            //gl.Vertex(0, 0, 10);
            //gl.Vertex(-1, -1, 9);
            //gl.End();

            #endregion

            gl.DrawText(20, 20, 0.0f, 1f, 0.0f, "Arial", 10, "ZOOM: " + Setting.zoomSize.ToString("F1"));
            gl.DrawText(20, 60, 0.0f, 1f, 0.0f, "Arial", 10, "POS X: " + Setting.PosX.ToString("F1") + "  Y: " + Setting.PosY.ToString("F1") + " Z: " + Setting.PosZ.ToString("F1"));

            gl.DrawText(20, 100, 0.0f, 1f, 0.0f, "Arial", 10, "ROTATE X: " + Setting.PosAngleX.ToString("F1") + "  Y: " + Setting.PosAngleY.ToString("F1") + " Z: " + Setting.PosAngleZ.ToString("F1"));

            //gl.Color(1.0F, 1.0F, 1.0F);
            //gl.DrawText3D("Arial", 25, 2, 0.1f, "123456");


            #region Отображение координатной сетки
            //TODO: optimize
            int minX = 0;
            int maxX = 0;
            int minY = 0;
            int maxY = 0;

            foreach (Segment seg in DrawVectors)
            {
                foreach (Location point in seg.Points)
                {
                    //координаты следующей точки
                    int pointX = (int)point.X;
                    int pointY = (int)point.Y;

                    if (pointX < minX) minX = pointX;

                    if (pointY < minY) minY = pointY;

                    if (pointX > maxX) maxX = pointX;

                    if (pointY > maxY) maxY = pointY;
                }
            }

            if (DrawVectors.Count > 0)
            {
                gl.LineWidth(0.1f);
                gl.Color(CalcColor(100), 0F, CalcColor(204)); //(1.0f/255)*color
                gl.Begin(OpenGL.GL_LINES);

                int gridStep = (int)numericUpDown2.Value;


                for (int gX = minX; gX < maxX + 1; gX += gridStep)
                {
                    gl.Vertex(gX, minY, -0.01);
                    gl.Vertex(gX, maxY, -0.01);
                }

                for (int gY = minY; gY < maxY + 1; gY += gridStep)
                {
                    gl.Vertex(minX, gY, -0.01);
                    gl.Vertex(maxX, gY, -0.01);
                }

                gl.End();
            }

            #endregion






            #region Отображение данных

            foreach (Segment Vector in DrawVectors)
            {
                float sizeVec = (float)numericUpDown1.Value;

                if (Vector.IndividualPoints)
                {
                    foreach (Location point in Vector.Points)
                    {
                        gl.PointSize(sizeVec);

                        gl.Begin(OpenGL.GL_POINTS);
                        gl.Color(0f, 0f, 0f);//(1.0f/255)*color

                        //координаты следующей точки
                        float pointX = (float)point.X;
                        float pointY = (float)point.Y;

                        gl.Vertex((double)pointX, (double)pointY, 0);

                        gl.End();
                    }
                }
                else
                {
                    gl.LineWidth(sizeVec);

                    gl.Begin(OpenGL.GL_LINE_STRIP);
                    gl.Color(0f, 1.0f, 0);

                    foreach (Location point in Vector.Points)
                    {
                        //координаты следующей точки
                        float pointX = (float)point.X;
                        float pointY = (float)point.Y;

                        gl.Vertex((double)pointX, (double)pointY, 0);
                    }
                    gl.End();                    
                }



            }

            #endregion








            #region Отображение выделенных данных

            //выделение отдельной траектории
            foreach (Segment Vector in DrawVectors)
            {

                if (!Vector.Selected) continue;



                float sizeLine = 3;

                gl.LineWidth(sizeLine);

                gl.Begin(OpenGL.GL_LINE_STRIP);
                gl.Color(1.0f, 0.4f, 0);

                foreach (Location point in Vector.Points)
                {
                    //координаты следующей точки
                    float pointX = (float)point.X;
                    float pointY = (float)point.Y;
                    //float pointZ = (float)point.Z;

                    gl.Vertex((double)pointX, (double)pointY, 0);
                }
                gl.End();
            }

            //выделение отдельной точки
            foreach (Segment Vector in DrawVectors)
            {
                foreach (Location point in Vector.Points)
                {
                    if (!point.Selected) continue;




                    gl.PointSize(5f);

                    gl.Begin(OpenGL.GL_POINTS);
                    gl.Color(1.0f, 0.4f, 1.0f);

                    //координаты следующей точки
                    float pointX = (float)point.X;
                    float pointY = (float)point.Y;
                    //float pointZ = (float)point.Z;

                    gl.Vertex((double)pointX, (double)pointY, 0);

                    gl.End();

                }
            }



            #endregion




            //gl.Begin(OpenGL.GL_TRIANGLES);

            //gl.Color(1.0f, 0.0f, 0.0f);                      // Красный
            //gl.Vertex(0.0f, 1.0f, 0.0f);                  // Верх треугольника (Передняя)
            //gl.Color(0.0f, 1.0f, 0.0f);                      // Зеленный
            //gl.Vertex(-1.0f, -1.0f, 1.0f);                  // Левая точка
            //gl.Color(0.0f, 0.0f, 1.0f);                      // Синий
            //gl.Vertex(1.0f, -1.0f, 1.0f);                  // Правая точка

            //gl.End();










            gl.Disable(OpenGL.GL_BLEND);
            //выключение сглаживания линий
            gl.Disable(OpenGL.GL_LINE_SMOOTH);
            // завершаем отрисовку примитивов 
            gl.End();
            // возвращаем состояние матрицы 
            gl.PopMatrix();
            // отрисовываем геометрию 
            gl.Flush();
        }

        private void openGLControl1_OpenGLInitialized(object sender, EventArgs e)
        {
            OpenGL gl = openGLControl1.OpenGL;




            //// инициализация OpenGL
            //// инициализация бибилиотеки glut 
            //Glut.glutInit();
            //// инициализация режима экрана 
            //Glut.glutInitDisplayMode(Glut.GLUT_RGB | Glut.GLUT_DOUBLE); //Цвет RGB и двойной буфер использовать

            //// установка цвета очистки экрана (RGBA) 
            gl.ClearColor(255, 255, 255, 1);

            //// установка размера отображения 
            gl.Viewport(0, 0, openGLControl1.Width, openGLControl1.Height);

            //// активация проекционной матрицы 
            //gl.MatrixMode(MatrixMode.Projection);

            //// очистка матрицы 
            gl.LoadIdentity();

            //Glu.gluPerspective(45, OpenGL_preview.Width / (float)OpenGL_preview.Height, 0.1, 200);
            gl.MatrixMode(MatrixMode.Modelview);
            gl.LoadIdentity();
            gl.Enable(OpenGL.GL_DEPTH_TEST);

        }

        private void openGLControl1_Resize(object sender, EventArgs e)
        {
            //OpenGL gl = openGLControl1.OpenGL;

            //gl.MatrixMode(OpenGL.GL_PROJECTION);				// Select The Projection Matrix
            //gl.LoadIdentity();					// Reset The Projection Matrix

            //// Calculate The Aspect Ratio Of The Window
            //gl.Perspective(45.0, aspect: Width / Height, zNear: 0.1, zFar: 200.0);

            //gl.MatrixMode(OpenGL.GL_MODELVIEW);				// Select The Modelview Matrix
            //gl.LoadIdentity();

            openGLControl1.Refresh();
        }

        private void openGLControl1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '+' || e.KeyChar == '=')
            {
                if (Setting.keyShift) ++Setting.PosZ;
                else ++Setting.zoomSize;
            }

            if (e.KeyChar == '-' || e.KeyChar == '_')
            {
                if (Setting.keyShift) --Setting.PosZ;
                else
                {
                    if (Setting.zoomSize > 1) --Setting.zoomSize;
                }
            }

            openGLControl1.Refresh();
        }

        private void openGLControl1_KeyDown(object sender, KeyEventArgs e)
        {
            Setting.keyShift = e.Shift;

            openGLControl1.Refresh();

        }

        private void openGLControl1_KeyUp(object sender, KeyEventArgs e)
        {
            Setting.keyShift = e.Shift;

            openGLControl1.Refresh();

        }

        int _oldPosX;
        int _oldPosY;

        private void openGLControl1_MouseDown(object sender, MouseEventArgs e)
        {
            Setting.move3D = true;
            Setting.mouseButton = e.Button;

            _oldPosX = e.X;
            _oldPosY = e.Y;

            openGLControl1.Refresh();
        }

        private void openGLControl1_MouseUp(object sender, MouseEventArgs e)
        {
            Setting.move3D = false;

            openGLControl1.Refresh();
        }

        private void openGLControl1_MouseMove(object sender, MouseEventArgs e)
        {
            int deltaX = _oldPosX - e.X;
            int deltaY = _oldPosY - e.Y;

            _oldPosX = e.X;
            _oldPosY = e.Y;

            if (!Setting.move3D) return;

            if (Setting.mouseButton == MouseButtons.Left)
            {
                Setting.PosX -= deltaX;
                Setting.PosY += deltaY;
            }

            if (Setting.mouseButton == MouseButtons.Right)
            {
                if (deltaY > 0)
                {

                    --Setting.PosAngleX;
                }
                else
                {
                    ++Setting.PosAngleX;
                }

                if (deltaX > 0)
                {
                    --Setting.PosAngleY;
                }
                else
                {
                    ++Setting.PosAngleY;
                }
            }


            openGLControl1.Refresh();
        }

        private void button3DDefault_Click(object sender, EventArgs e)
        {
            Setting.zoomSize = 1;
            Setting.PosX = 0;
            Setting.PosY = 0;
            Setting.PosZ = -300;
            Setting.PosAngleX = 0;
            Setting.PosAngleY = 0;
            Setting.PosAngleZ = 0;

            if (Property.Orientation == 1)
            {
                Setting.PosAngleX = 0;
                Setting.PosAngleY = 0;
                Setting.PosAngleZ = 0;
            }

            if (Property.Orientation == 2)
            {
                Setting.PosAngleX = 180;
                Setting.PosAngleY = 0;
                Setting.PosAngleZ = 0;
            }

            if (Property.Orientation == 3)
            {
                Setting.PosAngleX = -180;
                Setting.PosAngleY = -180;
                Setting.PosAngleZ = 0;
            }

            if (Property.Orientation == 4)
            {
                Setting.PosAngleX = 0;
                Setting.PosAngleY = -180;
                Setting.PosAngleZ = 0;
            }

            openGLControl1.Refresh();
        }




        private float CalcColor(int invalue)
        {
            float i = 0f;

            i = (1.0f / 255) * invalue;

            return i;
        }

        private void Preview_Vectors_Load(object sender, EventArgs e)
        {
            // подключение обработчика, колесика мышки
            MouseWheel += this_MouseWheel;

            containsData = (DrawVectors.Count > 0);

        }

        //событие от колёсика мышки
        void this_MouseWheel(object sender, MouseEventArgs e)
        {


            float step = 1f;

            if (Setting.keyShift) step = 0.1f;


            if (e.Delta > 0) Setting.zoomSize += step;
            else
            {
                if ((Setting.zoomSize - step) > 0) Setting.zoomSize -= step;


            }

            openGLControl1.Refresh();


        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            openGLControl1.Refresh();
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            openGLControl1.Refresh();
        }
    }
}
