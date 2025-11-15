using System;
using System.Collections.Generic;

namespace Utils
{
    public static class ArrayUtils
    {
        public static bool IsInsideBounds<T>(this IList<T> array, int index)
        {
            return index >= 0 && index < array.Count;
        }

        public static bool IsInsideBounds<T>(this IList<T> array, params int[] indexes)
        {
            for (int i = 0; i < indexes.Length; i++)
            {
                if (!IsInsideBounds(array, indexes[i]))
                    return false;
            }
            return true;
        }

        public static bool TryGetItem<T>(this IList<T> array, int index, out T item)
        {
            bool success = IsInsideBounds(array, index);
            
            item = success ? array[index] : default;

            return success;
        }

        public static bool ContainsItem<T>(this T[] array, T item)
        {
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i].Equals(item))
                    return true;
            }

            return false;
        }

        public static void Clear<T>(this T[] array)
        {
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = default;
            }
        }
        
        public static bool IsNullOrEmpty<T>(this IList<T> array)
            => array == null || array.Count == 0;

        public static T Last<T>(this IList<T> array)
        {
            var index = array.Count - 1;
            return array[index];
        }
        
        public static T First<T>(this IList<T> array)
        {
            return array[0];
        }

        public static int IndexOf<T>(this IList<T> array, T item)
        {
            for (int i = 0; i < array.Count; i++)
            {
                if (array[i].Equals(item))
                    return i;
            }
            
            return -1;
        }
    }
}