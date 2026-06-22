using Domain.Entities;
using Domain.Enums;

namespace Domain.Interfaces
{
    public interface IEmailService
    {
        Task<bool> EnviarNotificacionAsync(Client receptor, InvoiceState nuevoEstado);
    }
}