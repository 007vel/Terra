﻿using ConnectionLibrary.Network;
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
        public DeviceInfo Spray
        {
            get
            {
                return spray;
            }
            set
            {
                spray = value;
                OnPropertyChanged("Spray");
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
          //  WifiAdapter.Instance.WebSocketInit();
        }
        /// <summary>
        /// This method called during Initialization of ViewModel
        /// </summary>
        public override async void OnInit()
        {
            var rawSchedule = await GetScheduleFromService();
            Schedulers = DeserializSchedule(rawSchedule);
            Result.Invoke(Schedulers);
        }
        /// <summary>
        /// Get the schedules from Device service
        /// </summary>
        /// <returns>List of assigned schedules froms device</returns>
        private async Task<string> GetScheduleFromService()
        {
            DeviceInfoRequest deviceInfoRequest = new DeviceInfoRequest();
            deviceInfoRequest.request = "get";
            deviceInfoRequest.request = "scheduler";
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

        /// <summary>
        /// GetBatteryCount Wil return count from service
        /// </summary>
        private async void GetBatteryCount()
        {
            DeviceInfoRequest deviceInfoRequest = new DeviceInfoRequest();
            deviceInfoRequest.request = "get";
            deviceInfoRequest.request = "battery";
            var deviceRes = await DeviceService.GetDeviceInfo(deviceInfoRequest);
            Battery= DeserializDeviceInfo(deviceRes);
        }

        /// <summary>
        /// GetSprayCount Wil return count from service
        /// </summary>
        private async void GetSprayCount()
        {
            DeviceInfoRequest deviceInfoRequest = new DeviceInfoRequest();
            deviceInfoRequest.request = "get";
            deviceInfoRequest.request = "spray";
            var deviceRes = await DeviceService.GetDeviceInfo(deviceInfoRequest);
            Spray = DeserializDeviceInfo(deviceRes);
        }

        /// <summary>
        /// GetInitCount Wil return count from service
        /// </summary>
        private async void GetInitCount()
        {
            DeviceInfoRequest deviceInfoRequest = new DeviceInfoRequest();
            deviceInfoRequest.request = "init";
            deviceInfoRequest.request = "spray";
            var deviceRes = await DeviceService.GetDeviceInfo(deviceInfoRequest);
            InitializeSpray = DeserializDeviceInfo(deviceRes);
        }

        private DeviceInfo DeserializDeviceInfo(string deviceRes)
        {
            return JsonConvert.DeserializeObject<DeviceInfo>(deviceRes);
        }
    }
}
