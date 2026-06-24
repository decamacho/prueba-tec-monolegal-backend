using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services
{
    public class ConsoleEmailService : IEmailService
    {
        private readonly ILogger<ConsoleEmailService> _logger;

        public ConsoleEmailService(ILogger<ConsoleEmailService> logger)
        {
            _logger = logger;
        }

        public Task<bool> EnviarNotificacionAsync(Invoice factura, InvoiceState nuevoEstado)
        {
            _logger.LogWarning("");
            _logger.LogWarning("==========================================================");
            _logger.LogWarning(" SIMULACION DE ENVIO DE CORREO (Modo Desarrollo/Prueba)");
            _logger.LogWarning("==========================================================");
            _logger.LogWarning(" Para:     {Email} ({Nombre})", factura.Cliente.EmailContacto, factura.Cliente.Nombre);
            _logger.LogWarning(" Factura:  {Codigo}", factura.CodigoFactura);
            _logger.LogWarning(" Estado:   Pasando a '{Estado}'", nuevoEstado.ToString().ToUpper());
            _logger.LogWarning("----------------------------------------------------------");

            string mensajeMensaje = nuevoEstado switch
            {
                InvoiceState.primerrecordatorio => "Mensaje: Recordatorio preventivo de pago proximo a vencer.",
                InvoiceState.segundorecordatorio => "Mensaje: Segundo aviso de pago atrasado. Riesgo de suspension.",
                InvoiceState.desactivado => "Mensaje: Notificacion de suspensión de servicios por falta de pago.",
                _ => $"Mensaje: Cambio de estado a {nuevoEstado}"
            };

            _logger.LogWarning(" {Mensaje}", mensajeMensaje);
            _logger.LogWarning("==========================================================");
            _logger.LogWarning("");

            return Task.FromResult(true);
        }
    }
}