using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace CountriesMVVM.ViewModels
{
    public class StartViewModel : BaseViewModel
    {
        public ICommand IrAPaisesCommand { get; }

        public StartViewModel()
        {
            IrAPaisesCommand = new Command(async () =>
            {
                await Shell.Current.GoToAsync(nameof(Views.CountriesPage));
            });
        }
    }
}



