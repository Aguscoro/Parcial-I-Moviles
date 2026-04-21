using System.Text.Json.Serialization;

namespace CountriesMVVM.Models
{
    internal class CountryApiResponse
    {
        [JsonPropertyName("name")]
        public CountryName Name { get; set; } = new();

        [JsonPropertyName("capital")]
        public List<string>? Capital { get; set; }

        [JsonPropertyName("currencies")]
        public Dictionary<string, CountryCurrency>? Currencies { get; set; }
    }

    internal class CountryName
    {
        [JsonPropertyName("common")]
        public string Common { get; set; } = string.Empty;
    }

    internal class CountryCurrency
    {
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
    }
}

