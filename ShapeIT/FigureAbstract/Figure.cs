using System;
using System.Windows;
using System.Windows.Media;
using Newtonsoft.Json;

namespace FigureAbstract
{
    public abstract class Figure
    {
        public virtual int MaxPoints()
        {
            return 2;
        }

        public virtual int MinPoints()
        {
            return 2;
        }

        public int DotsFilled { get; protected set; } = 0;

        public virtual string GetName()
        {
            return "Figure";
        }
        public Point[] Points { get; set; }
        public virtual void AddPoint(Point cord)
        {

            if (DotsFilled < MaxPoints())
            {
                Point[] temp = new Point[Points.Length + 1];
                Array.Copy(Points, temp, Points.Length);
                temp[DotsFilled] = cord;
                Points = temp;
                DotsFilled++;
            }
        }

        public virtual void ReplacePoint(int ind, Point cord)
        {
            if (ind > 0 && ind < MaxPoints())
            {
                Points[ind] = cord;
            }
        }
        public abstract void DrawShape(DrawingContext drawingContext);
        public Color Fill { get; set; }
        public int StrokeThikness { get; set; }
        public Color Stroke { get; set; }

        [JsonIgnore]
        public SolidColorBrush BrushFill { get; set; }

        [JsonIgnore]
        public SolidColorBrush BrushStroke { get; set; }
    }

}
