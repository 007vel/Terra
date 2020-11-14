using ConnectionLibrary.Network;
using Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Plugin.CrossPlatformTintedImage.Abstractions;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
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
        DeviceDetailsViewModel context;
        Schedules schedules = new Schedules();
        
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
            schedules.scheduler = new List<Scheduler>();
            PageContext.Result += PageContext_Result1;
            ServiceProvider.Instance.SetBinding(this, typeof(DeviceDetailsViewModel));
            PageContext.Result += PageContext_Result;
            

            Grid grid = new Grid();
            grid.ColumnSpacing = 3;
            grid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Star });
            grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Star });
            grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Star });
            grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Star });
            grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Star });

            QuickAccessButton conf = new QuickAccessButton();
            conf.CardTitle = "Config";
            conf.CardDesc = "Spray";
            conf.IconSrc = "lock_button_24";
            conf.KeyBoardInputView = Keyboard.Text;
            CreateTapGesture(conf);
            Grid.SetColumn(conf,0);

            QuickAccessButton initSpray = new QuickAccessButton();
            initSpray.CardTitle = "Input Spray";
            initSpray.CardDesc = "0";
            initSpray.IconSrc = "lock_button_24";
            initSpray.KeyBoardInputView = Keyboard.Numeric;
            CreateTapGesture(initSpray);
            Grid.SetColumn(initSpray, 1);

            QuickAccessButton remainSpray = new QuickAccessButton();
            remainSpray.CardTitle = "rem_sprays";
            remainSpray.IconSrc = "lock_button_24";
            
            remainSpray.KeyBoardInputView = Keyboard.Numeric;
            CreateTapGesture(remainSpray);
            Grid.SetColumn(remainSpray, 2);

            QuickAccessButton dayCount = new QuickAccessButton();
            dayCount.CardTitle = "Days Left";
            dayCount.CardDesc="12";
            dayCount.IconSrc = "lock_button_24";
            dayCount.KeyBoardInputView = Keyboard.Numeric;
            CreateTapGesture(dayCount);
            Grid.SetColumn(dayCount, 3);

            grid.Children.Add(conf);
            grid.Children.Add(initSpray);
            grid.Children.Add(remainSpray);
            grid.Children.Add(dayCount);

            QuickAccess.Children.Add(grid);

            ScheduleView.Children.Add(GetAddButton());

            /// GetValues
            //var remSpray = context.GetRemSprayCount();
            //if(remSpray != null && remSpray.Result!=null)
            //{
            //    remainSpray.CardDesc = remSpray.Result.value;
            //}
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
           PageContext.OnInit();
        }
        private void GotoPopupInputpage1(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void PageContext_Result1(List<Entities.Scheduler> arg)
        {
            BuildScheduleUI(arg);
        }
        private void PageContext_Result(List<Entities.Scheduler> arg)
        {
            if(arg!=null)
            {
                foreach(var item in arg)
                {
                    BuildScheduleUI(arg);
                }
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
                    Schedule_6.editText = "edit";
                    Schedule_6.EditButtonClick += Schedule_1_EditButtonClick;
                }
            }
        }

        private Grid GetAddButton()
        {
            if(AddBtn == null)
            {
                AddBtn = new Grid();
                AddBtn.WidthRequest=40;
                AddBtn.HeightRequest = 40;
                ImageButton imageButton = new ImageButton();
                AddBtn.BackgroundColor = Color.Transparent;
                TintedImage tintedImage = new TintedImage();
                tintedImage.Source= ImageSource.FromFile("baseline_add_black_36");
                tintedImage.TintColor = Color.White;
                tintedImage.InputTransparent = true;
                tintedImage.Margin = new Thickness(8);
                imageButton.BackgroundColor = Color.Black;
                imageButton.CornerRadius = 1;
                
                AddBtn.Children.Add(imageButton);
                AddBtn.Children.Add(tintedImage);

                AddBtn.HorizontalOptions = LayoutOptions.EndAndExpand;
                AddBtn.VerticalOptions = LayoutOptions.StartAndExpand;
                AddBtn.Margin = new Thickness(10, 10, 15, 10);
                imageButton.Clicked += AddButtonClicked;
            }
            return AddBtn;
        }

        int countScheduleCount = 0;
        private void AddButtonClicked(object sender, EventArgs e)
        {
            CreateScheduleView(countScheduleCount);
            countScheduleCount++;
            if (countScheduleCount == 7)
            {
                AddBtn.IsVisible = false;
            }
        }
        private void CreateScheduleView(int index, Entities.Scheduler scheduler=null)
        {
            DayConfigControl Schedule_1 = new DayConfigControl(inputDate());
            Schedule_1.indexText = (index + 1).ToString();
            Schedule_1.editText = "edit";
            Schedule_1.EditButtonClick += Schedule_1_EditButtonClick;
            Schedule_1.DefaultUI = UIEnum.Schedul_ExpandView;
            ScheduleView.Children.Insert(index, Schedule_1);
            if (countScheduleCount == 7)
            {
                AddBtn.IsVisible = false;
            }
        }

        private List<UIDay> inputDate()
        {
            DateTime now = FirstDayOfWeek(DateTime.Now);
            List<UIDay> uIDays = new List<UIDay>();
            for (int i = 0; i < 7; i++)
            {
                UIDay uIDay = new UIDay();
                uIDay.day = now.ToString("ddd").Substring(0, 2);
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
            Schedule.EditButtonClick += Schedule_1_EditButtonClick;
            ScheduleView.Children.Insert(Convert.ToInt32(id)-1, Schedule);
            JObject jObject = new JObject();
            jObject.Add("id",id);
            jObject.Add("start", start.ToString());
            jObject.Add("end", stop.ToString());
            jObject.Add("interval", interval);
            var obj= JSONUtil.Build_Scheduler(uidays,start,stop,interval);
            schedules.scheduler.Add(obj);
            if (PageContext != null && obj!=null)
            {
                PageContext.DeviceService.SetScheduler(JsonConvert.SerializeObject(schedules));
            }
        }

        public static DateTime FirstDayOfWeek(DateTime dt)
        {
            var culture = System.Threading.Thread.CurrentThread.CurrentCulture;
            var diff = dt.DayOfWeek - culture.DateTimeFormat.FirstDayOfWeek;
            if (diff < 0)
                diff += 7;
            return dt.AddDays(-diff).Date;
        }
        async void OpenDialog()
        {
            await Navigation.PushPopupAsync(new ScheduleInputDialog());
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
