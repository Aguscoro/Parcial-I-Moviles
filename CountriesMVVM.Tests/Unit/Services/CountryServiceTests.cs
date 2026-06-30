using System.Net;
using CountriesMVVM.Data;
using CountriesMVVM.Exceptions;
using CountriesMVVM.Services;
using CountriesMVVM.Tests.Helpers;
using CountriesMVVM.Validations;
using Moq;

namespace CountriesMVVM.Tests.Unit.Services
{
    public class CountryServiceTests
    {
        [Fact]
        public async Task ObtenerPaisesAsync_ConRespuestaValida_RetornaPaisesOrdenados()
        {
            const string json = """
                [
                  {
                    "name": { "common": "Uruguay" },
                    "capital": ["Montevideo"],
                    "currencies": { "UYU": { "name": "Peso uruguayo" } }
                  },
                  {
                    "name": { "common": "Argentina" },
                    "capital": ["Buenos Aires"],
                    "currencies": { "ARS": { "name": "Argentine peso" } }
                  }
                ]
                """;

            var handler = new FakeHttpMessageHandler(_ => new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(json)
            });

            var repositoryMock = new Mock<ICountryRepository>();
            repositoryMock.Setup(r => r.SaveCountriesAsync(It.IsAny<IEnumerable<CountriesMVVM.Models.CountrySummary>>()))
                .Returns(Task.CompletedTask);

            var service = new CountryService(
                new HttpClient(handler),
                repositoryMock.Object,
                new CountryValidator());

            var resultado = await service.ObtenerPaisesAsync();

            Assert.Equal(2, resultado.Count);
            Assert.Equal("Argentina", resultado[0].Nombre);
            Assert.Equal("Uruguay", resultado[1].Nombre);
            repositoryMock.Verify(r => r.SaveCountriesAsync(It.IsAny<IEnumerable<CountriesMVVM.Models.CountrySummary>>()), Times.Once);
        }

        [Fact]
        public async Task ObtenerPaisesAsync_ConError404_LanzaServiceException()
        {
            var handler = new FakeHttpMessageHandler(_ => new HttpResponseMessage(HttpStatusCode.NotFound));
            var repositoryMock = new Mock<ICountryRepository>();
            repositoryMock.Setup(r => r.GetCountriesAsync()).ReturnsAsync(Array.Empty<CountriesMVVM.Models.CountrySummary>());

            var service = new CountryService(
                new HttpClient(handler),
                repositoryMock.Object,
                new CountryValidator());

            var ex = await Assert.ThrowsAsync<ServiceException>(() => service.ObtenerPaisesAsync());

            Assert.Contains("404", ex.UserMessage);
        }

        [Theory]
        [InlineData(HttpStatusCode.BadRequest, "400")]
        [InlineData(HttpStatusCode.InternalServerError, "500")]
        public void ObtenerMensajeError_MapeaCodigosHttp(HttpStatusCode status, string codigoEsperado)
        {
            var service = new CountryService(
                new HttpClient(new FakeHttpMessageHandler(_ => new HttpResponseMessage(status))),
                Mock.Of<ICountryRepository>(),
                new CountryValidator());
            var ex = new HttpRequestException(null, null, status);

            var mensaje = service.ObtenerMensajeError(ex);

            Assert.Contains(codigoEsperado, mensaje);
        }
    }
}
