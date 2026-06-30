using CountriesMVVM.ViewModels;

namespace CountriesMVVM.Views
{
    public partial class CountryDetailPage : ContentPage
    {
        public CountryDetailPage(CountryDetailViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}
