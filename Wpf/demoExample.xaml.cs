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

            statsBtn.Click += ShowStats;    
           
            
            
            //adding series will update and animate the chart automatically
            /*SeriesCollection.Add(new ColumnSeries
            {
                Title = "2016",
                Values = new ChartValues<double> { 11, 56, 42 }
            });*/

            //also adding values updates and animates the chart automatically
            //SeriesCollection[1].Values.Add(48d);


            lv.SelectionChanged += Select;

            Button btn = new Button();
            btn.Content = "show Image";
            btn.Click += ShowImage;
            Grid.SetColumn(btn, 2);
            Grid.SetRow(btn, 3);


            G.Children.Add(btn);

        }

        private void ShowStats(object sender, RoutedEventArgs e)
        {
            SeriesCollection = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title = "2015",
                    Values = new ChartValues<double> { 10, 50, 39, 50 }
                }
            };


            Labels = new[] { "Monday", "Tuesday", "Wendesday", "Thursday", "Friday" };
            Formatter = value => value.ToString();

            DataContext = this;

        }

        private void ShowImage(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            if (fileDialog.ShowDialog() == true)
            {
                BitmapImage bitmap = new BitmapImage(new Uri(fileDialog.FileName));
                pic.Source = bitmap;
                
                //temp.ImageSource = new BitmapImage(new Uri("smittyWerbenJaggerManJensen.jpg", UriKind.Relative));

            }
        }
        private Border CreateRemoveBtn()
        {
            Border remBtnBdr = new Border();
            remBtnBdr.BorderThickness = new Thickness(12);
            Button remBtn = new Button();
            remBtnBdr.Child = remBtn;

            Grid.SetRow(remBtnBdr, 2);
            Grid.SetColumn(remBtnBdr, 0);
            return remBtnBdr;
        }
        public SeriesCollection SeriesCollection { get; set; }
        public string[] Labels { get; set; }
        public Func<double, string> Formatter { get; set; }

        private void Select(object sender, SelectionChangedEventArgs e)
        {
            tb.Text = "Selected";
            Border remBtnBdr = new Border();
            remBtnBdr = CreateRemoveBtn();
            G.Children.Add(remBtnBdr);
        }
        
        
    }
}
