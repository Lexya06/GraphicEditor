
using System.Windows;
using System.Windows.Media;
using FigureAbstract;


namespace ShapeIT
{
    internal class MyRectangle:Figure
    {
        public override string GetName()
        {
            return "Rectangle";
        }
        public MyRectangle(){
            Points = new Point[0];
            BrushFill = new SolidColorBrush();
            BrushStroke = new SolidColorBrush();
        }
      
        public override void DrawShape(DrawingContext drawingContext)
        {
            
            BrushFill.Color = this.Fill;
            BrushStroke.Color = this.Stroke;
            Pen pen = new Pen(BrushStroke, this.StrokeThikness);
            drawingContext.DrawRectangle(BrushFill, pen, new Rect(Points[0], Points[1]));
          
        }
    }
}
