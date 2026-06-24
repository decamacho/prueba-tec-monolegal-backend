using Domain.Entities;
using Domain.Enums;

namespace Domain.Interfaces
{
    public interface IInvoiceRepository
    {        
        Task<IEnumerable<Invoice>> ObtenerFacturasPorEstadosAsync(IEnumerable<InvoiceState> estados);

        Task<IEnumerable<Invoice>> ObtenerFacturasPorClienteAsync(string documentoCliente);

        Task ActualizarEstadoFacturaAsync(string id, InvoiceState nuevoEstado);

        Task<Invoice> ObtenerFacturaPorIdAsync(string id);

        Task<IEnumerable<Client>> ObtenerClientesUnicosAsync();

        Task<IEnumerable<dynamic>> ObtenerItemsUnicosAsync();
    }
}