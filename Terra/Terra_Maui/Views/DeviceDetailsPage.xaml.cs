
using Acr.UserDialogs;
using CommunityToolkit.Maui.Behaviors;
using Entities;
using Mopups.Services;
using Newtonsoft.Json;
using System.Timers;
using Terra_Maui.Controls;
using Terra_Maui.Controls.UIInterface;
using Terra_Maui.Enum;
using Terra_Maui.Helper;
using Terra_Maui.Models;
using Terra_Maui.Utils;
using Terra_Maui.ViewModels;
using Terra_Maui.Views.PopUpPages;
using DeviceInfo = Entities.DeviceInfo;

namespace Terra_Maui.Views;

public partial class DeviceDetailsPage : ContentPage, IScheduleOperation
{
    Grid AddBtn = null;
    Button DelButton = null;
    DeviceDetailsViewModel context;
    Schedules scheduleList = new Schedules();

    QuickAccessButton conf = new QuickAccessButton();
    QuickAccessButton initSpray = new QuickAccessButton();
    QuickAccessButton remainSpray = new QuickAccessButton();
    QuickAccessButton dayCount = new QuickAccessButton();

    bool isPageInitilized = false;

    public delegate void TimeRuleDelegate(TimeSpan startTime, TimeSpan endTime, bool isStartTime);
    public DeviceDetailsViewModel PageContext
    {
        get
        {
            if (context == null)
            {
                context = this.BindingContext as DeviceDetailsViewModel;
            }
            return context;
        }
    }
    public DeviceDetailsPage()
    {
        InitializeComponent();
        isPageInitilized = true;
        conf.NotifyValueChange += Conf_NotifyValueChange;
        initSpray.NotifyValueChange += InitSpray_NotifyValueChange;

        scheduleList.scheduler = new List<Scheduler>();

        Terra_Maui.Service.ServiceProvider.Instance.SetBinding(this, typeof(DeviceDetailsViewModel));
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
        Grid.SetColumn(conf, 0);

        initSpray.CardTitle = "Count";
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
        dayCount.CardDesc = "1";
        dayCount.IconSrc = "lock_button_24";
        dayCount.KeyBoardInputView = Keyboard.Numeric;
        Grid.SetColumn(dayCount, 3);

        grid.Children.Add(conf);
        grid.Children.Add(initSpray);
        grid.Children.Add(remainSpray);
        grid.Children.Add(dayCount);

        QuickAccess.Children.Add(grid);
        ScheduleViewButtons.Children.Add(GetSingleDeleteButton());
        ScheduleViewButtons.Children.Add(GetDeleteButton());
        ScheduleViewButtons.Children.Add(GetAddButton());

    }

    private async void InitSpray_NotifyValueChange(string key, string val)
    {
        await PageContext.SetInitilizeSprayCount(val);

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
        initSpray.CardDesc = PageContext?.InitializeSpray?.value != null ? PageContext?.InitializeSpray?.value : "3200";
        remainSpray.CardDesc = PageContext?.RemSpray?.value;
        batteryView.Chartvalue = Convert.ToInt32(Convert.ToDouble(PageContext?.Battery?.value));
        //_timerLabel.IsVisible = true;
        //_timerLabel.Text= DateTime.Now.ToString(CultureInfo.CurrentCulture);
        InitTimer(Convert.ToInt32(PageContext?.NextSprayCounter?.value));
        if (!string.IsNullOrEmpty(PageContext?.DaysLeft?.value))
        {
            dayCount.CardDesc = (Convert.ToInt32(PageContext.DaysLeft.value) / 1).ToString();
        }
        else if (PageContext?.DaysLeft?.Days_left != null)
        {
            dayCount.CardDesc = (Convert.ToInt32(PageContext.DaysLeft.Days_left) / 1).ToString();
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
        if (!isTimerStarted)
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
                if (_TimeSpan.Seconds < 10)
                {
                    seconds = Utils.Utils.pad_an_int(_TimeSpan.Seconds, 2);
                }
                else
                {
                    seconds = _TimeSpan.Seconds.ToString();
                }
                timerBegin = timerBegin + 1;

            }
            else
            {
                timer?.Stop();
                isTimerStarted = false;
            }

        });

    }


    protected override void OnAppearing()
    {
        base.OnAppearing();
        OTAHelper.Instance.EnableHeartBeat = true;
        if (isPageInitilized)
        {
            isPageInitilized = false;
            InitVm();
        }
        //  SocketHelper.Instance.StartHeartBeatcheck(PageContext.deviceService);
    }
    private async void InitVm()
    {
        LoadingView.IsRunning = true;
        LoadingView.IsVisible = true;
        LoadingView.IsEnabled = true;
        Device.StartTimer(TimeSpan.FromMilliseconds(1000), () =>
        {
            LoadingView.IsRunning = true;
            LoadingView.IsVisible = true;
            LoadingView.IsEnabled = true;
            PageContext.OnInit();
            return false;
        });

    }


    private void PageContext_Result(List<Entities.Scheduler> arg)
    {
        try
        {
            if (arg != null)
            {
                int i = 1;
                foreach (var item in arg)
                {
                    item.index = i.ToString();
                    AddSchedule(item);
                    i++;
                }
                ButtonVisibleChange();
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(ex.ToString());  
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
        //to a copy and send it to UI builder so doesn't impact in original item
        var neList = new List<Scheduler>(scheduleList.scheduler);
        scheduleList.scheduler.Clear();
        ScheduleView.Children.Clear();
        PageContext_Result(neList);
    }

    private Grid GetAddButton()
    {
        AddBtn = new Grid
        {
            WidthRequest = 50,
            HeightRequest = 50,
            BackgroundColor = Colors.Transparent,
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center,
            Margin = new Thickness(10, 10, 15, 10)
        };

        var imageButton = new ImageButton
        {
            Source = "baseline_add_black_36.png",
            BackgroundColor = Colors.Black,

            WidthRequest = 50,
            HeightRequest = 50,
            CornerRadius = 3,
            Padding = 8,
            Aspect = Aspect.AspectFit,
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center
        };
        var behavior = new IconTintColorBehavior
        {
            TintColor = Colors.White
        };

        imageButton.Behaviors.Add(behavior);
        imageButton.Clicked += AddButtonClicked;
        AddBtn.Children.Add(imageButton);
        return AddBtn;
    }

    private Grid GetSingleDeleteButton()
    {
        var singleDeleteBtn = new Grid
        {
            WidthRequest = 50,
            HeightRequest = 50,
            BackgroundColor = Colors.Transparent,
            HorizontalOptions = LayoutOptions.CenterAndExpand,
            VerticalOptions = LayoutOptions.Center,
            Margin = new Thickness(10, 10, 15, 10)
        };

        var imageButton = new ImageButton
        {
            Source = "baseline_remove_black_24.png",
            BackgroundColor = Colors.Black,
            CornerRadius = 3,
            WidthRequest = 50,
            HeightRequest = 50,
            Padding = 8,
            Aspect = Aspect.AspectFit,
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center
        };
        var behavior = new IconTintColorBehavior
        {
            TintColor = Colors.White
        };

        imageButton.Behaviors.Add(behavior);
        imageButton.Clicked += SingleDeleteButtonClicked;
        singleDeleteBtn.Children.Add(imageButton);
        return singleDeleteBtn;
    }

    private Button GetDeleteButton()
    {
        DelButton = new Button();
        DelButton.BackgroundColor = Color.FromHex("#EF4736");
        DelButton.CornerRadius = 3;
        DelButton.Text = "DELETE ALL";
        DelButton.TextColor = Colors.White;

        DelButton.HorizontalOptions = LayoutOptions.Center;
        DelButton.VerticalOptions = LayoutOptions.Center;
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
        EnableDisableDeleteButtonInRow(false);
        AddSchedule();
        ButtonVisibleChange();
    }
    private void SingleDeleteButtonClicked(object sender, EventArgs e)
    {
        EnableDisableDeleteButtonInRow(true);
    }

    private void EnableDisableDeleteButtonInRow(bool val)
    {
        var DayConfigRows = ScheduleView.Children;
        if (DayConfigRows == null) return;
        foreach (var vi in DayConfigRows)
        {
            var rawView = (DayConfigControl)vi;
            if (rawView != null)
            {
                rawView.VisibleDeleteButton = val;
            }
        }
    }

    private void AddSchedule(Entities.Scheduler scheduler = null)
    {
        CreateScheduleView(GetScheduleNewIndex(), scheduler);
    }


    private async void CreateScheduleView(int index, Entities.Scheduler scheduler = null)
    {
        try
        {
            var Schedule_UI = new DayConfigControl(inputDate(), Navigation, this, true, scheduleList, scheduler)
            {
                indexText = index.ToString(),
                editText = "edit",
                DefaultUI = UIEnum.Schedul_NormalView
            };

            Schedule_UI.ScheduleReceived += ReceiveEditedORNewschedule;
            Schedule_UI.DeleteDelegate += SingleDeleteDelegate;

            var gestureRecognizer = new TapGestureRecognizer();
            gestureRecognizer.Tapped += GestureRecognizer_Tapped;
            Schedule_UI.GestureRecognizers.Add(gestureRecognizer);

            if (scheduler == null)
            {
                scheduler = new Scheduler { index = index.ToString() };
                scheduleList.scheduler.Add(scheduler);
                await Navigation.PushAsync(new ConfigurationSettingPage(
                    inputDate(),
                    this,
                    Schedule_UI.indexText,
                    Schedule_UI.SelectedStartTime,
                    Schedule_UI.SelectedStopTime,
                    "5",
                    true,
                    scheduleList));
            }
            else
            {
                scheduleList.scheduler.Add(scheduler);
            }
            ScheduleView.Children.Insert(index - 1, Schedule_UI);
           
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }


    private void GestureRecognizer_Tapped(object sender, EventArgs e)
    {
        Console.WriteLine("************GestureRecognizer_Tapped***********");
        //  throw new NotImplementedException();
    }

    private void ButtonVisibleChange()
    {
        var i = GetScheduleNewIndex();
        AddBtn.IsVisible = i > 6 ? false : true;
        DelButton.IsVisible = i > 1 ? true : false;
    }

    private List<UIDay> inputDate()
    {
        DateTime now = FirstDayOfWeek(DateTime.Now);
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
    public async void ReceiveEditedORNewschedule(object arg, string id, TimeSpan start, TimeSpan stop, string interval, bool active)
    {
        var iProg = PageContext.EnableSpin();
        try
        {

            EnableDisableDeleteButtonInRow(false);
            var viewObj = ScheduleView.Children[Convert.ToInt32(id) - 1];
            await Task.Delay(500);
            ScheduleView.Children.RemoveAt(Convert.ToInt32(id) - 1);

            var uidays = (List<UIDay>)arg;

            //Add new schedule item view in small row
            DayConfigControl Schedule = new DayConfigControl(uidays, Navigation, this, active, scheduleList);
            Schedule.indexText = id;
            Schedule.editText = "edit";
            Schedule.intervalText = interval;
            Schedule.StartTimeText = start;
            Schedule.StopTimeText = stop;
            Schedule.ScheduleReceived += ReceiveEditedORNewschedule;
            Schedule.DeleteDelegate += SingleDeleteDelegate;
            Schedule.DefaultUI = UIEnum.Schedul_NormalView;
            ScheduleView.Children.Insert(Convert.ToInt32(id) - 1, Schedule);


            //Make API call to update scheule to device
            var obj = JSONUtil.Build_Scheduler(uidays, start, stop, interval, active);
            obj.index = id.ToString();


            scheduleList.scheduler[Convert.ToInt32(obj.index) - 1] = obj;
            scheduleList.request = "set";
            scheduleList.info = "schedule";
            if (PageContext != null && obj != null)
            {
                PageContext.EnableSpin();
                await PageContext.SetSchedulerinVM(JsonConvert.SerializeObject(scheduleList));
                DeviceInfoRequest activeStatus = new DeviceInfoRequest();
                activeStatus.request = "status";
                activeStatus.info = "scheduler";
                activeStatus.index = Convert.ToInt32(id);
                activeStatus.value = active.ToString().ToLower();
                await PageContext.SetScheduleActiveInactiveStatusinVM(activeStatus);

            }

        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(ex);
            UserDialogs.Instance.HideLoading();
        }
        finally
        {
            PageContext.DisableSpin(iProg);
        }
    }

    /// <summary>
    /// it will return new index value for next element, ex: if the list count is 3 means, this methood will return 4
    /// </summary>
    /// <returns></returns>
    private int GetScheduleNewIndex()
    {
        int indx = 1;
        if (scheduleList.scheduler.Count > 0)
        {
            indx = Convert.ToInt32(scheduleList.scheduler[scheduleList.scheduler.Count - 1].index);
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
        if (sender != null)
        {
            QuickAccessButton quickAccessButton = (QuickAccessButton)sender;
            var loadingPage = new DialogPopupPage(quickAccessButton);

            await MopupService.Instance.PushAsync(loadingPage);

          //  await Navigation.PushPopupAsync(loadingPage);
        }
    }
    private void CreateTapGesture(QuickAccessButton quickAccessButton)
    {
        TapGestureRecognizer tapGestureRecognizer = new TapGestureRecognizer();
        tapGestureRecognizer.Tapped += GotoPopupInputpage;
        quickAccessButton.GestureRecognizers.Add(tapGestureRecognizer);
    }

    void RefreshClicked(System.Object sender, System.EventArgs e)
    {
        PageContext.IsScheduleGetError = false;
        InitVm();
    }



}