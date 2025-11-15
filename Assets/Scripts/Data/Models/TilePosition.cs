using System;
using Constants;
using Data.Serializable;
using Interfaces;
using Newtonsoft.Json;
using UnityEngine;
using Utils.Parsers;

namespace Data.Models
{
    [Serializable]
    [JsonConverter(typeof(TilePositionConverter))]
    public struct TilePosition : IEquatable<TilePosition>, IStringConvertible
    {
        #region Fields
        private readonly Int2 _value;
        #endregion

        #region Properties
        public int X => _value.x;
        public int Y => _value.y;
        public Int2 ToInt2() => _value;
        #endregion

        #region Constructors
        public TilePosition(int x, int y) => _value = new Int2(x, y);
        public TilePosition(Int2 value) => _value = value;
        #endregion

        #region Predefined Instances
        public static TilePosition Zero => new(0, 0);
        #endregion

        #region Conversion
        public Vector2Int ToVector2Int() => new(X, Y);
        public Vector2 ToVector2() => new(X + 0.5f, Y + 0.5f);
        public Vector3 ToVector3(float z = 0f) => new(X + 0.5f, Y + 0.5f, z);
        public WorldPosition ToWorldPosition() =>
            new((X + 0.5f) * TileConstants.TileLength, (Y + 0.5f) * TileConstants.TileLength);
        #endregion

        #region Directional Helpers
        public TilePosition Up(int count = 1) => new(X, Y + count);
        public TilePosition Down(int count = 1) => new(X, Y - count);
        public TilePosition Left(int count = 1) => new(X - count, Y);
        public TilePosition Right(int count = 1) => new(X + count, Y);
        #endregion

        #region Operators
        public static implicit operator Vector2Int(TilePosition tilePos) => new(tilePos.X, tilePos.Y);
        public static implicit operator TilePosition(Vector2Int v) => new(v.x, v.y);

        public static TilePosition operator -(TilePosition a) => new(-a.X, -a.Y);
        public static TilePosition operator -(TilePosition a, TilePosition b) => new(a.X - b.X, a.Y - b.Y);
        public static TilePosition operator +(TilePosition a, TilePosition b) => new(a.X + b.X, a.Y + b.Y);
        #endregion

        #region IEquatable<TilePosition>
        public bool Equals(TilePosition other) => _value.Equals(other._value);

        public override bool Equals(object obj) =>
            obj is TilePosition other && Equals(other);

        public override int GetHashCode() => _value.GetHashCode();

        public static bool operator ==(TilePosition a, TilePosition b) => a.Equals(b);
        public static bool operator !=(TilePosition a, TilePosition b) => !a.Equals(b);
        #endregion

        #region Overrides
        public override string ToString() => $"TilePosition({X}, {Y})";
        #endregion

        #region Serialization
        public string ToStringValue() => IntVectorParser.ToStringValue(X, Y);
        #endregion

        #region Converter
        public class TilePositionConverter : JsonConverter<TilePosition>
        {
            public override void WriteJson(JsonWriter writer, TilePosition value, JsonSerializer serializer)
            {
                writer.WriteValue(value.ToStringValue());
            }

            public override TilePosition ReadJson(JsonReader reader, Type objectType, TilePosition existingValue, bool hasExistingValue, JsonSerializer serializer)
            {
                var str = (string)reader.Value;
                if (IntVectorParser.TryParse(str, out var tuple))
                    return new TilePosition(tuple.x, tuple.y);
                throw new JsonSerializationException("Invalid TilePosition");
            }

            public override bool CanRead => true;
            public override bool CanWrite => true;
        }
        #endregion
    }
}
