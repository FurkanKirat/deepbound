using System.Globalization;
using Utils.Extensions;

namespace Utils.Parsers
{
    public static class IntVectorParser
    {
        public static bool TryParse(string s, out (int x, int y) result)
        {
            var parts = s?.Split(',');
            if (parts?.Length != 2)
            {
                result = default;
                return false;
            }

            if (int.TryParse(parts[0], NumberStyles.Integer, CultureInfo.InvariantCulture, out int x) &&
                int.TryParse(parts[1], NumberStyles.Integer, CultureInfo.InvariantCulture, out int y))
            {
                result = (x, y);
                return true;
            }

            result = default;
            return false;
        }

        public static string ToStringValue(int x, int y) =>
            $"{x.ToInvariantString()},{y.ToInvariantString()}";
    }

}