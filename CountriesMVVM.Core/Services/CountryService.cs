using CountriesMVVM.Data;
using CountriesMVVM.Exceptions;
using CountriesMVVM.Models;
using CountriesMVVM.Validations;
using System.Net;
using System.Text.Json;

namespace CountriesMVVM.Services
{
    public class CountryService : ICountryService
    {
        private const string UrlBase = "https://restcountries.com/v3.1/all?fields=name,capital,currencies";
        private readonly HttpClient clienteHttp;
        private readonly ICountryRepository countryRepository;
        private readonly ICountryValidator countryValidator;

        public CountryService(
            HttpClient clienteHttp,
            ICountryRepository countryRepository,
            ICountryValidator countryValidator)
        {
            this.clienteHttp = clienteHttp;
            this.countryRepository = countryRepository;
            this.countryValidator = countryValidator;
        }

        internal string ObtenerMensajeError(HttpRequestException ex)
        {
            return ex.StatusCode switch
            {
                HttpStatusCode.NotFound => "Error 404: Recurso no encontrado.",
                HttpStatusCode.BadRequest => "Error 400: Solicitud inválida.",
                HttpStatusCode.InternalServerError => "Error 500: Problema en el servidor de la API.",
                _ => $"Error de conexión o HTTP ({ex.StatusCode}): {ex.Message}"
            };
        }

        public async Task<IReadOnlyList<CountrySummary>> ObtenerPaisesAsync()
        {
            try
            {
                var paisesApi = await ObtenerPaisesDesdeApiAsync();
                var paisesValidos = countryValidator.FilterValidCountries(paisesApi);

                if (paisesValidos.Count == 0)
                    throw new ServiceException("No se encontraron países válidos para mostrar.");

                await countryRepository.SaveCountriesAsync(paisesValidos);
                return paisesValidos;
            }
            catch (ServiceException)
            {
                throw;
            }
            catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException or JsonException)
            {
                var cache = await countryRepository.GetCountriesAsync();
                if (cache.Count > 0)
                    return cache;

                throw new ServiceException(ObtenerMensajeAmigable(ex), ex);
            }
            catch (Exception ex)
            {
                throw new ServiceException("No se pudieron cargar los países.", ex);
            }
        }

        private async Task<List<CountrySummary>> ObtenerPaisesDesdeApiAsync()
        {
            HttpResponseMessage respuesta = await clienteHttp.GetAsync(UrlBase);

            if (!respuesta.IsSuccessStatusCode)
                throw new HttpRequestException(null, null, respuesta.StatusCode);

            string contenidoJson = await respuesta.Content.ReadAsStringAsync();

            var opciones = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var paisesApi = JsonSerializer.Deserialize<List<CountryApiResponse>>(contenidoJson, opciones) ?? new();

            return paisesApi
                .Where(p => !string.IsNullOrWhiteSpace(p.Name.Common))
                .Select(p => new CountrySummary
                {
                    Nombre = p.Name.Common,
                    Capital = p.Capital?.FirstOrDefault() ?? "Sin capital registrada",
                    Moneda = p.Currencies?.FirstOrDefault().Value?.Name ?? "Sin moneda registrada"
                })
                .OrderBy(p => p.Nombre)
                .ToList();
        }

        private static string ObtenerMensajeAmigable(Exception ex)
        {
            return ex switch
            {
                HttpRequestException httpEx when httpEx.StatusCode.HasValue => httpEx.StatusCode switch
                {
                    HttpStatusCode.NotFound => "Error 404: Recurso no encontrado.",
                    HttpStatusCode.BadRequest => "Error 400: Solicitud inválida.",
                    HttpStatusCode.InternalServerError => "Error 500: Problema en el servidor de la API.",
                    _ => $"Error de conexión o HTTP ({httpEx.StatusCode})."
                },
                TaskCanceledException => "La solicitud tardó demasiado. Verificá tu conexión.",
                JsonException => "La respuesta del servidor no tiene un formato válido.",
                _ => "No se pudo conectar con el servicio de países."
            };
        }
    }
}
