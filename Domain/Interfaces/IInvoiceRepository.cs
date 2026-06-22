using Domain.Entities;
using Domain.Enums;

namespace Domain.Interfaces
{
    public interface IInvoiceRepository
    {        
        Task<IEnumerable<Invoice>> ObtenerFacturasPorEstadosAsync(IEnumerable<InvoiceState> estados);
        Task ActualizarEstadoFacturaAsync(string id, InvoiceState nuevoEstado);
    }
}