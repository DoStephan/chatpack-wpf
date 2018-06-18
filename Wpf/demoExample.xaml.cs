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
using System.IO;

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

            btn1.Click += ShowText;
            btn3.Click += ShowPopUp;
            btnmation.Click += AnimationBorder;

            string path1 = Directory.GetCurrentDirectory();
            path1 = Directory.GetParent(path1).ToString();
            path1 = Directory.GetParent(path1).ToString();

            //string filter = @"\Debug";


            tb2.Text = path1;
            

            //BitmapImage img = new BitmapImage(new Uri(@"\ProfilePicture\default.png", UriKind.Relative));
            //Img.Source = img;


            Thread i = new Thread(GetActiveWindowTitle);
            i.IsBackground = true;

            i.Start();
        }

        private void ShowText(object sender, RoutedEventArgs e)
        {
            tb.Visibility = Visibility.Visible;
            Button b = sender as Button;
            b.Click -= ShowText;
            b.Click += HideText;
        }

        private void HideText(object sender, RoutedEventArgs e)
        {

            tb.Visibility = Visibility.Hidden;
            Button b = sender as Button;
            b.Click += ShowText;
            b.Click -= HideText;
        }

        private void AnimationBorder(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;

            DoubleAnimation db = new DoubleAnimation();
            db.To = 150;
            db.Duration = TimeSpan.FromSeconds(0.5);
            db.AutoReverse = false;
            db.RepeatBehavior = new RepeatBehavior(1);
            bordermation.BeginAnimation(StackPanel.WidthProperty, db);

            btn.Click -= AnimationBorder;
            btn.Click += Reanimate;

        }

        private void Reanimate(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;

            DoubleAnimation db = new DoubleAnimation();
            db.To = 0;
            db.Duration = TimeSpan.FromSeconds(0.5);
            db.AutoReverse = false;
            db.RepeatBehavior = new RepeatBehavior(1);
            bordermation.BeginAnimation(StackPanel.WidthProperty, db);
            btn.Click -= Reanimate;
            btn.Click += AnimationBorder;

        }

        private void ShowPopUp(object sender, RoutedEventArgs e)
        {

            ppu.IsOpen = true;
        }

    
        private delegate void ChangeWin(string text);
        
        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();
        [DllImport("user32.dll")]
        static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);

        private void GetActiveWindowTitle()
        {

            const int nChars = 256;
            StringBuilder Buff = new StringBuilder(nChars);
            while (true)
            {
                IntPtr handle = GetForegroundWindow();
                if (GetWindowText(handle, Buff, nChars) > 0)
                {
                    ChangeWin dele = new ChangeWin(ChangeText);
                    Thread.Sleep(700);
                    //dele.BeginInvoke(Buff.ToString(), null, null);
                    actWind.Dispatcher.BeginInvoke(dele, Buff.ToString());
                    //Buff.ToString();//return Buff.ToString();


                }
            }
        }
        private void ChangeText(string text)
        {
            actWind.Text = text;
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
        private void OpenText(object sender, RoutedEventArgs e)
        {
            TextBlock tb = new TextBlock();
            tb.Text = "sad";
            Grid.SetColumn(tb, 1);
            Grid.SetRow(tb, 3);

            
        }
        private void ppu2_MouseLeave(object sender, MouseEventArgs e)
        {

            ppu.IsOpen = false;
        }
    }
}
