﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Entities;
using Terra.Core.Controls.UIInterface;
using Terra.Core.Enum;
using Terra.Core.Helper;
using Terra.Core.Models;
using Terra.Core.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Terra.Core.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DayConfigControl : StackLayout
    {
        List<UIDay> uIDays;
        WeekControl weekControl;
        public delegate void ScheduleResult(object arg, string id, TimeSpan startTimeSpan, TimeSpan stopTimeSpan, string interval, bool isActive);
        public event ScheduleResult ScheduleReceived;

        public delegate void DeleteSchedule(string id);
        public event DeleteSchedule DeleteDelegate;
        INavigation navigation = null;
        IScheduleOperation scheduleOperation = null;
        Schedules rawSchedules;

        bool isEditMode =true;
        public DayConfigControl(List<UIDay> _uIDays, INavigation navigation,IScheduleOperation scheduleOperation, bool _isActive, Schedules schedules, Entities.Scheduler scheduler=null)
        {
            InitializeComponent();
            this.uIDays = _uIDays;
            IntervalList= BuildIntervalList();
            if(scheduler!=null)
            {
                SetIntervalValue(scheduler);
                SetStartAndStopValue(scheduler);
                SetDaysValue(scheduler);
                SetInterval();
                SetActiveStatusValue(scheduler);
            }
            this.scheduleOperation = scheduleOperation;
            weekControl = new WeekControl(weekFrameLayout);
            weekControl.DaysList = this.uIDays;
            weekFrameLayout.Children.Add(weekControl);
            WeekCardControl weekCardControl = new WeekCardControl();
            weekCardControl.DaysList = this.uIDays;
            weekexpand.Children.Add(weekCardControl);
            rawSchedules = schedules;

            this.navigation = navigation;
            BindingContext = this;
            if(scheduler==null)
            {
                isActive = _isActive;
                toggle.IsToggled  = _isActive;
            }

            toggle.Toggled += Toggle_Toggled;
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
        TimeSpan selectedStopTime;
        public TimeSpan SelectedStopTime
        {
            get
            {
                return selectedStopTime;
            }
            set
            {
                selectedStopTime = value;
                OnPropertyChanged("SelectedStopTime");
            }
        }
        TimeSpan selectedStartTime;
        public TimeSpan SelectedStartTime
        {
            get
            {
                return selectedStartTime;
            }
            set
            {
                selectedStartTime = value;
                OnPropertyChanged("SelectedStartTime");
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
        string id;
        public string ID
        {
            get
            {
                return id;
            }
            set
            {
                id = value;
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
               // index.Text = value;
               // index.FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label));
                OnPropertyChanged();
            }
        }

        public bool VisibleDeleteButton
        {
            set
            {
                deleteBtnLayout.IsVisible = value;
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
            //    editBtn.Text = value;
             //   editBtn.FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label));
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

        bool isActive;
        public bool IsActive
        {
            get
            {
                return isActive;
            }
            set
            {
                isActive = value;
                OnPropertyChanged("IsActive");
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
                }
                else
                {
                    ViewInvisible(expandView,0);
                    ViewVisible(schduleView,40);
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
        public static BindableProperty StopTimeTextProperty = BindableProperty.Create(
                                     propertyName: "StopTimeText",
                                     returnType: typeof(TimeSpan),
                                     declaringType: typeof(TimeSpan),
                                     defaultBindingMode: BindingMode.TwoWay,
                                     propertyChanged: StopTimeTextPropertyChanged);

        public static void StopTimeTextPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable != null)
            {
                var _view = (DayConfigControl)bindable;
                _view.ViewStopTimeText = (TimeSpan)newValue;
            }
        }
        public TimeSpan StopTimeText
        {
            get { return (TimeSpan)base.GetValue(StopTimeTextProperty); }
            set { base.SetValue(StopTimeTextProperty, value); }
        }
        public TimeSpan ViewStopTimeText
        {
            set
            {
                SelectedStopTime = value;
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

        private async void Edit_Tapped(object sender, EventArgs e)
        {
            Console.WriteLine("***************Edit_Tapped**************");
         //   return;
            if (isEditMode)
            {
                await navigation.PushAsync(new ConfigurationSettingPage(uIDays, scheduleOperation, indexText, SelectedStartTime, SelectedStopTime, SelectedIntervsl, isActive, rawSchedules));
                return;
                AnimationHelper.Instance.AnimationInvisible(schduleView, schduleView.HeightRequest);
                AnimationHelper.Instance.AnimationVisible(expandView, 125);
            }
            else
            {
                return;
                AnimationHelper.Instance.AnimationInvisible(expandView, expandView.HeightRequest);
                AnimationHelper.Instance.AnimationVisible(schduleView, 40);
                ScheduleReceived.Invoke(uIDays, indexText, new TimeSpan(SelectedStartTime.Ticks), new TimeSpan(SelectedStopTime.Ticks), SelectedIntervsl,isActive);
                SetInterval();
            }
        }
        public bool IsValidDateTime(TimeSpan startTimeSpan, TimeSpan endTimeSpan, bool isStartChanged)
        {
            if (startTimeSpan != null && endTimeSpan != null)
            {
                if (isStartChanged)
                {
                    if (TimeSpan.Compare(startTimeSpan, endTimeSpan) != -1)
                    {
                        App.Current.MainPage.DisplayAlert("Alert", "Start time must be less than end time", cancel: "Ok");
                        return false;
                    }
                }
                else
                {
                    if (TimeSpan.Compare(startTimeSpan, endTimeSpan) != -1)
                    {
                        App.Current.MainPage.DisplayAlert("Alert", "End time must be greater than start time", cancel: "Ok");
                        return false;
                    }   
                }
            }
            return true;
        }
        private void SetInterval()
        {
            string uiIntervsl = " / " + SelectedIntervsl + "'";
            DateTime time = DateTime.Today.Add(SelectedStartTime);
            timeLabel.Text = time.ToString("HH:mm") + System.Environment.NewLine + DateTime.Today.Add(SelectedStopTime).ToString("HH:mm");
            IntervalLabel.Text = uiIntervsl;
        }

        private void SetActiveStatusValue(Scheduler scheduler)
        {
            isActive = Convert.ToBoolean(scheduler?.active??"false");
            OnPropertyChanged("IsActive");
        }
         private ObservableCollection<string> BuildIntervalList()
        {
            ObservableCollection<string> list = new ObservableCollection<string>();
            for (int i=0; i<61; i++)
            {
                list.Add(i.ToString());
            }
            return list;
        }

        private void SetIntervalValue(Scheduler scheduler)
        {
            try
            {
                if(scheduler!=null)
                {
                    SelectedIntervsl = IntervalList.IndexOf(scheduler.interval.ToString()).ToString();
                }
            }
            catch(Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);
            }
        }
        private void SetStartAndStopValue(Scheduler scheduler)
        {
            try
            {
                if (scheduler != null)
                {
                    SelectedStartTime =  DateTime.Today.AddSeconds(Convert.ToDouble( scheduler.start)).TimeOfDay;
                    SelectedStopTime = DateTime.Today.AddSeconds(Convert.ToDouble(scheduler.stop)).TimeOfDay;
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);
            }
        }
        private void SetId(Scheduler scheduler)
        {
            try
            {
                if (scheduler != null)
                {
                    ID = scheduler.index;
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);
            }
        }
        private void SetDaysValue(Scheduler scheduler)
        {
            try
            {
                if (scheduler != null)
                {
                    foreach(var item in scheduler.day.Split(',').ToList())
                    {
                        this.uIDays.SingleOrDefault(i=>i.dateTime.ToString("dddd").ToLower()== item.ToLower()).selectionStatus=SelectionStatus.Selected;
                    }
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);
            }
        }

        void RemoveIconClicked(object sender, EventArgs e)
        {
            DeleteDelegate?.Invoke(this.indexText);
        }

        void SwipeView_PropertyChanged(System.Object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            Console.WriteLine("***********SwipeView_PropertyChanged*********"+e.PropertyName);
        }

        void Toggle_Toggled(System.Object sender, Xamarin.Forms.ToggledEventArgs e)
        {
            ScheduleReceived.Invoke(uIDays, indexText, new TimeSpan(SelectedStartTime.Ticks), new TimeSpan(SelectedStopTime.Ticks), SelectedIntervsl, e.Value);
        }

    }
}