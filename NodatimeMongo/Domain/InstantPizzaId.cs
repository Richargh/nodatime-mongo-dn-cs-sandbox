using System;
using MongoDB.Bson.Serialization.Attributes;
using Richargh.Sandbox.NodatimeMongo.Lang;

namespace Richargh.Sandbox.NodatimeMongo.Domain
{
    [BsonSerializer(typeof(InstantPizzaIdSerializer))]
    public class InstantPizzaId : WrappedString
    {

        public override string RawValue { get; }
        
        public InstantPizzaId(string rawValue)
        {
            RawValue = rawValue;
        }

        public static InstantPizzaId Random()
        {
            return new InstantPizzaId(Guid.NewGuid().ToString());
        }

        private class InstantPizzaIdSerializer : WrappedStringSerializer<InstantPizzaId>
        {
            protected override InstantPizzaId Create(string rawValue) => new InstantPizzaId(rawValue);
        }
    }
}