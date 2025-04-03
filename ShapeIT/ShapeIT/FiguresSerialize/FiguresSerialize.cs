using System;
using System.Collections.Generic;
using System.IO.Packaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShapeIT.Serialize
{
    public class FiguresSerialize
    {
        public void Serialize(string fileName, ICollection<Figure> figures)
        {
        }
        public void Deserialize(string fileName, out ICollection<Figure> figures)
        {
            figures = new List<Figure>();
        }

    }
}
