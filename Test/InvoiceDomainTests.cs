using Domain.Entities;
using Xunit;

namespace Test
{
    public class InvoiceDomainTests
    {
        [Fact]
        public void Invoice_AgregarYRemoverItem_CalculaTotalCorrectamente()
        {
            var factura = new Invoice();

            var item1 = new InvoiceItem { Descripcion = "P1", Cantidad = 2, PrecioUnitario = 10m };
            var item2 = new InvoiceItem { Descripcion = "P2", Cantidad = 1, PrecioUnitario = 5.5m };

            factura.AgregarItem(item1);
            factura.AgregarItem(item2);

            var totalEsperado = 2 * 10m + 1 * 5.5m;
            Assert.Equal(totalEsperado, factura.CalcularTotal());

            var removed = factura.RemoverItem(item1);
            Assert.True(removed);
            Assert.Equal(1, factura.Items.Count);
            Assert.Equal(5.5m, factura.CalcularTotal());
        }
    }
}
