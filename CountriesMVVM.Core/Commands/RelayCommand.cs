namespace CountriesMVVM.Commands
{
    public class RelayCommand : ICommand
    {
        private readonly Action? execute;
        private readonly Func<Task>? executeAsync;

        public RelayCommand(Action execute) => this.execute = execute;

        public RelayCommand(Func<Task> executeAsync) => this.executeAsync = executeAsync;

#pragma warning disable CS0067
        public event EventHandler? CanExecuteChanged;
#pragma warning restore CS0067

        public bool CanExecute(object? parameter) => true;

        public void Execute(object? parameter)
        {
            if (executeAsync is not null)
                _ = executeAsync();
            else
                execute?.Invoke();
        }
    }
}
