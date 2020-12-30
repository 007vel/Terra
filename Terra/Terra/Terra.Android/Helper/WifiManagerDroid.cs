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
using Entities.Common;
using Android.Net;
using System.Threading;
using Android.Net.Wifi;

[assembly: Xamarin.Forms.Dependency(typeof(WifiManagerDroid))]
namespace FlyMe.Droid.Helper
{
    public class WifiManagerDroid : IPlatformWifiManager
    {
        MobileHelper mobileHelper = new MobileHelper();
        private Context context = null;
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
            mainActivity = Forms.Context as MainActivity;
            wifiReceiver = new WifiNetworkReceiver(wifiManager);
        }

        /// <summary>
		/// Get the list of wifi in droid
        /// Start a wifi scan and register the Broadcast receiver to get the list of available Wifi Networks
		/// </summary>
        public void RequestWifiNetworks()
        {
            
            context.RegisterReceiver(wifiReceiver, new IntentFilter(WifiManager.ScanResultsAvailableAction));
            context.RegisterReceiver(wifiReceiver, new IntentFilter(WifiManager.NetworkStateChangedAction));
            wifiManager.StartScan();
        }

        /// <summary>
        /// Connect to wifi using ssid and password
        /// </summary>
        /// <param name="_ssid"></param>
        /// <param name="_pwd"></param>
        /// <returns></returns>
        public async Task<bool> Connect(string _ssid, string _pwd)
        {

            Connect1(_ssid, _pwd);
            //   wifiManager = (WifiManager)Android.App.Application.Context.GetSystemService(Context.WifiService);
            
            string ssid = "";
            string pwd = "";
            ssid = $"\"{_ssid}\"";
            pwd = $"\"{_pwd}\"";
            //mobileHelper.Log("WifiConfiguration : " + ssid +"     "+ pwd);


            //var list = wifiManager.ConfiguredNetworks;
            //foreach(var config in list)
            //{
            //    wifiManager.RemoveNetwork(config.NetworkId);
            //}

            //var currentConnection = wifiManager.ConnectionInfo;
            //if(currentConnection!=null)
            //{
            //    wifiManager.DisableNetwork(currentConnection.NetworkId);
            //}

            //WifiConfiguration wifiConfig = new WifiConfiguration();
            //wifiConfig.Ssid = ssid;
            //wifiConfig.PreSharedKey = pwd;
            //wifiConfig.StatusField = WifiStatus.Enabled;
            //wifiConfig.AllowedKeyManagement.Set((int)Android.Net.Wifi.KeyManagementType.None);
            //wifiConfig.AllowedGroupCiphers.Set((int)Android.Net.Wifi.GroupCipherType.Wep40);
            //int netId = wifiManager.AddNetwork(wifiConfig);
            //wifiManager.EnableNetwork(netId, true);
            //wifiManager.DisableNetwork(wifiManager.ConnectionInfo.NetworkId);
            //wifiManager.EnableNetwork(wifiManager.ConnectionInfo.NetworkId, true);

            DateTime timeSpan = DateTime.Now;
            WifiInfo _network = null;

            while (true)
            {
                string msg = wifiManager.ConnectionInfo?.SSID;
                mobileHelper.Log("2st while: "+ msg);
                System.Diagnostics.Debug.WriteLine(msg);
                _network = wifiManager.ConnectionInfo;
                //Thread.Sleep(1000);
                await Task.Delay(1 * 1000);

                if (_network.SupplicantState == SupplicantState.Completed && _network.SSID == ssid)
                {
                    break;
                }
                else if(DateTime.Now.Subtract(timeSpan).TotalSeconds < 20)
                {
                    continue;
                }
                else
                {
                    _network = null;
                    break;
                }
                
            }
            //await Task.Delay(2 * 1000);
            if (_network==null)    
            {
                System.Diagnostics.Debug.WriteLine("ConnectionInfo:"+ wifiManager.ConnectionInfo?.SSID);
                mobileHelper.Log("_network false:" + wifiManager.ConnectionInfo?.SSID);
                return false;
            }
            mobileHelper.Log("_network true:" + wifiManager.ConnectionInfo?.SSID);
            return true;
        }

        ////////////////////////////////////////////
        ConnectivityManager _wifiManager = Android.App.Application.Context.GetSystemService(Context.ConnectivityService) as ConnectivityManager;
        NetworkCallback _networkCallback = null;
        public async void Connect1(string _ssid, string _pwd)
        {
            string ssid = "";
            string pwd = "";
            ssid = $"\"{_ssid}\"";
            pwd = $"\"{_pwd}\"";
            var specifier = new WifiNetworkSpecifier.Builder()
                           .SetSsid(_ssid)
                           .SetWpa2Passphrase(_pwd)
                           .Build();

            var request = new NetworkRequest.Builder()
                .AddTransportType(TransportType.Wifi) // we want WiFi
                .RemoveCapability(NetCapability.Internet) // Internet not required
                .SetNetworkSpecifier(specifier) // we want _our_ network
                .Build();

            NetworkCallback _callback = new NetworkCallback(_wifiManager);
            _wifiManager.RequestNetwork(request, _callback);

        }

        public void DisconnectWifi()
        {
            if (_networkCallback is null || _wifiManager is null)
                return;
            if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.Q)
            {
                _wifiManager.UnregisterNetworkCallback(_networkCallback);
            }


        }
        //////////////////////////////////////////

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool IsWifiEnabled()
        {
            return wifiManager.IsWifiEnabled;
        }

        public bool IsGpsEnable()
        {
            LocationManager locationManager = (LocationManager)Android.App.Application.Context.GetSystemService(Context.LocationService);
            return locationManager.IsProviderEnabled(LocationManager.GpsProvider);
        }
      
        public bool EnableWifi()
        {
            Intent panelIntent = new Intent(Android.Provider.Settings.Panel.ActionWifi);
            mainActivity.StartActivityForResult(panelIntent,1);
            return wifiManager.SetWifiEnabled(true);
        }

        public bool DisableWifi()
        {
            return wifiManager.SetWifiEnabled(false);
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

        public string GetSsId()
        {
            if (wifiManager != null)
            {
                return wifiManager.ConnectionInfo.SSID;
            }
            else
            {
                return "";
            }
        }

        class WifiNetworkReceiver : BroadcastReceiver
        {
            public List<Wifi> WiFiNetworks;
            WifiManager RcvWifiManager;
            public WifiNetworkReceiver(WifiManager wifiManager)
            {
                RcvWifiManager = wifiManager;
            }

            /// <summary>
            /// Once the scan is completed, the OnReceive method will receive available WiFiNetworks
            /// </summary>
            /// <param name="context"></param>
            /// <param name="intent"></param>
            public override void OnReceive(Context context, Intent intent)
            {
                WiFiNetworks = new List<Wifi>();
                IList<ScanResult> scanwifinetworks = RcvWifiManager.ScanResults;
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


        private class NetworkCallback : ConnectivityManager.NetworkCallback
        {
            private ConnectivityManager _conn;
            public Action<Network> NetworkAvailable { get; set; }
            public Action NetworkUnavailable { get; set; }

            public NetworkCallback(ConnectivityManager connectivityManager)
            {
                _conn = connectivityManager;
            }

            public override void OnAvailable(Network network)
            {
                base.OnAvailable(network);
                // Need this to bind to network otherwise it is connected to wifi 
                // but traffic is not routed to the wifi specified
                _conn.BindProcessToNetwork(network);
                NetworkAvailable?.Invoke(network);
            }

            public override void OnUnavailable()
            {
                base.OnUnavailable();

                NetworkUnavailable?.Invoke();
            }
        }
    }
}
