using System.Globalization;
using Data.Serializable;
using Utils.Extensions;

namespace Utils.Parsers
{
    public static class IntRectParser
    {
        public static bool TryParse(string value, out IntRect rect)
        {
            var parts = value.Split('-');
            string[] coordinates = parts[0].Split(",");
            string[] size = parts[1].Split("x");
                
            if (int.TryParse(coordinates[0], NumberStyles.Integer, CultureInfo.InvariantCulture,out var x) &&
                int.TryParse(coordinates[1], NumberStyles.Integer, CultureInfo.InvariantCulture, out var y) &&
                int.TryParse(size[0], NumberStyles.Integer, CultureInfo.InvariantCulture, out var width) &&
                int.TryParse(size[1], NumberStyles.Integer, CultureInfo.InvariantCulture,out var height)
                )
            {
                rect = new IntRect(x, y, width, height);
                return true;
            }
            rect = default;
            return false;
        }
        
        public static string ToStringValue(IntRect rect)
        {
            string x = rect.x.ToInvariantString();
            string y = rect.y.ToInvariantString();
            string w = rect.width.ToInvariantString();
            string h = rect.height.ToInvariantString();
            return $"{x},{y}-{w}x{h}";
        }
            
    }
}