using CountriesMVVM.ViewModels;

namespace CountriesMVVM.Views
{
    public partial class StartPage : ContentPage
    {
        public StartPage(StartViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}
