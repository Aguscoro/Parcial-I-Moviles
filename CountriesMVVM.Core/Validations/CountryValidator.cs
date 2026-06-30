using CountriesMVVM.Models;

namespace CountriesMVVM.Validations
{
    public class CountryValidator : ICountryValidator
    {
        private const int MaxSearchLength = 100;

        public ValidationResult Validate(CountrySummary country)
        {
            if (country is null)
                return ValidationResult.Failure("El país no puede ser nulo.");

            if (string.IsNullOrWhiteSpace(country.Nombre))
                return ValidationResult.Failure("El nombre del país es obligatorio.");

            if (country.Nombre.Length > 150)
                return ValidationResult.Failure("El nombre del país no puede superar 150 caracteres.");

            return ValidationResult.Success();
        }

        public ValidationResult ValidateSearchText(string? searchText)
        {
            if (searchText is null)
                return ValidationResult.Success();

            if (searchText.Length > MaxSearchLength)
                return ValidationResult.Failure($"La búsqueda no puede superar {MaxSearchLength} caracteres.");

            return ValidationResult.Success();
        }
    }
}
