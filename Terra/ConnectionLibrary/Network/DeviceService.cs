using ConnectionLibrary.Interface;
using ConnectionLibrary.Network.Util;
using Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace ConnectionLibrary.Network
{
    public class DeviceService : IDevice
    {
        ClientWebSocket client;

        private static HttpClient httpClient;

        static DeviceService deviceService = null;
        public static DeviceService Instance
        {
            get
            {
                if(deviceService==null)
                {
                    deviceService = new DeviceService();
                }
                return deviceService;
            }
            set
            {
                deviceService = value;
            }
        }

        private void InitializeHttpClient()
        {
            if (httpClient == null)
            {
                System.Diagnostics.Debug.WriteLine("Initializing HttpClient....");
                httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.Timeout = TimeSpan.FromMinutes(1);
            }
        }

        object m_lock = new object();
        private async Task<ClientWebSocket> InitClientWebSocket(string endPoint)
        {
            //lock(m_lock)
            {
                if (client == null || client.State != WebSocketState.Open)
                {
                    client =  await WifiAdapter.Instance.StartWebSocketConnection(endPoint);
                }
                return client;
            }
            
        }

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
                    var info = await GetWsData(UrlConfig.GetFullURL(Endpoint.info, Endpoint_Method.GET, isNewFW:true), jsonIgnoreNullValues);
                    return info;
                }
            }
            catch(Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);
            }
            return string.Empty;
        }
        public async Task<string> GetDeviceSnapShotInfo(DeviceInfoRequest deviceInfoRequest)
        {
            try
            {
                if (deviceInfoRequest != null)
                {
                    string jsonIgnoreNullValues = JsonConvert.SerializeObject(deviceInfoRequest, Formatting.Indented, new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore
                    });
                    var info = await GetWsData(UrlConfig.GetFullURL(Endpoint.snapshot, Endpoint_Method.GET), jsonIgnoreNullValues);
                    return info;
                }
            }
            catch (Exception e)
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
                var _schedule= await GetWsData(UrlConfig.GetFullURL(Endpoint.scheduler, Endpoint_Method.GET), jsonIgnoreNullValues, ErrorCode.SCHEDULE_GET);
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

        public async Task<string> SetScheduler(string schedule)
        {
            try
            {
                var res = await GetWsData(UrlConfig.GetFullURL(Endpoint.scheduler, Endpoint_Method.POST),schedule);
                return res;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);
            }
            return null;
        }
        private async Task<string> GetWsData(string endPoint, string deviceInfoRequest, string defaultResponse = null)
        {
            NetworkServiceUtil.Log("Socket GetWsData: start");
            try
            {
                if (deviceInfoRequest != null)
                {
                    NetworkServiceUtil.Log("Socket GetWsData:1");
                    client = await InitClientWebSocket(endPoint);

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
                return defaultResponse;
            }
            finally
            {
                NetworkServiceUtil.Log("Socket GetWsData: finally");
              //  WifiAdapter.Instance.StopWebSocketConnection(client);
            }
            NetworkServiceUtil.Log("Socket GetWsData: end");
            return null;
        }
        private async Task<bool> SetWsData(string endPoint, string deviceInfoRequest)
        {

            try
            {
                if (deviceInfoRequest != null)
                {
                    client = await InitClientWebSocket(endPoint);
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
               // WifiAdapter.Instance.StopWebSocketConnection(client);
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

        public async Task<bool> DeleteScheduleIndex(ScheduleIndex scheduleIndex)
        {
            try
            {
                if (scheduleIndex != null)
                {
                    string jsonIgnoreNullValues = JsonConvert.SerializeObject(scheduleIndex, Formatting.Indented, new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore
                    });
                    var info = await SetWsData(UrlConfig.GetFullURL(Endpoint.deleteschedule, Endpoint_Method.POST), jsonIgnoreNullValues);
                    return info;
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);
            }
            return true;
        }

        public async Task<bool> PutBinary(string endPoint, byte[] requestBody)
        {
            try
            {
                //Task.Delay(1 * 1000);
                string url = UrlConfig.GetFullURL(Endpoint.upload, Endpoint_Method.POST);
                InitializeHttpClient();
                var requestContent = new StreamContent(new MemoryStream(requestBody));

                HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, url);
                httpRequestMessage.Content = requestContent;

                System.Diagnostics.Debug.WriteLine("Service call Put url : " + url);
                System.Diagnostics.Debug.WriteLine("Service call Put requestBody : " + requestBody);

                var httpResponse = await httpClient.SendAsync(httpRequestMessage);

                System.Diagnostics.Debug.WriteLine("Service call Put response: " + httpResponse);

                if (httpResponse.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);
                return false;
            }

        }
    }
}

