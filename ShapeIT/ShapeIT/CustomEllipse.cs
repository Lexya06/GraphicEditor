using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;
using System.Windows.Shapes;
using System.Windows.Controls;

namespace ShapeIT
{
    internal class CustomEllipse:Figure
    {
        public override string GetName()
        {
            return "Ellipse";
        }
        //public int RadiusX { get; set; }
        //public int RadiusY { get; set; }
        public CustomEllipse()
        {
            Points = new Point[2];
            this.Fill = Colors.Red;
            this.Stroke = Colors.Black;
            this.StrokeThikness = 5;
        }
        public override void DrawShape(DrawingContext drawingContext, SolidColorBrush brushFill, SolidColorBrush brushStroke)
        {

            brushFill.Color = this.Fill;
            brushStroke.Color = this.Stroke;
            Pen pen = new Pen(brushStroke, this.StrokeThikness);
            drawingContext.DrawEllipse(brushFill,pen,Points[0], Points[1].X - Points[0].X, Points[1].Y - Points[0].Y);
            
        }
    }
}
