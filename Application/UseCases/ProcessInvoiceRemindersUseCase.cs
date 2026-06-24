using Application.DTO;
using Application.Interfaces;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using Application.Interfaces;

namespace Application.UseCases
{
    public class ProcessInvoiceRemindersUseCase : IProcessInvoiceReminders
    {
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly IEmailService _emailService;
        private readonly IAppLogger<ProcessInvoiceRemindersUseCase> _logger;

        // inyeccion de dependencias a traves del constructor
        public ProcessInvoiceRemindersUseCase(
            IInvoiceRepository invoiceRepository,
            IEmailService emailService,
            IAppLogger<ProcessInvoiceRemindersUseCase> logger)
        {
            _invoiceRepository = invoiceRepository;
            _emailService = emailService;
            _logger = logger;
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
                try
                {
                    // la entidad de dominio encapsula la regla de a que estado debe pasar
                    factura.TransicionarEstado();

                    bool correoEnviado = await _emailService.EnviarNotificacionAsync(factura, factura.Estado);

                    // solo si el correo se envio con exito, se actualiza la base de datos
                    if (correoEnviado)
                    {
                        await _invoiceRepository.ActualizarEstadoFacturaAsync(factura.Id, factura.Estado);
                    }
                }
                catch (Exception ex)
                {
                    // Loguear el error y continuar con la siguiente factura para evitar detener el batch
                    _logger?.LogError(ex, "Error procesando factura {InvoiceId}", factura?.Id);
                    continue;
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

        public async Task<IEnumerable<InvoiceSummaryDto>> ObtenerFacturasPorClienteAsync(string documentoCliente)
        {
            // obtener las facturas del cliente desde el repositorio
            var facturas = await _invoiceRepository.ObtenerFacturasPorClienteAsync(documentoCliente);

            // mapear a DTO para mantener limpia la salida hacia la interfaz grafica
            return facturas.Select(f => new InvoiceSummaryDto
            {
                Id = f.Id,
                CodigoFactura = f.CodigoFactura,
                NombreCliente = f.Cliente.Nombre,
                EmailContacto = f.Cliente.EmailContacto,
                CantidadProductos = f.Items.Count,
                TotalCobro = f.ResumenFinanciero.Total,
                EstadoActual = f.Estado.ToString()
            });
        }

        public async Task<bool> ProcesarRecordatorioFacturaAsync(string facturaId)
        {
            var factura = await _invoiceRepository.ObtenerFacturaPorIdAsync(facturaId);
            if (factura == null || factura.Estado == InvoiceState.desactivado)
            {
                return false;
            }

            factura.TransicionarEstado();

            bool correoEnviado = await _emailService.EnviarNotificacionAsync(factura, factura.Estado);

            if (correoEnviado)
            {
                await _invoiceRepository.ActualizarEstadoFacturaAsync(factura.Id, factura.Estado);
                return true;
            }

            return false;
        }

        public async Task<IEnumerable<Client>> ObtenerClientesExistentesAsync()
        {
            return await _invoiceRepository.ObtenerClientesUnicosAsync();
        }

        public async Task<IEnumerable<dynamic>> ObtenerItemsExistentesAsync()
        {
            return await _invoiceRepository.ObtenerItemsUnicosAsync();
        }
    }
}