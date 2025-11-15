using System;
using Data.Models.References;
using Newtonsoft.Json;

namespace Data.Serializable.Converters
{
    public abstract class BaseRefConverter<TRef, TTarget> : JsonConverter<TRef>
        where TRef : IRef<TTarget>
    {
        public override void WriteJson(JsonWriter writer, TRef value, JsonSerializer serializer)
        {
            writer.WriteValue(value.Key);
        }

        public override TRef ReadJson(JsonReader reader, Type objectType, TRef existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            string key = (string)reader.Value;
            return CreateRef(key);
        }

        protected abstract TRef CreateRef(string key);
    }
    
}