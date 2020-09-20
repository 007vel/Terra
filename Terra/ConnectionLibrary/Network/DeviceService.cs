using ConnectionLibrary.Interface;
using Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ConnectionLibrary.Network
{
    public class DeviceService : IDevice
    {
        public async Task<string> GetDeviceInfo(DeviceInfoRequest deviceInfoRequest)
        {
            try
            {
                if (deviceInfoRequest != null)
                {
                    var info = await GetResult(UrlConfig.GetFullURL(Endpoint.info, Endpoint_Method.GET), JsonConvert.SerializeObject(deviceInfoRequest));
                    return info;
                }
            }
            catch(Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);
            }
            return string.Empty;
        }

        public async Task<string> GetScheduler(DeviceInfoRequest deviceInfoRequest)
        {
            try
            {
                var _schedule= await GetResult(UrlConfig.GetFullURL(Endpoint.scheduler, Endpoint_Method.GET),JsonConvert.SerializeObject(deviceInfoRequest));
                return _schedule;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);
            }
            return string.Empty;
        }

        public async Task<string> SetScheduler(string schedule)
        {
            try
            {
                var res = await GetResult(UrlConfig.GetFullURL(Endpoint.scheduler, Endpoint_Method.GET),schedule);
                return res;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);
            }
            return string.Empty;
        }
        private async Task<string> GetResult(string endPoint, string deviceInfoRequest)
        {
            try
            {
                if (deviceInfoRequest != null)
                {
                    WifiAdapter.Instance.StartWebSocketConnection("");
                    WifiAdapter.Instance.SendMessageAsync(deviceInfoRequest);
                    var result = await WifiAdapter.Instance.ReadMessage();
                    return result;
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);
            }
            return null;
        }
    }
}
