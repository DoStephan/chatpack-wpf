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
using System.Text.RegularExpressions;

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

            Regex reg = new Regex(@"\w+");

            if (reg.IsMatch(tb1.Text))
                tb2.Text = "Match";
            else
                tb2.Text = "no";
        }

        private void tb1_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Return)
            {
                Regex reg = new Regex(@"\w+");

                if (reg.IsMatch(tb1.Text))
                    tb2.Text = "Match";
                else
                    tb2.Text = "no";
            }
            
        }
    }
}
