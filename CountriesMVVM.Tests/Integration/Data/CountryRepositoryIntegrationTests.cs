using CountriesMVVM.Data;
using CountriesMVVM.Models;
using Microsoft.Data.Sqlite;

namespace CountriesMVVM.Tests.Integration.Data
{
    public class CountryRepositoryIntegrationTests : IAsyncLifetime
    {
        private readonly string dbName = $"CountriesTestDb_{Guid.NewGuid():N}";
        private SqliteConnection connection = null!;
        private CountryRepository repository = null!;

        public Task InitializeAsync()
        {
            var connectionString = $"Data Source={dbName};Mode=Memory;Cache=Shared";
            connection = new SqliteConnection(connectionString);
            connection.Open();
            repository = new CountryRepository(connectionString);
            return Task.CompletedTask;
        }

        public async Task DisposeAsync()
        {
            await repository.DisposeAsync();
            await connection.DisposeAsync();
        }

        [Fact]
        public async Task SaveAndGetCountries_PersisteDatosEnSqliteInMemory()
        {
            var paises = new List<CountrySummary>
            {
                new() { Nombre = "Espana", Capital = "Madrid", Moneda = "Euro" },
                new() { Nombre = "Francia", Capital = "Paris", Moneda = "Euro" }
            };

            await repository.SaveCountriesAsync(paises);
            var recuperados = await repository.GetCountriesAsync();

            Assert.Equal(2, recuperados.Count);
            Assert.Equal("Espana", recuperados[0].Nombre);
            Assert.Equal("Francia", recuperados[1].Nombre);
        }

        [Fact]
        public async Task SaveCountries_ReemplazaDatosAnteriores()
        {
            await repository.SaveCountriesAsync(new[]
            {
                new CountrySummary { Nombre = "Italia", Capital = "Roma", Moneda = "Euro" }
            });

            await repository.SaveCountriesAsync(new[]
            {
                new CountrySummary { Nombre = "Portugal", Capital = "Lisboa", Moneda = "Euro" }
            });

            var recuperados = await repository.GetCountriesAsync();

            Assert.Single(recuperados);
            Assert.Equal("Portugal", recuperados[0].Nombre);
        }

        [Fact]
        public async Task ClearAsync_EliminaTodosLosRegistros()
        {
            await repository.SaveCountriesAsync(new[]
            {
                new CountrySummary { Nombre = "Alemania", Capital = "Berlin", Moneda = "Euro" }
            });

            await repository.ClearAsync();
            var recuperados = await repository.GetCountriesAsync();

            Assert.Empty(recuperados);
        }
    }
}
