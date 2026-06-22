using Application.DTO;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

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
    }
}