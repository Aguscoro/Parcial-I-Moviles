using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using CountriesMVVM.Services;
using CountriesMVVM.Models;
using System.Linq;

namespace CountriesMVVM.ViewModels
{
    internal class CountriesViewModel : BaseViewModel
    {
        private readonly CountryService countryService;
        private bool datosCargados;

        private ObservableCollection<CountrySummary> TodosLosPaises { get; set; } = new();
        public ObservableCollection<CountrySummary> ListaPaises { get; set; } = new();

        // Mensaje de estado
        private string mensajeEstado;
        public string MensajeEstado
        {
            get => mensajeEstado;
            set => SetProperty(ref mensajeEstado, value);
        }

        // Texto de búsqueda
        private string textoBusqueda;
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

        public CountriesViewModel()
        {
            countryService = new CountryService();
            CargarPaisesCommand = new Command(async () => await CargarPaisesAsync());
            FiltrarPaisesCommand = new Command(FiltrarPaises);
            _ = CargarPaisesAsync();
        }

        private async Task CargarPaisesAsync()
        {
            if (datosCargados)
                return;

            try
            {
                MensajeEstado = "Cargando paises...";

                var lista = await countryService.ObtenerPaisesAsync();

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

        private void FiltrarPaises()
        {
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
            var parametros = new Dictionary<string, object>
            {
                { "nombrePais", Uri.EscapeDataString(pais.Nombre) },
                { "capitalPais", Uri.EscapeDataString(pais.Capital) },
                { "monedaPais", Uri.EscapeDataString(pais.Moneda) }
            };

            await Shell.Current.GoToAsync(nameof(Views.CountryDetailPage), parametros);
            PaisSeleccionado = null;
        }
    }
}



