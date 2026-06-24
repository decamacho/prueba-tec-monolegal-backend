using Application.UseCases;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using Moq;
using Xunit;

namespace Test
{
    public class ProcessInvoiceRemindersUseCaseTests
    {
        private readonly Mock<IInvoiceRepository> _invoiceRepositoryMock;
        private readonly Mock<IEmailService> _emailServiceMock;
        private readonly ProcessInvoiceRemindersUseCase _useCase;

        public ProcessInvoiceRemindersUseCaseTests()
        {
            _invoiceRepositoryMock = new Mock<IInvoiceRepository>();
            _emailServiceMock = new Mock<IEmailService>();

            _useCase = new ProcessInvoiceRemindersUseCase(
                _invoiceRepositoryMock.Object,
                _emailServiceMock.Object
            );
        }

        [Fact]
        public async Task ProcesarRecordatoriosAsync_DebeActualizarDB_CuandoElCorreoSeEnviaExitosamente()
        {
            // Arrange
            var clienteDummy = new Client { Nombre = "Cliente Prueba", EmailContacto = "test@prueba.com" };
            var factura = new Invoice
            {
                Id = "1",
                CodigoFactura = "FAC-001",
                Cliente = clienteDummy,
            };

            factura.SetEstadoInicial(InvoiceState.primerrecordatorio);

            var listaFacturas = new List<Invoice> { factura };

            _invoiceRepositoryMock
                .Setup(repo => repo.ObtenerFacturasPorEstadosAsync(It.IsAny<List<InvoiceState>>()))
                .ReturnsAsync(listaFacturas);

            _emailServiceMock
                .Setup(email => email.EnviarNotificacionAsync(It.IsAny<Invoice>(), It.IsAny<InvoiceState>()))
                .ReturnsAsync(true); // Simulamos que el correo se envió con éxito

            // Act
            await _useCase.ProcesarRecordatoriosAsync();

            // Assert
            // verificar que la entidad transiciono su estado correctamente en memoria
            Assert.Equal(InvoiceState.segundorecordatorio, factura.Estado);

            // verificar que se intento enviar el correo con el nuevo estado
            _emailServiceMock.Verify(
                email => email.EnviarNotificacionAsync(factura, InvoiceState.segundorecordatorio),
                Times.Once);

            // verificar que se actualizó la base de datos
            _invoiceRepositoryMock.Verify(
                repo => repo.ActualizarEstadoFacturaAsync(factura.Id, InvoiceState.segundorecordatorio),
                Times.Once);
        }

        [Fact]
        public async Task ProcesarRecordatoriosAsync_NoDebeActualizarDB_CuandoFallaElEnvioDeCorreo()
        {
            // Arrange
            var clienteDummy = new Client { Nombre = "Cliente Falla", EmailContacto = "falla@prueba.com" };
            var factura = new Invoice
            {
                Id = "2",
                CodigoFactura = "FAC-002",
                Cliente = clienteDummy,
            };

            factura.SetEstadoInicial(InvoiceState.segundorecordatorio);

            var listaFacturas = new List<Invoice> { factura };

            _invoiceRepositoryMock
                .Setup(repo => repo.ObtenerFacturasPorEstadosAsync(It.IsAny<List<InvoiceState>>()))
                .ReturnsAsync(listaFacturas);

            _emailServiceMock
                .Setup(email => email.EnviarNotificacionAsync(It.IsAny<Invoice>(), It.IsAny<InvoiceState>()))
                .ReturnsAsync(false); // simular un FALLO en SendGrid

            // Act
            await _useCase.ProcesarRecordatoriosAsync();

            // Assert
            // verificar que se intento enviar el correo
            _emailServiceMock.Verify(
                email => email.EnviarNotificacionAsync(factura, InvoiceState.desactivado),
                Times.Once);

            // garantizar que la DB NUNCA se actualice si el correo fallo
            _invoiceRepositoryMock.Verify(
                repo => repo.ActualizarEstadoFacturaAsync(It.IsAny<string>(), It.IsAny<InvoiceState>()),
                Times.Never);
        }
    }
}