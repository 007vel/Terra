using Android.App;
using Android.Content.PM;
using Android.Net.Wifi;
using Android.OS;
using Microsoft.Maui.Controls.Compatibility.Platform.Android;
using System.Globalization;

namespace Terra_Maui
{
    [Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, LaunchMode = LaunchMode.SingleTop, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
    public class MainActivity : MauiAppCompatActivity
    {
        public WifiManager.LocalOnlyHotspotReservation mReservation { get; set; }

        protected override void OnCreate(Bundle bundle)
        {
            try
            {
                base.OnCreate(bundle);
                Window?.SetStatusBarColor(Color.FromHex("#EF4736").ToAndroid());
                this.SetLocale();
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        void SetLocale()
        {
            CultureInfo ci = new CultureInfo("el-GR");

            Thread.CurrentThread.CurrentCulture = ci;
            Thread.CurrentThread.CurrentUICulture = ci;

            Console.WriteLine("CurrentCulture set: " + ci.Name);
        }
    }
}
