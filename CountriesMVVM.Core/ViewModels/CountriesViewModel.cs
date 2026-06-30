using System.Collections.ObjectModel;
using System.Windows.Input;
using CountriesMVVM.Commands;
using CountriesMVVM.Data;
using CountriesMVVM.Models;
using CountriesMVVM.Services;
using CountriesMVVM.Validations;

namespace CountriesMVVM.ViewModels
{
    public class CountriesViewModel : BaseViewModel
    {
        private readonly ICountryService countryService;
        private readonly ICountryRepository countryRepository;
        private readonly ICountryValidator countryValidator;
        private readonly INavigationService navigationService;
        private bool datosCargados;

        private ObservableCollection<CountrySummary> TodosLosPaises { get; set; } = new();
        public ObservableCollection<CountrySummary> ListaPaises { get; set; } = new();

        private string mensajeEstado = string.Empty;
        public string MensajeEstado
        {
            get => mensajeEstado;
            set => SetProperty(ref mensajeEstado, value);
        }

        private string textoBusqueda = string.Empty;
        public string TextoBusqueda
        {
            get => textoBusqueda;
            set
            {
                if (SetProperty(ref textoBusqueda, value))
                {
                    FiltrarPaises();
                }
            }
        }

        public ICommand CargarPaisesCommand { get; }
        public ICommand FiltrarPaisesCommand { get; }

        public CountriesViewModel(
            ICountryService countryService,
            ICountryRepository countryRepository,
            ICountryValidator countryValidator,
            INavigationService navigationService)
        {
            this.countryService = countryService;
            this.countryRepository = countryRepository;
            this.countryValidator = countryValidator;
            this.navigationService = navigationService;

            CargarPaisesCommand = new RelayCommand(CargarPaisesAsync);
            FiltrarPaisesCommand = new RelayCommand(FiltrarPaises);
        }

        public async Task CargarPaisesAsync()
        {
            if (datosCargados)
                return;

            try
            {
                MensajeEstado = "Cargando paises...";

                var lista = await countryService.ObtenerPaisesAsync();

                foreach (var pais in lista)
                {
                    var validacion = countryValidator.Validate(pais);
                    if (!validacion.IsValid)
                        throw new InvalidOperationException(validacion.ErrorMessage);
                }

                await countryRepository.SaveCountriesAsync(lista);

                TodosLosPaises.Clear();
                ListaPaises.Clear();

                foreach (var pais in lista)
                {
                    TodosLosPaises.Add(pais);
                    ListaPaises.Add(pais);
                }

                datosCargados = true;
                MensajeEstado = "Paises cargados exitosamente.";
            }
            catch (Exception ex)
            {
                MensajeEstado = ex.Message;
            }
        }

        public void FiltrarPaises()
        {
            var validacion = countryValidator.ValidateSearchText(TextoBusqueda);
            if (!validacion.IsValid)
            {
                MensajeEstado = validacion.ErrorMessage;
                return;
            }

            ListaPaises.Clear();

            var filtrados = string.IsNullOrWhiteSpace(TextoBusqueda)
                ? TodosLosPaises
                : TodosLosPaises.Where(p =>
                    p.Nombre.Contains(TextoBusqueda, StringComparison.OrdinalIgnoreCase));

            foreach (var p in filtrados)
                ListaPaises.Add(p);
        }

        private CountrySummary? paisSeleccionado;
        public CountrySummary? PaisSeleccionado
        {
            get => paisSeleccionado;
            set
            {
                if (!SetProperty(ref paisSeleccionado, value) || value is null)
                    return;

                _ = NavegarADetalleAsync(value);
            }
        }

        private async Task NavegarADetalleAsync(CountrySummary pais)
        {
            await navigationService.GoToCountryDetailAsync(pais);
            PaisSeleccionado = null;
        }
    }
}
