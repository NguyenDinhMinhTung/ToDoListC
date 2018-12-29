using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ToDoList
{
    /// <summary>
    /// DayItem.xaml の相互作用ロジック
    /// </summary>
    public partial class DayItem : UserControl
    {

        private DateTime _date;

        public DateTime Date { get { return _date; }
                               set { _date = value;
                                     txtDate.Text = _date.ToString("MM月dd日"); } }

        public DayItem()
        {
            InitializeComponent();
        }
    }
}
