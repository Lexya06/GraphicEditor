
using System.Windows.Media;
using System.Windows;

namespace ShapeIT
{
    internal class MySegment:Figure
    {
        public override string GetName()
        {
            return "Segment";
        }
        public MySegment()
        {
            Points = new Point[2];
            BrushFill = new SolidColorBrush();
            BrushStroke = new SolidColorBrush();
            
        }
        public override void DrawShape(DrawingContext drawingContext)
        {
            BrushFill.Color = this.Fill;
            BrushStroke.Color = this.Stroke;
            Pen pen = new Pen(BrushStroke,this.StrokeThikness);
            drawingContext.DrawLine(pen, Points[0], Points[1]);
        }
    }
}
