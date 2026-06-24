using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Infrastructure.Persistence
{
    public class MongoDbInvoiceRepository : IInvoiceRepository
    {
        private readonly IMongoDbContext _context;

        public MongoDbInvoiceRepository(IMongoDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Invoice>> ObtenerFacturasPorEstadosAsync(IEnumerable<InvoiceState> estados)
        {
            var filter = Builders<Invoice>.Filter.In(i => i.Estado, estados);

            return await _context.Invoices
                .Find(filter)
                .ToListAsync();
        }

        public async Task ActualizarEstadoFacturaAsync(string id, InvoiceState nuevoEstado)
        {
            var filter = Builders<Invoice>.Filter.Eq(i => i.Id, id);
            var update = Builders<Invoice>.Update.Set(i => i.Estado, nuevoEstado);

            await _context.Invoices.UpdateOneAsync(filter, update);
        }

        public async Task<IEnumerable<Invoice>> ObtenerFacturasPorClienteAsync(string documentoCliente)
        {
            var filter = Builders<Invoice>.Filter.Eq(i => i.Cliente.Documento, documentoCliente);

            return await _context.Invoices
                .Find(filter)
                .ToListAsync();
        }

        public async Task<Invoice> ObtenerFacturaPorIdAsync(string id)
        {
            var filter = Builders<Invoice>.Filter.Eq("_id", new ObjectId(id));

            return await _context.Invoices
                .Find(filter)
                .FirstOrDefaultAsync();
        }

        // extraer clientes unicos basados en las facturas existentes
        public async Task<IEnumerable<Client>> ObtenerClientesUnicosAsync()
        {
            return await _context.Invoices.Aggregate()
                .Group(
                    f => f.Cliente.Documento,

                g => new Client
                {
                    Documento = g.Key,
                    Nombre = g.First().Cliente.Nombre,
                    EmailContacto = g.First().Cliente.EmailContacto,
                    Telefono = g.First().Cliente.Telefono,
                    Direccion = g.First().Cliente.Direccion,

                    // Corregir mapeo: SumaFacturas debe ser la suma de totales, NumeroFacturas el conteo
                    SumaFacturas = g.Sum(f => f.ResumenFinanciero.Total),
                    NumeroFacturas = g.Count()
                }
                )
                .ToListAsync();
        }

        // extraer ítems => productos unicos basados en el historico de facturas
        public async Task<IEnumerable<dynamic>> ObtenerItemsUnicosAsync()
        {
            var facturas = await _context.Invoices.Find(_ => true).ToListAsync();

            return facturas
                .SelectMany(f => f.Items)
                .GroupBy(i => i.Descripcion)
                .Select(g => new
                {
                    Descripcion = g.Key,
                    PrecioUnitario = g.First().PrecioUnitario
                })
                .ToList();
        }
    }
}