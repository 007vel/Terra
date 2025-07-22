using Acr.UserDialogs;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Controls.Hosting;
using Mopups.Hosting;
using SkiaSharp.Views.Maui.Controls.Hosting;
namespace Terra_Maui
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseSkiaSharp()
                .ConfigureMopups()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif
#if IOS
            builder.Services.AddSingleton(UserDialogs.Instance);
#endif

            return builder.Build();
        }
    }
}
