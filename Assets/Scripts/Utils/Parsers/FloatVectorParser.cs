using System.Globalization;
using Utils.Extensions;

namespace Utils.Parsers
{
    public static class FloatVectorParser
    {
        public static bool TryParse(string input, out (float x, float y) result)
        {
            var parts = input.Split(',');
            if (parts.Length == 2 &&
                float.TryParse(parts[0], NumberStyles.Float, CultureInfo.InvariantCulture, out float x) &&
                float.TryParse(parts[1], NumberStyles.Float, CultureInfo.InvariantCulture, out float y))
            {
                result = (x, y);
                return true;
            }
            result = default;
            return false;
        }
        public static string ToStringValue(float x, float y) => $"{x.ToInvariantString()},{y.ToInvariantString()}";

    }
    
}