﻿using System;

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
using System.Globalization;
using System.Threading;

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

            global::Xamarin.Forms.Forms.SetFlags("Shapes_Experimental");

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            Rg.Plugins.Popup.Popup.Init(this, savedInstanceState);
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
        }
    }
}