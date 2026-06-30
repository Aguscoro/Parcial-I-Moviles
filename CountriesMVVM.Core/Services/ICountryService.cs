using CountriesMVVM.Models;

namespace CountriesMVVM.Services
{
    public interface ICountryService
    {
        Task<IReadOnlyList<CountrySummary>> ObtenerPaisesAsync();
    }
}
