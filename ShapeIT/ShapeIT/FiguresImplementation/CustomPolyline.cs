
using System;
using System.Windows;
using System.Windows.Media;

namespace ShapeIT
{
    internal class MyPolyline:Figure
    {
        public override void AddPoint(Point cord)
        {
            Point[] temp = new Point[Points.Length+1];
            Array.Copy(Points, temp, Points.Length);
            temp[DotsFilled] = cord;
            Points = temp;
            DotsFilled++;
        }
        public override string GetName()
        {
            return "Polyline";
        }
        public MyPolyline() 
        {
            Points = new Point[0];
            BrushFill = new SolidColorBrush();
            BrushStroke = new SolidColorBrush();
        }
        public override void DrawShape(DrawingContext drawingContext)
        {
            BrushFill.Color = this.Fill;
            BrushStroke.Color = this.Stroke;
            Pen pen = new Pen(BrushStroke, this.StrokeThikness);
            for (int i = 0; i < Points.Length-1; i++)
            {
                drawingContext.DrawLine(pen, Points[i], Points[i + 1]);
            }
            
        }
    }
}
