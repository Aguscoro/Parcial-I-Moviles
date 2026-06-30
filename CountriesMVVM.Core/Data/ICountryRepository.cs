using CountriesMVVM.Models;

namespace CountriesMVVM.Data
{
    public interface ICountryRepository
    {
        Task InitializeAsync();
        Task SaveCountriesAsync(IEnumerable<CountrySummary> countries);
        Task<IReadOnlyList<CountrySummary>> GetCountriesAsync();
        Task ClearAsync();
    }
}
