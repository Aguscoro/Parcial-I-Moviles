using CountriesMVVM.ViewModels;

namespace CountriesMVVM.Views
{
    public partial class CountryDetailPage : ContentPage
    {
        private readonly CountryDetailViewModel viewModel;

        public CountryDetailPage(CountryDetailViewModel viewModel)
        {
            InitializeComponent();
            this.viewModel = viewModel;
            BindingContext = viewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            viewModel.IniciarSensores();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            viewModel.DetenerSensores();
        }
    }
}
