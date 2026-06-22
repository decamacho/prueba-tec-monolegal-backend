using Domain.Entities;
using Infrastructure.Configurations;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;

namespace Infrastructure.Persistence
{
    public class MongoDbContext
    {
        private readonly IMongoDatabase _database;

        public MongoDbContext(IOptions<MongoDbSettings> mongoSettings)
        {

            Console.WriteLine($"=== CONECTANDO A DB: {mongoSettings.Value.DatabaseName} ===");
            Console.WriteLine($"=== CONNECTION STRING: {mongoSettings.Value.ConnectionString} ===");

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
    }
}