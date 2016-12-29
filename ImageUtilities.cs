// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

using System.Collections.Generic;
using ToolsGenGkode.pages;

namespace ToolsGenGkode
{
    class ImageUtilities
    {
    }


    static class VectorUtilities
    {

        public static List<Segment> Rotate(List<Segment> dataCVectors)
        {
            List<Segment> returnValue = new List<Segment>();

            // получим границы изображения
            decimal minX = 99999;
            decimal maxX = -99999;
            decimal minY = 99999;
            decimal maxY = -99999;

            foreach (Segment vector in dataCVectors)
            {
                foreach (Location point in vector.Points)
                {
                    if (minX > point.X) minX = point.X;

                    if (maxX < point.X) maxX = point.X;

                    if (minY > point.Y) minY = point.Y;

                    if (maxY < point.Y) maxY = point.Y;
                }
            }



            if (Property.Orientation == 1)
            {
                //реверс по оси Y


                List<Segment> tmp = new List<Segment>();

                //вычислим дельту, для смещения векторов
                decimal delta = minY + maxY;

                List<Segment> vectors = new List<Segment>();
                List<Location> points = new List<Location>();

                foreach (Segment vector in dataCVectors)
                {
                    points = new List<Location>();

                    foreach (Location point in vector.Points)
                    {
                        points.Add(new Location(point.X, (-point.Y) + delta, point.Svalue, point.Fvalue, point.Pvalue, point.SpindelOn, point.Selected,point.Bright));
                    }
                    tmp.Add(new Segment(points, vector.Selected,vector.TempTraectory,vector.Dirrect,vector.IndividualPoints));
                    points = new List<Location>();
                }

                returnValue = tmp;

                vectors = new List<Segment>();
                points = new List<Location>();

            }


            if (Property.Orientation == 2)
            {
                //ничего не делаем



            }


            if (Property.Orientation == 3)
            {
                //реверс по оси X


                List<Segment> tmp = new List<Segment>();

                //вычислим дельту, для смещения векторов
                decimal deltaX = minX + maxX;

                List<Segment> vectors = new List<Segment>();
                List<Location> points = new List<Location>();

                foreach (Segment vector in dataCVectors)
                {
                    points = new List<Location>();

                    foreach (Location point in vector.Points)
                    {
                        points.Add(new Location((-point.X) + deltaX, point.Y, 0, 0, 0, false, point.Selected));
                    }
                    tmp.Add(new Segment(points, vector.Selected));
                    points = new List<Location>();
                }

                returnValue = tmp;

                vectors = new List<Segment>();
                points = new List<Location>();
            }


            if (Property.Orientation == 4)
            {
                //реверс по обоим осям

                List<Segment> tmp = new List<Segment>();

                //вычислим дельту, для смещения векторов
                decimal deltaY = minY + maxY;
                decimal deltaX = minX + maxX;

                List<Segment> vectors = new List<Segment>();
                List<Location> points = new List<Location>();

                foreach (Segment vector in dataCVectors)
                {
                    points = new List<Location>();

                    foreach (Location point in vector.Points)
                    {
                        points.Add(new Location((-point.X) + deltaX, (-point.Y) + deltaY, 0, 0, 0, false, point.Selected));
                    }
                    tmp.Add(new Segment(points, vector.Selected));
                    points = new List<Location>();
                }

                returnValue = tmp;

                vectors = new List<Segment>();
                points = new List<Location>();
            }

            return returnValue;
        }

        public static List<Location> Rotate(List<Location> dataCPoints)
        {
            List<Location> returnValue = new List<Location>();

            // получим границы изображения
            decimal minX = 99999;
            decimal maxX = -99999;
            decimal minY = 99999;
            decimal maxY = -99999;

            foreach (Location point in dataCPoints)
            {
                if (minX > point.X) minX = point.X;

                if (maxX < point.X) maxX = point.X;

                if (minY > point.Y) minY = point.Y;

                if (maxY < point.Y) maxY = point.Y;
            }
    



            if (Property.Orientation == 1)
            {
                //реверс по оси Y
                //вычислим дельту, для смещения векторов
                decimal delta = minY + maxY;

                List<Location> points = new List<Location>();

                foreach (Location point in dataCPoints)
                {
                    points.Add(new Location(point.X, (-point.Y) + delta, 0, 0, 0, false, point.Selected));
                }
                returnValue = points;

                points = new List<Location>();
            }


            if (Property.Orientation == 2)
            {
                //ничего не делаем
            }

            if (Property.Orientation == 3)
            {
                //реверс по оси X

                //вычислим дельту, для смещения векторов
                decimal deltaX = minX + maxX;

                List<Location> points = new List<Location>();

                foreach (Location point in dataCPoints)
                {
                    points.Add(new Location((-point.X) + deltaX, point.Y, 0, 0, 0, false, point.Selected));
                }
                

                returnValue = points;

                points = new List<Location>();
            }


            if (Property.Orientation == 4)
            {
                //реверс по обоим осям
                //вычислим дельту, для смещения векторов
                decimal deltaY = minY + maxY;
                decimal deltaX = minX + maxX;

                List<Location> points = new List<Location>();

                foreach (Location point in dataCPoints)
                {
                    points.Add(new Location((-point.X) + deltaX, (-point.Y) + deltaY, 0, 0, 0, false, point.Selected));
                }
                returnValue = points;
                points = new List<Location>();
            }

            return returnValue;
        }




    }
}
