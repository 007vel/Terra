using Acr.UserDialogs;
using ConnectionLibrary.Interface;
using ConnectionLibrary.Network;
using ConnectionLibrary.Network.Mock;
using ConnectionLibrary.Network.Util;
using Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Input;
using Xamarin.Forms;

namespace Terra.Core.ViewModels
{
    public class DeviceDetailsViewModel : ViewModelBase
    {
        System.Timers.Timer timer;
        int expireTime = 0;
        public delegate void ActionResult(List<Scheduler> arg);
        public event ActionResult Result;
        public delegate void DeviceInfoResult(DeviceInfo deviceInfo);
        public event DeviceInfoResult DeviceInfoReceived;
        // public IDevice deviceService = new MockDeviceService();
       //   public IDevice deviceService = new DeviceService();
        public IDevice deviceService = DeviceService.Instance;

        public ICommand DemoCommand => new Command(SetDemoClicked);
        public ICommand SleepCommand => new Command(SetSleepClicked);
        public ICommand OtaCommand => new Command(OtaUploadClicked); 

        int sleeptime = 750;

        private List<Scheduler> schedulers;
        public List<Scheduler> Schedulers
        {
            get
            {
                return schedulers;
            }
            set
            {
                schedulers = value;
                OnPropertyChanged("Schedulers");
            }
        }

        DeviceInfo battery;
        public DeviceInfo Battery
        {
            get
            {
                return battery;
            }
            set
            {
                battery = value;
                OnPropertyChanged("Battery");
            }
        }

        DeviceInfo spray;
        public DeviceInfo RemSpray
        {
            get
            {
                return spray;
            }
            set
            {
                spray = value;
                OnPropertyChanged("RemSpray");
            }
        }

        DeviceInfo initializeSpray;
        public DeviceInfo InitializeSpray
        {
            get
            {
                return initializeSpray;
            }
            set
            {
                initializeSpray = value;
                OnPropertyChanged("InitializeSpray");
            }
        }
        bool isScheduleGetError;
        public bool IsScheduleGetError
        {
            get
            {
                return isScheduleGetError;
            }
            set
            {
                isScheduleGetError = value;
                OnPropertyChanged("IsScheduleGetError");
            }
        }

        DeviceInfo daysLeft;
        public DeviceInfo DaysLeft
        {
            get
            {
                return daysLeft;
            }
            set
            {
                daysLeft = value;
                OnPropertyChanged("DaysLeft");
            }
        }
        DeviceInfo nextSprayCounter;
        public DeviceInfo NextSprayCounter
        {
            get
            {
                return nextSprayCounter;
            }
            set
            {
                nextSprayCounter = value;
                OnPropertyChanged("NextSprayCounter");
            }
        }
        public DeviceDetailsViewModel()
        {
            
        }

        /// <summary>
        /// This method called during Initialization of ViewModel
        /// </summary>
        public async Task OnInit()
        {
            LoadData();
        }

        private async void LoadData()
        {
            await SetTime();
            Thread.Sleep(sleeptime);
           
            var rawSchedule = await GetScheduleFromService();
            NetworkServiceUtil.Log("DeviceDetailsViewModel OnInit rawSchedule: " + rawSchedule);
            IsScheduleGetError = DoesHaveErrorCode(rawSchedule);
            if(isScheduleGetError)
            {
                bool wifiNotConnected = NetworkViewModel.WIFI_ID == WifiAdapter.Instance.FormWifiManager.GetSsId().ToLower();
                if(!wifiNotConnected)
                {
                    Terra.Core.Utils.Utils.Toast("Device connection lost!");
                    await Shell.Current.Navigation.PopAsync();
                    return;
                }
            }
            Schedulers = DeserializSchedule(rawSchedule);
            Result?.Invoke(Schedulers);
          //  return;
            Thread.Sleep(1100);

          //  await GetBatteryCount();
            //Thread.Sleep(sleeptime);
           // await GetInitilizeSprayCount();
            //Thread.Sleep(sleeptime);
           // await GetRemSprayCount();
            //Thread.Sleep(sleeptime);
           // await GetDaysLeftCount();
            //Thread.Sleep(sleeptime);
          //  await GetNextSprayCounterCount();
            await GetSnapshotAPI();
        }

        /// <summary>
        /// Get the schedules from Device service
        /// </summary>
        /// <returns>List of assigned schedules froms device</returns>
        private async Task<string> GetScheduleFromService()
        {
            DeviceInfoRequest deviceInfoRequest = new DeviceInfoRequest();
            deviceInfoRequest.request = "get";
            deviceInfoRequest.info = "scheduler";
            var schedule = await deviceService.GetScheduler(deviceInfoRequest);
            return schedule;
        }

        /// <summary>
        /// Deserialize raw response and make a schedule list
        /// </summary>
        /// <param name="raw">raw string schedules</param>
        /// <returns>Deserialized schedule list</returns>
        private List<Scheduler> DeserializSchedule(string raw)
        {
            try
            {
                return JSONHelper.DeserializSchedule(raw);
            }
            catch(Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);
            }
            return null;
        }

        private bool DoesHaveErrorCode(string response)
        {
            return response == ErrorCode.SCHEDULE_GET;
        }

        ////////////////////////////////////////////////
        ////////////////////////////////////////////////
        /////////////////Get device values//////////////
        ////////////////////////////////////////////////
        ////////////////////////////////////////////////


        /// <summary>
        /// GetBatteryCount Will return count from service
        /// </summary>
        public async Task<bool> GetBatteryCount()
        {
            DeviceInfoRequest deviceInfoRequest = new DeviceInfoRequest();
            deviceInfoRequest.request = "get";
            deviceInfoRequest.info = "battery";
            var deviceRes = await deviceService.GetDeviceInfo(deviceInfoRequest);
            NetworkServiceUtil.Log("DeviceDetailsViewModel GetBatteryCount get battery: " + deviceRes);
            Battery = DeserializDeviceInfo(deviceRes);
            DeviceInfoReceived?.Invoke(Battery);
            return false;
        }

        /// <summary>
        /// GetRemSprayCount Will return count from service
        /// </summary>
        public async Task<bool> GetRemSprayCount()
        {
            return false;
            DeviceInfoRequest deviceInfoRequest = new DeviceInfoRequest();
            deviceInfoRequest.request = "get";
            deviceInfoRequest.info = "spray";
            var deviceRes = await deviceService.GetDeviceInfo(deviceInfoRequest);
            NetworkServiceUtil.Log("DeviceDetailsViewModel GetRemSprayCount get spray: " + deviceRes);
            RemSpray = DeserializDeviceInfo(deviceRes);
            DeviceInfoReceived?.Invoke(RemSpray);
            //  CalculateRemainingDays();
            return false;
        }

        /// <summary>
        /// GetInitilizeSprayCount(Input) Will return count from service
        /// </summary>
        public async Task<bool> GetInitilizeSprayCount()
        {
            DeviceInfoRequest deviceInfoRequest = new DeviceInfoRequest();
            deviceInfoRequest.request = "init";
            deviceInfoRequest.info = "spray";
            var deviceRes = await deviceService.GetDeviceInfo(deviceInfoRequest);
            NetworkServiceUtil.Log("DeviceDetailsViewModel GetInitilizeSprayCount init spray: " + deviceRes);
            InitializeSpray = DeserializDeviceInfo(deviceRes);
            DeviceInfoReceived?.Invoke(InitializeSpray);
            return true;
        }
        bool trap = false;
        /// <summary>
        /// GetDaysLeftCount Will return count from service
        /// </summary>
        public async Task<bool> GetDaysLeftCount()
        {
            return false;
            DeviceInfoRequest deviceInfoRequest = new DeviceInfoRequest();
            deviceInfoRequest.request = "get";
            deviceInfoRequest.info = "days_left";
            var deviceRes = await deviceService.GetDeviceInfo(deviceInfoRequest);
            NetworkServiceUtil.Log("DeviceDetailsViewModel GetDaysLeftCount get spray: " + deviceRes);
            var _DaysLeft = DeserializDeviceInfo(deviceRes);
            DaysLeft = _DaysLeft;
            DeviceInfoReceived?.Invoke(DaysLeft);
            // CalculateRemainingDays();
            return true;
        }
        /// <summary>
        /// GetNextSprayCounterCount Will return count from service
        /// </summary>
        public async Task<bool> GetNextSprayCounterCount()
        {
            DeviceInfoRequest deviceInfoRequest = new DeviceInfoRequest();
            deviceInfoRequest.request = "get";
            deviceInfoRequest.info = "spray_counter";
            var deviceRes = await deviceService.GetDeviceInfo(deviceInfoRequest);
            NetworkServiceUtil.Log("DeviceDetailsViewModel GetNextSprayCounterCount: " + deviceRes);
            NextSprayCounter = DeserializDeviceInfo(deviceRes);
            DeviceInfoReceived?.Invoke(NextSprayCounter);
            // CalculateRemainingDays();
            return false;
        }

        /// <summary>
        /// GetSnapshotAPI Will return count from service
        /// </summary>
        public async Task<bool> GetSnapshotAPI()
        {
            DeviceInfoRequest deviceInfoRequest = new DeviceInfoRequest();
            deviceInfoRequest.request = "get";
            deviceInfoRequest.info = "snapshot";
            var deviceRes = await deviceService.GetDeviceSnapShotInfo(deviceInfoRequest);
            NetworkServiceUtil.Log("DeviceDetailsViewModel GetNextSprayCounterCount: " + deviceRes);
            Devicesnapshot devicesnapshot = null;
            if (!string.IsNullOrEmpty(deviceRes))
            {
                devicesnapshot = JsonConvert.DeserializeObject<Devicesnapshot>(deviceRes);
            }
            if(devicesnapshot!=null && devicesnapshot.snapshotinfo!=null)
            {
                foreach (var i in devicesnapshot.snapshotinfo)
                {
                    if(i.request=="get" && i.info== "battery")
                    {
                        Battery = i;
                    }else if (i.request == "init" && i.info == "spray")
                    {
                        InitializeSpray = i;
                    }else if (i.request == "get" && i.info == "days_left")
                    {
                        DaysLeft = i;
                    }else if (i.request == "get" && i.info == "spray")
                    {
                        RemSpray = i;
                    }
                    else if (i.request == "get" && i.info == "spray_counter")
                    {
                        NextSprayCounter = i;
                    }
                }
            }
            
            DeviceInfoReceived?.Invoke(RemSpray);
            DeviceInfoReceived?.Invoke(NextSprayCounter);
            DeviceInfoReceived?.Invoke(DaysLeft);
            DeviceInfoReceived?.Invoke(InitializeSpray);
            // CalculateRemainingDays();
            return false;
        }


        ////////////////////////////////////////////////
        ////////////////////////////////////////////////
        /////////////////Set device values//////////////
        ////////////////////////////////////////////////
        ////////////////////////////////////////////////




        public async Task<string> SetSchedulerinVM(string schedules)
        {
           var daysleft = await deviceService.SetScheduler(schedules);
            var _DaysLeft = DeserializDeviceInfo(daysleft);
            DaysLeft = _DaysLeft;
            DeviceInfoReceived?.Invoke(DaysLeft);
            return daysleft;
        }


        /// <summary>
        /// SetInitCount Will post value to device
        /// </summary>
        public async void SetInitilizeSprayCount(string val)
        {
            DeviceInfoRequest deviceInfoRequest = new DeviceInfoRequest();
            deviceInfoRequest.request = "init";
            deviceInfoRequest.info = "spray";
            deviceInfoRequest.value = val;
            var deviceRes = await deviceService.SetDeviceInfo(deviceInfoRequest);
        }

        /// <summary>
        /// SetInitCount Will post value to device
        /// </summary>
        public async void SetSleepmode()
        {
            Config config = new Config();
            config.request = "set";
            config.sleep_mode = 1;
            var deviceRes = await deviceService.SetDeviceConfig(config);
        }

        public async void SetDemoClicked()
        {
            DemoInfo config = new DemoInfo();
            config.request = "set";
            config.demo = 1;
            config.info = "demo";
            var deviceRes = await deviceService.SetDeviceDemo(config);
        }
        public async void SetSleepClicked()
        {
            Config config = new Config();
            config.sleep_mode = 1;
            config.request = "set";
            config.info = "config";
            var deviceRes = await deviceService.SetDeviceConfig(config);
            Device.BeginInvokeOnMainThread(async () => {

                try
                {
                    using (UserDialogs.Instance.Loading("Closing app..."))
                    {
                        await Task.Delay(1000*3);
                        UserDialogs.Instance.HideLoading();
                        IMobile mobile = DependencyService.Get<IMobile>();
                        mobile.TerminateApp();
                    }
                }
                catch (Exception ex)
                {
                    var val = ex.Message;
                    
                }
            });

        }

        public async void OtaUploadClicked()
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                try
                {
                    using (UserDialogs.Instance.Loading("Uploading..."))
                    {
                        var otaByte = DependencyService.Get<IMobile>().ReadOtaFile();
                        await deviceService.PutBinary("", otaByte);
                        //Task.Delay(2*1000);
                        UserDialogs.Instance.HideLoading();
                        await Shell.Current.Navigation.PopAsync();
                    }
                    UserDialogs.Instance.HideLoading();
                }
                catch (Exception ex)
                {
                    await App.Current.MainPage.DisplayAlert(title: "Alert", message: "Error in OTA update", cancel: "OK");
                    Console.WriteLine(ex);
                    UserDialogs.Instance.HideLoading();
                }
            });
        }

        public async void SetDispanserType(string type)
        {
            DeviceInfoRequest deviceInfoRequest = new DeviceInfoRequest();
            deviceInfoRequest.Dispenser_Type = type;
            deviceInfoRequest.request = "set";
            deviceInfoRequest.info = "type_disp";
            var deviceRes = await deviceService.SetDeviceInfo(deviceInfoRequest);
        }

        private async Task<bool> SetTime()
        {
            Config timeConfig = new Config();
            timeConfig.request = "set";
            timeConfig.current_epoch = Terra.Core.Utils.Utils.GetEpochSeconds();
            timeConfig.timeZone = Terra.Core.Utils.Utils.GetTimeZoneInfo();
            var deviceRes = await deviceService.SetDeviceConfig(timeConfig);
            return deviceRes;
        }

        private DeviceInfo DeserializDeviceInfo(string deviceRes)
        {
            try
            {
                return JSONHelper.DeserializDeviceInfo(deviceRes);
            }
            catch(Exception e)
            {
                NetworkServiceUtil.Log("DeviceDetailsViewModel DeserializDeviceInfo Exception: " + e);
            }
            return null;
        }

        public async Task<bool> DeleteScheduleItem(string index)
        {
            NetworkServiceUtil.Log("DeleteScheduleItem ==> called");
            ScheduleIndex scheduleIndex = new ScheduleIndex();
            scheduleIndex.deleteindex = (Convert.ToInt32( index)-1).ToString();
            scheduleIndex.request = "delete";
            scheduleIndex.info = "scheduler";
            var deviceRes = await deviceService.DeleteScheduleIndex(scheduleIndex);
            return deviceRes;
        }
        
        public IProgressDialog EnableSpin()
        {
            return UserDialogs.Instance.Loading("Saving...");
        }

        public void DisableSpin(IProgressDialog progressDialog)
        {
            progressDialog?.Hide();
        }
    }
}
