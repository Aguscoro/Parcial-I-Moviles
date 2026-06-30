using CountriesMVVM.Models;

namespace CountriesMVVM.Services
{
    public interface ICountryService
    {
        Task<List<CountrySummary>> ObtenerPaisesAsync();
    }
}
