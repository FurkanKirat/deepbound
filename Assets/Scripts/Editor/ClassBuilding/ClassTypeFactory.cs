using System;
using System.Collections.Generic;
using Data.Models;
using Utils.Parsers;

namespace Editor.ClassBuilding
{
    public static class ClassTypeFactory
    {
        private static readonly Dictionary<KnownClassType, ITypeDescriptor> Types = new()
        {
            { KnownClassType.String, new StringTypeDescriptor() },
            { KnownClassType.Int, new IntegerTypeDescriptor() },
            { KnownClassType.Float, new FloatTypeDescriptor() },
            { KnownClassType.Bool, new BoolTypeDescriptor() },
            { KnownClassType.WorldPosition, new WorldPositionTypeDescriptor()},
            { KnownClassType.TilePosition, new TilePositionTypeDescriptor()}
        };

        public static ITypeDescriptor GetDescriptor(KnownClassType type)
        {
            if (Types.TryGetValue(type, out var classType))
                return classType;
            throw new ArgumentException($"Unsupported class type: {type}");
        }
    }
    public interface ITypeDescriptor
    {
        public string TypeName { get; }
        public KnownClassType Type { get; }
        bool CanBeConst { get; }
        public string FormatValueLiteral(string variableValue);
        public string[] RequiredUsings => Array.Empty<string>();
    }
    
    public enum KnownClassType : ushort
    {
        String,
        Int,
        Float,
        Bool,
        WorldPosition,
        TilePosition
    }

    public class StringTypeDescriptor : ITypeDescriptor
    {
        public string TypeName => "string";
        public KnownClassType Type => KnownClassType.String;
        public bool CanBeConst => true;
        public string FormatValueLiteral(string value) => $"\"{value}\"";
    }

    public class IntegerTypeDescriptor : ITypeDescriptor
    {
        public string TypeName => "int";
        public KnownClassType Type => KnownClassType.Int;
        public bool CanBeConst => true;
        public string FormatValueLiteral(string value) => value;
    }
    
    public class FloatTypeDescriptor : ITypeDescriptor
    {
        public string TypeName => "float";
        public KnownClassType Type => KnownClassType.Float;
        public bool CanBeConst => true;
        public string FormatValueLiteral(string value) => $"{value}f";
    }
    
    public class BoolTypeDescriptor : ITypeDescriptor
    {
        public string TypeName => "bool";
        public KnownClassType Type => KnownClassType.Bool;
        public bool CanBeConst => true;
        public string FormatValueLiteral(string value) => value.ToLowerInvariant();
    }

    public class WorldPositionTypeDescriptor : ITypeDescriptor
    {
        public string TypeName => "WorldPosition";
        public KnownClassType Type => KnownClassType.WorldPosition;
        public bool CanBeConst => false;
        public string FormatValueLiteral(string value)
        {
            if (FloatVectorParser.TryParse(value, out var worldPosition))
                return $"new WorldPosition({worldPosition.x}f, {worldPosition.y}f)";
            throw new ArgumentException($"Invalid WorldPosition string: {value}");
        }

        public string[] RequiredUsings => new[] { typeof(WorldPosition).Namespace };
    }
    
    public class TilePositionTypeDescriptor : ITypeDescriptor
    {
        public string TypeName => "TilePosition";
        public KnownClassType Type => KnownClassType.TilePosition;
        public bool CanBeConst => false;
        public string FormatValueLiteral(string value)
        {
            if (IntVectorParser.TryParse(value, out var worldPosition))
                return $"new TilePosition({worldPosition.x}, {worldPosition.y})";
            throw new ArgumentException($"Invalid TilePosition string: {value}");
        }

        public string[] RequiredUsings => new[] { typeof(TilePosition).Namespace };
    }
    
    
}