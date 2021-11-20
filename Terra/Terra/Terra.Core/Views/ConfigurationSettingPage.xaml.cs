using System;
using System.Collections.Generic;
using Terra.Core.Controls;
using Terra.Core.Controls.UIInterface;
using Terra.Core.Models;
using Xamarin.Forms;
using System.Linq;
using Entities;
using Terra.Core.Utils;

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

        Schedules schedulesRawList;
        TimeSpan rawStartTimeSpan;
        TimeSpan rawStopTimeSpan;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="uIDays"></param>
        /// <param name="scheduleOperation"></param>
        /// <param name="id"></param>
        /// <param name="startTimeSpan"></param>
        /// <param name="stopTimeSpan"></param>
        /// <param name="interval"></param>
        /// <param name="active"></param>
        /// <param name="schedules">Entier schedule list(original)</param>
        public ConfigurationSettingPage(List<UIDay> uIDays, IScheduleOperation scheduleOperation, string id, TimeSpan startTimeSpan, TimeSpan stopTimeSpan, string interval, bool active, Schedules schedules)
        {
            InitializeComponent();
            BindingContext = this;
            this.uIDays = uIDays;
            this.scheduleOperation = scheduleOperation;
            this.id = id;
            SelectedStartTime = rawStartTimeSpan = startTimeSpan;
            SelectedStopTime = rawStopTimeSpan = stopTimeSpan;
            this.interval = interval;
            IsActive = active;
            Interval = Convert.ToInt32( interval);
            BuildDayView(uIDays);
            schedulesRawList = schedules;
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
      
       async void SaveToolbarItem_Clicked(System.Object sender, System.EventArgs e)
        {
            if(!IsValidStartAndStopTime())
            {
                await App.Current.MainPage.DisplayAlert("Alert", "End time must be greater than start time", cancel: "Ok");
                return;
            }
            if (Interval<1)
            {
                await App.Current.MainPage.DisplayAlert("Alert", "Interval must be greater than 1", cancel: "Ok");
                return;
            }
            if (!IsValidDays())
            {
                await App.Current.MainPage.DisplayAlert("Alert", "Please select days for schedule", cancel: "Ok");
                return;
            }
            if (DoesOverlappingWithExistingSchedule())
            {
                await App.Current.MainPage.DisplayAlert("Alert", "The Day and Time is overlapping with existing schedule", cancel: "Ok");
                return;
            }

            scheduleOperation?.ReceiveEditedORNewschedule(uIDays, id, new TimeSpan(selectedStartTime.Ticks), new TimeSpan(selectedStopTime.Ticks), Interval.ToString(), isActive);
            await Navigation.PopAsync();
        }

        private bool IsValidStartAndStopTime()
        {
            return selectedStartTime.Ticks < selectedStopTime.Ticks;
        }

        private bool IsValidDays()
        {
           return uIDays.Where(i=>i.selectionStatus==Enum.SelectionStatus.Selected).Count() > 0;
        }

        private bool DoesOverlappingWithExistingSchedule()
        {
            if(schedulesRawList!=null && schedulesRawList.scheduler!=null && uIDays!=null)
            {
                bool isFirstSchedule = schedulesRawList.scheduler.Count==1 && schedulesRawList.scheduler[0].day == null;

                if (isFirstSchedule) return false;

                foreach (var schdl in schedulesRawList.scheduler)
                {
                    if(schdl.day!=null && schdl.index!=id)
                    {
                        var parsedDays = schdl.day.Split(',').ToList();//now.ToString("ddd").Substring(0, 2).ToLower();
                        var selectedUIDays = uIDays.Where(i => i.selectionStatus == Enum.SelectionStatus.Selected).ToList();

                        var commonDays = parsedDays.Select(i => i.Substring(0, 2).ToLower()).Intersect(selectedUIDays.Select(j => j.day)).ToList();
                        if (commonDays != null && commonDays.Count > 0)
                        {
                            var daysOverlap = JSONUtil.timeSpantoSeconds(SelectedStartTime) <= schdl.stop && schdl.stop <= JSONUtil.timeSpantoSeconds(SelectedStopTime);
                            if(daysOverlap)
                            {
                                return daysOverlap;
                            }

                        }
                    }
                    
                }
            }
            return false;
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
