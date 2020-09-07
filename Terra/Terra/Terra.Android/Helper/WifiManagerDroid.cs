using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Android.Content;
using Android.Net.Wifi;
using ConnectionLibrary.Interface;
using ConnectionLibrary.Network;
using Entities.Wifi;
using FlyMe.Droid.Helper;
using System.Linq;
using Terra.Droid.Helper;
using Android.OS;
using Xamarin.Forms;
using Android.App;
using Terra.Droid;
using Android.Locations;

[assembly: Xamarin.Forms.Dependency(typeof(WifiManagerDroid))]
namespace FlyMe.Droid.Helper
{
    public class WifiManagerDroid : IPlatformWifiManager
    {
        private Context context = null;
        private static WifiManager wifi;
        private WifiNetworkReceiver wifiReceiver;
        WifiManager wifiManager;
        LocationManager LocationManager;
        MainActivity mainActivity;

        public WifiManagerDroid()
        {
            this.context = Android.App.Application.Context;
            wifiManager = (WifiManager)Android.App.Application.Context.GetSystemService(Context.WifiService);
            LocationManager= (LocationManager)Android.App.Application.Context.GetSystemService(Context.LocationService);
            wifi = (WifiManager)context.GetSystemService(Context.WifiService);
            mainActivity = Forms.Context as MainActivity;
            
           // DisableWifiHotSpot();
        }

        /// <summary>
		/// Get the list of wifi in droid
        /// Start a wifi scan and register the Broadcast receiver to get the list of available Wifi Networks
		/// </summary>
        public void RequestWifiNetworks()
        {
          //  wifi.setWifiEnabled(true);
            wifiReceiver = new WifiNetworkReceiver();
            context.RegisterReceiver(wifiReceiver, new IntentFilter(WifiManager.ScanResultsAvailableAction));
            wifi.StartScan();
        }
        public async Task<bool> Connect(string _ssid, string _pwd)
        {
            var ssid = $"\"{_ssid}\"";
            var pwd = $"\"{_pwd}\"";
            WifiConfiguration wifiConfig = new WifiConfiguration();
            wifiConfig.Ssid = ssid;
            wifiConfig.PreSharedKey = pwd;
            

            
           
            // Use ID


            int netId = wifiManager.AddNetwork(wifiConfig);
            wifiManager.Disconnect();
            wifiManager.EnableNetwork(netId, true);
            wifiManager.Reconnect();
            await Task.Delay(2*1000);

            if (wifiManager.ConnectionInfo?.SSID != ssid)    
            {
                Console.WriteLine($"Cannot connect to network: {ssid}");
                return false;
            }
            return true;
        }

        public bool IsWifiEnabled()
        {
            return wifi.IsWifiEnabled;
        }

        public bool EnableWifi()
        {
           return wifi.SetWifiEnabled(true);
        }

        public bool DisableWifi()
        {
            return wifi.SetWifiEnabled(false);
        }
        private void TurnOnHotspot()
        {
            wifiManager.StartLocalOnlyHotspot(new WifiHotspotReservation(mainActivity), new Handler());
        }

        public void DisableWifiHotSpot()
        {
            if (mainActivity.mReservation != null)
            {
                mainActivity.mReservation.Close();
            }
        }
        public void NavigateLocationSetting()
        {
            if(!LocationManager.IsProviderEnabled(LocationManager.GpsProvider))
            {
                Xamarin.Forms.Forms.Context.StartActivity(new Android.Content.Intent(Android.Provider.Settings.ActionLocat‌​ionSourceSettings));
            }
            
        }
        class WifiNetworkReceiver : BroadcastReceiver
        {
            public List<Wifi> WiFiNetworks;

            /// <summary>
            /// Once the scan is completed, the OnReceive method will receive available WiFiNetworks
            /// </summary>
            /// <param name="context"></param>
            /// <param name="intent"></param>
            public override void OnReceive(Context context, Intent intent)
            {
                WiFiNetworks = new List<Wifi>();
                IList<ScanResult> scanwifinetworks = wifi.ScanResults;
                foreach (ScanResult wifinetwork in scanwifinetworks)
                {
                    Wifi wifi = new Wifi();
                    wifi.name = wifinetwork.Ssid;
                    wifi.ipAdrs = wifinetwork.Bssid;
                    WiFiNetworks.Add(wifi);
                    System.Diagnostics.Debug.WriteLine("-------------------------");
                }
                var instance = WifiAdapter.Instance;
                if(instance!=null)
                {
                    instance.OnReceiveAvailableNetworks(WiFiNetworks);
                }
            }
        }
    }
}
