using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Graphics;
using FlyMe.Droid;
using Xamarin.Forms;
using Plugin.CrossPlatformTintedImage.Android;
using Android.Net.Wifi;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Terra.Droid.Helper;
using Acr.UserDialogs;
using Plugin.CurrentActivity;
using Android;

namespace Terra.Droid
{
    [Activity(Label = "Terra", NoHistory = false, Theme = "@style/MainTheme", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        MobileHelper mobileHelper = null;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;
           // RequestLocationWithDisclosure();
            base.OnCreate(savedInstanceState);

            this.SetStatusBarColor(Android.Graphics.Color.Black);

            global::Xamarin.Forms.Forms.SetFlags("Shapes_Experimental");

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            Rg.Plugins.Popup.Popup.Init(this, savedInstanceState);
            UserDialogs.Init(this);
            AppDomain.CurrentDomain.UnhandledException += CurrentDomainOnUnhandledException;
            TaskScheduler.UnobservedTaskException += TaskSchedulerOnUnobservedTaskException;
            CrossCurrentActivity.Current.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            TintedImageRenderer.Init();
            
            LoadApplication(new App());
        }
        public WifiManager.LocalOnlyHotspotReservation mReservation { get; set; }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
          //  Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

           // base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
        protected override void OnResume()
        {
            base.OnResume();
            var userSelectedCulture = CultureInfo.CreateSpecificCulture("en-US");
            userSelectedCulture.NumberFormat.CurrencyNegativePattern = 1;
            Thread.CurrentThread.CurrentCulture = userSelectedCulture;
            mobileHelper = new MobileHelper();
            
        }
        private void GetPermission()
        {
          //  if (CheckSelfPermission(Manifest.Permission.AccessCoarseLocation) != (int)Permission.Granted)
            {
              //  RequestPermissions(new string[] { Manifest.Permission.AccessCoarseLocation, Manifest.Permission.AccessFineLocation }, 0);
            }
        }
        private void RequestLocationWithDisclosure()
        {
            AlertDialog.Builder alert = new AlertDialog.Builder(this);
            alert.SetTitle("GPS Disclosure");
            alert.SetMessage(string.Format("Scent pluse app collects location data in background to enable wifi scanning, even when the app is closed or not in use."));
            alert.SetPositiveButton("Allow", (sender, e) => { GetPermission(); });
            alert.SetNegativeButton("Deny", (sender, e) => {  });

            var dialog = alert.Create();
            dialog.Show();
        }
        //   ‪#‎region‬ Error handling
        private void TaskSchedulerOnUnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs unobservedTaskExceptionEventArgs)
        {
            var newExc = new Exception("TaskSchedulerOnUnobservedTaskException", unobservedTaskExceptionEventArgs.Exception);
            LogUnhandledException(newExc);
        }

        private void CurrentDomainOnUnhandledException(object sender, UnhandledExceptionEventArgs unhandledExceptionEventArgs)
        {
            var newExc = new Exception("CurrentDomainOnUnhandledException", unhandledExceptionEventArgs.ExceptionObject as Exception);
            LogUnhandledException(newExc);
        }

        internal void LogUnhandledException(Exception exception)
        {
            try
            {
                if (mobileHelper == null) return;

            }
            catch
            {
                // just suppress any error logging exceptions
            }
        }
    }
}