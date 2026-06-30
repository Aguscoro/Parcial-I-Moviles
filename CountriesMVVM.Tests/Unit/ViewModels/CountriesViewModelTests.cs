using CountriesMVVM.Data;
using CountriesMVVM.Models;
using CountriesMVVM.Services;
using CountriesMVVM.Validations;
using CountriesMVVM.ViewModels;
using Moq;

namespace CountriesMVVM.Tests.Unit.ViewModels
{
    public class CountriesViewModelTests
    {
        [Fact]
        public async Task CargarPaisesAsync_ConServicioMock_PueblaListaYGuardaEnRepositorio()
        {
            var paises = new List<CountrySummary>
            {
                new() { Nombre = "Brasil", Capital = "Brasilia", Moneda = "Real" },
                new() { Nombre = "Peru", Capital = "Lima", Moneda = "Sol" }
            };

            var serviceMock = new Mock<ICountryService>();
            serviceMock.Setup(s => s.ObtenerPaisesAsync()).ReturnsAsync(paises);

            var repositoryMock = new Mock<ICountryRepository>();
            repositoryMock.Setup(r => r.SaveCountriesAsync(It.IsAny<IEnumerable<CountrySummary>>()))
                .Returns(Task.CompletedTask);

            var navigationMock = new Mock<INavigationService>();

            var viewModel = new CountriesViewModel(
                serviceMock.Object,
                repositoryMock.Object,
                new CountryValidator(),
                navigationMock.Object);

            await viewModel.CargarPaisesAsync();

            Assert.Equal(2, viewModel.ListaPaises.Count);
            Assert.Equal("Paises cargados exitosamente.", viewModel.MensajeEstado);
            repositoryMock.Verify(r => r.SaveCountriesAsync(paises), Times.Once);
        }

        [Fact]
        public async Task CargarPaisesAsync_CuandoServicioFalla_MuestraMensajeDeError()
        {
            var serviceMock = new Mock<ICountryService>();
            serviceMock.Setup(s => s.ObtenerPaisesAsync())
                .ThrowsAsync(new Exception("Sin conexion"));

            var viewModel = new CountriesViewModel(
                serviceMock.Object,
                Mock.Of<ICountryRepository>(),
                new CountryValidator(),
                Mock.Of<INavigationService>());

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
                Mock.Of<ICountryRepository>(),
                new CountryValidator(),
                Mock.Of<INavigationService>());

            await viewModel.CargarPaisesAsync();
            viewModel.TextoBusqueda = "col";

            Assert.Single(viewModel.ListaPaises);
            Assert.Equal("Colombia", viewModel.ListaPaises[0].Nombre);
        }
    }
}
