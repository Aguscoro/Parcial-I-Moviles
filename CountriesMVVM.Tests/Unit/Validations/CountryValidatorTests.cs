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
            var pais = new CountrySummary { Nombre = "   " };

            var resultado = validator.Validate(pais);

            Assert.False(resultado.IsValid);
            Assert.Contains("obligatorio", resultado.ErrorMessage);
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
        public void ValidateSearchText_ConTextoNulo_RetornaExito()
        {
            var resultado = validator.ValidateSearchText(null);

            Assert.True(resultado.IsValid);
        }
    }
}
