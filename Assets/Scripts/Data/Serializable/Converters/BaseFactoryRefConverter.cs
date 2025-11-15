using System;
using Data.Models.References;
using Newtonsoft.Json;

namespace Data.Serializable.Converters
{
    public abstract class BaseFactoryRefConverter<TRef, TTarget, TContext> : JsonConverter<TRef>
        where TRef : IFactoryRef<TContext, TTarget>
    {
        private readonly Func<string, TRef> _creator;

        protected BaseFactoryRefConverter(Func<string, TRef> creator)
        {
            _creator = creator;
        }
        public override void WriteJson(JsonWriter writer, TRef value, JsonSerializer serializer)
        {
            writer.WriteValue(value.Key);
        }

        public override TRef ReadJson(JsonReader reader, Type objectType, TRef existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            string key = (string)reader.Value;
            return _creator(key);
        }
    }
}