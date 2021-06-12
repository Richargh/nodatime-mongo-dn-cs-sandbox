using System;
using MongoDB.Bson.Serialization;

namespace Richargh.Sandbox.NodatimeMongo.Lang
{
    public abstract class WrappedStringSerializer<T> : IBsonSerializer<T> where T: WrappedString
    {
        public Type ValueType => typeof(T);
        protected abstract T Create(string rawValue);

        public void Serialize(BsonSerializationContext context, BsonSerializationArgs args, object value)
        {
            Serialize(context, args, (T) value);
        }
        public void Serialize(BsonSerializationContext context, BsonSerializationArgs args, T value)
        {
            context.Writer.WriteString(value.RawValue);
        }

        public T Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            var rawValue = context.Reader.ReadString();
            return Create(rawValue);
        }
        
        object IBsonSerializer.Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            return Deserialize(context, args);
        }
    }
}