using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;


namespace ShapeIT
{
    public abstract class Figure
    {
        protected int DotsFilled { get; set; } = 0;
        public virtual string GetName()
        {
            return "Figure";
        }
        public Point[] Points { get; set; }
        public virtual void AddPoint(Point cord)
        {

            Points[DotsFilled] = cord;
            if (DotsFilled < Points.Length - 1)
            {
                DotsFilled++;
            }
        }

        public abstract void DrawShape(DrawingContext drawingContext);
        public Color Fill { get; set; }
        public int StrokeThikness { get; set; }
        public Color Stroke { get; set; }
        public SolidColorBrush BrushFill { get; set; }
        public SolidColorBrush BrushStroke { get; set; }
    }
}
