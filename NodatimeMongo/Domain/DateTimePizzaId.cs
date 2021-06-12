using System;
using MongoDB.Bson.Serialization.Attributes;
using Richargh.Sandbox.NodatimeMongo.Lang;

namespace Richargh.Sandbox.NodatimeMongo.Domain
{
    [BsonSerializer(typeof(PizzaIdSerializer))]
    public class DateTimePizzaId : WrappedString
    {

        public override string RawValue { get; }
        
        public DateTimePizzaId(string rawValue)
        {
            RawValue = rawValue;
        }

        public static DateTimePizzaId Random()
        {
            return new DateTimePizzaId(Guid.NewGuid().ToString());
        }

        private class PizzaIdSerializer : WrappedStringSerializer<DateTimePizzaId>
        {
            protected override DateTimePizzaId Create(string rawValue) => new DateTimePizzaId(rawValue);
        }
    }
}