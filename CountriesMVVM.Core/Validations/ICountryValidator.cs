using CountriesMVVM.Models;

namespace CountriesMVVM.Validations
{
    public interface ICountryValidator
    {
        ValidationResult Validate(CountrySummary country);
        ValidationResult ValidateSearchText(string? searchText);
        ValidationResult ValidateDetail(string? nombre, string? capital, string? moneda);
        IReadOnlyList<CountrySummary> FilterValidCountries(IEnumerable<CountrySummary> countries);
    }
}
