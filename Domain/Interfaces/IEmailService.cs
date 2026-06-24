using Domain.Entities;
using Domain.Enums;

namespace Domain.Interfaces
{
    public interface IEmailService
    {
        Task<bool> EnviarNotificacionAsync(Invoice factura, InvoiceState nuevoEstado);
    }
}