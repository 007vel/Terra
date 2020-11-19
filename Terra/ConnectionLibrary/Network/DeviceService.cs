using ConnectionLibrary.Interface;
using Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.WebSockets;
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
                    string jsonIgnoreNullValues = JsonConvert.SerializeObject(deviceInfoRequest, Formatting.Indented, new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore
                    });
                    var info = await GetWsData(UrlConfig.GetFullURL(Endpoint.info, Endpoint_Method.GET), jsonIgnoreNullValues);
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
                string jsonIgnoreNullValues = JsonConvert.SerializeObject(deviceInfoRequest, Formatting.Indented, new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                });
                var _schedule= await GetWsData(UrlConfig.GetFullURL(Endpoint.scheduler, Endpoint_Method.GET), jsonIgnoreNullValues);
                return _schedule;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);
            }
            return string.Empty;
        }

        public async Task<bool> SetDeviceInfo(DeviceInfoRequest deviceInfoRequest)
        {
            try
            {
                string jsonIgnoreNullValues = JsonConvert.SerializeObject(deviceInfoRequest, Formatting.Indented, new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                });
                var result = await SetWsData(UrlConfig.GetFullURL(Endpoint.info, Endpoint_Method.POST), jsonIgnoreNullValues);
                return result;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);
            }
            return true;
        }
        public async Task<bool> SetDeviceConfig(Config config)
        {
            try
            {
                string jsonIgnoreNullValues = JsonConvert.SerializeObject(config, Formatting.Indented, new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                });
                var result = await SetWsData(UrlConfig.GetFullURL(Endpoint.config, Endpoint_Method.POST), jsonIgnoreNullValues);
                return result;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);
            }
            return true;
        }

        public async Task<bool> SetScheduler(string schedule)
        {
            try
            {
                var res = await SetWsData(UrlConfig.GetFullURL(Endpoint.scheduler, Endpoint_Method.POST),schedule);
                return res;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);
            }
            return true;
        }
        private async Task<string> GetWsData(string endPoint, string deviceInfoRequest)
        {
            NetworkServiceUtil.Log("Socket GetWsData: start");
            ClientWebSocket client = null;
            try
            {
                if (deviceInfoRequest != null)
                {
                    NetworkServiceUtil.Log("Socket GetWsData:1");
                    client = await WifiAdapter.Instance.StartWebSocketConnection(endPoint);
                    NetworkServiceUtil.Log("Socket GetWsData:2");
                    await WifiAdapter.Instance.SendMessageAsync(deviceInfoRequest, client);
                    NetworkServiceUtil.Log("Socket GetWsData:3");
                    var result = await WifiAdapter.Instance.ReadMessage(client);
                    NetworkServiceUtil.Log("Socket GetWsData:4");
                    return result;
                }
            }
            catch (Exception e)
            {
                NetworkServiceUtil.Log("Socket GetWsData: Exception");
                System.Diagnostics.Debug.WriteLine(e);
            }
            finally
            {
                NetworkServiceUtil.Log("Socket GetWsData: finally");
                WifiAdapter.Instance.StopWebSocketConnection(client);
            }
            NetworkServiceUtil.Log("Socket GetWsData: end");
            return null;
        }
        private async Task<bool> SetWsData(string endPoint, string deviceInfoRequest)
        {
            ClientWebSocket client = null;
            try
            {
                if (deviceInfoRequest != null)
                {
                    client=await WifiAdapter.Instance.StartWebSocketConnection(endPoint);
                    await WifiAdapter.Instance.SendMessageAsync(deviceInfoRequest, client);
                    return true;
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);
            }
            finally
            {
                WifiAdapter.Instance.StopWebSocketConnection(client);
            }
            return true;
        }

        public async Task<bool> SetDeviceDemo(DemoInfo config)
        {
            try
            {
                if (config != null)
                {
                    string jsonIgnoreNullValues = JsonConvert.SerializeObject(config, Formatting.Indented, new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore
                    });
                    var info = await SetWsData(UrlConfig.GetFullURL(Endpoint.config, Endpoint_Method.POST), jsonIgnoreNullValues);
                    return info;
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);
            }
            return true;
        }

    }
}

