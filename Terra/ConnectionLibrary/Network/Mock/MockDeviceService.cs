﻿using System;
using System.Threading.Tasks;
using ConnectionLibrary.Interface;
using Entities;
using Newtonsoft.Json;

namespace ConnectionLibrary.Network.Mock
{
    public class MockDeviceService: IDevice
    {
        public Task<string> DeleteScheduleIndex(ScheduleIndex scheduleIndex)
        {
           throw new NotImplementedException();
        }
        private string mockJson = string.Empty;
        public async Task<string> GetDeviceInfo(DeviceInfoRequest deviceInfoRequest)
        {
          //  return null;
            if(deviceInfoRequest!=null)
            {
                // remaining spray
                if(deviceInfoRequest.request=="get" && deviceInfoRequest.info== "spray")
                {
                    return "{\"request\":\"get\",\"info\":\"spray\",\"value\":56}";
                }else if (deviceInfoRequest.request == "get" && deviceInfoRequest.info == "battery")
                {
                    // get battery
                    return "{\"request\":\"get\",\"info\":\"battery\",\"value\":95}";
                }else if (deviceInfoRequest.request == "init" && deviceInfoRequest.info == "spray")
                {
                    // Initial spray
                    return "{\r\n\t\"request\":\"init\",\r\n\t\"info\":\"spray\",\r\n\t\"value\": 200\r\n}";
                }
                else if (deviceInfoRequest.request == "get" && deviceInfoRequest.info == "days_left")
                {
                    // GET DAYS LEFT
                    return "{\r\n\t\"request\":\"get\",\r\n\t\"info\":\"days_left\",\r\n\t\"value\": 200000\r\n}";
                }
                else if (deviceInfoRequest.request == "get" && deviceInfoRequest.info == "spray_counter")
                {
                    // GET NEXT SPRAY COUNTER
                    return "{\r\n    \"request\": \"get\",\r\n    \"info\":\"spray_counter\",\r\n    \"value\":10040\r\n}";
                }
            }

            return null;
        }

        public async Task<string> GetDeviceSnapShotInfo(DeviceInfoRequest deviceInfoRequest)
        {
            if (deviceInfoRequest.request == "get" && deviceInfoRequest.info == "snapshot")
            {
                return "{\r\n\"snapshotinfo\":[\r\n{\r\n\"request\": \"get\",\r\n\"info\": \"battery\",\r\n\"value\": 4\r\n},\r\n{\r\n\"request\": \"init\",\r\n\"info\": \"spray\",\r\n\"value\": 3200\r\n},\r\n{\r\n\"request\": \"get\",\r\n\"info\": \"days_left\",\r\n\"value\": 35683200\r\n},\r\n{\r\n\"request\": \"get\",\r\n\"info\": \"spray\",\r\n\"value\": 3200\r\n},\r\n{\r\n\"request\": \"get\",\r\n\"info\": \"spray_counter\",\r\n\"value\": 3200\r\n}]\r\n\r\n}";
            }
            return null;
        }

        public Task<string> GetHealthCheck(HeartBeat _heartBeat)
        {
            throw new NotImplementedException();
        }

        public async Task<string> GetScheduler(DeviceInfoRequest deviceInfoRequest)
        {
         //   return null;
            if(deviceInfoRequest!=null)
            {
                // get schedule
                if (deviceInfoRequest.request == "get" && deviceInfoRequest.info == "scheduler")
                {
                    if (!string.IsNullOrEmpty(mockJson)) return mockJson;

                    return "{\"scheduler_size\":1,\"scheduler\":[ { \"start\" :3600,\"stop\" :7200,\"interval\" :12,\"day\" :\"Monday\"},{\r\n\"start\":3600,\r\n\"stop\":5000,\r\n\"interval\":12,\r\n\"day\":\"Monday,tuesday\"\r\n}]}";
                }
                    
            }
            return null;
        }

        public Task<bool> PutBinary(string url, byte[] requestBody)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> SetDeviceConfig(Config config)
        {
            string jsonIgnoreNullValues = JsonConvert.SerializeObject(config, Formatting.Indented, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });
            return true;
        }

        public async Task<bool> SetDeviceDemo(DemoInfo config)
        {
            string jsonIgnoreNullValues = JsonConvert.SerializeObject(config, Formatting.Indented, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });
            return true;
        }

        public async Task<bool> SetDeviceInfo(DeviceInfoRequest deviceInfoRequest)
        {
            string jsonIgnoreNullValues = JsonConvert.SerializeObject(deviceInfoRequest, Formatting.Indented, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });
            return true;
        }

        public async Task<string> SetScheduler(string schedule)
        {
            return mockJson = schedule;
        }
    }
}
