using System;
using System.Globalization;
using Newtonsoft.Json;
using UnityEngine;
using Utils.Extensions;

namespace Systems.WorldSystem
{
    [JsonConverter(typeof(ValueGeneratorConverter))]
    public interface IValueGenerator
    {
        int Evaluate(int context);
        string Serialize();
    }

    public static class ValueGeneratorExtensions
    {
        public static int EvaluateOrDefault(this IValueGenerator generator, int context, int defaultValue)
        {
            return generator?.Evaluate(context) ?? defaultValue;
        }
    }
    
    public class FixedValueGenerator : IValueGenerator
    {
        private readonly int _value;

        public FixedValueGenerator(int value) => _value = value;

        public int Evaluate(int context) => _value;

        public string Serialize() => _value.ToInvariantString();

        public static bool TryParse(JsonReader reader, out IValueGenerator result)
        {
            if (reader.TokenType is JsonToken.Integer or JsonToken.Float)
            {
                result = new FixedValueGenerator(Convert.ToInt32(reader.Value, CultureInfo.InvariantCulture));
                return true;
            }
            result = null;
            return false;
        }
    }

    public class NormalizedValueGenerator : IValueGenerator 
    {
        private readonly float _normalized;

        public NormalizedValueGenerator(float normalized)
        {
            _normalized = normalized;
        }

        public int Evaluate(int context)
        { 
            float val = Mathf.Clamp01(_normalized);
            return Mathf.FloorToInt(val * context);
        }
        
        public string Serialize() =>
            (_normalized * 100f).ToString("0.##", CultureInfo.InvariantCulture) + "%";

        public static bool TryParse(string str, out IValueGenerator result)
        {
            result = null;
            if (!str.EndsWith("%")) return false;
            string numberPart = str[..^1];
            if (!float.TryParse(numberPart, NumberStyles.Float, CultureInfo.InvariantCulture, out float percent))
                return false;
            result = new NormalizedValueGenerator(percent / 100f);
            return true;
        }

        public override string ToString()
        {
            return $"{nameof(NormalizedValueGenerator)}({nameof(_normalized)}: {_normalized})";
        }
    }
    
    public class FractionValueGenerator : IValueGenerator 
    {
        private readonly int _numerator;
        private readonly int _denominator;

        public FractionValueGenerator(int numerator, int denominator)
        {
            _numerator = numerator;
            _denominator = denominator;
        }
        
        public string Serialize() => $"{_numerator}/{_denominator}";

        public static bool TryParse(string str, out IValueGenerator result)
        {
            result = null;
            var parts = str.Split('/');
            if (parts.Length == 2 &&
                int.TryParse(parts[0], out int num) &&
                int.TryParse(parts[1], out int den) &&
                den != 0)
            {
                result = new FractionValueGenerator(num, den);
                return true;
            }
            return false;
        }


        public int Evaluate(int context) => context * _numerator / _denominator;

        public override string ToString()
        {
            return $"{nameof(FractionValueGenerator)}({nameof(_numerator)}: {_numerator}, {nameof(_denominator)}: {_denominator})";
        }
    }
    
    public class ValueGeneratorConverter : JsonConverter<IValueGenerator>
    {
        public override IValueGenerator ReadJson(JsonReader reader, Type objectType, IValueGenerator existingValueGenerator, bool hasExistingValue, JsonSerializer serializer)
        {
            if (reader.TokenType is JsonToken.Integer or JsonToken.Float)
            {
                if (FixedValueGenerator.TryParse(reader, out var value))
                    return value;
            }
            
            if (reader.TokenType == JsonToken.String)
            {
                string str = (reader.Value as string)?.Trim();
                if (string.IsNullOrEmpty(str))
                    throw new JsonSerializationException("Argument is null");

                if (NormalizedValueGenerator.TryParse(str, out var normalized))
                    return normalized;

                if (FractionValueGenerator.TryParse(str, out var fraction))
                    return fraction;
            }

            throw new JsonSerializationException($"Invalid value generator format: {reader.Value}");
        }

        public override void WriteJson(JsonWriter writer, IValueGenerator valueGenerator, JsonSerializer serializer)
        {
            writer.WriteValue(valueGenerator.Serialize());
        }
    }
    
}