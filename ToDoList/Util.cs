using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace ToDoList
{
    class Util
    {
        public static Color[] colors = { Colors.White, Color.FromRgb(255, 112, 67), Color.FromRgb(156, 204, 101), Color.FromRgb(253, 216, 53), Color.FromRgb(186,104,200) };
        public static int getNewId()
        {
            int id = 1;
            var even= Model.DataProvider.Ins.DB.EVENS.OrderBy(p => -p.id).FirstOrDefault();

            if (even != null)
            {
                id = even.id + 1;
            }

            return id;
        }
    }
}
