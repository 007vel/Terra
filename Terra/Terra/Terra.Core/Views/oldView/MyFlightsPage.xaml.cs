
using System.Linq;
using System.Threading.Tasks;
using ConnectionLibrary.Network;
using Terra.Service;
using Unity;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Terra.Views
{
    public partial class MyFlightsPage
    {
        public MyFlightsPage()
        {
            InitializeComponent();

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            var inAnims = new Animation();
            inAnims.Add(0, 1, new Animation(v => this.MilesProgressBar.WidthRequest = v, 0, 200, Easing.SpringIn));
            inAnims.Add(0.25, 1, new Animation(v => this.SegmentsProgressBar.WidthRequest = v, 0, 100, Easing.SpringIn));

            inAnims.Commit(this, "grow_bar", 16, 2000);
            var profiles = Connectivity.ConnectionProfiles;
            if (profiles.Contains(ConnectionProfile.WiFi))
            {
                // Active Wi-Fi connection.
            }
            var wifiadptr= ServiceProvider.Instance.Container.Resolve(typeof(WifiAdapter)) as IWifiManager;
            wifiadptr.OnRequestAvailableNetworks();
        }

        void Button_Clicked(System.Object sender, System.EventArgs e)
        {
            Shell.Current.GoToAsync("//book?from=STL");
        }
    }
}
