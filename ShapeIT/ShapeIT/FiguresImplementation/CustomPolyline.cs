
using System;
using System.Windows;
using System.Windows.Media;
using FigureAbstract;

namespace ShapeIT
{
    internal class MyPolyline:Figure
    {
        public override int MaxPoints()
        {
            return 10000;
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
