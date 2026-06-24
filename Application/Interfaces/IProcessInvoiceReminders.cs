using Application.DTO;
using Domain.Entities;

namespace Application.Interfaces
{
    public interface IProcessInvoiceReminders
    {
        Task ProcesarRecordatoriosAsync();

        Task<IEnumerable<InvoiceSummaryDto>> ObtenerResumenFacturasAsync();

        Task<IEnumerable<InvoiceSummaryDto>> ObtenerFacturasPorClienteAsync(string documentoCliente);

        Task<bool> ProcesarRecordatorioFacturaAsync(string facturaId);

        Task<IEnumerable<Client>> ObtenerClientesExistentesAsync();

        Task<IEnumerable<dynamic>> ObtenerItemsExistentesAsync();
    }
}