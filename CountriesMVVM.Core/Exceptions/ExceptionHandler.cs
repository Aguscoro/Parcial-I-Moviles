namespace CountriesMVVM.Exceptions
{
    public class ExceptionHandler : IExceptionHandler
    {
        public string GetUserMessage(Exception exception)
        {
            return exception switch
            {
                ServiceException serviceException => serviceException.UserMessage,
                InvalidOperationException invalidOperation => invalidOperation.Message,
                UnauthorizedAccessException => "No tenés permisos para realizar esta acción.",
                _ => "Ocurrió un error inesperado. Intentá nuevamente."
            };
        }
    }
}
