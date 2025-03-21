using Microsoft.Extensions.Logging;

namespace WeatherApp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif
            // NavigationPage beállítása
            builder.Services.AddSingleton<WeatherPage>();
            builder.Services.AddTransient<SettingsPage>();
            builder.Services.AddTransient<FavsPage>();
            builder.Services.AddTransient<ClickedOnFavorite>();

            return builder.Build();
        }
    }
}
