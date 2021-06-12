using System;
using MongoDB.Bson.Serialization.Attributes;

namespace Richargh.Sandbox.NodatimeMongo.Domain
{
    public class DateTimePizza
    {
        [BsonId]
        public DateTimePizzaId Id { get; }

        public DateTime DateTimeUtc { get; }
        
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime DateTimeLocal { get; }

        public DateTimePizza(
            DateTimePizzaId id, 
            DateTime dateTimeUtc, 
            DateTime dateTimeLocal)
        {
            Id = id;
            
            DateTimeUtc = dateTimeUtc;
            DateTimeLocal = dateTimeLocal;
        }
            
    }
}