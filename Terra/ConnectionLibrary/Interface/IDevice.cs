using Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ConnectionLibrary.Interface
{
   public interface IDevice
    {
        Task<string> GetScheduler(DeviceInfoRequest deviceInfoRequest);
        Task<bool> SetScheduler(string schedule);
        Task<string> GetDeviceInfo(DeviceInfoRequest deviceInfoRequest);
        Task<bool> SetDeviceInfo(DeviceInfoRequest deviceInfoRequest);
        Task<bool> SetDeviceConfig(Config config);
        Task<bool> SetDeviceDemo(DemoInfo config);
    }
}
