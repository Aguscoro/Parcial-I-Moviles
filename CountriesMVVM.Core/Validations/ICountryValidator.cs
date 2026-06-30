using CountriesMVVM.Models;

namespace CountriesMVVM.Validations
{
    public interface ICountryValidator
    {
        ValidationResult Validate(CountrySummary country);
        ValidationResult ValidateSearchText(string? searchText);
    }
}
