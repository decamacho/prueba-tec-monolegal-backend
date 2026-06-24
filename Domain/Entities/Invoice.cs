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
        public List<InvoiceItem> Items { get; set; } = new List<InvoiceItem>();

        public FinanceResume ResumenFinanciero { get; set; } = new FinanceResume();
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

        public void AgregarItem(InvoiceItem item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));

            Items.Add(item);
            RecalcularResumenFinanciero();
        }

        public bool RemoverItem(InvoiceItem item)
        {
            if (item == null) return false;

            var removed = Items.Remove(item);
            if (removed) RecalcularResumenFinanciero();
            return removed;
        }

        public decimal CalcularTotal()
        {
            return Items.Sum(i => i.Cantidad * i.PrecioUnitario);
        }

        private void RecalcularResumenFinanciero()
        {
            ResumenFinanciero.Total = CalcularTotal();
        }
    }
}