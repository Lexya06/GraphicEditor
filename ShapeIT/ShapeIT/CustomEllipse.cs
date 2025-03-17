﻿using System;
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
            this.Fill = Colors.Red;
            this.Stroke = Colors.Black;
            this.StrokeThikness = 5;
        }
        public override void DrawShape(DrawingContext drawingContext, SolidColorBrush brushFill, SolidColorBrush brushStroke)
        {

            brushFill.Color = this.Fill;
            brushStroke.Color = this.Stroke;
            center = new Point((Points[1].X + Points[0].X) / 2, (Points[1].Y + Points[0].Y) / 2);
            Pen pen = new Pen(brushStroke, this.StrokeThikness);
            drawingContext.DrawEllipse(brushFill,pen,center, Math.Abs((Points[1].X - Points[0].X)/2), Math.Abs((Points[1].Y - Points[0].Y)/2));
            
        }
    }
}
