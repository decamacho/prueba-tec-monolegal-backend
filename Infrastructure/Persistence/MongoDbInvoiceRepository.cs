using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using MongoDB.Driver;

namespace Infrastructure.Persistence
{
    public class MongoDbInvoiceRepository : IInvoiceRepository
    {
        private readonly MongoDbContext _context;

        public MongoDbInvoiceRepository(MongoDbContext context)
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
    }
}