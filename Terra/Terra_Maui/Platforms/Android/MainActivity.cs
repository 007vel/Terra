using Android.App;
using Android.Content.PM;
using Android.Net.Wifi;
using Android.OS;
using Microsoft.Maui.Platform;
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
                // Always use Light theme for native Android views (dialogs, pickers), ignore system Dark mode
                AndroidX.AppCompat.App.AppCompatDelegate.DefaultNightMode = AndroidX.AppCompat.App.AppCompatDelegate.ModeNightNo;
                base.OnCreate(bundle);
                Window?.SetStatusBarColor(Color.FromArgb("#EF4736").ToPlatform());
               // this.SetLocale();
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
