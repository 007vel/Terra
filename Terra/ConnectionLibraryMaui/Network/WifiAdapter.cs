using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ConnectionLibrary.Interface;
using Entities.Wifi;
using Newtonsoft.Json.Linq;

namespace ConnectionLibrary.Network
{
    public class WifiAdapter : IDisposable
    {
        static WifiAdapter wifiAdapter=null;
        string m_ssid, m_pwd;
        System.Timers.Timer _timer = new System.Timers.Timer(10*1000);
        public static WifiAdapter Instance
        {
            get
            {
                if(wifiAdapter==null)
                {
                    wifiAdapter = new WifiAdapter();
                }
                return wifiAdapter;
            }
        }
        private WifiAdapter()
        {
            
        }
        public string CurrentDeviceFWVersion
        {
            get; set;
        }
        public void OnReceiveAvailableNetworks(List<Wifi> wifi)
        {
            MessagingCenter.Send(this, "WifiAdapter", wifi);
        }
        IPlatformWifiManager formWifiManager;
        public IPlatformWifiManager FormWifiManager
        {
            get
            {
                if(formWifiManager==null)
                {
                    formWifiManager= DependencyService.Get<IPlatformWifiManager>();
                }
                return formWifiManager;
            }
        }
        public bool IsGpsEnabled()
        {
            return DependencyService.Get<IPlatformWifiManager>().IsGpsEnable();
        }
        public void OnRequestAvailableNetworks()
        {
            FormWifiManager.RequestWifiNetworks();
        }
        public async Task<bool> ConnectToWifi(string ssid, string pwd)
        {
            m_ssid = ssid;
            m_pwd = pwd;
            FormWifiManager.DisconnectWifi();
            var res= await FormWifiManager.Connect(ssid,pwd);
            NetworkServiceUtil.Log("Socket ConnectToWifi");
            return res;
        }
        public async Task<bool> ConnectToWifi()
        {
          return await ConnectToWifi(m_ssid,m_pwd);
        }
        public string GetBssid()
        {
            var res =  FormWifiManager.GetBssId();
            NetworkServiceUtil.Log("Socket GetBssid");
            return res;
        }
        internal async Task<ClientWebSocket> StartWebSocketConnection(string url)
        {
            //await Task.Delay(1000);
            ClientWebSocket client = null;
            try
            {
                NetworkServiceUtil.Log("Socket StartWebSocketConnection: "+url);
                client = new ClientWebSocket();
                client.Options.KeepAliveInterval = TimeSpan.FromDays(1);
                await client.ConnectAsync(new Uri(url), CancellationToken.None);
            }
            catch(Exception e)
            {
                NetworkServiceUtil.Log("Socket StartWebSocketConnection Exception: " + e);
            }
            return client;
        }
        private bool IsClientUsable(ClientWebSocket client)
        {
            return client != null &&
                   client.State == WebSocketState.Open;
        }

        internal async Task<string> ReadMessage(ClientWebSocket client)
        {
            NetworkServiceUtil.Log("Socket ReadMessage: start");
            try
            {
                var buffer = new byte[4096];
                var message = new ArraySegment<byte>(buffer);
                var sb = new StringBuilder();

                while (true)
                {
                    if (!IsClientUsable(client))
                        return null;

                    WebSocketReceiveResult result;
                    try
                    {
                        result = await client.ReceiveAsync(message, CancellationToken.None);
                    }
                    catch (WebSocketException wex)
                    {
                        // Handle abrupt disconnect (EOF, server crash, timeout)
                        NetworkServiceUtil.Log("Socket closed unexpectedly: " + wex.Message);
                        return null;
                    }

                    if (result.MessageType == WebSocketMessageType.Close)
                    {
                        NetworkServiceUtil.Log("Socket closed by server (graceful)");
                        await client.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", CancellationToken.None);
                        return null;
                    }

                    sb.Append(Encoding.UTF8.GetString(buffer, 0, result.Count));

                    if (result.EndOfMessage)
                        break;
                }

                var data = sb.ToString();
                NetworkServiceUtil.Log("Socket Received: " + data);
                return data;
            }
            catch (Exception e)
            {
                NetworkServiceUtil.Log("Socket Received Exception: " + e);
                return null; // swallow to avoid crashing your GetWsData
            }
        }



        internal async Task<bool> SendMessageAsync(string message, ClientWebSocket client)
        {
            NetworkServiceUtil.Log("Socket SendMessageAsync: start" );
            try
            {
                if(client!=null)
                {
                    if(client.State==WebSocketState.Open)
                    {
                        var byteMessage = Encoding.UTF8.GetBytes(message);
                        var segmnet = new ArraySegment<byte>(byteMessage);
                        NetworkServiceUtil.Log("Socket SendMessageAsync: " + message);
                       await client.SendAsync(segmnet, WebSocketMessageType.Text, true, CancellationToken.None);
                    }
                }
            }
            catch(Exception e)
            {
                NetworkServiceUtil.Log("Socket SendMessageAsync Exception: " + e);
               
            }
            NetworkServiceUtil.Log("Socket SendMessageAsync: end");
            return true;
        }
        
        ClientWebSocket _client;
        public void Dispose()
        {
            if(_client != null)
            {
                _client.Dispose();
                _client = null;
                
                NetworkServiceUtil.Log("Socket Dispose");
            }
        //    wifiAdapter = null;
        }
    }
}
