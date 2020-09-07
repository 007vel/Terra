﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Terra.Core.Enum;
using Terra.Core.Helper;
using Terra.Core.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Terra.Core.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DayConfigControl : StackLayout
    {
        List<UIDay> uIDays;
        WeekControl weekControl;
        public delegate void ActionResult(object arg, string id, TimeSpan startTimeSpan, TimeSpan stopTimeSpan, string interval);
        public event ActionResult EditButtonClick;

        bool isEditMode;
        public DayConfigControl(List<UIDay> _uIDays)
        {
            InitializeComponent();
            this.uIDays = _uIDays;
           // weekFrameLayout.Margin = new Thickness(-10, -10, -10, -10);
            weekControl = new WeekControl(weekFrameLayout);
          //  weekFrameLayout.BackgroundColor = Color.Yellow;

            
            weekControl.DaysList = this.uIDays;
            weekFrameLayout.Children.Add ( weekControl);
            WeekCardControl weekCardControl = new WeekCardControl();
            weekCardControl.DaysList = this.uIDays;
            weekexpand.Children.Add(weekCardControl);
            BindingContext = this;
            IntervalList= BuildIntervalList();
        }
        public DayConfigControl()
        {
            InitializeComponent();
            weekControl = new WeekControl(weekFrameLayout);

            weekControl.DaysList = this.uIDays;
            weekFrameLayout.Children.Add(weekControl);
            WeekCardControl weekCardControl = new WeekCardControl();
            weekCardControl.DaysList = this.uIDays;
            weekexpand.Children.Add(weekCardControl);
            BindingContext = this;
            IntervalList = BuildIntervalList();
        }
        TimeSpan someDate;
        public TimeSpan SelectedStopTime
        {
            get
            {
                return someDate;
            }
            set
            {
                someDate = value;
                OnPropertyChanged();
            }
        }
        TimeSpan sometoDate;
        public TimeSpan SelectedStartTime
        {
            get
            {
                return sometoDate;
            }
            set
            {
                sometoDate = value;
                OnPropertyChanged();
            }
        }
        ObservableCollection<string> intervalList=new ObservableCollection<string>();
        public ObservableCollection<string> IntervalList
        {
            get
            {
                return intervalList;
            }
            set
            {
                intervalList = value;
                OnPropertyChanged();
            }
        }
        string selectedIntervsl;
        public string SelectedIntervsl
        {
            get
            {
                return selectedIntervsl;
            }
            set
            {
                selectedIntervsl = value;
                intervalLabel.CardDesc = value;
                OnPropertyChanged();
            }
        }
        public static BindableProperty indexTextProperty = BindableProperty.Create(
                                              propertyName: "indexText",
                                              returnType: typeof(string),
                                              declaringType: typeof(string),
                                              defaultBindingMode: BindingMode.TwoWay,
                                              propertyChanged: indexTextPropertyChanged);

        public static void indexTextPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable != null)
            {
                var _view = (DayConfigControl)bindable;
                _view.ViewIndexText = (string)newValue;
                
            }
        }
        public string indexText
        {
            get { return (string)base.GetValue(indexTextProperty); }
            set { base.SetValue(indexTextProperty, value); }
        }
        public string ViewIndexText
        {
            set
            {
                index.Text = value;
                index.FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label));
                OnPropertyChanged();
            }
        }



        public static BindableProperty editTextProperty = BindableProperty.Create(
                                              propertyName: "editText",
                                              returnType: typeof(string),
                                              declaringType: typeof(string),
                                              defaultBindingMode: BindingMode.TwoWay,
                                              propertyChanged: editTextPropertyChanged);

        public static void editTextPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable != null)
            {
                var _view = (DayConfigControl)bindable;
                _view.ViewEditText = (string)newValue;
            }
        }
        public string editText
        {
            get { return (string)base.GetValue(editTextProperty); }
            set { base.SetValue(editTextProperty, value); }
        }
        public string ViewEditText
        {
            set
            {
                editBtn.Text = value;
                editBtn.FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label));
                OnPropertyChanged();
            }
        }

        public static BindableProperty intervalTextProperty = BindableProperty.Create(
                                              propertyName: "intervalText",
                                              returnType: typeof(string),
                                              declaringType: typeof(string),
                                              defaultBindingMode: BindingMode.TwoWay,
                                              propertyChanged: intervalTextPropertyChanged);

        public static void intervalTextPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable != null)
            {
                var _view = (DayConfigControl)bindable;
                _view.ViewintervalText = (string)newValue;
            }
        }
        public string intervalText
        {
            get { return (string)base.GetValue(intervalTextProperty); }
            set { base.SetValue(intervalTextProperty, value); }
        }
        public string ViewintervalText
        {
            set
            {
                SelectedIntervsl = value;
                OnPropertyChanged();
            }
        }

        public static BindableProperty DefaultUIProperty = BindableProperty.Create(
                                              propertyName: "DefaultUI",
                                              returnType: typeof(UIEnum),
                                              declaringType: typeof(UIEnum),
                                              defaultBindingMode: BindingMode.TwoWay,
                                              propertyChanged: DefaultUIPropertyChanged);

        public static void DefaultUIPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable != null)
            {
                var _view = (DayConfigControl)bindable;
                _view.DefaultUIProp = (UIEnum)newValue;                
            }
        }

        public UIEnum DefaultUIProp
        {
            set
            {
                if (value == UIEnum.Schedul_ExpandView)
                {
                    ViewInvisible(schduleView, 0);
                    ViewVisible(expandView, 125);
                    isEditMode = true;
                }
                else
                {
                    ViewInvisible(expandView,0);
                    ViewVisible(schduleView,35);
                }
                OnPropertyChanged();
            }
        }
        public UIEnum DefaultUI
        {
            get { return (UIEnum)base.GetValue(DefaultUIProperty); }
            set { base.SetValue(DefaultUIProperty, value); }
        }

        public static BindableProperty StartTimeTextProperty = BindableProperty.Create(
                                      propertyName: "StartTimeText",
                                      returnType: typeof(TimeSpan),
                                      declaringType: typeof(TimeSpan),
                                      defaultBindingMode: BindingMode.TwoWay,
                                      propertyChanged: StartTimeTextPropertyChanged);

        public static void StartTimeTextPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable != null)
            {
                var _view = (DayConfigControl)bindable;
                _view.ViewStartTimeText = (TimeSpan)newValue;
            }
        }
        public TimeSpan StartTimeText
        {
            get { return (TimeSpan)base.GetValue(StartTimeTextProperty); }
            set { base.SetValue(StartTimeTextProperty, value); }
        }
        public TimeSpan ViewStartTimeText
        {
            set
            {
                SelectedStartTime = value;
                SetInterval();
                OnPropertyChanged();
            }
        }

        public async void ViewInvisible(StackLayout InputLayout, double startHeight)
        {
            InputLayout.HeightRequest = 0;
            InputLayout.FadeTo(0, 400);
        }
        public async void ViewVisible(StackLayout InputLayout, double startHeight)
        {
            InputLayout.HeightRequest = startHeight;
            InputLayout.FadeTo(1, 500);
        }

        private void Edit_Tapped(object sender, EventArgs e)
        {
            isEditMode = !isEditMode;
            if (isEditMode)
            {
                AnimationHelper.Instance.AnimationInvisible(schduleView, schduleView.HeightRequest);
                AnimationHelper.Instance.AnimationVisible(expandView, 125);
            }
            else
            {
                AnimationHelper.Instance.AnimationInvisible(expandView, expandView.HeightRequest);
                AnimationHelper.Instance.AnimationVisible(schduleView, 35);
                EditButtonClick.Invoke(uIDays, indexText, SelectedStartTime, SelectedStopTime, SelectedIntervsl);
                SetInterval();
            }
        }

        private void SetInterval()
        {
            string uiIntervsl = " / " + SelectedIntervsl + "'";
            DateTime time = DateTime.Today.Add(SelectedStartTime);
            startLabel.Text = time.ToString("HH:mm") + uiIntervsl;
        }
         private ObservableCollection<string> BuildIntervalList()
        {
            ObservableCollection<string> list = new ObservableCollection<string>();
            for (int i=1; i<61; i++)
            {
                list.Add(i.ToString());
            }
            return list;
        }
    }
}