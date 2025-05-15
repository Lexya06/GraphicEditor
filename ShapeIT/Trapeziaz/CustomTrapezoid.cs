using System;
using System.Windows;
using FigureAbstract;
using System.Windows.Media;

namespace Trapezoid
{
    public class MyTrapezoid:Figure
    {
        public void ReParallel()
        {
            if (Points.Length == MaxPoints())
            {
                double A = (Points[0].Y - Points[1].Y) / (Points[0].X - Points[1].X);
                Points[3].Y = Points[2].Y + A * (Points[3].X - Points[2].X);
            }
        }

        public override int MaxPoints()
        {
            return 4;
        }
        public override int MinPoints()
        {
            return 2;
        }
        public override string GetName()
        {
            return "Trapezoid";
        }
        public MyTrapezoid()
        {
            Points = new Point[0];
            BrushFill = new SolidColorBrush();
            BrushStroke = new SolidColorBrush();

        }

        public override void DrawShape(DrawingContext drawingContext)
        {
            BrushFill.Color = this.Fill;
            BrushStroke.Color = this.Stroke;
            Pen pen = new Pen(BrushStroke, this.StrokeThikness);
            StreamGeometry streamGeometry = new StreamGeometry();
            ReParallel();
            using (StreamGeometryContext g = streamGeometry.Open())
            {
                g.BeginFigure(Points[0], true, true);
                g.PolyLineTo(Points, true, true);
            }
            
            drawingContext.DrawGeometry(BrushFill, pen, streamGeometry);
        }
    }
}

