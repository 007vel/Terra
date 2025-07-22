using System;
using System.Collections.Generic;
using Entities.Wifi;

namespace ConnectionLibrary.Network
{
    public interface IWifiManager
    {
        void OnRequestAvailableNetworks();
        void OnReceiveAvailableNetworks(List<Wifi> wifi);
    }
}
