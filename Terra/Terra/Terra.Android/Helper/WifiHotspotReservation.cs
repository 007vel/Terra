using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Net.Wifi;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace Terra.Droid.Helper
{
    public class WifiHotspotReservation : WifiManager.LocalOnlyHotspotCallback
    {
        private const string TAG = nameof(WifiHotspotReservation);

        private MainActivity mainActivity;

        public WifiHotspotReservation(Activity _activity)
        {
            if (_activity.GetType() == typeof(MainActivity))
                mainActivity = (MainActivity)_activity;
            
        }

        public override void OnStarted(WifiManager.LocalOnlyHotspotReservation reservation)
        {
            base.OnStarted(reservation);
            Log.Debug(TAG, "Wifi Hotspot is on now");
            mainActivity.mReservation = reservation;
            var tt = reservation.WifiConfiguration;
        }

        public override void OnFailed([GeneratedEnum] LocalOnlyHotspotCallbackErrorCode reason)
        {
            base.OnFailed(reason);
            Log.Debug(TAG, "onStopped: ");
        }

        public override void OnStopped()
        {
            base.OnStopped();
            Log.Debug(TAG, "onFailed: ");
        }
    }
}