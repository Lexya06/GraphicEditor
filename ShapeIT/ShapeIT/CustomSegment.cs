using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Shapes;
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
            this.Fill = Colors.Red;
            this.Stroke = Colors.Black;
            this.StrokeThikness = 5;
        }
        public override void DrawShape(DrawingContext drawingContext, SolidColorBrush brushFill,SolidColorBrush brushStroke)
        {
            brushFill.Color = this.Fill;
            brushStroke.Color = this.Stroke;
            Pen pen = new Pen(brushStroke,this.StrokeThikness);
            drawingContext.DrawLine(pen, Points[0], Points[1]);
        }
    }
}
