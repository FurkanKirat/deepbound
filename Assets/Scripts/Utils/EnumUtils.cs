using System;
using System.Collections.Generic;
using System.Linq;

namespace Utils
{
    public static class EnumUtils<T> where T : Enum
    {
        private static readonly T[] Values = Enum.GetValues(typeof(T)).Cast<T>().ToArray();
        public static int Count => Values.Length;
        public static T First => Values.First();
        public static T Last => Values.Last();
        public static IReadOnlyList<T> AllValues => Values;
    }
    public static class EnumUtils
    {
        public static object ToInt<TEnum>(this TEnum e) where TEnum : Enum
        {
            Type underlyingType = Enum.GetUnderlyingType(typeof(TEnum));
            return Convert.ChangeType(e, underlyingType);
        }

        public static int ToIntSafe<TEnum>(this TEnum e) where TEnum : Enum
        {
            Type underlyingType = Enum.GetUnderlyingType(typeof(TEnum));
            object value = e;

            return underlyingType switch
            {
                { } t when t == typeof(byte) => (byte)value,
                { } t when t == typeof(sbyte) => (sbyte)value,
                { } t when t == typeof(short) => (short)value,
                { } t when t == typeof(ushort) => (ushort)value,
                { } t when t == typeof(int) => (int)value,
                { } t when t == typeof(uint) => checked((int)(uint)value),
                { } t when t == typeof(long) => checked((int)(long)value),
                { } t when t == typeof(ulong) => checked((int)(ulong)value),
                _ => throw new NotSupportedException($"Unsupported enum underlying type: {underlyingType.Name}")
            };
        }

        public static TResult ToUnderlyingValue<TEnum, TResult>(this TEnum e)
            where TEnum : Enum
        {
            return (TResult)Convert.ChangeType(e, typeof(TResult));
        }

        public static int GetEnumCount<T>() where T : Enum
        {
            return EnumUtils<T>.Count;
        }

        public static string ToJsonString(this Enum e)
        {
            var stringValue = e.ToString();
            return CaseUtils.ToSnakeCase(stringValue);
        }

        public static T GetFirst<T>() where T : Enum
        {
            return EnumUtils<T>.First;
        }

        public static T GetLast<T>() where T : Enum
        {
            return EnumUtils<T>.Last;
        }
    }
}