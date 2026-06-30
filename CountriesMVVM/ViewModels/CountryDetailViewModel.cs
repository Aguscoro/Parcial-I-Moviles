namespace CountriesMVVM.ViewModels
{
    [QueryProperty(nameof(NombrePais), "nombrePais")]
    [QueryProperty(nameof(CapitalPais), "capitalPais")]
    [QueryProperty(nameof(MonedaPais), "monedaPais")]
    public class CountryDetailViewModel : BaseViewModel
    {
        private string nombrePais = string.Empty;
        public string NombrePais { get => nombrePais; set => SetProperty(ref nombrePais, Uri.UnescapeDataString(value ?? string.Empty)); }

        private string capitalPais = string.Empty;
        public string CapitalPais { get => capitalPais; set => SetProperty(ref capitalPais, Uri.UnescapeDataString(value ?? string.Empty)); }

        private string monedaPais = string.Empty;
        public string MonedaPais { get => monedaPais; set => SetProperty(ref monedaPais, Uri.UnescapeDataString(value ?? string.Empty)); }
    }
}
