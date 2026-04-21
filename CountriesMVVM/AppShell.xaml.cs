using CountriesMVVM.Views;

namespace CountriesMVVM
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(StartPage), typeof(StartPage));
            Routing.RegisterRoute(nameof(Views.CountriesPage), typeof(Views.CountriesPage));
            Routing.RegisterRoute(nameof(CountryDetailPage), typeof(CountryDetailPage));
        }
    }
}



