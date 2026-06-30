using CountriesMVVM.Models;
using Microsoft.Data.Sqlite;

namespace CountriesMVVM.Data
{
    public class CountryRepository : ICountryRepository, IAsyncDisposable
    {
        private readonly SqliteConnection connection;
        private bool initialized;

        public CountryRepository(string connectionString)
        {
            connection = new SqliteConnection(connectionString);
        }

        public async Task InitializeAsync()
        {
            if (initialized)
                return;

            await connection.OpenAsync();

            var createTable = """
                CREATE TABLE IF NOT EXISTS Countries (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Nombre TEXT NOT NULL UNIQUE,
                    Capital TEXT NOT NULL,
                    Moneda TEXT NOT NULL
                );
                """;

            await using var command = connection.CreateCommand();
            command.CommandText = createTable;
            await command.ExecuteNonQueryAsync();

            initialized = true;
        }

        public async Task SaveCountriesAsync(IEnumerable<CountrySummary> countries)
        {
            await InitializeAsync();

            await using var transaction = await connection.BeginTransactionAsync();

            var deleteCommand = connection.CreateCommand();
            deleteCommand.CommandText = "DELETE FROM Countries;";
            deleteCommand.Transaction = (SqliteTransaction)transaction;
            await deleteCommand.ExecuteNonQueryAsync();

            foreach (var country in countries)
            {
                await using var insertCommand = connection.CreateCommand();
                insertCommand.Transaction = (SqliteTransaction)transaction;
                insertCommand.CommandText = """
                    INSERT INTO Countries (Nombre, Capital, Moneda)
                    VALUES ($nombre, $capital, $moneda);
                    """;
                insertCommand.Parameters.AddWithValue("$nombre", country.Nombre);
                insertCommand.Parameters.AddWithValue("$capital", country.Capital);
                insertCommand.Parameters.AddWithValue("$moneda", country.Moneda);
                await insertCommand.ExecuteNonQueryAsync();
            }

            await transaction.CommitAsync();
        }

        public async Task<IReadOnlyList<CountrySummary>> GetCountriesAsync()
        {
            await InitializeAsync();

            var countries = new List<CountrySummary>();

            await using var command = connection.CreateCommand();
            command.CommandText = """
                SELECT Nombre, Capital, Moneda
                FROM Countries
                ORDER BY Nombre;
                """;

            await using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                countries.Add(new CountrySummary
                {
                    Nombre = reader.GetString(0),
                    Capital = reader.GetString(1),
                    Moneda = reader.GetString(2)
                });
            }

            return countries;
        }

        public async Task ClearAsync()
        {
            await InitializeAsync();

            await using var command = connection.CreateCommand();
            command.CommandText = "DELETE FROM Countries;";
            await command.ExecuteNonQueryAsync();
        }

        public async ValueTask DisposeAsync()
        {
            await connection.DisposeAsync();
        }
    }
}
