﻿using System;
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

        private bool isInfoOn = false;

        private List<User> friendsList = new List<User>();
        private List<StackPanel> spList = new List<StackPanel>();

        private SolidColorBrush[] blueColors = new SolidColorBrush[3];
        private SolidColorBrush[] greyColors = new SolidColorBrush[3];
        string[] blueHex = new string[] { "#5978f2", "#3455d8", "#4286f4" };
        string[] grey = new string[] { "#597392", "#3a75d8", "#4ef6f4" };

        private Button profileBtn = new Button();
        private Button saveBtn = new Button();
        private Button cancalBtn = new Button();
        private String currName;
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
            
            this.FontSize = 16;

            InitColor(blueColors, blueHex);
            InitColor(greyColors, grey);

            SetBackgroundColor(blueColors);
           
            #region User profile
            ImageBrush myBrush = new ImageBrush();
            myBrush.ImageSource = new BitmapImage(new Uri("smittyWerbenJaggerManJensen.jpg",UriKind.Relative));
            myBrush.Stretch = Stretch.UniformToFill;        //@"C:\Users\Stephan\Desktop\lsad\Wpf\ProfilePicture\smittyWerbenJaggerManJensen.jpg"
            profPic.Fill = myBrush;
            profPic.Height = 60;
            profPic.Width = 60;
            #endregion

            popUpSetting.VerticalOffset = -btnSetting.ActualHeight;
            popUpSetting.HorizontalOffset = -btnSetting.ActualWidth;

            ReadFile("friends.txt");
            CreateSPItem();
            friendsView.ItemsSource = spList;
            
            addBtn.Click += TypeTagNumber;

            btnBlue.IsEnabled = false;
        }

        private void InitColor(SolidColorBrush[] br, string[] colors)
        {            
            Color c;
            SolidColorBrush scb;
            for (int i = 0; i < br.Length; i++)
            {
                c = new Color();
                c = (Color)ColorConverter.ConvertFromString(colors[i]);
                scb = new SolidColorBrush();
                scb.Color = c;
                br[i] = scb;
            }
        }

        private void SetBackgroundColor(SolidColorBrush[] color)
        {
            left_Grid.Background = color[0];
            center_Grid.Background = color[1];
            right_Grid.Background = color[2];
        }
        private void ChangeColor(object sender, RoutedEventArgs e)
        {
            if (sender.Equals(btnBlue))
                SetBackgroundColor(blueColors);
            else
                SetBackgroundColor(greyColors);
        
            btnBlue.IsEnabled = !btnBlue.IsEnabled;
            btnVio.IsEnabled = !btnVio.IsEnabled;
        }
        /// <summary>
        /// Create the friends for the list and the listview
        /// </summary>
        /// <param name="path"></param>
        /// <param name="spList"></param>
        private void CreateSPItem()
        {
            spList = new List<StackPanel>();
            TextBlock tb;
            StackPanel sp;

            for (int i = 0; i < friendsList.Count; i++)
            {
                //name
                tb = new TextBlock();
                tb.FontSize = 16;
                tb.VerticalAlignment = VerticalAlignment.Center;
                tb.Text = friendsList[i].Name;

                //picture
                Ellipse ellImg = new Ellipse();
                ImageBrush imgBrush = new ImageBrush();
                imgBrush.ImageSource = friendsList[i].Img;
                imgBrush.Stretch = Stretch.UniformToFill;
                ellImg.Fill = imgBrush;
                ellImg.Height = 56;
                ellImg.Width = 56;
                ellImg.Margin = new Thickness(10);

                //add to Stackpanel
                sp = new StackPanel();
                sp.Orientation = Orientation.Horizontal;
                sp.Children.Add(ellImg);
                sp.Children.Add(tb);

                spList.Add(sp);
            }
        }      
        /// <summary>
        /// Reads the files and sets the list
        /// </summary>
        /// <param name="filepath"></param>    
        public void ReadFile(string filepath)
        {
            string[] row = File.ReadAllLines(filepath);
            for (int i = 0; i < row.Length; i++)
            {
                string[] elem = row[i].Split(';');

                User friend;
                if (elem.Length == 2)
                     friend = new User(elem[0],elem[1]);

                else
                    friend = new User(elem[0], elem[1], elem[2]);
                
                friendsList.Add(friend);
            }
            friendsList.Sort();
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
        /// <summary>
        /// Send the message via "enter"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SendingMessage(object sender, RoutedEventArgs e)
        {
            SendMessage();
        }
        /// <summary>
        /// Key handle for enter/sending
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnKeyEnterHandler(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
                SendMessage();
        }
        public void SendMessage()
        {
            if (InputBox.Text == "")
                return;
            
            DateTime dateTime = DateTime.Now;
            for (int i = 0; i < friendsList.Count; i++)
            {
                if(friendsList[i].Name == currName)
                {
                    friendsList[i].MessageSent = dateTime.ToString("hh:mm    ")+InputBox.Text + "\n";
                    friendsList[i].CurrMessageAmount++;
                    break;
                }
            }
            //ShowInputBlock.Text += friendsList[i].Message;
            ShowInputBlock.Text += dateTime.ToString("hh:mm    ")+InputBox.Text +"\n";
            InputBox.Text = String.Empty;
            scrollView.ScrollToEnd();
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
            else
                btn.Click += Removefriend;

            return border;
    }
        /// <summary>
        /// Removes friend
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Removefriend(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < friendsList.Count; i++)
            {
                if (friendsList[i].Name == currName)
                {
                    friendsList.Remove(friendsList[i]);
                    break;
                }
            }
            CreateSPItem();            
            friendsView.ItemsSource = spList;
        }
        /// <summary>
        /// Shows the friend's name, image and 2 buttons
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void SelectFriend(object sender, SelectionChangedEventArgs e)
        {
            selFriendGrid.Children.Clear();
            StackPanel tempSP;
            StackPanel sp = new StackPanel();
            sp.Orientation = Orientation.Horizontal;

            //Create name
            TextBlock tb = new TextBlock();
            tb.FontSize = 16;
            tb.VerticalAlignment = VerticalAlignment.Center;

            //Create picture
            Ellipse el = new Ellipse();
            el.Width = 56;
            el.Height = 56;
            el.Margin = new Thickness(10);
            //read selected friend
            //save the select elem to tempSP
            tempSP = (StackPanel)friendsView.SelectedItem;
            if (tempSP == null)
            {    
                remStatGrid.Children.Clear();
                return;
            }
            //set "selectSP" with tempSP's data
            tb.Text = (tempSP.Children[1] as TextBlock).Text;
            el.Fill = (tempSP.Children[0] as Ellipse).Fill;

            //EEEE
            //change name to tag
            currName = tb.Text;
            ShowInputBlock.Clear();
            int tempIndex = 0;
            for (int i = 0; i < friendsList.Count; i++)
            {
                if (friendsList[i].Name == currName)
                    tempIndex = i;
            }
            //EEEE
            //Shows only sent message at the monment
            ShowInputBlock.Text = friendsList[tempIndex].MessageSent;

            sp.Children.Add(el);
            sp.Children.Add(tb);
            
            selFriendGrid.Children.Add(sp);

            //add buttons
            Border remBtnBdr = new Border();
            remBtnBdr = CreateCenterButton("Remove");

            Border statsBtnBdr = new Border();
            statsBtnBdr = CreateCenterButton("Stats");
            Grid.SetColumn(statsBtnBdr, 1);


            remStatGrid.Children.Add(remBtnBdr);
            remStatGrid.Children.Add(statsBtnBdr);
        }
        /// <summary>                 
        /// Shows the stats
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ShowStats(object sender, RoutedEventArgs e)
        {
            //change button's clickevent and name 
            Button btn = sender as Button;
            btn.Content = "Chat";

            btn.Click -= ShowStats;
            btn.Click += ShowChat;
            
            ShowInputBlock.Visibility = Visibility.Collapsed;

            GenerateStatusInfo();

            User u = GetCurrentFriend();

            SeriesCollection = new SeriesCollection
            {
                new ColumnSeries
                {
                    Values = new ChartValues<double> { u.CurrMessageAmount }//, 50, 39, 50, 35 }
                }
            };

            Labels = new[] { "Monday" };//, "Tuesday", "Wednesday", "Thursday", "Friday" };
            Formatter = value => value.ToString();

            DataContext = this;
        }

        private User GetCurrentFriend()
        {
            for (int i = 0; i < friendsList.Count; i++)
            {
                if (friendsList[i].Name == currName)
                {
                    return friendsList[i];
                }
            }
            return null;
        }

        /// <summary>
        /// Shows the chat-history
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ShowChat(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            btn.Content = "Stats";
            btn.Click -= ShowChat;
            btn.Click += ShowStats;
            ShowInputBlock.Visibility = Visibility.Visible;

            right_Grid.Children.Clear();

        }
        /// <summary>
        /// Create the information for the right corner 
        /// </summary>
        public void GenerateStatusInfo()
        {
            StackPanel s = new StackPanel();

            TextBlock[] tb = new TextBlock[4];

            for (int i = 0; i < tb.Length; i++)
            {
                tb[i] = new TextBlock();
            }
            for (int i = 0; i < friendsList.Count; i++)
            {
                if (friendsList[i].Name == currName)
                {
                    tb[0].Text = "Friends since: " + DateTime.Today;
                    tb[1].Text = "Messages sent: " + friendsList[i].CountMessagesSent();
                    tb[2].Text = "Messages received: " + friendsList[i].CountMessagesReceive();
                    tb[3].Text = "Total Messages: " + friendsList[i].GetTotalMessages();

                    break;
                }
                
            }
            for (int i = 0; i < tb.Length; i++)
            {
                s.Children.Add(tb[i]);
            }
            right_Grid.Children.Add(s);
        }
        /// <summary>
        /// Open setting popup
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Settings(object sender, RoutedEventArgs e)
        {      
            popUpSetting.IsOpen = !popUpSetting.IsOpen;
        }
        /// <summary>
        /// Set the information for the user e.g. username, ...
        /// </summary>
        public void SetInformation()
        {
            friendsView.Visibility = Visibility.Collapsed;
            Info.Background = new SolidColorBrush(Colors.White);
            tBoxEditName.Text = tBoxName.Text;
            lbTag.Content = "#1236";
            lbDate.Content = DateTime.Now.ToString();
            lbTotalFriends.Content = friendsList.Count;
        }
        /// <summary>
        /// Shows the user's information e.g. name, message amount, tag, etc
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 
        private void ShowUserInfo(object sender, RoutedEventArgs e)
        {
            if (!IsInfoOn)
            {
                SetInformation();
                IsInfoOn = true;
            }
            else
            {
                friendsView.Visibility = Visibility.Visible;
                DeleteButtonsInInfo();
                isInfoOn = false;
            }
        }
        /// <summary>
        /// Change the user information
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChangeInformation(object sender, RoutedEventArgs e)
        {
            if (!isInfoOn)
            {
                SetInformation();
                IsInfoOn = true;
            }
            popUpSetting.IsOpen = !popUpSetting.IsOpen;
            tBoxEditName.IsReadOnly = false;

            profileBtn.Width = 100;
            profileBtn.Height = 35;
            profileBtn.VerticalAlignment = VerticalAlignment.Center;
            profileBtn.Content = "Change Image";
            profileBtn.Click += OpenFileDiaForImg;
            Grid.SetRow(profileBtn, 4);
            Grid.SetColumnSpan(profileBtn, 2);

            saveBtn.Content = "save";
            saveBtn.Click += SaveInfo;
            Grid.SetRow(saveBtn,5);
            

            
            Info.Children.Add(profileBtn);
            Info.Children.Add(saveBtn);
        }
        /// <summary>
        /// Delete the button, which pop up by editing the infos
        /// </summary>
        public void DeleteButtonsInInfo()
        {
            while (Info.Children.Count > 8)
            {
                Info.Children.RemoveAt(8);
            }
        }
        /// <summary>
        /// Save the user's changes 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveInfo(object sender, RoutedEventArgs e)
        {
            tBoxName.Text = tBoxEditName.Text;
            tBoxEditName.IsReadOnly = true;
            DeleteButtonsInInfo();
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
        /// <summary>
        /// Unselect a friend by clicking somewhere else
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void friendsView_MouseDown(object sender, MouseButtonEventArgs e)
        {
            HitTestResult r = VisualTreeHelper.HitTest(this, e.GetPosition(this));
            if (r.VisualHit.GetType() != typeof(ListBoxItem))
            {
                friendsView.UnselectAll();
            }
                

        }

        private void btnSetting_MouseEnter(object sender, MouseEventArgs e)
        {
            ppuSetting.IsOpen = true;
        }

        private void btnSetting_MouseLeave(object sender, MouseEventArgs e)
        {
            ppuSetting.IsOpen = false;
        }
    }
}
