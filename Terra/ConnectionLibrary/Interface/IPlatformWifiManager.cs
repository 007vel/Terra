using System;
using System.Threading.Tasks;

namespace ConnectionLibrary.Interface
{
    public interface IPlatformWifiManager
    {
        void RequestWifiNetworks();
        Task<bool> Connect(string ssid, string pwd);

        bool IsWifiEnabled();

        bool EnableWifi();

        bool DisableWifi();

        void DisableWifiHotSpot();

        void NavigateLocationSetting();

        string GetBssId();

    }
}
