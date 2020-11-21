using System;
using EspTouchMultiPlatformLIbrary;
using Terra.Core;
using Xamarin.Forms;

[assembly: Dependency(typeof(Terra.Droid.Helper.SmartConfig_Droid))]
namespace Terra.Droid.Helper
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
