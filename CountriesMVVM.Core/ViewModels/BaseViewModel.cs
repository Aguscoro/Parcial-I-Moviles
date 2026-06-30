using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CountriesMVVM.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        private bool isBusy;

        public event PropertyChangedEventHandler? PropertyChanged;

        public bool IsBusy
        {
            get => isBusy;
            set => SetProperty(ref isBusy, value);
        }

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value))
                return false;

            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}
