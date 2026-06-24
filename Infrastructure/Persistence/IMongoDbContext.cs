using Domain.Entities;
using MongoDB.Driver;

namespace Infrastructure.Persistence
{
    public interface IMongoDbContext
    {
        IMongoCollection<Invoice> Invoices { get; }

        // permite obtener otras colecciones si es necesario, restringiendo la surface pero manteniendo flexibilidad
        IMongoCollection<T> GetCollection<T>(string name);
    }
}
