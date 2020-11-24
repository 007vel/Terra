using Terra.Core.Views;
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
            Routing.RegisterRoute("DeviceDetailsPage", typeof(DeviceDetailsPage));
            Routing.RegisterRoute("AboutPage", typeof(AboutPage));
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
