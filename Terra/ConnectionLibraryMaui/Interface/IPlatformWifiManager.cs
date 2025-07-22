using System;
using System.Threading.Tasks;
using Entities.Common;

namespace ConnectionLibrary.Interface
{
    public interface IPlatformWifiManager
    {
        void RequestWifiNetworks();
        Task<bool> Connect(string ssid, string pwd);

        void DisconnectWifi();

        bool IsWifiEnabled();

        bool EnableWifi();

        bool DisableWifi();

        void TurnOffWifiHotSpot();

        void TurnOnHotspot();

        bool IsHotSpotEnabled();

        void ForceWifiOverCellular();

        void OpenSetting(MobileSetting mobileSetting);

        string GetBssId();

        string GetSsId();

        bool IsGpsEnable();

    }
}
