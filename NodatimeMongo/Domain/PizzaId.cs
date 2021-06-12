using System;
using MongoDB.Bson.Serialization.Attributes;
using Richargh.Sandbox.NodatimeMongo.Lang;

namespace Richargh.Sandbox.NodatimeMongo.Domain
{
    [BsonSerializer(typeof(PizzaIdSerializer))]
    public class PizzaId : WrappedString
    {

        public override string RawValue { get; }
        
        public PizzaId(string rawValue)
        {
            RawValue = rawValue;
        }

        public static PizzaId Random()
        {
            return new PizzaId(Guid.NewGuid().ToString());
        }

        private class PizzaIdSerializer : WrappedStringSerializer<PizzaId>
        {
            protected override PizzaId Create(string rawValue) => new PizzaId(rawValue);
        }
    }
}