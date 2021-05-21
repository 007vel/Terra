using System;
using System.Collections.Generic;
using Terra.Core.Controls;
using Terra.Core.Controls.UIInterface;
using Terra.Core.Models;
using Xamarin.Forms;

namespace Terra.Core.Views
{
    public partial class ConfigurationSettingPage : ContentPage
    {
        IScheduleOperation scheduleOperation;
        List<UIDay> uIDays = null;
        string id;
       // TimeSpan startTimeSpan;
       // TimeSpan stopTimeSpan;
        string interval;
        public ConfigurationSettingPage(List<UIDay> uIDays, IScheduleOperation scheduleOperation, string id, TimeSpan startTimeSpan, TimeSpan stopTimeSpan, string interval, bool active)
        {
            InitializeComponent();
            BindingContext = this;
            this.uIDays = uIDays;
            this.scheduleOperation = scheduleOperation;
            this.id = id;
            SelectedStartTime = startTimeSpan;
            SelectedStopTime = stopTimeSpan;
            this.interval = interval;
            IsActive = active;
            Interval = Convert.ToInt32( interval);
            BuildDayView(uIDays);
        }

        int _interval;
        public int Interval
        {
            get
            {
                return _interval;
            }
            set
            {
                _interval = value;
                OnPropertyChanged();
            }
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
                OnPropertyChanged();
            }
        }
        TimeSpan selectedStartTime = new TimeSpan(minutes: 0, seconds: 0, hours: 1);
        public TimeSpan SelectedStartTime
        {
            get
            {
                return selectedStartTime;
            }
            set
            {
                selectedStartTime = value;
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
                OnPropertyChanged();
            }
        }

        private void BuildDayView(List<UIDay> uIDays)
        {
            this.uIDays = new List<UIDay>( uIDays);
            WeekCardControl weekCardControl = new WeekCardControl();
            weekCardControl.DaysList = uIDays;
            DayConfig.Children.Add(weekCardControl);
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
                    //-1 means t1 is shorter than t2. 1 means t1 is longer than t2. 0 means	t1 is equal to t2.
                    if (TimeSpan.Compare(startTimeSpan, endTimeSpan) != -1)
                    {
                        App.Current.MainPage.DisplayAlert("Alert", "End time must be greater than start time", cancel: "Ok");
                        return false;
                    }
                }
            }
            return true;
        }
        void ToolbarItem_Clicked(System.Object sender, System.EventArgs e)
        {
            scheduleOperation?.Schedule_1_EditButtonClick(uIDays, id, new TimeSpan(selectedStartTime.Ticks), new TimeSpan(selectedStopTime.Ticks), Interval.ToString(), isActive);
            Navigation.PopAsync();
        }

        void ButtonIncrease_Clicked(System.Object sender, System.EventArgs e)
        {
            Interval = Interval + 1;
        }

        void ButtonDeccrease_Clicked(System.Object sender, System.EventArgs e)
        {
            if (Interval < 2) return;

            Interval = Interval - 1;
        }
    }
}
