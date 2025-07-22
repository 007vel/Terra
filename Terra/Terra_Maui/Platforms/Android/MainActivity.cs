using Android.App;
using Android.Content.PM;
using Android.Net.Wifi;
using Android.OS;

namespace Terra_Maui
{
    [Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, LaunchMode = LaunchMode.SingleTop, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
    public class MainActivity : MauiAppCompatActivity
    {
        public WifiManager.LocalOnlyHotspotReservation mReservation { get; set; }
    }
}
