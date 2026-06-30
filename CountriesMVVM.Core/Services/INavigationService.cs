using CountriesMVVM.Models;

namespace CountriesMVVM.Services
{
    public interface INavigationService
    {
        Task GoToCountriesAsync();
        Task GoToCountryDetailAsync(CountrySummary country);
    }
}
