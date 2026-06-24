using Application.DTO;
using Application.Interfaces;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using MongoDB.Bson;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class InvoiceController : ControllerBase
    {
        private readonly IProcessInvoiceReminders _processInvoiceUseCase;

        public InvoiceController(IProcessInvoiceReminders processInvoiceUseCase)
        {
            _processInvoiceUseCase = processInvoiceUseCase;
        }

        [HttpGet("summary")]
        [ProducesResponseType(typeof(IEnumerable<InvoiceSummaryDto>), 200)]
        public async Task<IActionResult> GetSummary()
        {
            var summary = await _processInvoiceUseCase.ObtenerResumenFacturasAsync();
            return Ok(ApiResponse<object>.Success(summary, "Resumen de facturas obtenido correctamente."));
        }


        // obtener facturas de un cliente especifico usando su documento
        [HttpGet("client/{documento}")]
        public async Task<IActionResult> GetByClient(string documento)
        {
            var summary = await _processInvoiceUseCase.ObtenerFacturasPorClienteAsync(documento);
        if (!summary.Any())
            {
                return Ok(ApiResponse<object>.Success(summary, $"No se encontraron facturas para el cliente con documento {documento}"));
            }

            return Ok(ApiResponse<object>.Success(summary, "Resumen de facturas por cliente obtenido correctamente"));
        }

        // modo automatico
        [HttpPost("process-send-reminders/masive")]
        public async Task<IActionResult> ProcessRemindersBatch()
        {
            await _processInvoiceUseCase.ProcesarRecordatoriosAsync();
            return Ok(ApiResponse<object>.Success(null, "Proceso masivo de recordatorios ejecutado exitosamente"));
        }

        // modo manual => por Factura
        [HttpPost("{id}/process-send-reminders")]
        public async Task<IActionResult> ProcessReminderSingle(string id)
        {

            if (!ObjectId.TryParse(id, out _))
            {
                return BadRequest(ApiResponse<object>.Error("El formato del ID proporcionado no es válido para MongoDB"));
            }

            var resultado = await _processInvoiceUseCase.ProcesarRecordatorioFacturaAsync(id);

            if (!resultado)
            {
                return BadRequest(ApiResponse<object>.Error("No se pudo procesar la factura. Verifique si existe o si ya esta desactivada"));
            }

            return Ok(ApiResponse<object>.Success(null, $"Factura {id} procesada y notificada exitosamente"));
        }

        // obtener catalogo de clientes unicos de las facturas
        [HttpGet("clients")]
        public async Task<IActionResult> GetClients()
        {
            var clientes = await _processInvoiceUseCase.ObtenerClientesExistentesAsync();
            return Ok(ApiResponse<object>.Success(clientes, "Listado de clientes obtenido correctamente"));
        }

        // obtener catalogo de items unicos de las facturas
        [HttpGet("items")]
        public async Task<IActionResult> GetItems()
        {
            var items = await _processInvoiceUseCase.ObtenerItemsExistentesAsync();
            return Ok(ApiResponse<object>.Success(items, "Listado de ítems obtenido correctamente"));
        }
    }
}