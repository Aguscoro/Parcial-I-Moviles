namespace CountriesMVVM.Exceptions
{
    public class ServiceException : Exception
    {
        public ServiceException(string userMessage, Exception? innerException = null)
            : base(userMessage, innerException)
        {
            UserMessage = userMessage;
        }

        public string UserMessage { get; }
    }
}
