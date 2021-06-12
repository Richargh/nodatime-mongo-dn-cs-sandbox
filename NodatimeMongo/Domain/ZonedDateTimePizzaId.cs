using System;
using MongoDB.Bson.Serialization.Attributes;
using Richargh.Sandbox.NodatimeMongo.Lang;

namespace Richargh.Sandbox.NodatimeMongo.Domain
{
    [BsonSerializer(typeof(ZonedPizzaIdSerializer))]
    public class ZonedDateTimePizzaId : WrappedString
    {

        public override string RawValue { get; }
        
        public ZonedDateTimePizzaId(string rawValue)
        {
            RawValue = rawValue;
        }

        public static ZonedDateTimePizzaId Random()
        {
            return new ZonedDateTimePizzaId(Guid.NewGuid().ToString());
        }

        private class ZonedPizzaIdSerializer : WrappedStringSerializer<ZonedDateTimePizzaId>
        {
            protected override ZonedDateTimePizzaId Create(string rawValue) => new ZonedDateTimePizzaId(rawValue);
        }
    }
}