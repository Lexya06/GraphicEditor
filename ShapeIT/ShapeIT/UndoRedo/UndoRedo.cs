using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShapeIT
{
    internal class FiguresCache
    {
        const int maxRollback = 1024;
        Stack<Figure> popFigure = new Stack<Figure>();
        public int Forward { get; private set; }
        public int Backward { get; private set; }
        Stack<Figure> cache = new Stack<Figure>();
        public void FigCacheUpdate(List<Figure> figures)
        {
            Backward = (figures.Count > maxRollback) ? maxRollback : figures.Count;
            Forward = 0;
            cache.Clear();
            popFigure.Clear();
            foreach (Figure figure in figures)
            {
                cache.Push(figure);
            }
        }
        public void Undo()
        {
            if (Backward > 0)
            {
                popFigure.Push(cache.Pop());
                Backward--;
                Forward++;
            }
        }
        public void Redo()
        {
            if (Forward > 0)
            {
                cache.Push(popFigure.Pop());
                Backward++;
                Forward--;
            }
        }

        public List<Figure> GetFinalList()
        {
            List<Figure> list = new List<Figure>();
            foreach (Figure f in cache)
            {
                list.Add(f);
            }
            list.Reverse();
            return list;
        }
    }
}
