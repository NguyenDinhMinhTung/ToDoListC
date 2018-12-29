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
    /// AddWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class AddWindow : Window
    {
        Model.EVEN even;
        List<ComboBoxItem> lstNotifiItem = new List<ComboBoxItem>();
        TextBox txtNotifiDay;

        public AddWindow()
        {
            InitializeComponent();

            setCbbNotifi(0);
        }

        public AddWindow(Model.EVEN even)
        {
            InitializeComponent();
            setCbbNotifi(0);

            this.even = even;

            txtTitle.Text = even.evenname;
            txtComment.Text = even.comment;
            sltDate.Text = even.daytime.ToString("yyyy/MM/dd");
            sltTime.Text = even.daytime.ToString("HH:mm");
            ckbAllDay.IsChecked = even.type == 0 ? false : true;
            cbbColor.SelectedIndex = even.color;

            switch (even.notiday)
            {
                case 0:
                    cbbNofification.SelectedIndex = 0;
                    break;

                case 1:
                    cbbNofification.SelectedIndex = 1;
                    break;

                case 7:
                    cbbNofification.SelectedIndex = 2;
                    break;

                case 30:
                    cbbNofification.SelectedIndex = 3;
                    break;

                default:
                    addItemNotifi(even.notiday);
                    break;
            }
            ckbAllDay_Click(null, null);
        }

        void addItemNotifi(int day)
        {
            lstNotifiItem.Insert(lstNotifiItem.Count - 1, new ComboBoxItem() { Content = day + "日前", Tag = day });

            cbbNofification.Items.Clear();
            foreach (ComboBoxItem cbbitem in lstNotifiItem)
            {
                cbbNofification.Items.Add(cbbitem);
            }

            cbbNofification.SelectedIndex = lstNotifiItem.Count - 2;
        }

        public void setCbbNotifi(int selectIndex)
        {
            lstNotifiItem.Add(new ComboBoxItem() { Content = "当日", Tag = 0 });
            lstNotifiItem.Add(new ComboBoxItem() { Content = "一日前", Tag = 1 });
            lstNotifiItem.Add(new ComboBoxItem() { Content = "一週間前", Tag = 7 });
            lstNotifiItem.Add(new ComboBoxItem() { Content = "一か月前", Tag = 30 });
            ComboBoxItem cbbItemAdd = new ComboBoxItem();
            StackPanel stkAdd = new StackPanel()
            {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Center
            };

            txtNotifiDay = new TextBox()
            {
                TextAlignment = TextAlignment.Center,
                Width = 40,
                Name = "txtAddNotifiItem"
            };

            stkAdd.Children.Add(txtNotifiDay);

            stkAdd.Children.Add(new TextBlock()
            {
                VerticalAlignment = VerticalAlignment.Center,
                Text = "日前"
            });

            Button btnAddNotifi = new Button()
            {
                Margin = new Thickness(10, 0, 0, 0),
                Content = "OK",
            };
            btnAddNotifi.Click += btnAddNotifi_Click;

            stkAdd.Children.Add(btnAddNotifi);

            cbbItemAdd.Content = stkAdd;
            lstNotifiItem.Add(cbbItemAdd);

            cbbNofification.Items.Clear();
            foreach (ComboBoxItem cbbitem in lstNotifiItem)
            {
                cbbNofification.Items.Add(cbbitem);
            }

            cbbNofification.SelectedIndex = selectIndex;
        }

        private void ckbAllDay_Click(object sender, RoutedEventArgs e)
        {
            if (ckbAllDay.IsChecked == true)
            {
                sltTime.Visibility = Visibility.Collapsed;
                sltDate.Width = 210;
            }
            else
            {
                sltTime.Visibility = Visibility.Visible;
                sltDate.Width = 100;
            }
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            if (txtTitle.Text.Trim() == "")
            {
                new Message().ShowDialog("タイトルはまだ入力されていません。");
            }
            else if (sltDate.Text.Trim() == "")
            {
                new Message().ShowDialog("日付はまだ入力されていません。");
            }
            else if (!(bool)ckbAllDay.IsChecked && (sltTime.Text == null || sltTime.Text.Trim() == ""))
            {
                new Message().ShowDialog("時間はまだ入力されていません。");
            }
            else
            {
                string[] dateArray = sltDate.Text.Trim().Split('/');
                DateTime dateTime;

                if (!(bool)ckbAllDay.IsChecked)
                {
                    string[] timeArray = sltTime.Text.Trim().Split(':');
                    dateTime = new DateTime(int.Parse(dateArray[0]), int.Parse(dateArray[1]), int.Parse(dateArray[2]),
                        int.Parse(timeArray[0]), int.Parse(timeArray[1]), 0);
                }
                else
                {
                    dateTime = new DateTime(int.Parse(dateArray[0]), int.Parse(dateArray[1]), int.Parse(dateArray[2]), 23, 59, 59);
                }

                if (even == null)
                {
                    even = new Model.EVEN();
                    even.evenid = Util.getNewId();
                    Model.DataProvider.Ins.DB.EVENS.Add(even);
                }

                even.evenname = txtTitle.Text;
                even.daytime = dateTime;
                even.comment = txtComment.Text;
                even.status = false;
                even.color = cbbColor.SelectedIndex;
                even.type = (bool)ckbAllDay.IsChecked ? 1 : 0;

                even.notiday = (int)lstNotifiItem[cbbNofification.SelectedIndex].Tag;

                Model.DataProvider.Ins.DB.SaveChanges();

                Sync.PushToSyncQueue(even.evenid);
                Sync.StartSyncToServer();

                Close();
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void btnAddNotifi_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                addItemNotifi(int.Parse(txtNotifiDay.Text));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                txtNotifiDay.Text = "";
            }
        }

        private void cbbColor_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbbColor.SelectedIndex >= 0)
            {
                boxColor.Background = new SolidColorBrush(Util.colors[cbbColor.SelectedIndex]);
            }
        }

        private void cbbNofification_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbbNofification.SelectedIndex == cbbNofification.Items.Count - 1 && cbbNofification.Items.Count - 2 >= 0)
            {
                cbbNofification.SelectedIndex = cbbNofification.Items.Count - 2;
            }
        }
    }
}
