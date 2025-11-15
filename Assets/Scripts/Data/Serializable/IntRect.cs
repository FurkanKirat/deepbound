using System;
using Newtonsoft.Json;
using Utils.Parsers;

namespace Data.Serializable
{
    using UnityEngine;

    [Serializable]
    [JsonConverter(typeof(IntRectConverter))]
    public struct IntRect
    {
        public int x, y, width, height;

        public IntRect(int x, int y, int width, int height)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
        }
        
        public IntRect(RectInt r)
        {
            x = r.x;
            y = r.y;
            width = r.width;
            height = r.height;
        }

        public Rect ToRect() => new Rect(x, y, width, height);

        public override string ToString()
        {
            return $"{nameof(x)}: {x}, {nameof(y)}: {y}, {nameof(width)}: {width}, {nameof(height)}: {height}";
        }
        
        
        public class IntRectConverter : JsonConverter<IntRect>
        {
            public override void WriteJson(JsonWriter writer, IntRect value, JsonSerializer serializer)
            {
                writer.WriteValue(IntRectParser.ToStringValue(value));
            }

            public override IntRect ReadJson(JsonReader reader, Type objectType, IntRect existingValue, bool hasExistingValue, JsonSerializer serializer)
            {
                if (IntRectParser.TryParse(reader.Value as string, out IntRect r))
                    return r;
                return default;
            }
        }
    }
}