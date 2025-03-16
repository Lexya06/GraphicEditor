using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

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
            this.Fill = Colors.Red;
            this.Stroke = Colors.Black;
            this.StrokeThikness = 5;
        }
        //public int Width {  get; set; }
        //public int Height { get; set; }
        public override void DrawShape(DrawingContext drawingContext,SolidColorBrush brushFill,SolidColorBrush brushStroke)
        {
            brushFill.Color = this.Fill;
            brushStroke.Color = this.Stroke;
            Pen pen = new Pen(brushStroke, this.StrokeThikness);
            drawingContext.DrawRectangle(brushFill, pen, new Rect(Points[0], Points[1]));
            

        }
    }
}
