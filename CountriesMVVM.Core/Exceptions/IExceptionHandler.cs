namespace CountriesMVVM.Exceptions
{
    public interface IExceptionHandler
    {
        string GetUserMessage(Exception exception);
    }
}
