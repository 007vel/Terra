using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Graphics;
using Android.Support.V4.View;
using FlyMe.Droid;
using Xamarin.Forms;
using Plugin.CrossPlatformTintedImage.Android;
using Android.Net.Wifi;

namespace Terra.Droid
{
    [Activity(Label = "Terra", Theme = "@style/MainTheme", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            this.SetStatusBarColor(Android.Graphics.Color.Black);
            

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            Xamarin.Forms.FormsMaterial.Init(this, savedInstanceState);
            //Xamarin.Forms.Forms.SetFlags("CarouselView_Experimental");
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            /* DependencyService.Register<ToastNotification>(); // Register your dependency

             ToastNotification.Init(this);*/
            
            TintedImageRenderer.Init();
            Rg.Plugins.Popup.Popup.Init(this, savedInstanceState);
            LoadApplication(new App());

        }
        public WifiManager.LocalOnlyHotspotReservation mReservation { get; set; }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}