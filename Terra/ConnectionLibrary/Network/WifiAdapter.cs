﻿using System;
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
using Xamarin.Forms;

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
                await client.ConnectAsync(new Uri(url), CancellationToken.None);
            }
            catch(Exception e)
            {
                NetworkServiceUtil.Log("Socket StartWebSocketConnection Exception: " + e);
            }
            return client;
        }
       
        internal async Task<string> ReadMessage(ClientWebSocket client)
        {
           // Thread.Sleep(2100);
            NetworkServiceUtil.Log("Socket ReadMessage: start");
            try
            {
                NetworkServiceUtil.Log("Socket ReadMessage: 1");
                WebSocketReceiveResult result;
                string data = string.Empty;
                NetworkServiceUtil.Log("Socket ReadMessage: 2");
               // var rcvBytes = new byte[128];
               // var message = new ArraySegment<byte>(rcvBytes);
                var message = new ArraySegment<byte>(new byte[4096]);
                NetworkServiceUtil.Log("Socket ReadMessage: 3");
                bool IsIntiger;
                do
                {
                    var _result = client.ReceiveAsync(message, CancellationToken.None);
                    NetworkServiceUtil.Log("Socket ReadMessage: 3.1");
                    //   result = _result!=null? _result.Result:null;

                    result = _result.Result;

                    NetworkServiceUtil.Log("Socket ReadMessage: 4");
                    // if (result.MessageType != WebSocketMessageType.Text)
                    //   break;
                    var messageBytes = message.Skip(message.Offset).Take(result.Count).ToArray();
                    NetworkServiceUtil.Log("Socket ReadMessage: 5");
                    string receivedMessage = Encoding.UTF8.GetString(messageBytes);
                    NetworkServiceUtil.Log("Socket ReadMessage: 6");
                    data = receivedMessage;
                    NetworkServiceUtil.Log("Socket ReadMessage: 7");
                    NetworkServiceUtil.Log("Socket appendMsg: " + receivedMessage);
                    int a;
                    IsIntiger = int.TryParse(data, out a);
                }
                while (result!=null && !result.EndOfMessage || IsIntiger || string.IsNullOrEmpty(data));
                NetworkServiceUtil.Log("Socket Received: " + data);
                return data;
            }
            catch(Exception e)
            {
                NetworkServiceUtil.Log("Socket Received Exception: " + e);
                throw;
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
                        client.SendAsync(segmnet, WebSocketMessageType.Text, true, CancellationToken.None).Wait();
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
