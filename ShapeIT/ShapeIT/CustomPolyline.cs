
using System;
using System.Windows;
using System.Windows.Media;

namespace ShapeIT
{
    internal class MyPolyline:Figure
    {
        public override void AddPoint(int numArrPoint, Point cord)
        {
            Point[] temp = new Point[Points.Length+1];
            Array.Copy(Points, temp, Points.Length);
            temp[numArrPoint] = cord;
            Points = temp;

        }
        public override string GetName()
        {
            return "Polyline";
        }
        public MyPolyline() 
        {
            Points = new Point[0];
            this.Fill = Colors.Red;
            this.Stroke = Colors.Black;
            this.StrokeThikness = 5;
        }
        public override void DrawShape(DrawingContext drawingContext,SolidColorBrush brushFill, SolidColorBrush brushStroke)
        {
            
            brushFill.Color = this.Fill;
            brushStroke.Color = this.Stroke;
            Pen pen = new Pen(brushStroke, this.StrokeThikness);
            for (int i = 0; i < Points.Length-1; i++)
            {
                drawingContext.DrawLine(pen, Points[i], Points[i + 1]);
            }
            
        }
    }
}
