using System;
using System.Threading.Tasks;
using Entities.Common;

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

        void OpenSetting(MobileSetting mobileSetting);

        string GetBssId();
        bool IsGpsEnable();

    }
}
