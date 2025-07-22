using Foundation;
using UIKit;

namespace Terra_Maui
{
    [Register("AppDelegate")]
    public class AppDelegate : MauiUIApplicationDelegate
    {
        protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();

        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            UINavigationBar.Appearance.Translucent = false;

            return base.FinishedLaunching(app, options);
        }
    }
}
