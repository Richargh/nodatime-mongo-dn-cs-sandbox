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
        public Instant Instant { get; }
        
        public InstantPizza(
            InstantPizzaId id, 
            Instant instant)
        {
            Id = id;
            
            Instant = instant;
        }
    }
}