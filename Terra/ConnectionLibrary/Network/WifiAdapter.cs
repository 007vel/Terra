using System;
using System.Collections.Generic;
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
        System.Timers.Timer _timer = new System.Timers.Timer(10*1000);
        ClientWebSocket client = new ClientWebSocket();
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

        public void OnRequestAvailableNetworks()
        {
            FormWifiManager.RequestWifiNetworks();
        }
        public async Task<bool> ConnectToWifi(string ssid, string pwd)
        {
           var res= await FormWifiManager.Connect(ssid,pwd);
            return res;
        }
        internal async void StartWebSocketConnection(string url)
        {
            try
            {
                var cts = new CancellationTokenSource();
                await client.ConnectAsync(new Uri(url), cts.Token);
            }
            catch(Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);
            }
        }
        internal async Task<string> ReadMessage()
        {
            try
            {
                WebSocketReceiveResult result;
                string data = string.Empty;
                var message = new ArraySegment<byte>(new byte[4096]);
                do
                {
                    result = await client.ReceiveAsync(message, CancellationToken.None);
                    if (result.MessageType != WebSocketMessageType.Text)
                        break;
                    var messageBytes = message.Skip(message.Offset).Take(result.Count).ToArray();
                    string receivedMessage = Encoding.UTF8.GetString(messageBytes);
                    data += receivedMessage;
                    Console.WriteLine("Received: {0}", receivedMessage);
                }
                while (!result.EndOfMessage);
                return data;
            }
            catch(Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);
            }
            return string.Empty;
        }

        internal async void SendMessageAsync(string message)
        {
            var byteMessage = Encoding.UTF8.GetBytes(message);
            var segmnet = new ArraySegment<byte>(byteMessage);
            await client.SendAsync(segmnet, WebSocketMessageType.Text, true, CancellationToken.None);
        }
        internal async void StopWebSocketConnection()
        {
            try
            {
                if(client!=null)
                {
                    if (client.State != WebSocketState.Closed)
                        await client.CloseAsync(WebSocketCloseStatus.Empty, String.Empty, CancellationToken.None);
                }
            }
            finally
            {
                client.Dispose();
            }
        }
        public void Dispose()
        {
            if(client!=null)
            {
                client.Dispose();
                client = null;
            }
        }
    }
}
