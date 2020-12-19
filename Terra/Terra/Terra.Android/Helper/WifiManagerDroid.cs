﻿using System;
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
using Entities.Common;
using Android.Net;

[assembly: Xamarin.Forms.Dependency(typeof(WifiManagerDroid))]
namespace FlyMe.Droid.Helper
{
    public class WifiManagerDroid : IPlatformWifiManager
    {
        MobileHelper mobileHelper = new MobileHelper();
        private Context context = null;
        private static WifiManager wifi;
        private WifiNetworkReceiver wifiReceiver;
        WifiManager wifiManager;
        LocationManager LocationManager;
        MainActivity mainActivity;
        bool isHotspotEnabled = false;

        public WifiManagerDroid()
        {
            this.context = Android.App.Application.Context;
            wifiManager = (WifiManager)Android.App.Application.Context.GetSystemService(Context.WifiService);
            LocationManager= (LocationManager)Android.App.Application.Context.GetSystemService(Context.LocationService);
            wifi = (WifiManager)context.GetSystemService(Context.WifiService);
            mainActivity = Forms.Context as MainActivity;
            
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
        /// <summary>
        /// Connect to wifi using ssid and password
        /// </summary>
        /// <param name="_ssid"></param>
        /// <param name="_pwd"></param>
        /// <returns></returns>
        public async Task<bool> Connect(string _ssid, string _pwd)
        {
            wifiManager = (WifiManager)Android.App.Application.Context.GetSystemService(Context.WifiService);
           //  return true;
            string ssid = "";
            string pwd = "";
            ssid = $"\"{_ssid}\"";
            pwd = $"\"{_pwd}\"";
            mobileHelper.Log("WifiConfiguration : " + ssid +"     "+ pwd);

            WifiConfiguration wifiConfig = new WifiConfiguration();
            wifiConfig.Ssid = ssid;
            wifiConfig.PreSharedKey = pwd;
            wifiConfig.StatusField = WifiStatus.Enabled;

            var list = wifiManager.ConfiguredNetworks;
            foreach(var config in list)
            {
                wifiManager.RemoveNetwork(config.NetworkId);
            }
            int netId = wifiManager.AddNetwork(wifiConfig);
            wifiManager.Disconnect();
            wifiManager.EnableNetwork(netId, true);
            wifiManager.SaveConfiguration();
            wifiManager.Reconnect();
            await Task.Delay(2*1000);
            TimeSpan timeSpan = DateTime.Today.TimeOfDay;
           
            timeSpan = DateTime.Today.TimeOfDay;
            WifiInfo _network = null;
            // Check 1 minute for ConnectionInfo
            wifiManager = (WifiManager)Android.App.Application.Context.GetSystemService(Context.WifiService);
            while (true)
            {
                mobileHelper.Log("2st while: "+wifiManager.ConnectionInfo?.SSID);
                _network = wifiManager.ConnectionInfo;

                if(_network.SSID == ssid)
                {
                    break;
                }
                else if(DateTime.Today.TimeOfDay.Subtract(timeSpan).TotalSeconds < 60)
                {
                    continue;
                }
                else
                {
                    _network = null;
                    break;
                }
            }
            await Task.Delay(2 * 1000);
            if (_network==null)    
            {
                System.Diagnostics.Debug.WriteLine("ConnectionInfo:"+ wifiManager.ConnectionInfo?.SSID);
                mobileHelper.Log("_network false:" + wifiManager.ConnectionInfo?.SSID);
                return false;
            }
            mobileHelper.Log("_network true:" + wifiManager.ConnectionInfo?.SSID);
            return true;
        }

        public bool IsWifiEnabled()
        {
            return wifi.IsWifiEnabled;
        }
        public bool IsGpsEnable()
        {
            LocationManager locationManager = (LocationManager)Android.App.Application.Context.GetSystemService(Context.LocationService);
            return locationManager.IsProviderEnabled(LocationManager.GpsProvider);
        }
      
        public bool EnableWifi()
        {
           return wifi.SetWifiEnabled(true);
        }

        public bool DisableWifi()
        {
            return wifi.SetWifiEnabled(false);
        }
        public void TurnOnHotspot()
        {
            if(!isHotspotEnabled)
            {
                isHotspotEnabled = true;
                wifiManager.StartLocalOnlyHotspot(new WifiHotspotReservation(mainActivity), new Handler());
            }
        }

        public void TurnOffWifiHotSpot()
        {
            if (isHotspotEnabled)
            {
                if (mainActivity.mReservation != null)
                {
                    mainActivity.mReservation.Close();
                    isHotspotEnabled = false;
                }
            }
        }
        public void OpenSetting(MobileSetting mobileSetting)
        {
            if(mobileSetting== MobileSetting.Location)
            {
                if (!LocationManager.IsProviderEnabled(LocationManager.GpsProvider))
                {
                    Xamarin.Forms.Forms.Context.StartActivity(new Android.Content.Intent(Android.Provider.Settings.ActionLocat‌​ionSourceSettings));
                }
            }else if (mobileSetting == MobileSetting.Data)
            {
                Xamarin.Forms.Forms.Context.StartActivity(new Android.Content.Intent(Android.Provider.Settings.ActionDateSettings));
            }
        }

        public string GetBssId()
        {
            wifiManager = (WifiManager)Android.App.Application.Context.GetSystemService(Context.WifiService);
            if (wifiManager != null)
            {
                return  wifiManager.ConnectionInfo.BSSID;
            }
            else
            {
                return "";
            }
        }

        public bool IsHotSpotEnabled()
        {
            var method = wifiManager.Class.GetDeclaredMethod("getWifiApState");
            method.Accessible=true;
            int actualState = (int)method.Invoke(wifiManager,null);
            return actualState == 13 || actualState == 12;
        }


        /// <summary>
        /// Forces the wifi over cellular
        /// </summary>
        public void ForceWifiOverCellular()
        {
            //ForceCellularOverWifi();
            //return;
            ConnectivityManager connection_manager = (ConnectivityManager)context.GetSystemService(Context.ConnectivityService);

            NetworkRequest.Builder request = new NetworkRequest.Builder();
            request.AddTransportType(TransportType.Wifi);

            var callback = new ConnectivityManager.NetworkCallback();
            connection_manager.RegisterNetworkCallback(request.Build(), new CustomNetworkAvailableCallBack());
        }

        /// <summary>
        /// Forces the cellular over wifi.
        /// </summary>
        public void ForceCellularOverWifi()
        {
            ConnectivityManager connection_manager = (ConnectivityManager)context.GetSystemService(Context.ConnectivityService);

            NetworkRequest.Builder request = new NetworkRequest.Builder();
            request.AddTransportType(TransportType.Cellular);

            connection_manager.RegisterNetworkCallback(request.Build(), new CustomNetworkAvailableCallBack());
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
                    wifi.ssid = wifinetwork.Ssid;
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




        /// <summary>
        /// Custom network available call back.
        /// </summary>
        public class CustomNetworkAvailableCallBack : ConnectivityManager.NetworkCallback
        {
            public static Context _context = Android.App.Application.Context;

            ConnectivityManager connection_manager = (ConnectivityManager)_context.GetSystemService(Context.ConnectivityService);

            public override void OnAvailable(Network network)
            {
                //ConnectivityManager.SetProcessDefaultNetwork(network);    //deprecated (but works even in Android P)
                connection_manager.BindProcessToNetwork(network);           //this works in Android P
            }
        }
    }
}
