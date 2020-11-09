using System;
using ConnectionLibrary.Interface;
using Xamarin.Forms;

namespace ConnectionLibrary.Network
{
    public class NetworkServiceUtil
    {
        public static void Log(string str,bool printConsole=true)
        {
            IMobile mobile = DependencyService.Get<IMobile>();
            if(printConsole)
            {
                Console.WriteLine(str);
            }
            mobile.Log(str);
        }
    }
}
