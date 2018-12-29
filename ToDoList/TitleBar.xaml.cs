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
    /// TitleBar.xaml の相互作用ロジック
    /// </summary>
    public partial class TitleBar : UserControl
    {

        public TitleBar()
        {
            InitializeComponent();
        }

        private Window getParentWindow(FrameworkElement f)
        {
            FrameworkElement parent = f;
            while (parent.Parent != null)
            {
                parent = parent.Parent as FrameworkElement;
            }

            return (Window)parent;
        }

        private void ColorZone_DragEnter(object sender, DragEventArgs e)
        {

        }

        private void ColorZone_MouseDown(object sender, MouseButtonEventArgs e)
        {
            
        }

        private void ColorZone_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
               getParentWindow(this).DragMove();
            }
        }

        private void DockPanel_MouseUp(object sender, MouseButtonEventArgs e)
        {
            getParentWindow(this).Close();
        }
    }
}
