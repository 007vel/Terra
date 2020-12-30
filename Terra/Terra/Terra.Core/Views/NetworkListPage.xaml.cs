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
        NetworkViewModel context = null;
        public NetworkViewModel Context
        {
            get
            {
                if(context==null)
                {
                    context = this.BindingContext as NetworkViewModel;
                }
                return context;
            }
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
         
            if(context!=null)
            {
                context.PageNavigation = Navigation;
                context.Init();
            }
        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
        }

    }
}
