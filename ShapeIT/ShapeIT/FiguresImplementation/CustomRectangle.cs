
using System.Windows;
using System.Windows.Media;


namespace ShapeIT
{
    internal class MyRectangle:Figure
    {
        public override string GetName()
        {
            return "Rectangle";
        }
        public MyRectangle(){
            Points = new Point[2];
            BrushFill = new SolidColorBrush();
            BrushStroke = new SolidColorBrush();
            this.StrokeThikness = 5;
        }
        //public int Width {  get; set; }
        //public int Height { get; set; }
        public override void DrawShape(DrawingContext drawingContext)
        {
            
            BrushFill.Color = this.Fill;
            BrushStroke.Color = this.Stroke;
            Pen pen = new Pen(BrushStroke, this.StrokeThikness);
            drawingContext.DrawRectangle(BrushFill, pen, new Rect(Points[0], Points[1]));
          
        }
    }
}
