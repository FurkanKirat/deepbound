using System;
using Newtonsoft.Json;
using UnityEngine;

namespace Data.Models.References
{
    [JsonConverter(typeof(Converter))]
    public class ColorRef
    {
        private string Key { get; }
        private Color? _cached;

        public ColorRef(string key)
        {
            Key = key;
        }

        public Color Load()
        {
            if (_cached.HasValue)
                return _cached.Value;
            
            if (ColorUtility.TryParseHtmlString(Key, out var color))
                _cached = color;
            else
                _cached = Color.white;

            return _cached.Value;
        }

        public override string ToString()
        {
            var loaded = Load();
            return $"Key: {Key}, Cached: {loaded}";
        }
        private class Converter : JsonConverter<ColorRef>
        {
            public override void WriteJson(JsonWriter writer, ColorRef value, JsonSerializer serializer)
            {
                writer.WriteValue(value.Key);
            }

            public override ColorRef ReadJson(JsonReader reader, Type objectType, ColorRef existingValue, bool hasExistingValue,
                JsonSerializer serializer)
            {
                string key = (string)reader.Value;
                return new ColorRef(key);
            }
        }
    
    }
}