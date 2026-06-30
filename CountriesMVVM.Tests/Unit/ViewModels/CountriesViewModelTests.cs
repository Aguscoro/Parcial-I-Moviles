using CountriesMVVM.Exceptions;
using CountriesMVVM.Models;
using CountriesMVVM.Services;
using CountriesMVVM.Validations;
using CountriesMVVM.ViewModels;
using Moq;

namespace CountriesMVVM.Tests.Unit.ViewModels
{
    public class CountriesViewModelTests
    {
        private readonly IExceptionHandler exceptionHandler = new ExceptionHandler();

        [Fact]
        public async Task CargarPaisesAsync_ConServicioMock_PueblaLista()
        {
            var paises = new List<CountrySummary>
            {
                new() { Nombre = "Brasil", Capital = "Brasilia", Moneda = "Real" },
                new() { Nombre = "Peru", Capital = "Lima", Moneda = "Sol" }
            };

            var serviceMock = new Mock<ICountryService>();
            serviceMock.Setup(s => s.ObtenerPaisesAsync()).ReturnsAsync(paises);

            var navigationMock = new Mock<INavigationService>();

            var viewModel = new CountriesViewModel(
                serviceMock.Object,
                new CountryValidator(),
                navigationMock.Object,
                exceptionHandler);

            await viewModel.CargarPaisesAsync();

            Assert.Equal(2, viewModel.ListaPaises.Count);
            Assert.Equal("Paises cargados exitosamente.", viewModel.MensajeEstado);
        }

        [Fact]
        public async Task CargarPaisesAsync_CuandoServicioFalla_MuestraMensajeAmigable()
        {
            var serviceMock = new Mock<ICountryService>();
            serviceMock.Setup(s => s.ObtenerPaisesAsync())
                .ThrowsAsync(new ServiceException("Sin conexion"));

            var viewModel = new CountriesViewModel(
                serviceMock.Object,
                new CountryValidator(),
                Mock.Of<INavigationService>(),
                exceptionHandler);

            await viewModel.CargarPaisesAsync();

            Assert.Empty(viewModel.ListaPaises);
            Assert.Equal("Sin conexion", viewModel.MensajeEstado);
        }

        [Fact]
        public async Task FiltrarPaises_ConTextoBusqueda_FiltraResultados()
        {
            var paises = new List<CountrySummary>
            {
                new() { Nombre = "Colombia", Capital = "Bogota", Moneda = "Peso" },
                new() { Nombre = "Mexico", Capital = "CDMX", Moneda = "Peso" }
            };

            var serviceMock = new Mock<ICountryService>();
            serviceMock.Setup(s => s.ObtenerPaisesAsync()).ReturnsAsync(paises);

            var viewModel = new CountriesViewModel(
                serviceMock.Object,
                new CountryValidator(),
                Mock.Of<INavigationService>(),
                exceptionHandler);

            await viewModel.CargarPaisesAsync();
            viewModel.TextoBusqueda = "col";

            Assert.Single(viewModel.ListaPaises);
            Assert.Equal("Colombia", viewModel.ListaPaises[0].Nombre);
        }

        [Fact]
        public void FiltrarPaises_ConTextoMuyLargo_MuestraErrorBusqueda()
        {
            var viewModel = new CountriesViewModel(
                Mock.Of<ICountryService>(),
                new CountryValidator(),
                Mock.Of<INavigationService>(),
                exceptionHandler);

            viewModel.TextoBusqueda = new string('a', 101);

            Assert.True(viewModel.TieneErrorBusqueda);
            Assert.Contains("100", viewModel.ErrorBusqueda);
        }
    }
}
