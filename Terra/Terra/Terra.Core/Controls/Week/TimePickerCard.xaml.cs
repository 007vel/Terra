using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Terra.Core.Controls.Week
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TimePickerCard : Card
    {
        public TimePickerCard()
        {
            InitializeComponent();
        }
        public static BindableProperty CardTitleProperty = BindableProperty.Create(
                                                      propertyName: "CardTitle",
                                                      returnType: typeof(string),
                                                      declaringType: typeof(string),
                                                      defaultBindingMode: BindingMode.TwoWay,
                                                      propertyChanged: CardTitlePropertyChanged);

        public static void CardTitlePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable != null)
            {
                var _view = (TimePickerCard)bindable;
                _view.Titletext = (string)newValue;
            }
        }
        public string CardTitle
        {
            get { return (string)base.GetValue(CardTitleProperty); }
            set { base.SetValue(CardTitleProperty, value); }
        }
        public string Titletext
        {
            set
            {
                Title.Text = value;
                Title.FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label));
                OnPropertyChanged();
            }
        }


        public static BindableProperty CardDescProperty = BindableProperty.Create(
                                                    propertyName: "CardDesc",
                                                    returnType: typeof(string),
                                                    declaringType: typeof(string),
                                                    defaultBindingMode: BindingMode.TwoWay,
                                                    propertyChanged: CardDescPropertyChanged);

        public static void CardDescPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable != null)
            {
                var _view = (TimePickerCard)bindable;
                _view.DescText = (string)newValue;
            }
        }
        public string CardDesc
        {
            get { return (string)base.GetValue(CardDescProperty); }
            set { base.SetValue(CardDescProperty, value); }
        }
        public string DescText
        {
            set
            {
                Desc.Text = value;
                Desc.FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Label));
                OnPropertyChanged();
            }
        }
    }
}