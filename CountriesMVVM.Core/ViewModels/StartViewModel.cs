using System.Windows.Input;
using CountriesMVVM.Commands;
using CountriesMVVM.Services;

namespace CountriesMVVM.ViewModels
{
    public class StartViewModel : BaseViewModel
    {
        private readonly INavigationService navigationService;

        public ICommand IrAPaisesCommand { get; }

        public StartViewModel(INavigationService navigationService)
        {
            this.navigationService = navigationService;
            IrAPaisesCommand = new RelayCommand(async () => await navigationService.GoToCountriesAsync());
        }
    }
}
