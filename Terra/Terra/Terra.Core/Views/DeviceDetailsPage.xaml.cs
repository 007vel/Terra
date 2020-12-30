using ConnectionLibrary.Network;
using Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Plugin.CrossPlatformTintedImage.Abstractions;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using System.Timers;
using Terra.Core.Controls;
using Terra.Core.Enum;
using Terra.Core.Models;
using Terra.Core.Utils;
using Terra.Core.ViewModels;
using Terra.Core.Views.PopUpPages;
using Terra.Service;
using Xamarin.Forms;

namespace Terra.Core.Views
{
    public partial class DeviceDetailsPage : ContentPage
    {
        Grid AddBtn = null;
        Button DelButton = null;
        DeviceDetailsViewModel context;
        Schedules scheduleList = new Schedules();

        QuickAccessButton conf = new QuickAccessButton();
        QuickAccessButton initSpray = new QuickAccessButton();
        QuickAccessButton remainSpray = new QuickAccessButton();
        QuickAccessButton dayCount = new QuickAccessButton();

        public delegate void TimeRuleDelegate(TimeSpan startTime,TimeSpan endTime, bool isStartTime);
        public DeviceDetailsViewModel PageContext
        {
            get
            {
                if(context==null)
                {
                    context = this.BindingContext as DeviceDetailsViewModel;
                }
                return context;
            }
        }
        public DeviceDetailsPage()
        {
            InitializeComponent();

            conf.NotifyValueChange += Conf_NotifyValueChange;
            initSpray.NotifyValueChange += InitSpray_NotifyValueChange;

            scheduleList.scheduler = new List<Scheduler>();
           // PageContext.Result += PageContext_Result1;
            ServiceProvider.Instance.SetBinding(this, typeof(DeviceDetailsViewModel));
            PageContext.Result += PageContext_Result;
            PageContext.DeviceInfoReceived += PageContext_DeviceInfoReceived;

            Grid grid = new Grid();
            grid.ColumnSpacing = 3;
            grid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Star });
            grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Star });
            grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Star });
            grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Star });
            grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Star });

            conf.CardTitle = "Config";
            conf.CardDesc = "Spray";
            conf.IconSrc = "lock_button_24";
            conf.KeyBoardInputView = Keyboard.Text;
            CreateTapGesture(conf);
            Grid.SetColumn(conf,0);

            initSpray.CardTitle = "Input Spray";
            initSpray.IconSrc = "lock_button_24";
            
            initSpray.KeyBoardInputView = Keyboard.Numeric;
            CreateTapGesture(initSpray);
            Grid.SetColumn(initSpray, 1);

            remainSpray.CardTitle = "rem_sprays";
            remainSpray.IconSrc = "lock_button_24";
            remainSpray.SetBinding(QuickAccessButton.CardDescProperty, "Teststr");
            remainSpray.KeyBoardInputView = Keyboard.Numeric;
            Grid.SetColumn(remainSpray, 2);

            dayCount.CardTitle = "Days Left";
            dayCount.CardDesc="1";
            dayCount.IconSrc = "lock_button_24";
            dayCount.KeyBoardInputView = Keyboard.Numeric;
            Grid.SetColumn(dayCount, 3);

            grid.Children.Add(conf);
            grid.Children.Add(initSpray);
            grid.Children.Add(remainSpray);
            grid.Children.Add(dayCount);

            QuickAccess.Children.Add(grid);
            ScheduleViewButtons.Children.Add(GetAddButton());
            ScheduleViewButtons.Children.Add(GetDeleteButton());
          

        }

        private void InitSpray_NotifyValueChange(string key, string val)
        {
            PageContext.SetInitilizeSprayCount(val);
            GetRemainSprayWithTimeDelay();
        }

        private async Task<bool> GetRemainSprayWithTimeDelay()
        {
            await Task.Delay(4000);
            PageContext.GetRemSprayCount();
            return true;
        }

        private void Conf_NotifyValueChange(string key, string val)
        {
            PageContext.SetDispanserType(val);
        }


        /// <summary>
        /// Paint values in UI from viewmodel
        /// </summary>
        /// <param name="deviceInfo"></param>
        private void PageContext_DeviceInfoReceived(DeviceInfo deviceInfo)
        {
            initSpray.CardDesc = PageContext?.InitializeSpray?.value!=null? PageContext?.InitializeSpray?.value: "3200";
            remainSpray.CardDesc = PageContext?.RemSpray?.value;
            batteryView.Chartvalue = Convert.ToInt32( PageContext?.Battery?.value);
            //_timerLabel.IsVisible = true;
            //_timerLabel.Text= DateTime.Now.ToString(CultureInfo.CurrentCulture);
            InitTimer(Convert.ToInt32(PageContext?.NextSprayCounter?.value));
            if (!string.IsNullOrEmpty(PageContext?.DaysLeft?.value))
            {
                dayCount.CardDesc = (Convert.ToInt32( PageContext.DaysLeft.value)/86400).ToString();
            }
            LoadingView.IsRunning = false;
            LoadingView.IsVisible = false;
        }
        int expireTime = 0;
        bool isTimerStarted = false;
        System.Timers.Timer timer = null;
        private void InitTimer(int totalSec)
        {
            expireTime = totalSec;
            if(!isTimerStarted)
            {
                SetTimer();
                isTimerStarted = true;
            }
        }
        private void SetTimer()
        {
            TimerLayout.IsVisible = true;
            timer = new System.Timers.Timer(1000);
            timer.Elapsed += OnTimedEvent;
            timer.AutoReset = true;
            timer.Enabled = true;
            timer.Start();
            
           // timer.Dispose();
        }
        int timerBegin = 0;
        private void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
             Device.BeginInvokeOnMainThread(() =>
                {
                    if (timerBegin != expireTime)
                    {
                        DateTime dateTime = DateTime.Now;
                        TimeSpan _TimeSpan = TimeSpan.FromSeconds(expireTime - timerBegin);
                        TimerValue.Text = (expireTime - timerBegin).ToString();
                        string seconds = "00";
                        if (_TimeSpan.Seconds < 10) {
                            seconds = Terra.Core.Utils.Utils.pad_an_int(_TimeSpan.Seconds, 2);
                        } else {
                            seconds = _TimeSpan.Seconds.ToString();
                        }
                        timerBegin = timerBegin + 1;
                        
                    }else
                    {
                        timer?.Stop();
                        isTimerStarted = false;
                    }

                });
                
            }
            
        
        protected override void OnAppearing()
        {
           base.OnAppearing();
           InitVm();
        }
        private async void InitVm()
        {
            Device.StartTimer(TimeSpan.FromMilliseconds(2000),()=>
            {
                PageContext.OnInit();
                return false;
            });
           
        }
        

        //private void PageContext_Result1(List<Entities.Scheduler> arg)
        //{
        //    BuildScheduleUI(arg);
        //}
        private void PageContext_Result(List<Entities.Scheduler> arg)
        {
            if(arg!=null)
            {
                int i = 1;
                foreach(var item in arg)
                {
                    item.index = i.ToString();
                    AddSchedule(item);
                    i++;
                }
                ButtonVisibleChange();
            }
        }

        private void BuildScheduleUI(List<Entities.Scheduler> arg)
        {
            if (arg != null)
            {
                for (int i=0; i<arg.Count; i++ )
                {
                    DayConfigControl Schedule_6 = new DayConfigControl(inputDate());
                    Schedule_6.indexText = (i+1).ToString();
                    Schedule_6.ID = (i + 1).ToString();
                    Schedule_6.editText = "edit";
                    Schedule_6.ScheduleReceived += Schedule_1_EditButtonClick;
                    Schedule_6.DeleteDelegate += SingleDeleteDelegate;
                }
            }
        }

        private void SingleDeleteDelegate(string id)
        {
            DeleteSchedule(id);
            ButtonVisibleChange();
        }
        private void DeleteSchedule(string id)
        {
            PageContext.DeleteScheduleItem(id);
            if (id == "-1")
            {
                scheduleList.scheduler.Clear();
            }
            else
            {
                scheduleList.scheduler.RemoveAt(Convert.ToInt32(id) - 1);
                int index = 1;
                foreach (var item in scheduleList.scheduler)
                {
                    item.index = index.ToString();
                    index++;
                }
            }
            //to a copy and send it to UI builder so dont impact in original item
            var neList = new List<Scheduler>(scheduleList.scheduler);
            scheduleList.scheduler.Clear();
            ScheduleView.Children.Clear();
            PageContext_Result(neList);
        }

        private Grid GetAddButton()
        {
            AddBtn = new Grid();
            AddBtn.WidthRequest = 40;
            AddBtn.HeightRequest = 40;
            ImageButton imageButton = new ImageButton();
            AddBtn.BackgroundColor = Color.Transparent;
            TintedImage tintedImage = new TintedImage();
            tintedImage.Source = ImageSource.FromFile("baseline_add_black_36");
            tintedImage.TintColor = Color.White;
            tintedImage.InputTransparent = true;
            tintedImage.Margin = new Thickness(8);
            imageButton.BackgroundColor = Color.Black;
            imageButton.CornerRadius = 3;

            AddBtn.Children.Add(imageButton);
            AddBtn.Children.Add(tintedImage);

            AddBtn.HorizontalOptions = LayoutOptions.EndAndExpand;
            AddBtn.VerticalOptions = LayoutOptions.StartAndExpand;
            AddBtn.Margin = new Thickness(10, 10, 15, 10);
            imageButton.Clicked += AddButtonClicked;
            return AddBtn;
        }

        private Button GetDeleteButton()
        {
            DelButton = new Button();
            DelButton.BackgroundColor = Color.FromHex("#EF4736");
            DelButton.CornerRadius = 3;
            DelButton.Text = "Delete All";
            DelButton.TextColor = Color.White;

            DelButton.HorizontalOptions = LayoutOptions.EndAndExpand;
            DelButton.VerticalOptions = LayoutOptions.StartAndExpand;
            DelButton.Margin = new Thickness(10, 10, 15, 10);
            DelButton.Clicked += DelAllButton_Clicked; ;
            return DelButton;
        }



        private void DelAllButton_Clicked(object sender, EventArgs e)
        {
            SingleDeleteDelegate("-1");
            ButtonVisibleChange();
        }
        
        private void AddButtonClicked(object sender, EventArgs e)
        {
            AddSchedule();
            ButtonVisibleChange();
        }

        
        private void AddSchedule(Entities.Scheduler scheduler = null)
        {
            CreateScheduleView(GetScheduleNewIndex(), scheduler);
          //  ButtonVisibleChange();
        }
        private void CreateScheduleView(int index, Entities.Scheduler scheduler=null)
        {
            DayConfigControl Schedule_UI = new DayConfigControl(inputDate(), scheduler);
            Schedule_UI.indexText = (index).ToString();
            Schedule_UI.editText = "edit";
            Schedule_UI.ScheduleReceived += Schedule_1_EditButtonClick;
            Schedule_UI.DefaultUI = UIEnum.Schedul_NormalView;
            Schedule_UI.DeleteDelegate += SingleDeleteDelegate;
            ScheduleView.Children.Insert(index-1, Schedule_UI);
           
            if (scheduler==null)
            {
                scheduler = new Scheduler();
                scheduler.index = index.ToString();
                scheduleList.scheduler.Add(scheduler);
            }
            else
            {
                scheduleList.scheduler.Add(scheduler);
            }
            
        }

        private void ButtonVisibleChange()
        {
            var i=GetScheduleNewIndex();
            AddBtn.IsVisible = i > 6 ? false : true;
            DelButton.IsVisible = i > 1 ? true : false;
        }

        private List<UIDay> inputDate()
        {
            DateTime now = FirstDayOfWeek(DateTime.Now).AddDays(1);
            List<UIDay> uIDays = new List<UIDay>();
            for (int i = 0; i < 7; i++)
            {
                UIDay uIDay = new UIDay();
                uIDay.day = now.ToString("ddd").Substring(0, 2).ToLower();
                uIDay.dateTime = now;
               // uIDay.selectionStatus = uIDays.Count % 2 == 0 ? SelectionStatus.NotSlected : SelectionStatus.Selected;
                uIDays.Add(uIDay);

                now = now.AddDays(1);
            }
            return uIDays;
        }
        private void Schedule_1_EditButtonClick(object arg, string id, TimeSpan start, TimeSpan stop, string interval)
        {
            
            
            ScheduleView.Children.RemoveAt(Convert.ToInt32( id)-1);
            var uidays = (List<UIDay>)arg;
            DayConfigControl Schedule = new DayConfigControl(uidays);
            Schedule.indexText = id;
            Schedule.editText = "edit";
            Schedule.intervalText = interval;
            Schedule.StartTimeText = start;
            Schedule.StopTimeText = stop;
            Schedule.ScheduleReceived += Schedule_1_EditButtonClick;
            ScheduleView.Children.Insert(Convert.ToInt32(id)-1, Schedule);
            JObject jObject = new JObject();
            jObject.Add("id",id);
            jObject.Add("start", start.ToString());
            jObject.Add("end", stop.ToString());
            jObject.Add("interval", interval);
            var obj= JSONUtil.Build_Scheduler(uidays,start,stop,interval);
            obj.index = id.ToString();


            scheduleList.scheduler[Convert.ToInt32(obj.index)-1]=obj;

            if (PageContext != null && obj!=null)
            {
                PageContext.deviceService.SetScheduler(JsonConvert.SerializeObject(scheduleList));
                PageContext.GetDaysLeftCount();
            }
        }
        /// <summary>
        /// it will return new index value for next element, ex: if the lsit count is 3 means, this methood will return 4
        /// </summary>
        /// <returns></returns>
        private int GetScheduleNewIndex()
        {
            int indx = 1;
            if (scheduleList.scheduler.Count > 0)
            {
                indx = Convert.ToInt32(scheduleList.scheduler[scheduleList.scheduler.Count-1].index);
                indx += 1;
            }
            return indx;
        }
        public static DateTime FirstDayOfWeek(DateTime dt)
        {
            var culture = System.Threading.Thread.CurrentThread.CurrentCulture;
            var diff = dt.DayOfWeek - culture.DateTimeFormat.FirstDayOfWeek;
            if (diff < 0)
                diff += 7;
            return dt.AddDays(-diff).Date;
        }
        private async void GotoPopupInputpage(object sender, EventArgs e)
        {
            if(sender!=null)
            {
                QuickAccessButton quickAccessButton = (QuickAccessButton)sender;
                var loadingPage = new DialogPopupPage(quickAccessButton);
                await Navigation.PushPopupAsync(loadingPage);
            }
        }
        private void CreateTapGesture(QuickAccessButton quickAccessButton)
        {
            TapGestureRecognizer tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.Tapped += GotoPopupInputpage;
            quickAccessButton.GestureRecognizers.Add(tapGestureRecognizer);
        }
    }
}
