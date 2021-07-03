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
using EspTouchMultiPlatformLIbrary;
using Terra.Core.common;
using Terra.Core.Views.PopUpPages;
using Rg.Plugins.Popup.Extensions;
using ConnectionLibrary.Interface;

namespace Terra.Core.ViewModels
{
    public class NetworkViewModel : ViewModelBase, IDialog
    {
        public ICommand SelectionChanged => new Command<object>(OnSelectionChanged);
        public ICommand ScanWifi => new Command(OnScanWifiClick);
        public ICommand AboutCommand => new Command(AboutClicked);
        Dictionary<string, string> wifiPwdList = new Dictionary<string, string>();
        public Wifi LastSelectedItem = null;
        WifiAdapter wifiAdapter;
        Wifi SelectedWifi = null;
        public static readonly string WIFI_ID = "terradev";
        bool isFirstTimeLaunch = true;
        public NetworkViewModel()
        {
            Init();
        }

        public async void Init()
        {
            IsScanning = true;
            wifiAdapter = WifiAdapter.Instance;
           
            var locationPermissionAll= await PermissionHelper.Instance.CheckAndRequestPermissionAsync(new Permissions.LocationAlways());
            var locationPermissionWhenuse = await PermissionHelper.Instance.CheckAndRequestPermissionAsync(new Permissions.LocationWhenInUse());
            if (locationPermissionAll == PermissionStatus.Granted || locationPermissionWhenuse == PermissionStatus.Granted)
            {
                if(!wifiAdapter.IsGpsEnabled())
                {
                    wifiAdapter.FormWifiManager.OpenSetting(Entities.Common.MobileSetting.Location);
                }
                var popupres = false;
                if (!wifiAdapter.FormWifiManager.IsWifiEnabled())
                {
                    popupres = await App.Current.MainPage.DisplayAlert(title: "Use Wifi?", message: "The app wants to turn on your device wifi", "YES", "NO");
                    
                }
                if (popupres)
                {
                    wifiAdapter.FormWifiManager.TurnOffWifiHotSpot();
                    wifiAdapter.FormWifiManager.EnableWifi();
                }
                //Wifi ssid should be in lower case
                wifiPwdList.Add("sowmiya", "Avanthika@07");
                wifiPwdList.Add("home", "9943157172");
                wifiPwdList.Add("galaxy", "9943157172");
                wifiPwdList.Add("tab", "9786297172");
                wifiPwdList.Add("lap", "Sakthi@123");
              //  wifiPwdList.Add("terradev", "9943157172");
                wifiPwdList.Add("terradev", "12345678");
                wifiPwdList.Add("AndroidWifi", "9943157172");
                wifiPwdList.Add("karthi", "9943157172");
                wifiPwdList.Add("sathya", "9943157172");
                wifiPwdList.Add("Sakthivel’s MacBook Pro", "9943157172");
                wifiPwdList.Add("TARS 2.4G", "kjhgfjkhv");
                IsWifiLoading = true;
                MessagingCenter.Subscribe<WifiAdapter, List<Wifi>>(this, "WifiAdapter", (sender, arg) =>
                {
                    WifiAdapter_PropertyChanged(arg);
                    IsWifiLoading = false;
                    if(!isFirstTimeLaunch)
                    {
                        IsScanning = false;
                    }
                    else
                    {
                        isFirstTimeLaunch = false;
                    }
                    
                });
                wifiAdapter.OnRequestAvailableNetworks();

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
        bool isScanning;
        public bool IsScanning
        {
            get
            {
                return isScanning;
            }
            set
            {
                isScanning = value;
                OnPropertyChanged("IsScanning");
            }
        }
        public INavigation PageNavigation
        {
            get; set;
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

        string deviceConnectStatus = "   ";
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
            IsScanning = true;
            wifiAdapter.OnRequestAvailableNetworks();
        }

        private async void OnSelectionChanged(object obj)
        {
            if (obj != null)
            {
                var wifi = (Wifi)obj;
                SelectedWifi = wifi;
                SelectedItem = wifi;
                if (wifi != null)
                {
                    DeviceConnectStatus = "Connecting...";
                    OnPropertyChanged("DeviceConnectStatus");
                    await Task.Delay(500);
                    SelectedItem.LabelTextColor = Color.FromHex("#EF4736");
                    SelectedItem.Image = "terra_spray_orange_device_03";
                    if (LastSelectedItem != null)
                    {
                        LastSelectedItem.LabelTextColor = Color.FromHex("#989da0");
                        LastSelectedItem.Image = "terra_spray_device_03";
                    }
                    DeviceName = wifi.name;
                    
                    string pwd = string.Empty;
                    foreach(var item in wifiPwdList)
                    {
                        if(DeviceName.ToLower().Contains(item.Key.ToLower()))
                        {
                            pwd = item.Value;
                            break;
                        }
                    }
                    await ConnectNetwork(wifi.ssid, pwd);
                }
            }
            else
            {
                SelectedItem = null;
            }

            LastSelectedItem = SelectedItem;
        }

        private async Task<bool> ConnectNetwork(string ssid, string pwd)
        {
            NetworkServiceUtil.Log("DeviceDetailsViewModel ConfigDevice : " + DeviceName + "    " + pwd);
            bool wifiConnected = false;
            if (!string.IsNullOrEmpty(DeviceName) && !string.IsNullOrEmpty(pwd))
            {
                var Fssid = $"\"{ssid}\"";
                if (DependencyService.Get<IPlatformWifiManager>().GetSsId() == Fssid)
                {
                    wifiConnected = true;
                }

                if(!wifiConnected)
                {
                    if (await wifiAdapter.ConnectToWifi(ssid, pwd))
                    {
                        wifiConnected = true;
                        DependencyService.Get<IPlatformWifiManager>().ForceWifiOverCellular();
                        NetworkServiceUtil.Log("DeviceDetailsViewModel ConfigDevice success: " + DeviceName + "    " + pwd);
                        
                        SelectedWifi.isSelected = true;
                    }
                    else
                    {
                        DeviceConnectStatus = "Failed to Connect";
                        SelectedWifi.isSelected = false;
                    }
                }
                
            }
            else
            {
                DeviceConnectStatus = "Incorrect Password";
                SelectedWifi.isSelected = false;
            }

            if(wifiConnected)
            {
                DeviceConnectStatus = "Connected";
                await Shell.Current.GoToAsync("DeviceDetailsPage");
            }
            return true;
        }

        ISmartConfigTask smartconfig;
        public async Task<bool> ConfigDevice(String Ssid, String bssid, String Passphrase)
        {
            //  var Ssid = $"\"{_Ssid}\"";
            //  var Passphrase = $"\"{_Passphrase}\"";
         //   string bssid = wifiAdapter.GetBssid();
            smartconfig = DependencyService.Get<ISmartConfigHelper>().CreatePlatformTask();
          //  return true;
            smartconfig.SetSmartConfigTask(Ssid, bssid, Passphrase,false,60);
            NetworkServiceUtil.Log("DeviceDetailsViewModel ConfigDevice : " + Ssid+"    "+ Passphrase);
            string Message = "running configuration";
            await Task.Run(() =>
            {
                var result = smartconfig.executeForResult();
                Message = Message + "Device address set up: " + result.getInetAddress() + "\r\n";
                NetworkServiceUtil.Log("DeviceDetailsViewModel ConfigDevice : " + Message);
                return true;
            });
            return false;
        }

        private async void AboutClicked()
        {
            await Shell.Current.GoToAsync("AboutPage");
        }

        public string getValue()
        {
            return string.Empty;
        }

        public void setValue(string val)
        {
            ConnectNetwork(SelectedWifi.ssid, val.Trim());
        }

        public string getTitle()
        {
            return "Enter Wifi password";
        }

        public Keyboard getkeyBoardType()
        {
            return Keyboard.Default;
        }
    }
}
