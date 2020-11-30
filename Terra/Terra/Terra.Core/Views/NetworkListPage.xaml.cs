using System;
using System.Collections.Generic;
using System.Linq;
using Entities.Wifi;
using Terra.Core.ViewModels;
using Xamarin.Forms;
using Rg.Plugins.Popup.Extensions;

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
            var context = this.BindingContext as NetworkViewModel;
            if(context!=null)
            {
                context.PageNavigation = Navigation;
            }
        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
        }

    }
}
