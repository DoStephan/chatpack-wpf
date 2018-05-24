using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Wpf
{
    class User : IComparable<User>
    {
        private string _name;
        private string _tag;
        private BitmapImage _img;
        private string _messageSent = "";
        private string _messageReceive = "";
        private int _amountSent = 0;
        private int _amountReceive = 0;
        private DateTime _friendsSince;
        private double _currMessageAmount = 0;

        public User(string name, string tag)
        {
            _name = name;
            _tag = tag;
            _img = new BitmapImage(new Uri(@"C:\Schule\3Klasse\syp\repositories\chatpack-wpf\Wpf\ProfilePicture\default.png"));
        }
            public User(string name, string tag, string img):this(name, tag)
        {
            _name = name;
            _tag = tag;
            _img = new BitmapImage(new Uri(@"C:\Schule\3Klasse\syp\repositories\chatpack-wpf\Wpf\ProfilePicture\" + img));
        }//C:\Users\Stephan\Desktop\lsad\Wpf\ProfilePicture

        #region Prop
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }
        public BitmapImage Img
        {
            get
            {
                return _img;
            }

            set
            {
                _img = value;
            }
        }
        public string MessageSent
        {
            get
            {
                return _messageSent;
            }
            set
            {
                _messageSent += value;
            }
        }
        public string MessageReceive
        {
            get
            {
                return _messageReceive;
            }
            set
            {
                _messageReceive += value;
            }
        }
        public string Tag
        {
            get
            {
                return _tag;
            }

            set
            {
                _tag = value;
            }
        }
        public double CurrMessageAmount
        {
            get
            {
                return _currMessageAmount;
            }

            set
            {
                _currMessageAmount = value;
            }
        }
        public int AmountSent
        {
            get
            {
                return _amountSent;
            }
            set
            {
                _amountSent = value;
            }
        }
        public int AmountReceive
        {
            get
            {
                return _amountReceive;
            }
            set
            {
                _amountReceive = value;
            }
        }
        #endregion

        public int CompareTo(User other)
        {
            return this.Name.CompareTo(other.Name);
        }
        public override string ToString()
        {
            return this.Name;
        }
        
        public int GetTotalMessages()
        {
            int total;
            total = AmountReceive + AmountSent;

            return total;
        }
    }
}
