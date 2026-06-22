namespace Application.DTO
{
    public class InvoiceSummaryDto
    {
        public string Id { get; set; } = string.Empty;
        public string CodigoFactura { get; set; } = string.Empty;
        public string NombreCliente { get; set; } = string.Empty;

        public string EmailContacto { get; set; } = string.Empty;
        public int CantidadProductos { get; set; }

        public decimal TotalCobro { get; set; }
        public string EstadoActual { get; set; } = string.Empty;
    }
}