using Microsoft.Extensions.Logging;
using CountriesMVVM.Data;
using CountriesMVVM.Services;
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

            builder.Services.AddSingleton<HttpClient>();
            builder.Services.AddSingleton<ICountryRepository>(_ => new CountryRepository(connectionString));
            builder.Services.AddSingleton<ICountryService, CountryService>();
            builder.Services.AddSingleton<ICountryValidator, CountryValidator>();
            builder.Services.AddSingleton<INavigationService, ShellNavigationService>();

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
