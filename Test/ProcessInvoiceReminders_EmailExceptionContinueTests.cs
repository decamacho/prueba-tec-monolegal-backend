using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Application.UseCases;
using Xunit;

namespace Test
{
    public class ProcessInvoiceReminders_EmailExceptionContinueTests
    {
        [Fact]
        public async Task ProcesarRecordatoriosAsync_NoDetiene_AnteExcepcionDeEmail_YProcesaSiguientes()
        {
            var repoMock = new Mock<IInvoiceRepository>();
            var emailMock = new Mock<IEmailService>();
            var loggerMock = new Mock<Application.Interfaces.IAppLogger<ProcessInvoiceRemindersUseCase>>();

            var factura1 = new Invoice { Id = "1", CodigoFactura = "F1", Cliente = new Client { Nombre = "A", EmailContacto = "a@a" } };
            factura1.SetEstadoInicial(InvoiceState.primerrecordatorio);

            var factura2 = new Invoice { Id = "2", CodigoFactura = "F2", Cliente = new Client { Nombre = "B", EmailContacto = "b@b" } };
            factura2.SetEstadoInicial(InvoiceState.primerrecordatorio);

            var lista = new List<Invoice> { factura1, factura2 };

            repoMock.Setup(r => r.ObtenerFacturasPorEstadosAsync(It.IsAny<IEnumerable<InvoiceState>>()))
                .ReturnsAsync(lista);

            // Simular excepción en el primer envío
            emailMock.Setup(e => e.EnviarNotificacionAsync(It.Is<Invoice>(f => f.Id == "1"), It.IsAny<InvoiceState>()))
                .ThrowsAsync(new System.Exception("SMTP error"));

            // Segundo envío OK
            emailMock.Setup(e => e.EnviarNotificacionAsync(It.Is<Invoice>(f => f.Id == "2"), It.IsAny<InvoiceState>()))
                .ReturnsAsync(true);

            var useCase = new ProcessInvoiceRemindersUseCase(repoMock.Object, emailMock.Object, loggerMock.Object);

            await useCase.ProcesarRecordatoriosAsync();

            // Verificar que se intento enviar a ambas facturas aunque la primera fallo
            emailMock.Verify(e => e.EnviarNotificacionAsync(factura1, It.IsAny<InvoiceState>()), Times.Once);
            emailMock.Verify(e => e.EnviarNotificacionAsync(factura2, It.IsAny<InvoiceState>()), Times.Once);

            // Solo la segunda debe haber actualizado la DB
            repoMock.Verify(r => r.ActualizarEstadoFacturaAsync(factura2.Id, It.IsAny<InvoiceState>()), Times.Once);
            repoMock.Verify(r => r.ActualizarEstadoFacturaAsync(factura1.Id, It.IsAny<InvoiceState>()), Times.Never);
        }
    }
}
