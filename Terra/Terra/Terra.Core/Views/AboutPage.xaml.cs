using System;
using System.Collections.Generic;
using ConnectionLibrary.Interface;

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
            fwLbl.Text = fileName!=default? new List<string>(fileName.Split('_'))[3]:"";
        }
   }
}
