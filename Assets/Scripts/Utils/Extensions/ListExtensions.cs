using System.Collections.Generic;
using System.Text;

namespace Utils.Extensions
{
    public static class ListExtensions
    {
        public static string ToDebugString<T>(this IList<T> list)
        {
            if (list == null || list.Count == 0)
                return "{}";

            var sb = new StringBuilder();
            sb.Append("{ ");
            foreach (var item in list)
            {
                sb.Append($"[{item}], ");
            }
            sb.Length -= 2; // remove last comma
            sb.Append(" }");
            return sb.ToString();
        }
    }
}