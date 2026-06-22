using Domain.Entities;
using Domain.Enums;

namespace Domain.Entities
{
    public class Invoice
    {
        public string Id { get; set; } = string.Empty;
        public string CodigoFactura { get; set; } = string.Empty;
        public DateTime FechaEmision { get; set; }
        public Client Cliente { get; set; } = new Client();

        // AÑADIDO: Lista de productos/servicios cobrados
        public List<InvoiceItem> Items { get; set; } = new List<InvoiceItem>();

        public FinanceResume FinanceResume { get; set; } = new FinanceResume();
        public InvoiceState Estado { get; private set; }

        public void SetEstadoInicial(InvoiceState estado)
        {
            Estado = estado;
        }

        public void TransicionarEstado()
        {
            if (Estado == InvoiceState.primerrecordatorio)
            {
                Estado = InvoiceState.segundorecordatorio;
            }
            else if (Estado == InvoiceState.segundorecordatorio)
            {
                Estado = InvoiceState.desactivado;
            }
        }
    }
}