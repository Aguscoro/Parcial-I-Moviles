using CountriesMVVM.Models;
using CountriesMVVM.Views;

namespace CountriesMVVM.Services
{
    public class ShellNavigationService : INavigationService
    {
        public async Task GoToCountriesAsync()
        {
            await Shell.Current.GoToAsync(nameof(CountriesPage));
        }

        public async Task GoToCountryDetailAsync(CountrySummary country)
        {
            var parametros = new Dictionary<string, object>
            {
                { "nombrePais", Uri.EscapeDataString(country.Nombre) },
                { "capitalPais", Uri.EscapeDataString(country.Capital) },
                { "monedaPais", Uri.EscapeDataString(country.Moneda) }
            };

            await Shell.Current.GoToAsync(nameof(CountryDetailPage), parametros);
        }
    }
}
