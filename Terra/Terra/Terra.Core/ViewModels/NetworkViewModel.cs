using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using ConnectionLibrary.Network;
using Entities.Wifi;
using Terra.Service;
using Unity;
using Xamarin.Forms;
using System.Linq;
using Terra.Core.Utils;
using Terra.Core.Helper;
using Xamarin.Essentials;

namespace Terra.Core.ViewModels
{
    public class NetworkViewModel : ViewModelBase
    {
        public ICommand SelectionChanged => new Command<object>(OnSelectionChanged);
        public ICommand ScanWifi => new Command(OnScanWifiClick);
        Dictionary<string, string> wifiPwdList = new Dictionary<string, string>();
        public Wifi LastSelectedItem = null;
        WifiAdapter wifiAdapter;
        public NetworkViewModel()
        {
            Init();
        }

        private async void Init()
        {
            wifiAdapter = WifiAdapter.Instance;
           
            var locationPermission= await PermissionHelper.Instance.CheckAndRequestPermissionAsync(new Permissions.LocationWhenInUse());
            if(locationPermission==PermissionStatus.Granted)
            {
                if (!wifiAdapter.FormWifiManager.IsWifiEnabled())
                {
                    var popupres = await App.Current.MainPage.DisplayAlert(title: "Use Wifi?", message: "The app wants to turn on your device wifi", "YES", "NO");
                    if (popupres)
                    {
                        wifiAdapter.FormWifiManager.NavigateLocationSetting();
                        wifiAdapter.FormWifiManager.DisableWifiHotSpot();
                        wifiAdapter.FormWifiManager.EnableWifi();
                    }
                }
                //Wifi ssid should be in lower case
                wifiPwdList.Add("sowmiya", "Avanthika@07");
                wifiPwdList.Add("home", "9943157172");
                wifiPwdList.Add("galaxy", "9943157172");
                wifiPwdList.Add("tab", "9786297172");
                wifiPwdList.Add("lap", "Sakthi@123");
                wifiPwdList.Add("terradev", "12345678");
                IsWifiLoading = true;
                MessagingCenter.Subscribe<WifiAdapter, List<Wifi>>(this, "WifiAdapter", (sender, arg) =>
                {
                    WifiAdapter_PropertyChanged(arg);
                    IsWifiLoading = false;
                });
                wifiAdapter.OnRequestAvailableNetworks();
                //      Utils.Utils.Toast("title", "OnRequestAvailableNetworks");
            }
        }
        string wifiCountString = "Wifi list count: ";
        private void WifiAdapter_PropertyChanged(List<Wifi> arg)
        {
            if (arg == null) return;
            var _networkList = new List<Wifi>(arg);
            if (_networkList != null)
            {
                _networkList.ForEach(i =>
                {
                    i.Image = "terra_spray_device_03";
                    i.LabelTextColor = Color.FromHex("#989da0"); ;
                });
                NetworkList = new ObservableCollection<Wifi>(_networkList);
                if(NetworkList.Count>0)
                {
                    WifiCount = wifiCountString + NetworkList.Count.ToString();
                }
            }
        }

        ObservableCollection<Wifi> networkList;
        public ObservableCollection<Wifi> NetworkList
        {
            get
            {
                return networkList;
            }
            set
            {
                networkList = value;
                OnPropertyChanged("NetworkList");
            }
        }

        Wifi selectedItem;
        public Wifi SelectedItem
        {
            get
            {
                return selectedItem;
            }
            set
            {
                selectedItem = value;
                OnPropertyChanged("SelectedItem");
            }
        }
        bool isWifiLoading;
         public bool IsWifiLoading
        {
            get
            {
                return isWifiLoading;
            }
            set
            {
                isWifiLoading = value;
                OnPropertyChanged("IsWifiLoading");
            }
        }

        string wifiCount = "Wifi list count: ";
        public string WifiCount
        {
            get
            {
                return wifiCount;
            }
            set
            {
                wifiCount = value;
                OnPropertyChanged("WifiCount");
            }
        }
        string deviceName = "";
        public string DeviceName
        {
            get
            {
                return deviceName;
            }
            set
            {
                deviceName = value;
                OnPropertyChanged("DeviceName");
            }
        }

        string deviceConnectStatus = "";
        public string DeviceConnectStatus
        {
            get
            {
                return deviceConnectStatus;
            }
            set
            {
                deviceConnectStatus = value;
                OnPropertyChanged("DeviceConnectStatus");
            }
        }
        void OnScanWifiClick()
        {
            wifiAdapter.OnRequestAvailableNetworks();
        }
        private async void OnSelectionChanged(object obj)
        {
            if (obj != null)
            {
                var wifi = (Wifi)obj;
                SelectedItem = wifi;
                if (wifi != null)
                {
                    //return if last selected item is same current selection, for color higlight
                    if (LastSelectedItem != null && LastSelectedItem.ipAdrs == wifi.ipAdrs)
                    {
                        //return;
                    }
                    SelectedItem.LabelTextColor = Color.FromHex("#EF4736");
                    SelectedItem.Image = "terra_spray_orange_device_03";
                    if (LastSelectedItem != null)
                    {
                        LastSelectedItem.LabelTextColor = Color.FromHex("#989da0");
                        LastSelectedItem.Image = "terra_spray_device_03";
                    }
                    DeviceName = wifi.name;
                    DeviceConnectStatus = "Connecting...";
                    string pwd = string.Empty;
                    foreach(var item in wifiPwdList)
                    {
                        if(DeviceName.ToLower().Contains(item.Key))
                        {
                            pwd = item.Value;
                            break;
                        }
                    }
                    if (await ConnectNetwork(DeviceName, pwd))
                    {
                        DeviceConnectStatus = "Connected";
                        wifi.isSelected = true;
                        await Shell.Current.GoToAsync("DeviceDetailsPage");
                    }
                    else
                    {
                        DeviceConnectStatus = "Failed to Connect";
                        wifi.isSelected = false;
                    }

                }
            }
            else
            {
                SelectedItem = null;
            }

            LastSelectedItem = SelectedItem;
            //#ef5145 orange
            //#989da0 gray
            //#373535 4,4
        }

        private async Task<bool> ConnectNetwork(string ssid, string pwd)
        {
            //await Task.Delay(1000*1);
            var res = await wifiAdapter.ConnectToWifi(ssid, pwd);
            return res;
        }


    }
}
