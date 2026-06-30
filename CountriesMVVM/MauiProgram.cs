using Microsoft.Extensions.Logging;
using CountriesMVVM.Data;
using CountriesMVVM.Exceptions;
using CountriesMVVM.Services;
using CountriesMVVM.Services.Sensors;
using CountriesMVVM.Validations;
using CountriesMVVM.ViewModels;
using CountriesMVVM.Views;

namespace CountriesMVVM
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

            var dbPath = Path.Combine(FileSystem.AppDataDirectory, "countries.db");
            var connectionString = $"Data Source={dbPath}";

            builder.Services.AddSingleton(_ =>
            {
                var client = new HttpClient
                {
                    Timeout = TimeSpan.FromSeconds(30)
                };
                return client;
            });

            builder.Services.AddSingleton<ICountryRepository>(_ => new CountryRepository(connectionString));
            builder.Services.AddSingleton<ICountryService, CountryService>();
            builder.Services.AddSingleton<ICountryValidator, CountryValidator>();
            builder.Services.AddSingleton<IExceptionHandler, ExceptionHandler>();
            builder.Services.AddSingleton<INavigationService, ShellNavigationService>();

            builder.Services.AddSingleton<IPermissionService, MauiPermissionService>();
            builder.Services.AddSingleton<ILocationService, MauiLocationService>();
            builder.Services.AddSingleton<ICameraService, MauiCameraService>();
            builder.Services.AddSingleton<IMotionSensorService, MauiMotionSensorService>();
            builder.Services.AddSingleton<IVibrationService, MauiVibrationService>();

            builder.Services.AddTransient<StartViewModel>();
            builder.Services.AddTransient<CountriesViewModel>();
            builder.Services.AddTransient<CountryDetailViewModel>();

            builder.Services.AddSingleton<AppShell>();

            builder.Services.AddTransient<StartPage>();
            builder.Services.AddTransient<CountriesPage>();
            builder.Services.AddTransient<CountryDetailPage>();

            return builder.Build();
        }
    }
}
