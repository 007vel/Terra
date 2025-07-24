using EspTouchMultiPlatformLIbrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terra_Maui.Platforms.Android.Helper;

[assembly: Dependency(typeof(SmartConfig_Droid))]
namespace Terra_Maui.Platforms.Android.Helper
{
    public class SmartConfig_Droid : ISmartConfigHelper
    {
        public SmartConfig_Droid()
        {
        }

        public ISmartConfigTask CreatePlatformTask()
        {
            return new SmartConfigTask_Droid();
        }
    }
}
