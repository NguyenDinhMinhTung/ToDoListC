using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Linq;
using Microsoft.Win32;
using System.Windows.Interop;

namespace ToDoList
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {

        public delegate void MouseMovedEvent();

        public MainWindow()
        {
            InitializeComponent();

            var currentDPI = (int)Registry.GetValue("HKEY_CURRENT_USER\\Control Panel\\Desktop\\WindowMetrics", "AppliedDPI", 96);
            var scale = 96 / (float)currentDPI;

            double x = Properties.Settings.Default.x;
            double y = Properties.Settings.Default.y;
            double width = Properties.Settings.Default.width;
            double height = Properties.Settings.Default.height;
            double opacity = Properties.Settings.Default.opacity;

            this.Left = x * scale;
            this.Top = y * scale;

            this.Width = width;
            this.Height = height;

            this.Opacity = opacity;

            updateEvenList();

            Sync.StartSyncToServer();
            syncFromServer();

            updateEvenList();

            //GlobalMouseHandler globalMouseHandler = new GlobalMouseHandler();
            //globalMouseHandler.TheMouseMoved += ()=> { txtTitle.Text = new Random().Next().ToString(); };
            //Application.AddMessageFilter(gmh);

            //ComponentDispatcher.ThreadFilterMessage += ComponentDispatcher_ThreadFilterMessage;

        }

        private void ComponentDispatcher_ThreadFilterMessage(ref MSG msg, ref bool handled)
        {
            if (msg.message == 513)//MOUSE_LEFTBUTTON_DOWN
            {
                txtTitle.Text = new Random().Next().ToString();
            }
        }

        void updateLastUpdateTextBlock()
        {
            DateTime updateTime = Properties.Settings.Default.lastUpdate;

            txtLastUpdate.Text = "同期:" + updateTime;
        }

        int compareDay(DateTime datetime1, DateTime datetime2)
        {
            return String.Compare(datetime1.ToShortDateString(), datetime2.ToShortDateString());
        }

        void updateEvenList()
        {
            StackPanel stackItem = new StackPanel();
            DateTime nowDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);

            List<Model.EVEN> evens = Model.DataProvider.Ins.DB.EVENS.Where(p => DateTime.Compare(p.daytime, nowDate) >= 0).ToList();
            evens.Sort((a, b) => { return DateTime.Compare(a.daytime.AddDays(-1 * a.notiday), b.daytime.AddDays(-1 * b.notiday)); });

            DayItem today = new DayItem();
            today.Date = DateTime.Now;
            stackItem.Children.Add(today);

            bool hasEvenToday = false;
            bool isSeparatorAdded = false;

            DateTime nowDay = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);

            foreach (Model.EVEN even in evens)
            {

                DateTime startDay = even.daytime.AddDays(-1 * even.notiday);
                if (compareDay(startDay, DateTime.Now) <= 0)
                {
                    stackItem.Children.Add(new EvenItem(even, DateTime.Now,
                        (status) => { even.status = status; Model.DataProvider.Ins.DB.SaveChanges(); Sync.PushToSyncQueue(even.evenid); Sync.StartSyncToServer(); },
                        () => { Model.DataProvider.Ins.DB.EVENS.Remove(even); Model.DataProvider.Ins.DB.SaveChanges(); updateEvenList(); },
                        () => { new AddWindow(even).ShowDialog(); updateEvenList(); }));
                    hasEvenToday = true;
                }
                else
                {
                    if (!isSeparatorAdded)
                    {
                        if (!hasEvenToday)
                        {
                            stackItem.Children.Add(new TextBlock() { Text = "今日は行事がありません。", FontStyle = FontStyles.Italic, Margin = new Thickness(10), HorizontalAlignment = HorizontalAlignment.Center });
                        }
                        stackItem.Children.Add(new Separator() { Margin = new Thickness(5) });
                        isSeparatorAdded = true;
                    }

                    if (compareDay(startDay, nowDay) != 0)
                    {
                        nowDay = new DateTime(startDay.Year, startDay.Month, startDay.Day);
                        DayItem dayItem = new DayItem();
                        dayItem.Date = nowDay;
                        stackItem.Children.Add(dayItem);
                    }

                    stackItem.Children.Add(new EvenItem(even, null,
                        (status) => { even.status = status; Model.DataProvider.Ins.DB.SaveChanges(); Sync.PushToSyncQueue(even.evenid); Sync.StartSyncToServer(); },
                        () => { Model.DataProvider.Ins.DB.EVENS.Remove(even); Model.DataProvider.Ins.DB.SaveChanges(); updateEvenList(); },
                        () => { new AddWindow(even).ShowDialog(); updateEvenList(); }));
                }
            }

            mainViewer.Content = stackItem;
        }

        private void ColorZone_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void btnAdd_MouseUp(object sender, MouseButtonEventArgs e)
        {
            AddWindow addWindow = new AddWindow();
            addWindow.ShowDialog();

            updateEvenList();
        }

        private void ckbCanResize_Checked(object sender, RoutedEventArgs e)
        {
            //this.AllowsTransparency = false;
            this.ResizeMode = ResizeMode.CanResize;
        }

        private void ckbCanResize_Unchecked(object sender, RoutedEventArgs e)
        {
            //this.AllowsTransparency = true;
            this.ResizeMode = ResizeMode.NoResize;
        }

        private void mnuClose_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Close();
        }

        private void Window_LocationChanged(object sender, EventArgs e)
        {
            var point = this.PointToScreen(new Point(0, 0));

            Properties.Settings.Default.x = point.X;
            Properties.Settings.Default.y = point.Y;

            Properties.Settings.Default.Save();
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Properties.Settings.Default.height = this.Height;
            Properties.Settings.Default.width = this.Width;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.SizeChanged += Window_SizeChanged;
            this.LocationChanged += Window_LocationChanged;
        }

        private void mnuSetting_MouseUp(object sender, MouseButtonEventArgs e)
        {
            new SettingWindow().ShowDialog(this.Opacity, (value) => { this.Opacity = value; Properties.Settings.Default.opacity = value; Properties.Settings.Default.Save(); });
        }

        private void syncFromServer()
        {
            DateTime nowDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);

            Sync.StartSyncFromServer(nowDate);

            updateLastUpdateTextBlock();
        }

        private void btnSync_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Sync.StartSyncToServer();
            syncFromServer();

            updateEvenList();
        }

        private void borMain_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void borMain_PreviewDragEnter(object sender, DragEventArgs e)
        {

        }

        private void borMain_DragEnter(object sender, DragEventArgs e)
        {
            txtTitle.Text = e.GetPosition(this).ToString();
        }
    }
}
