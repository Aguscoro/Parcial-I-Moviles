using CountriesMVVM.Exceptions;

namespace CountriesMVVM.Tests.Unit.Exceptions
{
    public class ExceptionHandlerTests
    {
        private readonly ExceptionHandler handler = new();

        [Fact]
        public void GetUserMessage_ConServiceException_RetornaMensajeDeUsuario()
        {
            var mensaje = handler.GetUserMessage(new ServiceException("Error de red"));

            Assert.Equal("Error de red", mensaje);
        }

        [Fact]
        public void GetUserMessage_ConExcepcionGenerica_RetornaMensajeGenerico()
        {
            var mensaje = handler.GetUserMessage(new Exception("detalle tecnico"));

            Assert.Equal("Ocurrió un error inesperado. Intentá nuevamente.", mensaje);
        }
    }
}
