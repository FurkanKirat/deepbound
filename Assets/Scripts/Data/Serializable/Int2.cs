using System;
using System.Diagnostics;
using Interfaces;
using Newtonsoft.Json;
using Utils.Parsers;

namespace Data.Serializable
{
    using UnityEngine;

    [DebuggerDisplay("({x}, {y})")] [Serializable] [JsonConverter(typeof(Converter))]
    public struct Int2 : IEquatable<Int2>, IStringConvertible
    {
        #region Fields
        public int x, y;
        #endregion

        #region Constructors & Deconstructors
        public Int2(Vector2Int v)
        {
            x = v.x;
            y = v.y;
        }

        public Int2(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
        public void Deconstruct(out int X, out int Y)
        {
            X = this.x;
            Y = this.y;
        }
        
        #endregion

        #region Predefined Instances
        public static Int2 Zero => new (0, 0);
        public static Int2 One => new(1, 1);
        public static Int2 Up => new(0, 1);
        public static Int2 Down => new(0, -1);
        public static Int2 Left => new(-1, 0);
        public static Int2 Right => new(1, 0);
        #endregion
        
        #region Methods

        public int Multiply() => x * y;
        #endregion
        
        #region Operators
        // Implicit conversions
        public static implicit operator Vector2Int(Int2 i2) => new(i2.x, i2.y);
        public static implicit operator Int2(Vector2Int v) => new(v.x, v.y);

        // Arithmetic
        public static Int2 operator -(Int2 a) => new(-a.x, -a.y);
        public static Int2 operator -(Int2 a, Int2 b) => new(a.x - b.x, a.y - b.y);
        public static Int2 operator +(Int2 a, Int2 b) => new(a.x + b.x, a.y + b.y);
        
        public static Int2 operator *(Int2 a, int s) => new(a.x * s, a.y * s);
        public static Int2 operator *(int s, Int2 a) => new(a.x * s, a.y * s);
        public static Int2 operator /(Int2 a, int s) => new(a.x / s, a.y / s);
        #endregion
        
        #region IEquatable<TilePosition>
        public bool Equals(Int2 other) => x == other.x && y == other.y;

        public override bool Equals(object obj) =>
            obj is Int2 other && Equals(other);

        public override int GetHashCode() => HashCode.Combine(x, y);

        public static bool operator ==(Int2 a, Int2 b) => a.Equals(b);
        public static bool operator !=(Int2 a, Int2 b) => !a.Equals(b);
        #endregion
        
        #region Conversion
        public Vector2Int ToVector2Int() => new (x, y);
        #endregion
        
        #region Serialization
        
        public string ToStringValue() => IntVectorParser.ToStringValue(x,y);
        public override string ToString() => ToStringValue();
        #endregion
        
        #region Converter

        //Format x,y
        public class Converter : JsonConverter<Int2>
        {
            public override void WriteJson(JsonWriter writer, Int2 value, JsonSerializer serializer)
            {
                writer.WriteValue(value.ToStringValue());
            }

            public override Int2 ReadJson(JsonReader reader, Type objectType, Int2 existingValue, bool hasExistingValue, JsonSerializer serializer)
            {
                var str = (string)reader.Value;
                if (IntVectorParser.TryParse(str, out var tuple))
                    return new Int2(tuple.x, tuple.y);
                throw new JsonSerializationException("Invalid Int2");
            }

            public override bool CanRead => true;
            public override bool CanWrite => true;
        }

        #endregion
        
    }

}