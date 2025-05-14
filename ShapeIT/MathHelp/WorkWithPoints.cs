using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MathHelp
{
    
    public class WorkWithPoints
    {
        private const double Epsilon = 1; // Погрешность для учёта ошибок вычислений

        public static bool FindIntersectBetween(Point pt1Start, Point pt1End, Point pt2Start, Point pt2End)
        {
            Point temp;
            if (pt1Start.Y > pt1End.Y)
            {
                temp = pt1Start;
                pt1Start = pt1End;
                pt1End = temp;
            }
            if (pt2Start.Y > pt2End.Y)
            {
                temp = pt2Start;
                pt2Start = pt2End;
                pt2End = temp;
            }
            bool intersect = false;
            if ((pt2Start.X > pt1Start.X && pt2End.X < pt1End.X) || (pt1Start.X > pt2Start.X && pt1End.X < pt2End.X))
            {
                if ((pt2End.Y > pt1Start.Y && pt2Start.Y < pt1End.Y) && (pt1End.Y > pt2Start.Y && pt1Start.Y < pt2End.Y))
                {
                    intersect = true;
                }
            }
            return intersect;
        }
        public static void CorrectIntersection(ref Point pt1Start, ref Point pt1End, ref Point pt2Start, ref Point pt2End)
        {
            if (FindIntersectBetween(pt1Start, pt1End, pt2Start, pt2End))
            {
                pt1End = pt2End;
            }
        }
    }
    
}
