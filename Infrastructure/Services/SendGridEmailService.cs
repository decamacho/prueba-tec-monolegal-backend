using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Infrastructure.Services
{
    public class SendGridEmailService : IEmailService
    {
        private readonly ILogger<SendGridEmailService> _logger;
        private readonly string _apiKey;
        private readonly string _fromEmail;
        private readonly string _fromName;

        public SendGridEmailService(ILogger<SendGridEmailService> logger, IConfiguration configuration)
        {
            _logger = logger;
            _apiKey = configuration["SendGrid:ApiKey"] ?? string.Empty;
            _fromEmail = configuration["SendGrid:FromEmail"] ?? string.Empty;
            _fromName = configuration["SendGrid:FromName"] ?? "Monolegal";
        }

        public async Task<bool> EnviarNotificacionAsync(Invoice factura, InvoiceState nuevoEstado)
        {
            try
            {
                var cliente = factura.Cliente;
                var client = new SendGridClient(_apiKey);
                var from = new EmailAddress(_fromEmail, _fromName);
                var to = new EmailAddress(cliente.EmailContacto, cliente.Nombre);

                string subject = nuevoEstado == InvoiceState.desactivado
                    ? $"Aviso sobre el estado de sus servicios - Factura {factura.CodigoFactura}"
                    : $"Notificación de Estado de Factura {factura.CodigoFactura} - Monolegal";

                // 2. Construcción del cuerpo del mensaje según el estado
                string mensajePrincipal = nuevoEstado switch
                {
                    InvoiceState.primerrecordatorio =>
                        $"Queremos recordarle amablemente que la factura <strong>{factura.CodigoFactura}</strong> se encuentra próxima a vencer o tiene un saldo pendiente. Le invitamos a realizar el pago para mantener su cuenta al día.",

                    InvoiceState.segundorecordatorio =>
                        $"Le escribimos para informarle que no hemos registrado el pago de la factura <strong>{factura.CodigoFactura}</strong>. Entendemos que en el día a día estos detalles pueden pasar desapercibidos, por lo que le invitamos a regularizar su estado de cuenta a la mayor brevedad posible para evitar interrupciones en su servicio.",

                    InvoiceState.desactivado =>
                        $"Lamentamos informarle que, al no registrar el pago de la factura <strong>{factura.CodigoFactura}</strong> tras nuestros recordatorios previos, los servicios asociados a su cuenta han sido pausados de manera preventiva. Tan pronto se confirme el pago, sus servicios serán restablecidos automáticamente.",

                    _ => $"Le notificamos que su factura <strong>{factura.CodigoFactura}</strong> ha cambiado al estado: {nuevoEstado}."
                };

                string htmlContent = $@"
                    <div style='font-family: Arial, sans-serif; color: #333; line-height: 1.6; max-width: 600px; margin: 0 auto; border: 1px solid #e5e7eb; border-radius: 8px; overflow: hidden;'>
                        <div style='background-color: #f8fafc; padding: 20px; border-bottom: 1px solid #e5e7eb; text-align: center;'>
                            <h2 style='color: #1e293b; margin: 0;'>{_fromName}</h2>
                        </div>
                        
                        <div style='padding: 30px;'>
                            <p>Estimado(a) <strong>{cliente.Nombre}</strong>,</p>
                            
                            <p style='margin-bottom: 24px;'>{mensajePrincipal}</p>
                            
                            <div style='background-color: #f1f5f9; padding: 15px; border-left: 4px solid #9ecb24; border-radius: 4px; margin-bottom: 24px;'>
                                <p style='margin: 0; font-size: 14px;'>
                                    <strong>¿Ya realizó el pago?</strong><br/>
                                    Si usted ya efectuó el pago de esta factura, por favor haga caso omiso a este correo o envíenos el comprobante respondiendo a este mensaje para actualizar nuestro sistema inmediatamente.
                                </p>
                            </div>

                            <p>Si tiene alguna duda o necesita asistencia con sus opciones de pago, nuestro equipo de soporte está a su entera disposición.</p>
                        </div>
                        
                        <div style='background-color: #f8fafc; padding: 20px; border-top: 1px solid #e5e7eb; font-size: 12px; color: #64748b; text-align: center;'>
                            <p style='margin: 0 0 5px 0;'>Atentamente,</p>
                            <p style='margin: 0; font-weight: bold; color: #1e293b; font-size: 14px;'>El equipo de {_fromName}</p>
                            <p style='margin: 10px 0 0 0;'>soporte@demo-monolegal.co | +57 300 000 5412</p>
                            <p style='margin: 5px 0 0 0;'>Edificio Nacional, torre 1 oficina 600, Tunja, Boyacá, Colombia</p>
                        </div>
                    </div>";

                string plainTextContent = "Por favor, active la vista HTML para visualizar este correo correctamente.";

                var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
                var response = await client.SendEmailAsync(msg);

                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("Correo enviado exitosamente a {Email} para la factura {Factura}", cliente.EmailContacto, factura.CodigoFactura);
                    return true;
                }

                _logger.LogWarning("Error de SendGrid al enviar a {Email}. StatusCode: {StatusCode}", cliente.EmailContacto, response.StatusCode);
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Excepción al enviar correo de la factura {FacturaId}", factura?.Id);
                return false;
            }
        }
    }
}