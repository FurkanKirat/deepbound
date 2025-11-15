using System;
using Constants;
using Interfaces;
using Newtonsoft.Json;
using UnityEngine;
using Utils.Parsers;

namespace Data.Models
{
    [Serializable]
    [JsonConverter(typeof(WorldPositionConverter))]
    public struct WorldPosition : IEquatable<WorldPosition>, IStringConvertible
    {
        #region Constants
        private const float MinMagnitude = 0.001f;
        #endregion
        
        #region Fields
        public float x;
        public float y;
        #endregion

        #region Constructors
        public WorldPosition(float x, float y)
        {
            this.x = x;
            this.y = y;
        }
        #endregion

        #region Predefined Instances
        public static WorldPosition Zero => new (0, 0);
        #endregion

        #region Properties
        public float Length => Mathf.Sqrt(SqrMagnitude);
        public float SqrMagnitude => x * x + y * y;
        public WorldPosition Normalized
        {
            get
            {
                if (SqrMagnitude < MinMagnitude * MinMagnitude)
                    return Zero;
                return this / Length;
            }
        }

        public WorldPosition XNegated => new(-x, y);
        public WorldPosition YNegated => new(x, -y);
        public WorldPosition Negated => new (-x, -y);
        #endregion
        
        #region Conversion
        public Vector2 ToVector2() => new(x, y);
        public Vector3 ToVector3(float z = 0) => new(x, y, z);
        public static WorldPosition FromVector2(Vector2 vec) => new(vec.x, vec.y);
        public static WorldPosition FromVector3(Vector3 vec) => new(vec.x, vec.y);
        public TilePosition ToTilePosition()
        {
            return new TilePosition(
                Mathf.FloorToInt(x / TileConstants.TileSize.x),
                Mathf.FloorToInt(y / TileConstants.TileSize.y)
            );
        }
        #endregion

        #region Math Helpers
        public static WorldPosition Min(WorldPosition a, WorldPosition b)
        {
            return new WorldPosition(Mathf.Min(a.x, b.x), Mathf.Min(a.y, b.y));
        }
        public static WorldPosition Max(WorldPosition a, WorldPosition b)
        {
            return new WorldPosition(Mathf.Max(a.x, b.x), Mathf.Max(a.y, b.y));
        }
        public static WorldPosition Clamp(WorldPosition value, WorldPosition min, WorldPosition max)
        {
            float clampedX = Mathf.Clamp(value.x, min.x, max.x);
            float clampedY = Mathf.Clamp(value.y, min.y, max.y);
            return new WorldPosition(clampedX, clampedY);
        }
        #endregion
        
        #region Distance
        public static float SquaredDistance(WorldPosition a, WorldPosition b)
        {
            return (a.x - b.x) * (a.x - b.x) + (a.y - b.y) * (a.y - b.y); 
        }
        public static float Distance(WorldPosition a, WorldPosition b)
        {
            return Mathf.Sqrt(SquaredDistance(a, b));
        }
        #endregion
        
        #region Operators
        // Unary
        public static WorldPosition operator -(WorldPosition a) => new (-a.x, -a.y);
        
        // Binary
        public static WorldPosition operator +(WorldPosition a, WorldPosition b) => new (a.x + b.x, a.y + b.y);
        public static WorldPosition operator -(WorldPosition a, WorldPosition b) => new (a.x - b.x, a.y - b.y);
        
        // Float - World Position
        public static WorldPosition operator /(WorldPosition a, float b) => new (a.x / b, a.y / b);
        public static WorldPosition operator *(WorldPosition a, float b) => new (a.x * b, a.y * b);
        public static WorldPosition operator *(float a, WorldPosition b) => new (b.x * a, b.y * a);

        // WorldPosition - TilePosition
        public static WorldPosition operator +(WorldPosition a, TilePosition b) => new (a.x + b.X, a.y + b.Y);
        public static WorldPosition operator +(TilePosition a, WorldPosition b) => new (a.X + b.x, a.Y + b.y);
        
        // Vector2 - WorldPosition
        public static WorldPosition operator +(WorldPosition wp, Vector2 b) => new (wp.x + b.x, wp.y + b.y);
        public static WorldPosition operator +(Vector2 b, WorldPosition wp) => new (wp.x + b.x, wp.y + b.y);

        
        #endregion
        
        #region IEquatable<WorldPosition>
        public bool Equals(WorldPosition other)
        {
            return Mathf.Approximately(x, other.x) && Mathf.Approximately(y, other.y);
        }

        public override bool Equals(object obj)
        {
            return obj is WorldPosition other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(x, y);
        }
        #endregion
        
        #region Overrides

        public override string ToString() => ToStringValue();
        #endregion

        #region Serialization

        public string ToStringValue() => FloatVectorParser.ToStringValue(x,y);

        #endregion

        #region Converter

        // Format: x,y
        private sealed class WorldPositionConverter : JsonConverter<WorldPosition>
        {
            public override void WriteJson(JsonWriter writer, WorldPosition value, JsonSerializer serializer)
            {
                writer.WriteValue(value.ToStringValue());
            }

            public override WorldPosition ReadJson(JsonReader reader, Type objectType, WorldPosition existingValue, bool hasExistingValue, JsonSerializer serializer)
            {
                var str = (string)reader.Value;
                if (FloatVectorParser.TryParse(str, out var tuple))
                    return new WorldPosition(tuple.x, tuple.y);
                throw new JsonSerializationException("Invalid WorldPosition");
            }

            public override bool CanRead => true;
            public override bool CanWrite => true;
        }

        #endregion
    }

}