using System;
using Interfaces;
using Newtonsoft.Json;
using Utils.Parsers;

namespace Data.Serializable
{
    using UnityEngine;

    [Serializable] [JsonConverter(typeof(Converter))]
    public struct Float2 : IStringConvertible
    {
        public float x, y;

        public Float2(float x, float y)
        {
            this.x = x;
            this.y = y;
        }
        public Float2(Vector2 v)
        {
            x = v.x;
            y = v.y;
        }
        

        public Vector2 ToVector2() => new (x, y);
        
        public static implicit operator Vector2(Float2 dto) => new (dto.x, dto.y);
        public static implicit operator Float2(Vector2 v) => new (v);
        
        public string ToStringValue() => FloatVectorParser.ToStringValue(x,y);

        public override string ToString() => ToStringValue();
        
        private sealed class Converter : JsonConverter<Float2>
        {
            public override void WriteJson(JsonWriter writer, Float2 value, JsonSerializer serializer)
            {
                if (float.IsNaN(value.x) || float.IsNaN(value.y) ||
                    float.IsInfinity(value.x) || float.IsInfinity(value.y))
                {
                    throw new JsonSerializationException("Invalid float value in Vector2Dto.");
                }
                writer.WriteValue(value.ToStringValue());
            }

            public override Float2 ReadJson(JsonReader reader, Type objectType, Float2 existingValue, bool hasExistingValue, JsonSerializer serializer)
            {
                var str = (string)reader.Value;
                if (FloatVectorParser.TryParse(str, out var tuple))
                    return new Float2(tuple.x, tuple.y);
                throw new JsonSerializationException("Invalid Vector2Dto");
            }

            public override bool CanRead => true;
            public override bool CanWrite => true;
        }
        
    }

}