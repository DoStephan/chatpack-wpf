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
using LiveCharts;
using LiveCharts.Wpf;
using Microsoft.Win32;
using System.Windows.Media.Animation;
using System.Runtime.InteropServices;
using System.Threading;

namespace Wpf
{
    /// <summary>
    /// Interaction logic for demoExample.xaml
    /// </summary>
    public partial class demoExample : Window
    {
        public demoExample()
        {
            InitializeComponent();
            //adding series will update and animate the chart automatically
            //SeriesCollection.Add(new ColumnSeries
            //{
            //    Title = "2016",
            //    Values = new ChartValues<double> { 11, 56, 42 }
            //});

            //also adding values updates and animates the chart automatically
            //SeriesCollection[1].Values.Add(48d);


            Thread i = new Thread(GetActiveWindowTitle);
            i.Start();
        }
        //private void ClickEvent(object sender, RoutedEventArgs e)
        //{
        //    string text = GetActiveWindowTitle();
        //    actWind.Text = text;
        //}

        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();
        [DllImport("user32.dll")]
        static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);

        private void GetActiveWindowTitle()
        {

            ChangeWin dele = new ChangeWin(ChangeText);
            const int nChars = 256;
            StringBuilder Buff = new StringBuilder(nChars);
            IntPtr handle = GetForegroundWindow();
            while (true)
            {
                Thread.Sleep(700);
                if (GetWindowText(handle, Buff, nChars) > 0)
                {

                    actWind.Dispatcher.BeginInvoke(dele, Buff.ToString());
                    //Buff.ToString();//return Buff.ToString();
                }
            }
        }
        private void ChangeText(string text)
        {
            actWind.Text = text;
        }
        private delegate void ChangeWin(string text);

        private void moveTb(object sender, RoutedEventArgs e)
        {
            if (tb.Visibility == Visibility.Visible)
                tb.Visibility = Visibility.Collapsed;
            else
                tb.Visibility = Visibility.Visible;

        }
        private void StackPanel_MouseEnter(object sender, MouseEventArgs e)
        {
            Border sp = sender as Border;
            DoubleAnimation db = new DoubleAnimation();
            //db.From = 12;
            db.To = 150;
            db.Duration = TimeSpan.FromSeconds(0.5);
            db.AutoReverse = false;
            db.RepeatBehavior = new RepeatBehavior(1);
            sp.BeginAnimation(StackPanel.HeightProperty, db);
        }

        private void StackPanel_MouseLeave(object sender, MouseEventArgs e)
        {
            Border sp = sender as Border;
            DoubleAnimation db = new DoubleAnimation();
            //db.From = 12;
            db.To = 12;
            db.Duration = TimeSpan.FromSeconds(0.5);
            db.AutoReverse = false;
            db.RepeatBehavior = new RepeatBehavior(1);
            sp.BeginAnimation(StackPanel.HeightProperty, db);
        }

        //private void ShowStats(object sender, RoutedEventArgs e)
        //{
        //    SeriesCollection = new SeriesCollection
        //    {
        //        new ColumnSeries
        //        {
        //            Title = "2015",
        //            Values = new ChartValues<double> { 10, 50, 39, 50 }
        //        }
        //    };


        //    Labels = new[] { "Monday", "Tuesday", "Wendesday", "Thursday", "Friday" };
        //    Formatter = value => value.ToString();

        //    DataContext = this;

        //}
        public SeriesCollection SeriesCollection { get; set; }
        public string[] Labels { get; set; }
        public Func<double, string> Formatter { get; set; }

        private void listv_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Button btn1 = new Button();
            btn1.Content = "C";
            btn1.Width = 20;
            btn1.Height = btn1.Width;
            Grid.SetColumn(btn1,2);
            Grid.SetRow(btn1, 3);

            G.Children.Add(btn1);

            btn1.Click += OpenText;
        }

        private void OpenText(object sender, RoutedEventArgs e)
        {
            TextBlock tb = new TextBlock();
            tb.Text = "sad";
            Grid.SetColumn(tb, 1);
            Grid.SetRow(tb, 3);

            
        }

        private void EnterKey(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                if (inBox.Text == "")
                    return;

                tbView.Text += inBox.Text + "\n";
                inBox.Text = String.Empty;
            }
        }
    }
}
