using Interfaces;
using Utils.Extensions;

namespace Data.Serializable
{
    using System;
    using Newtonsoft.Json;

    [JsonConverter(typeof(Converter))]
    public readonly struct RangeInt : IStringConvertible
    {
        public int Min { get; }
        public int Max { get; }

        public RangeInt(int min, int max)
        {
            Min = min;
            Max = max;
        }

        public RangeInt(int value)
        {
            Min = value;
            Max = value;
        }

        public int Roll(Random random)
        {
            return random.Next(Min, Max + 1);
        }

        public override string ToString() => ToStringValue();

        public string ToStringValue()
        {
            return Min == Max ? 
                Min.ToInvariantString() : 
                $"{Min.ToInvariantString()}-{Max.ToInvariantString()}";
        }
        public static RangeInt FromString(string str)
        {
            if (str.Contains("-"))
            {
                var parts = str.Split('-');
                if (parts.Length != 2)
                    throw new FormatException($"Invalid RangeInt format: {str}");
                
                if (int.TryParse(parts[0], out var min) 
                    && int.TryParse(parts[1], out var max))
                    return new RangeInt(min, max);
            }
            else
            {
                if (int.TryParse(str, out var count))
                    return new RangeInt(count);
            }
            
            throw new FormatException($"Invalid RangeInt format: {str}");
            
        }

        private sealed class Converter : JsonConverter<RangeInt>
        {
            public override void WriteJson(JsonWriter writer, RangeInt value, JsonSerializer serializer)
            {
                writer.WriteValue(value.ToStringValue());
            }

            public override RangeInt ReadJson(JsonReader reader, Type objectType, RangeInt existingValue, bool hasExistingValue, JsonSerializer serializer)
            {
                var str = reader.Value?.ToString();
                return string.IsNullOrEmpty(str) ? default : FromString(str);
            }
        }
    }

}