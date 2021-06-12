using System;
using MongoDb.Bson.NodaTime;
using MongoDB.Bson.Serialization.Attributes;
using NodaTime;

namespace Richargh.Sandbox.NodatimeMongo.Domain
{
    public class ZonedDateTimePizza
    {
        [BsonId]
        public ZonedDateTimePizzaId Id { get; }

        [BsonSerializer(typeof(ZonedDateTimeSerializer))]
        public ZonedDateTime DateTime { get; }

        public ZonedDateTimePizza(
            ZonedDateTimePizzaId id, 
            ZonedDateTime dateTime)
        {
            Id = id;
            
            DateTime = dateTime;
        }
    }
}