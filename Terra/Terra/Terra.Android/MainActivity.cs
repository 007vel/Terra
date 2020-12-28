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

namespace Terra.Droid
{
    [Activity(Label = "Terra", NoHistory = false, LaunchMode = LaunchMode.SingleInstance, Theme = "@style/MainTheme", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        MobileHelper mobileHelper = null;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            this.SetStatusBarColor(Android.Graphics.Color.Black);

            global::Xamarin.Forms.Forms.SetFlags("Shapes_Experimental");

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            Rg.Plugins.Popup.Popup.Init(this, savedInstanceState);
            UserDialogs.Init(this);
            AppDomain.CurrentDomain.UnhandledException += CurrentDomainOnUnhandledException;
            TaskScheduler.UnobservedTaskException += TaskSchedulerOnUnobservedTaskException;

            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            TintedImageRenderer.Init();
            
            LoadApplication(new App());
        }
        public WifiManager.LocalOnlyHotspotReservation mReservation { get; set; }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
        protected override void OnResume()
        {
            base.OnResume();
            var userSelectedCulture = CultureInfo.CreateSpecificCulture("en-US");
            userSelectedCulture.NumberFormat.CurrencyNegativePattern = 1;
            Thread.CurrentThread.CurrentCulture = userSelectedCulture;
            mobileHelper = new MobileHelper();
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

               // mobileHelper.Log("MainActivity  " + exception.ToString());
               
                //const string errorFileName = "Fatal.log";
                //var libraryPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal); // iOS: Environment.SpecialFolder.Resources
                //var errorFilePath = Path.Combine(libraryPath, errorFileName);
                //var errorMessage = String.Format("Time: {0}\r\nError: Unhandled Exception\r\n{1}",
                //DateTime.Now, exception.ToString());
                //File.WriteAllText(errorFilePath, errorMessage);

                // Log to Android Device Logging.
              //  Android.Util.Log.Error("Crash Report", exception.ToString());
            }
            catch
            {
                // just suppress any error logging exceptions
            }
        }
    }
}