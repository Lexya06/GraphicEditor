﻿using System;
using System.Windows;
using System.Windows.Media;
using FigureAbstract;

namespace ShapeIT
{
    internal class CustomPolygon:Figure
    {
        public override int MaxPoints()
        {
            return 5000;
        }
      
        public override string GetName()
        {
            return "Polygon";
        }
        public CustomPolygon()
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
            using (StreamGeometryContext g = streamGeometry.Open())
            {
                g.BeginFigure(Points[0], true, true);
                g.PolyLineTo(Points,true, true);
            }
            drawingContext.DrawGeometry(BrushFill, pen, streamGeometry);

        }
    }
}
