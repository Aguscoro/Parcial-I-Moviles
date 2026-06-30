using CountriesMVVM.Exceptions;
using CountriesMVVM.Models;
using CountriesMVVM.Validations;

namespace CountriesMVVM.Tests.Unit.Validations
{
    public class CountryValidatorTests
    {
        private readonly CountryValidator validator = new();

        [Fact]
        public void Validate_ConPaisValido_RetornaExito()
        {
            var pais = new CountrySummary
            {
                Nombre = "Chile",
                Capital = "Santiago",
                Moneda = "Peso chileno"
            };

            var resultado = validator.Validate(pais);

            Assert.True(resultado.IsValid);
            Assert.Empty(resultado.ErrorMessage);
        }

        [Fact]
        public void Validate_ConNombreVacio_RetornaError()
        {
            var pais = new CountrySummary { Nombre = "   ", Capital = "X", Moneda = "Y" };

            var resultado = validator.Validate(pais);

            Assert.False(resultado.IsValid);
            Assert.Contains("obligatorio", resultado.ErrorMessage);
        }

        [Fact]
        public void Validate_ConCapitalVacia_RetornaError()
        {
            var pais = new CountrySummary { Nombre = "Chile", Capital = " ", Moneda = "Peso" };

            var resultado = validator.Validate(pais);

            Assert.False(resultado.IsValid);
            Assert.Contains("capital", resultado.ErrorMessage, StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public void ValidateSearchText_ConTextoMuyLargo_RetornaError()
        {
            var texto = new string('a', 101);

            var resultado = validator.ValidateSearchText(texto);

            Assert.False(resultado.IsValid);
            Assert.Contains("100", resultado.ErrorMessage);
        }

        [Fact]
        public void FilterValidCountries_ExcluyePaisesInvalidos()
        {
            var paises = new[]
            {
                new CountrySummary { Nombre = "Chile", Capital = "Santiago", Moneda = "Peso" },
                new CountrySummary { Nombre = "", Capital = "X", Moneda = "Y" }
            };

            var validos = validator.FilterValidCountries(paises);

            Assert.Single(validos);
            Assert.Equal("Chile", validos[0].Nombre);
        }

        [Fact]
        public void ValidateDetail_ConDatosValidos_RetornaExito()
        {
            var resultado = validator.ValidateDetail("Peru", "Lima", "Sol");

            Assert.True(resultado.IsValid);
        }
    }
}
