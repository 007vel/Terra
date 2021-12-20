using System;
using System.Collections.Generic;
using Acr.UserDialogs;
using ConnectionLibrary.Interface;
using ConnectionLibrary.Network;
using Terra.Core.Helper;
using Xamarin.Forms;

namespace Terra.Core.Views
{
    public partial class AboutPage : ContentPage
    {
        public AboutPage()
        {
            InitializeComponent();
            IMobile mobile = DependencyService.Get<IMobile>();
            versionLbl.Text=mobile.GetVersion();
            getFWversion();
        }
        private void getFWversion()
        {
            fwLbl.Text = OTAHelper.Instance.getAssetFolderFWversion();
            DevicebundlefwLbl.Text = WifiAdapter.Instance.CurrentDeviceFWVersion??"-connect with device for device Firmware version-";

          //  FW_UpdateCheck(OTAHelper.Instance.getAssetFolderFWversion(), WifiAdapter.Instance.CurrentDeviceFWVersion);
        }

        void ToolbarItem_Clicked(System.Object sender, System.EventArgs e)
        {
            if(OTAHelper.Instance.DeviceService!=null)
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    try
                    {
                        using (UserDialogs.Instance.Loading("Uploading..."))
                        {
                            var otaByte = DependencyService.Get<IMobile>().ReadOtaFile();
                            await OTAHelper.Instance.DeviceService.PutBinary("", otaByte);
                            
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
}
