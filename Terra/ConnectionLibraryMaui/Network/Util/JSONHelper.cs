using System;
using System.Collections.Generic;
using Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ConnectionLibrary.Network.Util
{
    public class JSONHelper
    {
        public static List<Scheduler> DeserializSchedule(string raw)
        {
            try
            {
                List<Scheduler> SchedulerList = null;
                if (!string.IsNullOrEmpty(raw))
                {
                    JObject jObject = JObject.Parse(raw);
                    if (jObject != null)
                    {
                        SchedulerList = new List<Scheduler>();
                        JArray scheduleList = (JArray)jObject.SelectToken("scheduler");
                        if (scheduleList != null && scheduleList.Count > 0)
                        {
                            
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
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);
            }
            return null;
        }
        public static DeviceInfo DeserializDeviceInfo(string deviceRes)
        {
            try
            {
                return JsonConvert.DeserializeObject<DeviceInfo>(deviceRes);
            }
            catch (Exception e)
            {
                NetworkServiceUtil.Log("DeviceDetailsViewModel DeserializDeviceInfo Exception: " + e);
            }
            return null;
        }
      
    }
}
