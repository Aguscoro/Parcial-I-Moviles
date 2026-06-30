using CountriesMVVM.Models;
using CountriesMVVM.ViewModels;

namespace CountriesMVVM.Views
{
    public partial class CountriesPage : ContentPage
    {
        private readonly CountriesViewModel viewModel;

        public CountriesPage(CountriesViewModel viewModel)
        {
            InitializeComponent();
            this.viewModel = viewModel;
            BindingContext = viewModel;
            PaisesCollection.SelectionChanged += OnPaisSeleccionado;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            viewModel.CargarPaisesCommand.Execute(null);
        }

        private async void OnPaisSeleccionado(object? sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.FirstOrDefault() is not CountrySummary paisSeleccionado)
                return;

            PaisesCollection.SelectedItem = null;
            await viewModel.NavegarAlDetalleAsync(paisSeleccionado);
        }
    }
}
