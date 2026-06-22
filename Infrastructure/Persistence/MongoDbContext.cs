using Domain.Entities;
using Infrastructure.Configurations;
using Microsoft.Extensions.Options;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;

namespace Infrastructure.Persistence
{
    public class MongoDbContext
    {
        private readonly IMongoDatabase _database;

        public MongoDbContext(IOptions<MongoDbSettings> mongoSettings)
        {
            // CONFIGURACIÓN SENIOR: Registrar convención CamelCase antes de inicializar la conexión
            var conventionPack = new ConventionPack { new CamelCaseElementNameConvention() };
            ConventionRegistry.Register("CamelCaseConventions", conventionPack, t => true);

            // Inicializar el cliente y la base de datos
            var client = new MongoClient(mongoSettings.Value.ConnectionString);
            _database = client.GetDatabase(mongoSettings.Value.DatabaseName);
        }

        // Exponer la colección de facturas de forma tipada usando nuestra entidad de Dominio
        public IMongoCollection<Invoice> Invoices => _database.GetCollection<Invoice>("invoices");
    }
}