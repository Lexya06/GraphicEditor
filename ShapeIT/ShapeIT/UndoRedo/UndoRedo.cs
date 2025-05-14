using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FigureAbstract;

namespace ShapeIT
{
    internal class FiguresCache
    {
        
        public Stack<Figure> PopFigure { get; set; } = new Stack<Figure>();
        public Stack<Figure> Cache = new Stack<Figure>();
        public void FigCacheUpdate(List<Figure> figures)
        {
            
            Cache.Clear();
            PopFigure.Clear();
            foreach (Figure figure in figures)
            {
                Cache.Push(figure);
            }
        }
        public void Undo()
        {
            if (Cache.Count > 0)
            {
                PopFigure.Push(Cache.Pop());
            }
        }
        public void Redo()
        {
            if (PopFigure.Count > 0)
            {
                Cache.Push(PopFigure.Pop());
            }
        }

        public List<Figure> GetFinalList()
        {
            List<Figure> list = new List<Figure>();
            foreach (Figure f in Cache)
            {
                list.Add(f);
            }
            list.Reverse();
            return list;
        }
    }
}
