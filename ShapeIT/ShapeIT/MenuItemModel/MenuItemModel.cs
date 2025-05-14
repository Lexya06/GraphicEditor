using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShapeIT
{
    public class MenuItemModel
    {
        private int selectedItemInd = -1;
        public int SelectedItemInd { get { return selectedItemInd; } set { if (selectedItemInd != value) { selectedItemInd = value; } } }
    }
}
