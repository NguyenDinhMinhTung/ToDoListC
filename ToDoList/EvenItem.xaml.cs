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
    /// EvenItem.xaml の相互作用ロジック
    /// </summary>
    public partial class EvenItem : UserControl
    {
        Model.EVEN even;

        public delegate void SaveEvenStatusToDatabase(bool status);
        public delegate void Action();

        SaveEvenStatusToDatabase saveEvenStatusToDatabase;
        Action deleteEven;
        Action editEven;

        public EvenItem(Model.EVEN even, DateTime? now, SaveEvenStatusToDatabase saveEvenStatusToDatabase, Action deleteEven, Action editEven)
        {
            InitializeComponent();

            this.even = even;

            this.saveEvenStatusToDatabase = saveEvenStatusToDatabase;
            this.deleteEven = deleteEven;
            this.editEven = editEven;

            mainCard.Background = new SolidColorBrush(Util.colors[even.color]);

            txtEvenName.Text = even.evenname;

            if (even.comment == null || even.comment == "")
            {
                txtComment.Visibility = Visibility.Collapsed;
            }
            else
            {
                txtComment.Text = even.comment;
            }

            string remaining = "";

            if (now != null)
            {
                int dayRemaining = (even.daytime - (DateTime)now).Days;

                if (dayRemaining > 0)
                {
                    remaining = "(後" + dayRemaining + "日)";
                }
                else
                {
                    remaining = "(今日)";
                    borTime.Background = new SolidColorBrush(Colors.Red);
                }
            }

            if (even.type == 0)
                txtTime.Text = even.daytime.ToShortTimeString() + remaining;
            else
            {
                txtTime.Text = "全日" + remaining;
            }

            changeStatus(even.status);
        }

        void changeStatus(bool status)
        {
            ckbDone.IsChecked = status;

            if (status == true)
            {
                txtEvenName.TextDecorations = TextDecorations.Strikethrough;
                txtComment.TextDecorations = TextDecorations.Strikethrough;
                txtTime.TextDecorations = TextDecorations.Strikethrough;

                txtEvenName.Foreground = new SolidColorBrush(Colors.Gray);
                txtComment.Foreground = new SolidColorBrush(Colors.Gray);
                txtTime.Foreground = new SolidColorBrush(Colors.Gray);

            }
            else
            {
                txtEvenName.TextDecorations = null;
                txtComment.TextDecorations = null;
                txtTime.TextDecorations = null;

                txtEvenName.Foreground = new SolidColorBrush(Colors.Black);
                txtComment.Foreground = new SolidColorBrush(Colors.Black);
                txtTime.Foreground = new SolidColorBrush(Colors.Black);
            }
        }

        private void ckbDone_Click(object sender, RoutedEventArgs e)
        {
            changeStatus((bool)ckbDone.IsChecked);
            saveEvenStatusToDatabase((bool)ckbDone.IsChecked);
        }

        private void mnuDelete_Selected(object sender, RoutedEventArgs e)
        {
            popupBox.IsPopupOpen = false;
            deleteEven();
        }

        private void mnuEdit_MouseUp(object sender, MouseButtonEventArgs e)
        {
            popupBox.IsPopupOpen = false;

            editEven();
        }
    }
}
