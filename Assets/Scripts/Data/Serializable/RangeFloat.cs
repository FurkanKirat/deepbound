using System;
using Interfaces;
using Newtonsoft.Json;
using UnityEngine;
using Utils.Extensions;

namespace Data.Serializable
{
    [JsonConverter(typeof(Converter))]
    public readonly struct RangeFloat : IStringConvertible
    {
        public readonly float Min;
        public readonly float Max;

        public RangeFloat(float min, float max)
        {
            Min = min;
            Max = max;
        }

        public float RandomValue => UnityEngine.Random.Range(Min, Max);

        public override string ToString() => ToStringValue();
        
        public string ToStringValue()
        {
            return Mathf.Approximately(Max, Min) ? 
                Min.ToInvariantString() : 
                $"{Min.ToInvariantString()}-{Max.ToInvariantString()}";
        }
        public static RangeFloat FromString(string str)
        {
            if (str.Contains("-"))
            {
                var parts = str.Split('-');
                if (parts.Length != 2)
                    throw new FormatException($"Invalid RangeInt format: {str}");
                
                if (float.TryParse(parts[0], out var min) 
                    && float.TryParse(parts[1], out var max))
                    return new RangeFloat(min, max);
            }
            else
            {
                if (int.TryParse(str, out var count))
                    return new RangeFloat(count, count);
            }
            
            throw new FormatException($"Invalid RangeInt format: {str}");
            
        }
        private sealed class Converter : JsonConverter<RangeFloat>
        {
            public override void WriteJson(JsonWriter writer, RangeFloat value, JsonSerializer serializer)
            {
                writer.WriteValue(value.ToStringValue());
            }

            public override RangeFloat ReadJson(JsonReader reader, Type objectType, RangeFloat existingValue, bool hasExistingValue, JsonSerializer serializer)
            {
                var str = (string)reader.Value;
                return RangeFloat.FromString(str);
            }
        }
    }

}