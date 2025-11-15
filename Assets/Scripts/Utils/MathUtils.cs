using System;
using UnityEngine;

namespace Utils
{
    public static class MathUtils
    {
        public static bool ApproximatelyZero(float a) => Mathf.Approximately(a, 0f);
        public static int Min(int a, int b) => a < b ? a : b;
        public static int Max(int a, int b) => a > b ? a : b;

        public static int Progress(float total, float current, int count)
        {
            if (total <= 0 || count <= 0) 
                return 0;

            if (current < 0) current = 0;
            if (current > total) current = total;
            
            var ratio = current / total;
            for (int i = count; i > 0; i--)
            {
                if (ratio >= (float)i / count)
                    return i;
            }

            return 0;
        }

        public static T ProgressToEnum<T>(float total, float current)
            where T : Enum
        {
            int count = EnumUtils.GetEnumCount<T>() - 1;
            int value = Progress(total, current, count);
            return (T)Enum.ToObject(typeof(T), value);
        }
    }
}