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
        }
   }
}
