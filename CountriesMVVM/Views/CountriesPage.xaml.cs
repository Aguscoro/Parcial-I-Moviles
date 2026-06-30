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
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            viewModel.CargarPaisesCommand.Execute(null);
        }
    }
}
