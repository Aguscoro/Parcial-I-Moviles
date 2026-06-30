using CountriesMVVM.Models;

namespace CountriesMVVM.Validations
{
    public class CountryValidator : ICountryValidator
    {
        private const int MaxSearchLength = 100;
        private const int MaxFieldLength = 150;

        public ValidationResult Validate(CountrySummary country)
        {
            if (country is null)
                return ValidationResult.Failure("El país no puede ser nulo.");

            if (string.IsNullOrWhiteSpace(country.Nombre))
                return ValidationResult.Failure("El nombre del país es obligatorio.");

            if (country.Nombre.Length > MaxFieldLength)
                return ValidationResult.Failure($"El nombre del país no puede superar {MaxFieldLength} caracteres.");

            if (string.IsNullOrWhiteSpace(country.Capital))
                return ValidationResult.Failure("La capital del país es obligatoria.");

            if (country.Capital.Length > MaxFieldLength)
                return ValidationResult.Failure($"La capital no puede superar {MaxFieldLength} caracteres.");

            if (string.IsNullOrWhiteSpace(country.Moneda))
                return ValidationResult.Failure("La moneda del país es obligatoria.");

            if (country.Moneda.Length > MaxFieldLength)
                return ValidationResult.Failure($"La moneda no puede superar {MaxFieldLength} caracteres.");

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

        public ValidationResult ValidateDetail(string? nombre, string? capital, string? moneda)
        {
            return Validate(new CountrySummary
            {
                Nombre = nombre ?? string.Empty,
                Capital = capital ?? string.Empty,
                Moneda = moneda ?? string.Empty
            });
        }

        public IReadOnlyList<CountrySummary> FilterValidCountries(IEnumerable<CountrySummary> countries)
        {
            return countries
                .Where(country => Validate(country).IsValid)
                .ToList();
        }
    }
}
