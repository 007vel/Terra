using Terra.Core.Views;
using Terra.Views;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Terra
{
    public partial class AppShell
    {
        public AppShell()
        {
            InitializeComponent();

            
            InitRoutes();
        }

        private void InitRoutes()
        {
            Routing.RegisterRoute("results", typeof(FlightResultsPage));
            Routing.RegisterRoute("DeviceDetailsPage", typeof(DeviceDetailsPage));
            Routing.RegisterRoute("DeviceDetailsPage", typeof(DeviceDetailsPage));
        }

        private async void MenuItem_Clicked(object sender, System.EventArgs e)
        {
            await SecureStorage.SetAsync(App.REMEMBER_ME, false.ToString());
            await Shell.Current.GoToAsync("///login");
            Shell.SetFlyoutBehavior(Shell.Current, FlyoutBehavior.Disabled);
            Shell.Current.FlyoutIsPresented = false;
        }
    }
}
