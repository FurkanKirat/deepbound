using System.Collections.Generic;

namespace Utils
{
    public static class DebugUtils
    {
        public static void LogIsNull(object obj, string tag = "Default", bool printObject = true)
        {
            string message;
            if (obj == null)
            {
                message = "object is null";
            }
            else if (printObject)
            {
                message = $"object is not null, object\n {obj}";
            }
            else
            {
                message = "object is not null";
            }
            GameLogger.Log(message, tag);
        }
        

        public static void LogIList<T>(IList<T> list, string tag = "Default")
        {
            GameLogger.Log($"Count : {list.Count}", tag);
            for (int i = 0; i < list.Count; i++)
            {
                GameLogger.Log(list[i].ToString(), $"{tag} Element {i}");
            }
                
        }

        public static void LogObject(object obj, string tag = "Default")
        {
            GameLogger.Log(obj.ToString(), tag);
        }
    }
}