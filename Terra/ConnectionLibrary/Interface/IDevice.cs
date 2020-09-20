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
        Task<string> SetScheduler(string schedule);
        Task<string> GetDeviceInfo(DeviceInfoRequest deviceInfoRequest);
    }
}
