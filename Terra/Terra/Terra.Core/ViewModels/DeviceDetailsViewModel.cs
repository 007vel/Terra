using ConnectionLibrary.Interface;
using ConnectionLibrary.Network;
using ConnectionLibrary.Network.Mock;
using ConnectionLibrary.Network.Util;
using Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Terra.Core.ViewModels
{
    public class DeviceDetailsViewModel : ViewModelBase
    {
        public delegate void ActionResult(List<Scheduler> arg);
        public event ActionResult Result;
        public delegate void DeviceInfoResult(DeviceInfo deviceInfo);
        public event DeviceInfoResult DeviceInfoReceived;
      //  public IDevice deviceService = new MockDeviceService();
       public IDevice deviceService = new DeviceService();

        public ICommand DemoCommand => new Command(SetDemoClicked);
        public ICommand SleepCommand => new Command(SetSleepClicked);

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

        string daysLeft="0";
        public string DaysLeft
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

        public DeviceDetailsViewModel()
        {
            
        }

        /// <summary>
        /// This method called during Initialization of ViewModel
        /// </summary>
        public async Task OnInit()
        {
            SetTime();
            var rawSchedule = await GetScheduleFromService();
            NetworkServiceUtil.Log("DeviceDetailsViewModel OnInit rawSchedule: " + rawSchedule);
            Schedulers = DeserializSchedule(rawSchedule);
            Result?.Invoke(Schedulers);
            GetBatteryCount();
            GetInitilizeSprayCount();
            GetRemSprayCount();
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

        ////////////////////////////////////////////////
        ////////////////////////////////////////////////
        /////////////////Get device values//////////////
        ////////////////////////////////////////////////
        ////////////////////////////////////////////////


        /// <summary>
        /// GetBatteryCount Will return count from service
        /// </summary>
        public async void GetBatteryCount()
        {
            DeviceInfoRequest deviceInfoRequest = new DeviceInfoRequest();
            deviceInfoRequest.request = "get";
            deviceInfoRequest.info = "battery";
            var deviceRes = await deviceService.GetDeviceInfo(deviceInfoRequest);
            NetworkServiceUtil.Log("DeviceDetailsViewModel GetBatteryCount get battery: " + deviceRes);
            Battery = DeserializDeviceInfo(deviceRes);
            DeviceInfoReceived?.Invoke(Battery);
        }

        /// <summary>
        /// GetRemSprayCount Will return count from service
        /// </summary>
        public async void GetRemSprayCount()
        {
            DeviceInfoRequest deviceInfoRequest = new DeviceInfoRequest();
            deviceInfoRequest.request = "get";
            deviceInfoRequest.info = "spray";
            var deviceRes = await deviceService.GetDeviceInfo(deviceInfoRequest);
            NetworkServiceUtil.Log("DeviceDetailsViewModel GetRemSprayCount get spray: " + deviceRes);
            RemSpray = DeserializDeviceInfo(deviceRes);
            DeviceInfoReceived?.Invoke(RemSpray);
            CalculateRemainingDays();
        }

        /// <summary>
        /// GetInitilizeSprayCount(Input) Will return count from service
        /// </summary>
        public async void GetInitilizeSprayCount()
        {
            DeviceInfoRequest deviceInfoRequest = new DeviceInfoRequest();
            deviceInfoRequest.request = "init";
            deviceInfoRequest.info = "spray";
            var deviceRes = await deviceService.GetDeviceInfo(deviceInfoRequest);
            NetworkServiceUtil.Log("DeviceDetailsViewModel GetInitilizeSprayCount init spray: " + deviceRes);
            InitializeSpray = DeserializDeviceInfo(deviceRes);
            DeviceInfoReceived?.Invoke(InitializeSpray);
        }

        ////////////////////////////////////////////////
        ////////////////////////////////////////////////
        /////////////////Set device values//////////////
        ////////////////////////////////////////////////
        ////////////////////////////////////////////////


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
            var deviceRes = await deviceService.SetDeviceDemo(config);
        }
        public async void SetSleepClicked()
        {
            Config config = new Config();
            config.sleep_mode = 1;
            config.request = "set";
            var deviceRes = await deviceService.SetDeviceConfig(config);
        }

        public async void SetDispanserType(string type)
        {
            DeviceInfoRequest deviceInfoRequest = new DeviceInfoRequest();
            deviceInfoRequest.Dispenser_Type = type;
            deviceInfoRequest.request = "set";
            var deviceRes = await deviceService.SetDeviceInfo(deviceInfoRequest);
        }

        private async void SetTime()
        {
            Config timeConfig = new Config();
            timeConfig.request = "set";
            timeConfig.current_epoch = Terra.Core.Utils.Utils.GetEpochSeconds();
            timeConfig.timeZone = Terra.Core.Utils.Utils.GetTimeZoneInfo();
            var deviceRes = await deviceService.SetDeviceConfig(timeConfig);
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


        private void CalculateRemainingDays()
        {
            var schedulers = Schedulers;
            long NumberOfSpray=0;
            if (schedulers!=null && schedulers.Count>0)
            {
                foreach(var item in schedulers)
                {
                    if(! string.IsNullOrEmpty( item.day))
                    {
                        var daysList = item.day.Split(',');
                        if(daysList.Length>0)
                        {
                            var scheduleTimeInMinute = (item.stop - item.start)/60;
                            if(scheduleTimeInMinute>0)
                            {
                                NumberOfSpray += (scheduleTimeInMinute / item.interval)* daysList.Length;
                            }
                        }
                    }
                }
            }

            if(NumberOfSpray>0 && InitializeSpray!=null && RemSpray!=null)
            {
                if(!string.IsNullOrEmpty(InitializeSpray.value) && !string.IsNullOrEmpty(RemSpray.value))
                {
                    if(Convert.ToDouble(InitializeSpray.value)>0 && Convert.ToDouble(RemSpray.value) > 0)
                    {
                        double remWeeks = Convert.ToDouble(RemSpray.value) / NumberOfSpray;
                        double remDays = remWeeks * 7;
                        DaysLeft = Convert.ToInt32( Math.Round(remDays)).ToString();
                        DeviceInfoReceived?.Invoke(RemSpray);
                    }
                }
            }
        }


    }
}
