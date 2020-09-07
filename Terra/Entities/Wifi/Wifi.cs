using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.CompilerServices;
using Entities.Common;

namespace Entities.Wifi
{
    public class Wifi : BaseEntity, INotifyPropertyChanged
    {
       public string ssid { get; set; }
       public string password { get; set; }
       public string ipAdrs { get; set; }
       public bool isSelected { get; set; }

        string image;
        public string Image
        {
            set
            {
                image = value;
                onPropertyChanged();
            }
            get => image;
        }
        Color labelTextColor;
        public Color LabelTextColor
        {
            set
            {
                labelTextColor = value;
                onPropertyChanged();
            }
            get => labelTextColor;
        }
        public event PropertyChangedEventHandler PropertyChanged;
        void onPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
