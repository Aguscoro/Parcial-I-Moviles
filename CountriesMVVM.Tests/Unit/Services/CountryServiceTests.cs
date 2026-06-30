using System.Net;
using CountriesMVVM.Services;
using CountriesMVVM.Tests.Helpers;

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

            var service = new CountryService(new HttpClient(handler));

            var resultado = await service.ObtenerPaisesAsync();

            Assert.Equal(2, resultado.Count);
            Assert.Equal("Argentina", resultado[0].Nombre);
            Assert.Equal("Uruguay", resultado[1].Nombre);
            Assert.Equal("Montevideo", resultado[1].Capital);
            Assert.Equal("Peso uruguayo", resultado[1].Moneda);
        }

        [Fact]
        public async Task ObtenerPaisesAsync_ConError404_LanzaExcepcionConMensajeClaro()
        {
            var handler = new FakeHttpMessageHandler(_ => new HttpResponseMessage(HttpStatusCode.NotFound));
            var service = new CountryService(new HttpClient(handler));

            var ex = await Assert.ThrowsAsync<Exception>(() => service.ObtenerPaisesAsync());

            Assert.Contains("404", ex.Message);
        }

        [Theory]
        [InlineData(HttpStatusCode.BadRequest, "400")]
        [InlineData(HttpStatusCode.InternalServerError, "500")]
        public async Task ObtenerMensajeError_MapeaCodigosHttp(HttpStatusCode status, string codigoEsperado)
        {
            var service = new CountryService(new HttpClient(new FakeHttpMessageHandler(_ => new HttpResponseMessage(status))));
            var ex = new HttpRequestException(null, null, status);

            var mensaje = service.ObtenerMensajeError(ex);

            Assert.Contains(codigoEsperado, mensaje);
        }
    }
}
