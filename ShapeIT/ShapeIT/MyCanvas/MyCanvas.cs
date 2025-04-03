using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace ShapeIT
{
    public class MyCanvas : Canvas
    {
        public Figures figures { get; set; }
       
        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            this.figures.Draw(drawingContext);
        }
    }
}
