using System.Collections.ObjectModel;
using System.Windows.Input;
using CountriesMVVM.Commands;
using CountriesMVVM.Exceptions;
using CountriesMVVM.Models;
using CountriesMVVM.Services;
using CountriesMVVM.Validations;

namespace CountriesMVVM.ViewModels
{
    public class CountriesViewModel : BaseViewModel
    {
        private readonly ICountryService countryService;
        private readonly ICountryValidator countryValidator;
        private readonly INavigationService navigationService;
        private readonly IExceptionHandler exceptionHandler;
        private bool datosCargados;

        private ObservableCollection<CountrySummary> todosLosPaises = new();
        public ObservableCollection<CountrySummary> ListaPaises { get; } = new();

        private string mensajeEstado = string.Empty;
        public string MensajeEstado
        {
            get => mensajeEstado;
            set => SetProperty(ref mensajeEstado, value);
        }

        private string errorBusqueda = string.Empty;
        public string ErrorBusqueda
        {
            get => errorBusqueda;
            set
            {
                if (SetProperty(ref errorBusqueda, value))
                    OnPropertyChanged(nameof(TieneErrorBusqueda));
            }
        }

        public bool TieneErrorBusqueda => !string.IsNullOrWhiteSpace(ErrorBusqueda);

        private string textoBusqueda = string.Empty;
        public string TextoBusqueda
        {
            get => textoBusqueda;
            set
            {
                if (SetProperty(ref textoBusqueda, value))
                    FiltrarPaises();
            }
        }

        public ICommand CargarPaisesCommand { get; }

        public CountriesViewModel(
            ICountryService countryService,
            ICountryValidator countryValidator,
            INavigationService navigationService,
            IExceptionHandler exceptionHandler)
        {
            this.countryService = countryService;
            this.countryValidator = countryValidator;
            this.navigationService = navigationService;
            this.exceptionHandler = exceptionHandler;

            CargarPaisesCommand = new RelayCommand(CargarPaisesAsync, () => !IsBusy);
        }

        public async Task CargarPaisesAsync()
        {
            if (datosCargados || IsBusy)
                return;

            IsBusy = true;
            MensajeEstado = "Cargando paises...";

            try
            {
                var lista = await countryService.ObtenerPaisesAsync();

                todosLosPaises = new ObservableCollection<CountrySummary>(lista);
                ListaPaises.Clear();

                foreach (var pais in todosLosPaises)
                    ListaPaises.Add(pais);

                datosCargados = true;
                MensajeEstado = "Paises cargados exitosamente.";
            }
            catch (Exception ex)
            {
                MensajeEstado = exceptionHandler.GetUserMessage(ex);
            }
            finally
            {
                IsBusy = false;
                (CargarPaisesCommand as RelayCommand)?.RaiseCanExecuteChanged();
            }
        }

        public void FiltrarPaises()
        {
            var validacion = countryValidator.ValidateSearchText(TextoBusqueda);
            if (!validacion.IsValid)
            {
                ErrorBusqueda = validacion.ErrorMessage;
                return;
            }

            ErrorBusqueda = string.Empty;
            ListaPaises.Clear();

            var filtrados = string.IsNullOrWhiteSpace(TextoBusqueda)
                ? todosLosPaises
                : todosLosPaises.Where(p =>
                    p.Nombre.Contains(TextoBusqueda, StringComparison.OrdinalIgnoreCase));

            foreach (var pais in filtrados)
                ListaPaises.Add(pais);
        }

        public async Task NavegarAlDetalleAsync(CountrySummary pais)
        {
            if (IsBusy)
                return;

            var validacion = countryValidator.Validate(pais);
            if (!validacion.IsValid)
            {
                MensajeEstado = validacion.ErrorMessage;
                return;
            }

            await navigationService.GoToCountryDetailAsync(pais);
        }
    }
}
