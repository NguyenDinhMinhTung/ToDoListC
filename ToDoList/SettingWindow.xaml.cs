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
using System.Windows.Shapes;

namespace ToDoList
{
    /// <summary>
    /// SettingWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class SettingWindow : Window
    {
        public delegate void ChangeOpacity(double value);

        ChangeOpacity changeOpacity;

        public SettingWindow()
        {
            InitializeComponent();
        }

        public void ShowDialog(double opacity, ChangeOpacity changeOpacity)
        {
            this.changeOpacity = changeOpacity;
            sliTransparent.Value = opacity * 100;
            txtTransparent.Text = sliTransparent.Value.ToString();
            ShowDialog();
        }

        private void sliTransparent_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            int value = (int)Math.Round(sliTransparent.Value);
            txtTransparent.Text = value.ToString();

            changeOpacity(value / 100.0);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            sliTransparent.ValueChanged += sliTransparent_ValueChanged;
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
