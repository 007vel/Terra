using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Acr.UserDialogs;
using ConnectionLibrary.Interface;
using ConnectionLibrary.Network;
using Terra.Core.ViewModels;
using Xamarin.Forms;

namespace Terra.Core.Helper
{
    public class OTAHelper
    {
        static OTAHelper otaHelper = null;

        public DeviceService DeviceService { get; set; }
        public delegate void ActionResult(bool connected);
        public event ActionResult NotifyDeviceConnectionChange;
        private OTAHelper()
        {
            KeepCheckHeartBeat();
        }
        public static OTAHelper Instance
        {
            get
            {
                if(otaHelper==null)
                {
                    otaHelper = new OTAHelper();
                }
                return otaHelper;
            }
        }
        public bool EnableHeartBeat { set; get; }
        public string getAssetFolderFWversion()
        {
            IMobile mobile = DependencyService.Get<IMobile>();
            var files = mobile.GetAllAssetsName();
            string fileName = default;
            foreach (var f in files)
            {
                if (f.Contains("ota_data_initial"))
                {
                    fileName = f;
                    break;
                }
            }
            var otaVer = fileName != default ? new List<string>(fileName.Split('_'))[3] : "";

            return otaVer;
        }

        public bool VerifyNewFWupdatesAvailability(string assetOta, string devicefW)
        {
            try
            {
                if (!string.IsNullOrEmpty(assetOta) && !string.IsNullOrEmpty(devicefW))
                {
                    string otaversion = assetOta.Split('.')[2];
                    string deviceversion = devicefW.Split('.')[2];

                    if (Convert.ToInt32(otaversion) > Convert.ToInt32(deviceversion))
                    {
                        return true;
                    }
                }
            }
            catch (Exception es)
            {
                System.Diagnostics.Debug.WriteLine(es);
            }
            return false;

        }
        public bool IsDeviceConnected()
        {
            var ssid = WifiAdapter.Instance.FormWifiManager.GetSsId().ToLower().Replace("\"", "");
            return NetworkViewModel.WIFI_ID == ssid;
        }

        public async void KeepCheckHeartBeat()
        {
            while(true)
            {
                await Task.Delay(2000);
                if (EnableHeartBeat)
                {
                    NotifyDeviceConnectionChange?.Invoke(IsDeviceConnected());
                }
            }
        }

        public async void performOtaUpload()
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                try
                {
                    using (UserDialogs.Instance.Loading("Uploading..."))
                    {
                        var otaByte = DependencyService.Get<IMobile>().ReadOtaFile();
                        await OTAHelper.Instance.DeviceService.PutBinary("", otaByte);
                        //Task.Delay(2*1000);
                        UserDialogs.Instance.HideLoading();
                        await Shell.Current.Navigation.PopAsync();
                    }
                    UserDialogs.Instance.HideLoading();
                }
                catch (Exception ex)
                {
                    await App.Current.MainPage.DisplayAlert(title: "Alert", message: "Error in OTA update", cancel: "OK");
                    Console.WriteLine(ex);
                    UserDialogs.Instance.HideLoading();
                }
            });
        }
    }
}
