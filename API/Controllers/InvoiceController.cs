using Application.DTO;
using Application.Interfaces;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

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
            return Ok(summary);
        }

        [HttpPost("process-reminders")]
        public async Task<IActionResult> ProcessReminders()
        {
            await _processInvoiceUseCase.ProcesarRecordatoriosAsync();

            return Ok(new { message = "Proceso de recordatorios ejecutado exitosamente" });
        }

        [HttpGet("test-db")]
        public async Task<IActionResult> TestDatabaseConnection(
            [FromServices] MongoDbContext context)
        {
            try
            {
                var dbName = context.Invoices.Database.DatabaseNamespace.DatabaseName;

                var collectionsCursor = await context.Invoices.Database.ListCollectionNamesAsync();
                var collections = await collectionsCursor.ToListAsync();

                var documentCount = await context.Invoices.CountDocumentsAsync(new MongoDB.Bson.BsonDocument());

                return Ok(new
                {
                    BaseDeDatosConectada = dbName,
                    ColeccionesEncontradas = collections,
                    TotalDocumentosEnInvoices = documentCount
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = ex.Message });
            }
        }
    }
}