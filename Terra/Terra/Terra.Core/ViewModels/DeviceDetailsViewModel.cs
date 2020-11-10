using ConnectionLibrary.Network;
using Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Terra.Core.ViewModels
{
    public class DeviceDetailsViewModel : ViewModelBase
    {
        public delegate void ActionResult(List<Scheduler> arg);
        public event ActionResult Result;
        public DeviceService DeviceService = new DeviceService();
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
        public DeviceDetailsViewModel()
        {
            var _RemSpray = new DeviceInfo();
            _RemSpray.value = "12";
            RemSpray = _RemSpray;
        }
        /// <summary>
        /// This method called during Initialization of ViewModel
        /// </summary>
        public override async void OnInit()
        {
            var rawSchedule = await GetScheduleFromService();
            NetworkServiceUtil.Log("DeviceDetailsViewModel OnInit rawSchedule: " + rawSchedule);
          //  Schedulers = DeserializSchedule(rawSchedule);
          //  Result.Invoke(Schedulers);
            GetBatteryCount();
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
            var schedule = await DeviceService.GetScheduler(deviceInfoRequest);
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
                List<Scheduler> SchedulerList = null;
                if (!string.IsNullOrEmpty(raw))
                {
                    JObject jObject = new JObject(raw);
                    if (jObject != null)
                    {
                        JArray scheduleList = (JArray)jObject.SelectToken("scheduler");
                        if (scheduleList != null && scheduleList.Count > 0)
                        {
                            SchedulerList = new List<Scheduler>();
                            foreach (var sch in scheduleList)
                            {
                                Scheduler scheduler = (Scheduler)JsonConvert.DeserializeObject<Scheduler>(sch.ToString());
                                SchedulerList.Add(scheduler);
                            }
                        }
                    }
                }
                return SchedulerList;
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
            var deviceRes = await DeviceService.GetDeviceInfo(deviceInfoRequest);
            Battery= DeserializDeviceInfo(deviceRes);
        }

        /// <summary>
        /// GetRemSprayCount Will return count from service
        /// </summary>
        public async Task<DeviceInfo> GetRemSprayCount()
        {
            DeviceInfoRequest deviceInfoRequest = new DeviceInfoRequest();
            deviceInfoRequest.request = "get";
            deviceInfoRequest.info = "spray";
            var deviceRes = await DeviceService.GetDeviceInfo(deviceInfoRequest);
           // RemSpray = DeserializDeviceInfo(deviceRes);
            return RemSpray;
        }

        /// <summary>
        /// GetInitilizeSprayCount(Input) Will return count from service
        /// </summary>
        public async void GetInitilizeSprayCount()
        {
            DeviceInfoRequest deviceInfoRequest = new DeviceInfoRequest();
            deviceInfoRequest.request = "init";
            deviceInfoRequest.info = "spray";
            var deviceRes = await DeviceService.GetDeviceInfo(deviceInfoRequest);
            InitializeSpray = DeserializDeviceInfo(deviceRes);
        }

        ////////////////////////////////////////////////
        ////////////////////////////////////////////////
        /////////////////Set device values//////////////
        ////////////////////////////////////////////////
        ////////////////////////////////////////////////



        /// <summary>
        /// SetInitCount Will post value to device
        /// </summary>
        public async void SetInitilizeSprayCount()
        {
            DeviceInfoRequest deviceInfoRequest = new DeviceInfoRequest();
            deviceInfoRequest.request = "init";
            deviceInfoRequest.info = "spray";
            var deviceRes = await DeviceService.SetDeviceInfo(deviceInfoRequest);
        }

        private DeviceInfo DeserializDeviceInfo(string deviceRes)
        {
            try
            {
                return JsonConvert.DeserializeObject<DeviceInfo>(deviceRes);
            }catch(Exception e)
            {
                NetworkServiceUtil.Log("DeviceDetailsViewModel DeserializDeviceInfo Exception: " + e);
            }
            return null;
        }
    }
}
