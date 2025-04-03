using System;
using System.Windows.Media;
using System.Windows;

namespace ShapeIT
{
    internal class CustomEllipse:Figure
    {
        public Point center {  get; set; }
        public override string GetName()
        {
            return "Ellipse";
        }
        //public int RadiusX { get; set; }
        //public int RadiusY { get; set; }
        public CustomEllipse()
        {
            Points = new Point[2];
            BrushFill = new SolidColorBrush();
            BrushStroke = new SolidColorBrush();
            this.StrokeThikness = 5;
        }
        public override void DrawShape(DrawingContext drawingContext)
        {

            BrushFill.Color = this.Fill;
            BrushStroke.Color = this.Stroke;
            center = new Point((Points[1].X + Points[0].X) / 2, (Points[1].Y + Points[0].Y) / 2);
            Pen pen = new Pen(BrushStroke, this.StrokeThikness);
            drawingContext.DrawEllipse(BrushFill,pen,center, Math.Abs((Points[1].X - Points[0].X)/2), Math.Abs((Points[1].Y - Points[0].Y)/2));
            
        }
    }
}
