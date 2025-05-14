using System;
using System.Collections.Generic;
using System.Windows.Media;
using FigureAbstract;

namespace ShapeIT
{
    public class Figures
    {
        public Figures()
        {
            Linker = new List<Figure>();
        }
        public List<Type> FigureTypes { get; set; }
        public int IndPotentialFigure { get; set; } = -1;
        public Figure PotentialFigure { get; set; }
        public List<Figure> Linker { get; set; }

        public void Draw(DrawingContext drawingContext)
        {
            for (int i = 0; i < Linker.Count; i++)
            {
                Linker[i].DrawShape(drawingContext);
            }
        }
    }
}
