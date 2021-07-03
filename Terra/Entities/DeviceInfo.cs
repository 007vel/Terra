using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Entities
{
   public class DeviceInfo : INotifyPropertyChanged
    {
        public string request { get; set; }
        public string info { get; set; }

        string _value;
        public string value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
                this.NotifyPropertyChanged("value");
            }
        }

        int? days_left;
        public int? Days_left
        {
            get
            {
                return days_left;
            }
            set
            {
                days_left = value;
                this.NotifyPropertyChanged("Days_left");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
