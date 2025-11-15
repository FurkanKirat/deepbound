using System;
using System.Collections.Generic;
using System.Text;

namespace Utils.Extensions
{
    public static class DictionaryExtensions
    {
        public static string ToDebugString<TK, TV>(this Dictionary<TK, TV> dict)
        {
            if (dict == null || dict.Count == 0)
                return "{}";

            var sb = new StringBuilder();
            sb.AppendLine("{ ");
            foreach (var kvp in dict)
            {
                sb.AppendLine($"[{kvp.Key}: {kvp.Value}], ");
            }
            sb.Length -= 2; // remove last comma
            sb.AppendLine(" }");
            return sb.ToString();
        }
        
        public static string ToDebugString(this Dictionary<string, object> dict)
        {
            if (dict == null || dict.Count == 0)
                return "{}";

            var sb = new StringBuilder();
            sb.Append("{ ");
            foreach (var kvp in dict)
            {
                var valueStr = kvp.Value == null ? "null" : $"{kvp.Value} ({kvp.Value.GetType().Name})";
                sb.Append($"[{kvp.Key}: {valueStr}], ");
            }
            sb.Length -= 2; // remove last comma
            sb.Append(" }");
            return sb.ToString();
        }


        public static void EnsureListAndAdd<TK, TV>(this Dictionary<TK, List<TV>> dict, TK key, TV value)
        {
            if (dict.TryGetValue(key, out var list))
            {
                list.Add(value);
            }
            else
            {
                var newList = new List<TV> { value };
                dict.Add(key, newList);
            }
        }
        
        public static void EnsureArrayAndAdd<TK, TV>(this Dictionary<TK, TV[]> dict, TK key, TV value)
        {
            if (dict.TryGetValue(key, out var arr))
            {
                var newArr = new TV[arr.Length + 1];
                arr.CopyTo(newArr, 0);
                newArr[arr.Length] = value;
                dict[key] = newArr;
            }
            else
            {
                var newList = new [] { value };
                dict.Add(key, newList);
            }
        }
        
        public static T Get<T>(this Dictionary<string, object> dict, string key, T defaultValue = default)
        {
            if (!dict.TryGetValue(key, out var val) || val == null)
                return defaultValue;

            var targetType = typeof(T);

            if (val is T t) return t;

            if (targetType == typeof(float))
            {
                switch (val)
                {
                    case int i:
                        return (T)(object)(float)i;
                    case long l:
                        return (T)(object)(float)l;
                    case double d:
                        return (T)(object)(float)d;
                }

                if (float.TryParse(val.ToString(), out var f)) return (T)(object)f;
            }

            // int branch
            if (targetType == typeof(int))
            {
                switch (val)
                {
                    case long l:
                        return (T)(object)(int)l;
                    case double d:
                        return (T)(object)(int)d;
                    case float f:
                        return (T)(object)(int)f;
                }

                if (int.TryParse(val.ToString(), out var i)) return (T)(object)i;
            }

            // double branch
            if (targetType == typeof(double))
            {
                switch (val)
                {
                    case int i:
                        return (T)(object)(double)i;
                    case long l:
                        return (T)(object)(double)l;
                    case float f:
                        return (T)(object)(double)f;
                }

                if (double.TryParse(val.ToString(), out var d2)) return (T)(object)d2;
            }

            // string branch
            if (targetType == typeof(string))
                return (T)(object)val.ToString();

            if (val is IConvertible)
            {
                try { return (T)Convert.ChangeType(val, targetType); }
                catch { return defaultValue; }
            }

            return defaultValue;
        }

        

        
    }
}