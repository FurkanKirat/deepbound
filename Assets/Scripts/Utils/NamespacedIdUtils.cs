namespace Utils
{
    public static class NamespacedIdUtils
    {
        public static string StripNamespace(string str)
        {
            if (string.IsNullOrEmpty(str))
                return str;
            
            var parts = str.Split(':');
            if (parts.Length == 0)
                return str;
            return parts.Last();
        }
    }
}