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
    public class ProcessInvoiceReminders_NoInvoicesTests
    {
        [Fact]
        public async Task ProcesarRecordatoriosAsync_NoHaceNada_CuandoNoHayFacturas()
        {
            var repoMock = new Mock<IInvoiceRepository>();
            var emailMock = new Mock<IEmailService>();
            var loggerMock = new Mock<Application.Interfaces.IAppLogger<ProcessInvoiceRemindersUseCase>>();

            repoMock.Setup(r => r.ObtenerFacturasPorEstadosAsync(It.IsAny<IEnumerable<InvoiceState>>()))
                .ReturnsAsync(new List<Invoice>());

            var useCase = new ProcessInvoiceRemindersUseCase(repoMock.Object, emailMock.Object, loggerMock.Object);

            await useCase.ProcesarRecordatoriosAsync();

            emailMock.Verify(e => e.EnviarNotificacionAsync(It.IsAny<Invoice>(), It.IsAny<InvoiceState>()), Times.Never);
            repoMock.Verify(r => r.ActualizarEstadoFacturaAsync(It.IsAny<string>(), It.IsAny<InvoiceState>()), Times.Never);
        }
    }
}
