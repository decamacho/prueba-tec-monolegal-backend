using Application.DTO;
using Application.Interfaces;
using Domain.Enums;
using Domain.Interfaces;

namespace Application.UseCases
{
    public class ProcessInvoiceRemindersUseCase : IProcessInvoiceReminders
    {
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly IEmailService _emailService;

        // inyeccion de dependencias a traves del constructor
        public ProcessInvoiceRemindersUseCase(
            IInvoiceRepository invoiceRepository,
            IEmailService emailService)
        {
            _invoiceRepository = invoiceRepository;
            _emailService = emailService;
        }

        public async Task ProcesarRecordatoriosAsync()
        {
            // extraer facturas en estado primer o segundo recordatorio
            var estadosBuscados = new List<InvoiceState>
            {
                InvoiceState.primerrecordatorio,
                InvoiceState.segundorecordatorio
            };

            var facturasParaProcesar = await _invoiceRepository.ObtenerFacturasPorEstadosAsync(estadosBuscados);

            // iteracion y procesar cada factura
            foreach (var factura in facturasParaProcesar)
            {
                // la entidad de dominio encapsula la regla de a que estado debe pasar
                factura.TransicionarEstado();

                // enviar el correo informando el NUEVO estado (segun la prueba)
                bool correoEnviado = await _emailService.EnviarNotificacionAsync(factura.Cliente, factura.Estado);

                // solo si el correo se envio con exito, se actualiza la base de datos
                if (correoEnviado)
                {
                    await _invoiceRepository.ActualizarEstadoFacturaAsync(factura.Id, factura.Estado);
                }
            }
        }

        public async Task<IEnumerable<InvoiceSummaryDto>> ObtenerResumenFacturasAsync()
        {
            var todosLosEstados = Enum.GetValues(typeof(InvoiceState)).Cast<InvoiceState>();
            var facturas = await _invoiceRepository.ObtenerFacturasPorEstadosAsync(todosLosEstados);

            return facturas.Select(f => new InvoiceSummaryDto
            {
                Id = f.Id,
                CodigoFactura = f.CodigoFactura,
                NombreCliente = f.Cliente.Nombre,

                // mapeo de los nuevos campos
                EmailContacto = f.Cliente.EmailContacto,
                CantidadProductos = f.Items.Count,

                TotalCobro = f.ResumenFinanciero.Total,
                EstadoActual = f.Estado.ToString()
            });
        }
    }
}