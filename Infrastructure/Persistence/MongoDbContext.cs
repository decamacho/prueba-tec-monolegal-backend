using Domain.Entities;
using Infrastructure.Configurations;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;

namespace Infrastructure.Persistence
{
    public class MongoDbContext
        : IMongoDbContext
    {
        private readonly IMongoDatabase _database;
        private readonly ILogger<MongoDbContext> _logger;

        public MongoDbContext(IOptions<MongoDbSettings> mongoSettings, ILogger<MongoDbContext> logger)
        {
            _logger = logger;

            _logger.LogInformation("=== CONECTANDO A DB: {Database} ===", mongoSettings.Value.DatabaseName);
            _logger.LogDebug("=== CONNECTION STRING: {ConnectionString} ===", mongoSettings.Value.ConnectionString);

            // convenciones globales: CamelCase y Enums como Textos
            var conventionPack = new ConventionPack
            {
                new CamelCaseElementNameConvention(),
                new EnumRepresentationConvention(BsonType.String)
            };
            ConventionRegistry.Register("CustomConventions", conventionPack, t => true);

            // mapeo especifico de la Entidad (Para no ensuciar el Dominio con atributos [BsonId])
            if (!BsonClassMap.IsClassMapRegistered(typeof(Invoice)))
            {
                BsonClassMap.RegisterClassMap<Invoice>(cm =>
                {
                    cm.AutoMap();
                    // soluciona la traduccion entre el ObjectId de Mongo y el string Id
                    cm.MapIdProperty(i => i.Id)
                      .SetIdGenerator(StringObjectIdGenerator.Instance)
                      .SetSerializer(new StringSerializer(BsonType.ObjectId));
                });
            }

            // inicializar la conexion
            var client = new MongoClient(mongoSettings.Value.ConnectionString);
            _database = client.GetDatabase(mongoSettings.Value.DatabaseName);
        }

        public IMongoCollection<Invoice> Invoices => _database.GetCollection<Invoice>("invoices");
        public IMongoCollection<T> GetCollection<T>(string name) => _database.GetCollection<T>(name);
    }
}