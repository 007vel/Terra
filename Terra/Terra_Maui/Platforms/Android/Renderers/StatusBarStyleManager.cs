using Android.OS;
using Android.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terra_Maui.common;
using Terra_Maui.Platforms.Android.Renderers;
using Window = Android.Views.Window;

//[assembly: Dependency(typeof(StatusBarStyleManager))]
namespace Terra_Maui.Platforms.Android.Renderers
{
    //public class StatusBarStyleManager : IStatusBarStyleManager
    //{
    //    public void SetTheme(Color color)
    //    {
    //        if (Android.OS.Build.VERSION.SdkInt >= BuildVersionCodes.M)
    //        {
    //            Device.BeginInvokeOnMainThread(() =>
    //            {
    //                var currentWindow = GetCurrentWindow();
    //                currentWindow.DecorView.SystemUiVisibility = 0;
    //                currentWindow.SetStatusBarColor(color.ToAndroid());
    //            });
    //        }
    //    }


    //    Window GetCurrentWindow()
    //    {
    //        var window = CrossCurrentActivity.Current.Activity.Window;

    //        // clear FLAG_TRANSLUCENT_STATUS flag:
    //        window.ClearFlags(WindowManagerFlags.TranslucentStatus);

    //        // add FLAG_DRAWS_SYSTEM_BAR_BACKGROUNDS flag to the window
    //        window.AddFlags(WindowManagerFlags.DrawsSystemBarBackgrounds);

    //        return window;
    //    }
    //}
}
