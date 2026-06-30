using System.Windows.Input;

namespace CountriesMVVM.Commands
{
    public class RelayCommand : ICommand
    {
        private readonly Action? execute;
        private readonly Func<Task>? executeAsync;
        private readonly Func<bool>? canExecute;

        public RelayCommand(Action execute, Func<bool>? canExecute = null)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        public RelayCommand(Func<Task> executeAsync, Func<bool>? canExecute = null)
        {
            this.executeAsync = executeAsync;
            this.canExecute = canExecute;
        }

        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter) => canExecute?.Invoke() ?? true;

        public void Execute(object? parameter)
        {
            if (executeAsync is not null)
                _ = executeAsync();
            else
                execute?.Invoke();
        }

        public void RaiseCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
}
