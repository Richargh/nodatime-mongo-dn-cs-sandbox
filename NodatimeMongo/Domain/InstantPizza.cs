using System;
using MongoDb.Bson.NodaTime;
using MongoDB.Bson.Serialization.Attributes;
using NodaTime;

namespace Richargh.Sandbox.NodatimeMongo.Domain
{
    
    public class InstantPizza
    {
        [BsonId]
        public InstantPizzaId Id { get; }

        [BsonSerializer(typeof(InstantSerializer))]
        public Instant InstantUtc { get; }
        
        public InstantPizza(
            InstantPizzaId id, 
            Instant instantUtc)
        {
            Id = id;
            
            InstantUtc = instantUtc;
        }

        public static InstantPizza Now(InstantPizzaId id)
            => new InstantPizza(
                id,
                Instant.FromDateTimeUtc(DateTime.UtcNow));
    }
}