using System;
using System.Windows;
using System.Windows.Media;

namespace ShapeIT
{
    internal class CustomPolygon:Figure
    {
        public override void AddPoint(Point cord)
        {
            Point[] temp = new Point[Points.Length + 1];
            Array.Copy(Points, temp, Points.Length);
            temp[DotsFilled] = cord;
            Points = temp;
            DotsFilled++;
        }
        public override string GetName()
        {
            return "Polygon";
        }
        public CustomPolygon()
        {
            Points = new Point[0];
            BrushFill = new SolidColorBrush();
            BrushStroke = new SolidColorBrush();
            this.StrokeThikness = 5;
        }
        public override void DrawShape(DrawingContext drawingContext)
        {

            BrushFill.Color = this.Fill;
            BrushStroke.Color = this.Stroke;
            Pen pen = new Pen(BrushStroke, this.StrokeThikness);
            StreamGeometry streamGeometry = new StreamGeometry();
            using (StreamGeometryContext g = streamGeometry.Open())
            {
                g.BeginFigure(Points[0], true, true);
                g.PolyLineTo(Points,true, true);
            }
            drawingContext.DrawGeometry(BrushFill, pen, streamGeometry);

        }
    }
}
