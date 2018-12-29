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
    /// Message.xaml の相互作用ロジック
    /// </summary>
    public partial class Message : Window
    {
        public Message()
        {
            InitializeComponent();
        }

        public void ShowDialog(string mess)
        {
            txtMess.Text = mess;
            ShowDialog();
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
