using System;
using System.Collections.Generic;
using System.Linq;
using Entities.Wifi;
using Xamarin.Forms;

namespace Terra.Core.Views
{
    public partial class NetworkListPage : ContentPage
    {
        public NetworkListPage()
        {
            InitializeComponent();
         //   NavigationPage.SetHasNavigationBar(this, false);
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            
        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
        }

    }
}
