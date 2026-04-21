using CountriesMVVM.Models;
using System.Net;
using System.Text.Json;

namespace CountriesMVVM.Services
{
    internal class CountryService
    {
        private const string UrlBase = "https://restcountries.com/v3.1/all?fields=name,capital,currencies";
        private readonly HttpClient clienteHttp = new();

        private string ObtenerMensajeError(HttpRequestException ex)
        {
            return ex.StatusCode switch
            {
                HttpStatusCode.NotFound => "Error 404: Recurso no encontrado.",
                HttpStatusCode.BadRequest => "Error 400: Solicitud inválida.",
                HttpStatusCode.InternalServerError => "Error 500: Problema en el servidor de la API.",
                _ => $"Error de conexión o HTTP ({ex.StatusCode}): {ex.Message}"
            };
        }

        public async Task<List<CountrySummary>> ObtenerPaisesAsync()
        {
            try
            {
                HttpResponseMessage respuesta = await clienteHttp.GetAsync(UrlBase);

                if (!respuesta.IsSuccessStatusCode)
                {
                    throw new HttpRequestException(null, null, respuesta.StatusCode);
                }

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
            catch (HttpRequestException ex)
            {
                throw new Exception(ObtenerMensajeError(ex));
            }
            catch (Exception ex)
            {
                throw new Exception($"Error inesperado en el servicio ({ex.GetType().Name}): {ex.Message}");
            }
        }
    }
}


