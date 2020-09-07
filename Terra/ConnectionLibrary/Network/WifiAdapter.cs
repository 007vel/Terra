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
    public class WifiAdapter 
    {
        static WifiAdapter wifiAdapter=null;
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
            Wifi wifi1 = new Wifi();
            wifi1.name = "Terra Spray Office";
            wifi1.ipAdrs = "121.22.421.21";
            Wifi wifi2 = new Wifi();
            wifi2.name = "Terra Spray Conf";
            wifi2.ipAdrs = "196.12.121.210";
            Wifi wifi3 = new Wifi();
            wifi3.name = "Terra Spray hall";
            wifi3.ipAdrs = "196.126.121.210";
         //   OnReceiveAvailableNetworks(new List<Wifi>() { wifi1, wifi2, wifi3 });
        }

        public async Task<bool> ConnectToWifi(string ssid, string pwd)
        {
           var res= await FormWifiManager.Connect(ssid,pwd);
            return res;
        }
        CancellationTokenSource cts = new CancellationTokenSource();
        ClientWebSocket client = new ClientWebSocket();
        public async void WebSocketInit()
        {
            try
            {
                var cts = new CancellationTokenSource();
                await client.ConnectAsync(new Uri("ws://4GKYN13-IN-LE01:9898/"), cts.Token);
                // UpdateClientState();

                await Task.Factory.StartNew(async () =>
                {
                    while (true)
                    {
                        await ReadMessage();
                    }
                }, cts.Token, TaskCreationOptions.LongRunning, TaskScheduler.Default);
                SendMessageAsync("This is from Mobile app:" + DateTime.Now.ToString());
            }
            catch(Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);
            }

        }
        async Task ReadMessage()
        {
            WebSocketReceiveResult result;
            var message = new ArraySegment<byte>(new byte[4096]);
            do
            {
                result = await client.ReceiveAsync(message, cts.Token);
                if (result.MessageType != WebSocketMessageType.Text)
                    break;
                var messageBytes = message.Skip(message.Offset).Take(result.Count).ToArray();
                string receivedMessage = Encoding.UTF8.GetString(messageBytes);
                Console.WriteLine("Received: {0}", receivedMessage);
            }
            while (!result.EndOfMessage);
        }

        

        public async void SendMessageAsync(string message)
        {
            var byteMessage = Encoding.UTF8.GetBytes(message);
            var segmnet = new ArraySegment<byte>(byteMessage);

            await client.SendAsync(segmnet, WebSocketMessageType.Text, true, cts.Token);
        }

    }
}
