
using ConnectionLibrary.Interface;
using ConnectionLibrary.Network;
using System.Diagnostics;
#if ANDROID
using Terra_Maui.Platforms.Android.Helper;
#endif

namespace Terra_Maui
{
    public partial class App : Application
    {
        public static string REMEMBER_ME = "remember_me";

        public App()
        {
            // Thanks Bohdan! https://twitter.com/bbenetskyy
            //   StyleSheetRegistrar.RegisterStyle("-xf-horizontal-options", typeof(VisualElement), nameof(View.HorizontalOptionsProperty));
            //  StyleSheetRegistrar.RegisterStyle("-xf-shell-navbarhasshadow", typeof(Shell), nameof(Shell.NavBarHasShadowProperty));

            //Need To Work 
            //            Device.SetFlags(new[] {
            //    "CarouselView_Experimental",
            //    "IndicatorView_Experimental",
            //    "RadioButton_Experimental",
            //    "AppTheme_Experimental",
            //    "Markup_Experimental",
            //    "Expander_Experimental",
            //    "Shapes_Experimental",
            //    "SwipeView_Experimental"
            //});
            //  global::Xamarin.Forms.Forms.SetFlags("Shapes_Experimental", "CarouselView_Experimental");

#if ANDROID
            DependencyService.Register<IMobile, MobileHelper>();
            DependencyService.Register<IPlatformWifiManager, WifiManagerDroid>();
            DependencyService.Register<ISmartConfigHelper, SmartConfig_Droid>();
#endif
            InitializeComponent();

            MainPage = new AppShell();

            this.RequestedThemeChanged += App_RequestedThemeChanged;
            //  this.UnhandledException += (o, s) => { };
            //global::Xamarin.Forms.Forms.SetFlags("Shapes_Experimental", "CarouselView_Experimental");
        }

        private void App_RequestedThemeChanged(object sender, AppThemeChangedEventArgs e)
        {
            Debug.WriteLine($"Requested: {e.RequestedTheme}");

        }

        protected override void OnStart()
        {
            DeviceService.Instance = null;
            //  WifiAdapter.Instance.Dispose();
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps

        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
        protected override void CleanUp()
        {
            base.CleanUp();
        }
    }
}