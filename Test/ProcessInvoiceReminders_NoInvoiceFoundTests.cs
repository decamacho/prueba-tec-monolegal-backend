using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using Moq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Application.UseCases;
using Xunit;

namespace Test
{
    public class ProcessInvoiceReminders_NoInvoiceFoundTests
    {
        [Fact]
        public async Task ProcesarRecordatorioFacturaAsync_DevuelveFalse_SiFacturaNoExiste()
        {
            var repoMock = new Mock<IInvoiceRepository>();
            var emailMock = new Mock<IEmailService>();
            var loggerMock = new Mock<Application.Interfaces.IAppLogger<ProcessInvoiceRemindersUseCase>>();

            repoMock.Setup(r => r.ObtenerFacturaPorIdAsync(It.IsAny<string>())).ReturnsAsync((Invoice?)null);

            var useCase = new ProcessInvoiceRemindersUseCase(repoMock.Object, emailMock.Object, loggerMock.Object);

            var result = await useCase.ProcesarRecordatorioFacturaAsync("no-existe");

            Assert.False(result);
            emailMock.Verify(e => e.EnviarNotificacionAsync(It.IsAny<Invoice>(), It.IsAny<InvoiceState>()), Times.Never);
        }
    }
}
