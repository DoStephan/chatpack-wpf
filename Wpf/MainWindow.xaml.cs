using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.IO;
using Microsoft.Win32;
using LiveCharts;
using LiveCharts.Wpf;
using System.Media;

//using System.Data;

namespace Wpf
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Fields
        SoundPlayer plr = new SoundPlayer("oof.wav");

        private const int INFO_COLUMN = 6;

        private bool isInfoOn = false;
        private TextBlock[] infoUserBlock = new TextBlock[INFO_COLUMN];
        //später wichtig
        private List<User> friendlist = new List<User>();
        //
        private List<StackPanel> spList = new List<StackPanel>();
        private Color colorBlue = (Color)ColorConverter.ConvertFromString("#1d55af");
        private Color colorViolet = (Color)ColorConverter.ConvertFromString("#63085d");
        private SolidColorBrush bgColor = new SolidColorBrush();

        private StackPanel sp = new StackPanel();
        private Button profBtn = new Button();
        #endregion
        #region Propeties
        public bool IsInfoOn
        {
            get
            {
                return isInfoOn;
            }
            set
            {
                isInfoOn = value;
            }
        }
        public object SeriersCollection { get; private set; }
        public SeriesCollection SeriesCollection { get; set; }
        public string[] Labels { get; set; }
        public Func<object, object> Formatter { get; set; }
        #endregion
        public MainWindow()
        {
            InitializeComponent();
            
            this.FontFamily = new FontFamily("Comic Sans MS");

            bgColor.Color = colorBlue;
            center_Grid.Background = bgColor;

            ImageBrush myBrush = new ImageBrush();
            myBrush.ImageSource = new BitmapImage(new Uri("smittyWerbenJaggerManJensen.jpg",UriKind.Relative));
            myBrush.Stretch = Stretch.UniformToFill;        //@"C:\Users\Stephan\Desktop\lsad\Wpf\ProfilePicture\smittyWerbenJaggerManJensen.jpg"
            profPic.Fill = myBrush;
            profPic.Height = 60;
            profPic.Width = 60;

            popUpSetting.VerticalOffset = -btnSetting.ActualHeight;
            popUpSetting.HorizontalOffset = -btnSetting.ActualWidth;

            SetTextTitles();
            //FileRead("friends.txt");

            CreateSPItem("friends.txt", spList);
            friendsView.ItemsSource = spList;

            addBtn.Click += TypeTagNumber;

            btnBlue.IsEnabled = false;

            //
            sp = CreateUserInformation();
            //
        }
        /// <summary>
        /// Creates the two button remove and stats
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private Border CreateCenterButton(string text)
        {
            Border border = new Border();
            border.BorderThickness = new Thickness(12);
            Button btn = new Button();
            btn.Content = text;
            border.Child = btn;
            if (text == "Stats")
                btn.Click += ShowStats;

            return border;
        }
        /// <summary>
        /// Create the friends for the list and the listview
        /// </summary>
        /// <param name="path"></param>
        /// <param name="spList"></param>
        private void CreateSPItem(string path, List<StackPanel> spList)
        {
            StackPanel sp;
            TextBlock tb;

            string[] friendrow = File.ReadAllLines(path);
            for (int i = 0; i < friendrow.Length; i++)
            {
                string[] elem = friendrow[i].Split(';');
                User friend = new User(elem[0],elem[1]);

                sp = new StackPanel();
                sp.Orientation = Orientation.Horizontal;

                tb = new TextBlock();
                tb.FontSize = 16;
                tb.Text = friend.Name;
                tb.VerticalAlignment = VerticalAlignment.Center;

                Ellipse ellImg = new Ellipse();
                ImageBrush imgBrush= new ImageBrush();
                imgBrush.ImageSource = friend.Img;
                imgBrush.Stretch = Stretch.UniformToFill;
                ellImg.Fill = imgBrush;
                ellImg.Height = 56;
                ellImg.Width = 56;
                ellImg.Margin = new Thickness(10);

                
                sp.Children.Add(ellImg);
                sp.Children.Add(tb);
                
                spList.Add(sp);

            }
        }
        /// <summary>
        /// Opens tag popup for the tag-number input
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TypeTagNumber(object sender, RoutedEventArgs e)
        {
            if (popUpTag.IsOpen)
            {
                (sender as Button).Content = "+";
                (sender as Button).Width = 30;
            }
            else
            {
                (sender as Button).Content = "Cancel";
                (sender as Button).Width = 60;
            }
            popUpTag.IsOpen = !popUpTag.IsOpen;
        }
        //löschen
        public void FileRead(string filepath)
        {
            string[] friendRow = File.ReadAllLines(filepath);
            for (int i = 0; i < friendRow.Length; i++)
            {
                string[] elem = friendRow[i].Split(';');
                User friend = new User(elem[0],elem[1]);
                friendlist.Add(friend);
               
            }
            friendlist.Sort();
        }
        public Ellipse CreateEllipse(string imageName)
        {
            Ellipse pic = new Ellipse();
            ImageBrush myBrush = new ImageBrush();
            myBrush.ImageSource = new BitmapImage(new Uri(@"C:\Schule\3Klasse\syp\project\guiDemo\Wpf\ProfilePicture\" + imageName));
            myBrush.Stretch = Stretch.UniformToFill;
            pic.Height = 60;
            pic.Width = 60;

            pic.Fill = myBrush;

            return pic;
        }
        public StackPanel CreateUserInformation()
        {
            TextBox tb1 = new TextBox();
            tb1.Text = "Smitty";
            tb1.IsEnabled = false;
            //tb1.IsReadOnly = true;
            StackPanel sp1 = new StackPanel();
            sp1.Orientation = Orientation.Horizontal;
            sp1.Children.Add(infoUserBlock[0]);
            sp1.Children.Add(tb1);

            TextBlock tb2 = new TextBlock();
            tb2.Text = "#1841";
            StackPanel sp2 = new StackPanel();
            sp2.Orientation = Orientation.Horizontal;
            sp2.Children.Add(infoUserBlock[1]);
            sp2.Children.Add(tb2);

            TextBlock tb3 = new TextBlock();
            tb3.Text = "01.06.2017";
            StackPanel sp3 = new StackPanel();
            sp3.Orientation = Orientation.Horizontal;
            sp3.Children.Add(infoUserBlock[2]);
            sp3.Children.Add(tb3);

            StackPanel sp = new StackPanel();
            sp.Orientation = Orientation.Vertical;
            sp.Margin = new Thickness(0, 100, 0, 0);
            sp.Children.Add(sp1);
            sp.Children.Add(sp2);
            sp.Children.Add(sp3);

            return sp;
        }
        /// <summary>
        /// Shows the user's information e.g. name, message amount, tag, etc
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ShowUserInfo(object sender, RoutedEventArgs e)
        {

            friendsView.UnselectAll();
            if (!IsInfoOn)
            {
                //Info.Children.Clear();
                friendsView.ItemsSource = null;
                friendsView.Background = null;
                Info.Children.Add(sp);
                Info.Background = new SolidColorBrush(Colors.White);
                IsInfoOn = true;
            }
            else
            {
                friendsView.ItemsSource = spList;
                Info.Children.Clear();
                Info.Background = new SolidColorBrush();
                isInfoOn = false;
            }
        }
        public void SetTextTitles()
        {
            for (int i = 0; i < INFO_COLUMN; i++)
            {
                infoUserBlock[i] = new TextBlock();
            }
            infoUserBlock[0].Text = "Username: ";
            infoUserBlock[1].Text = "Tag-Number: ";
            infoUserBlock[2].Text = "Created since: ";
            infoUserBlock[3].Text = "Friends amount: ";
            infoUserBlock[4].Text = "Total messages sent: ";
            infoUserBlock[5].Text = "Total messages received:";
        }
        private void SendBtn(object sender, RoutedEventArgs e)
        {
            if (InputBox.Text == "")
                return;

            DateTime dateTime = DateTime.Now;
            
            ShowInputBlock.Text += dateTime.ToString("hh:mm    ") + InputBox.Text + "\n";
            InputBox.Text = String.Empty;

        }
        /// <summary>
        /// Key handle for enter/sending
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnKeyEnterHandler(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                if (InputBox.Text == "")
                    return;

                DateTime dateTime = DateTime.Now;
                ShowInputBlock.Text += dateTime.ToString("hh:mm    ") + InputBox.Text + "\n";
                InputBox.Text = String.Empty;
            }
        }
        /// <summary>
        /// Shows the friend's name and image with buttons
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void SelectFriend(object sender, SelectionChangedEventArgs e)
        {
            selFriendGrid.Children.Clear();
            StackPanel tempSP;
            StackPanel sp = new StackPanel();
            sp.Orientation = Orientation.Horizontal;

            TextBlock tb = new TextBlock();
            tb.FontSize = 16;
            tb.VerticalAlignment = VerticalAlignment.Center;

            Ellipse el = new Ellipse();
            el.Width = 56;
            el.Height = 56;
            el.Margin = new Thickness(10);

            Border remBtnBdr = new Border();
            remBtnBdr = CreateCenterButton("Remove");
            

            Border statsBtnBdr = new Border();
            statsBtnBdr = CreateCenterButton("Stats");
            Grid.SetColumn(statsBtnBdr, 1);

            tempSP = (StackPanel)friendsView.SelectedItem;
            if (tempSP == null)
            {    
                remStatGrid.Children.Clear();
                return;
            }
            tb.Text = (tempSP.Children[1] as TextBlock).Text;
            el.Fill = (tempSP.Children[0] as Ellipse).Fill;

            sp.Children.Add(el);
            sp.Children.Add(tb);
            
            selFriendGrid.Children.Add(sp);

            remStatGrid.Children.Add(remBtnBdr);
            remStatGrid.Children.Add(statsBtnBdr);
        }
        /// <summary>
        /// Open setting popup
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Settings(object sender, RoutedEventArgs e)
        {
          
            popUpSetting.IsOpen = !popUpSetting.IsOpen;

            //plr.Load();
            //plr.Play();

        }
        /// <summary>
        /// Shows the stats
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ShowStats(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            btn.Content = "Chat";

            btn.Click -= ShowStats;
            btn.Click += ShowChat;

            //ShowInputBlock = null;

            SeriesCollection = new SeriesCollection
            {
                new ColumnSeries
                {
                    Values = new ChartValues<double> { 10, 50, 39, 50, 35 }
                }
            };

            Labels = new[] { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday" };
            Formatter = value => value.ToString();

            DataContext = this;

            //plr.Load();
            //plr.Play();
        }
        /// <summary>
        /// Shows the chat-history
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ShowChat(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            btn.Content = "ShowStats";
            btn.Click -= ShowChat;
            btn.Click += ShowStats;
            //ShowInputBlock.Background = new SolidColorBrush(Colors.White);
        }

        /// <summary>
        /// Open the user's information for editing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChangeInformation(object sender, RoutedEventArgs e)
        {
            //plr.Load();
            //plr.Play();

            popUpSetting.IsOpen = !popUpSetting.IsOpen;
            Info.Children.Clear();
            Info.Background = new SolidColorBrush(Colors.White);
            IsInfoOn = true; ;
           
            profBtn.Width = 100;
            profBtn.Height = 70;
            profBtn.VerticalAlignment = VerticalAlignment.Center;
            profBtn.Content = "Change Image";
            profBtn.Click += OpenFileDiaForImg;
            
            Info.Children.Add(sp);
            Info.Children.Add(profBtn);

        }
        /// <summary>
        /// Open a filedialog for changing the image
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenFileDiaForImg(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDia = new OpenFileDialog();

            fileDia.Filter = "Images (*.png, *.jpg)|*.png; *jpg";
            if (fileDia.ShowDialog() == true)
            {
                ImageBrush temp = new ImageBrush();
                temp.ImageSource = new BitmapImage(new Uri(fileDia.FileName));
                profPic.Fill = temp;
            }
        }

        private void ChangeColor(object sender, RoutedEventArgs e)
        {
            if (sender.Equals(btnBlue))
                bgColor.Color = colorBlue;
            else
                bgColor.Color = colorViolet;

            btnBlue.IsEnabled = !btnBlue.IsEnabled;
            btnVio.IsEnabled = !btnVio.IsEnabled;

            center_Grid.Background = bgColor;
        }
        /// <summary>
        /// Unselect a friend by click somewhere else
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void friendsView_MouseDown(object sender, MouseButtonEventArgs e)
        {
            HitTestResult r = VisualTreeHelper.HitTest(this, e.GetPosition(this));
            if (r.VisualHit.GetType() != typeof(ListBoxItem))
                friendsView.UnselectAll();
        }

       
    }
}
