using System;
using System.Collections.ObjectModel;
using ConnectionLibrary.Interface;
using ConnectionLibrary.Network;
using Entities;
using Entities.Common;
using Xamarin.Forms;

namespace Terra.Core.ViewModels
{
    public class MobileConnectionVerificationViewModel : ViewModelBase
    {
        ObservableCollection<SettingConfig> list = new ObservableCollection<SettingConfig>();
        public MobileConnectionVerificationViewModel()
        {
            BuildSettingsList();
        }

        public ObservableCollection<SettingConfig> settingsList;
        public ObservableCollection<SettingConfig> SettingsList
        {
            get
            {
                return settingsList;
            }
            set
            {
                settingsList = value;
                OnPropertyChanged("SettingsList");
            }
        }

        private void BuildSettingsList()
        {
            AddLocationSetting();
            AddHotspotSetting();
            AddWifiSetting();
            AddMobiledataSetting();
            SettingsList = list;

        }
        private void AddLocationSetting()
        {
            SettingConfig location = new SettingConfig();
            location.id = "1";
            location.Name = "Open GPS";
            location.ButtonName = "Open";
            location.IsEnabled = WifiAdapter.Instance.IsGpsEnabled();
            list.Add(location);
        }

        private void AddHotspotSetting()
        {
            var list = new ObservableCollection<SettingConfig>();
            SettingConfig hotspot = new SettingConfig();
            hotspot.id = "2";
            hotspot.Name = "Turn Off Hotspot";
            hotspot.ButtonName = "Open";
            hotspot.IsEnabled = DependencyService.Get<IPlatformWifiManager>().IsHotSpotEnabled();
            list.Add(hotspot);
        }

        private void AddWifiSetting()
        {
            SettingConfig wifi = new SettingConfig();
            wifi.id = "3";
            wifi.Name = "Turn On Wifi";
            wifi.ButtonName = "Open";
            wifi.IsEnabled = DependencyService.Get<IPlatformWifiManager>().IsWifiEnabled();
            list.Add(wifi);
        }

        private void AddMobiledataSetting()
        {
            SettingConfig mobileData = new SettingConfig();
            mobileData.id = "4";
            mobileData.Name = "Turn Off Mobile data";
            mobileData.ButtonName = "Open";
            mobileData.IsEnabled = DependencyService.Get<IPlatformWifiManager>().IsHotSpotEnabled();
            list.Add(mobileData);
        }
    }
}
