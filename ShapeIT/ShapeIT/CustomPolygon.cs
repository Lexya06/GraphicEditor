using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            temp[dotsFilled] = cord;
            Points = temp;
            dotsFilled++;
        }
        public override string GetName()
        {
            return "Polygon";
        }
        public CustomPolygon()
        {
            Points = new Point[0];  
            this.Fill = Colors.Red;
            this.Stroke = Colors.Black;
            this.StrokeThikness = 5;
        }
        public override void DrawShape(DrawingContext drawingContext, SolidColorBrush brushFill, SolidColorBrush brushStroke)
        {
            brushFill.Color = this.Fill;
            brushStroke.Color = this.Stroke;
            Pen pen = new Pen(brushStroke, this.StrokeThikness);
            StreamGeometry streamGeometry = new StreamGeometry();
            using (StreamGeometryContext g = streamGeometry.Open())
            {
                g.BeginFigure(Points[0], true, true);
                g.PolyLineTo(Points,true, true);
            }
            drawingContext.DrawGeometry(brushFill, pen, streamGeometry);

        }
    }
}
