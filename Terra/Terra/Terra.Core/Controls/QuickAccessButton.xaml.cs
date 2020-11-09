using System;
using System.Collections.Generic;
using Terra.Core.common;
using Xamarin.Forms;

namespace Terra.Core.Controls
{
    public partial class QuickAccessButton : Frame, IDialog
    {
        public QuickAccessButton()
        {
            InitializeComponent();
            CornerRadius = 3;
            BorderColor = Color.Green;
            VerticalOptions = LayoutOptions.FillAndExpand;
            HorizontalOptions = LayoutOptions.FillAndExpand;
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
                var _view = (QuickAccessButton)bindable;
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
                var _view = (QuickAccessButton)bindable;
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

        public static BindableProperty CardIconProperty = BindableProperty.Create(
                                                 propertyName: "CardIcon",
                                                 returnType: typeof(string),
                                                 declaringType: typeof(string),
                                                 defaultBindingMode: BindingMode.TwoWay,
                                                 propertyChanged: CardIconPropertyChanged);

        public static void CardIconPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable != null)
            {
                var _view = (QuickAccessButton)bindable;
                _view.IconSrc = (string)newValue;
            }
        }

        public string getValue()
        {
            return CardDesc;
        }

        public void setValue(string val)
        {
            CardDesc = val;
        }

        public string getTitle()
        {
            return CardTitle;
        }

        public string CardIcon
        {
            get { return (string)base.GetValue(CardIconProperty); }
            set { base.SetValue(CardIconProperty, value); }
        }
        public string IconSrc
        {
            set
            {
                icon.Source = ImageSource.FromFile( value);
                OnPropertyChanged();
            }
        }

        public Keyboard KeyBoardInputView { set; get; }

        public Keyboard getkeyBoardType()
        {
            return KeyBoardInputView;
        }

    }
}
