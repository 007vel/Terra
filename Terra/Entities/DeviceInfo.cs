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

        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
