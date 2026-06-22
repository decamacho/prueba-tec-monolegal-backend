using Application.DTO;

namespace Application.Interfaces
{
    public interface IProcessInvoiceReminders
    {
        Task ProcesarRecordatoriosAsync();

        Task<IEnumerable<InvoiceSummaryDto>> ObtenerResumenFacturasAsync();
    }
}