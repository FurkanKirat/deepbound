using System.Collections.Generic;
using Utils;

namespace Data.Database
{
    public class OnDemandDatabase<T> : IMapDatabase<T>
    {
        private readonly Dictionary<string, T> _cache = new();

        public T this[string path]
        {
            get
            {
                if (TryGet(path, out var value))
                    return value;
                
                return default;
            }
        }

        public bool TryGet(string path, out T value)
        {
            if (string.IsNullOrEmpty(path))
            {
                value = default;
                return false;
            }
            
            if (_cache.TryGetValue(path, out value))
                return true;
            
            var loaded = ResourcesHelper.LoadJson<T>(path);
            if (loaded != null && _cache.TryAdd(path, loaded))
            {
                value = loaded;
                return true;
            }
            
            value = default;
            return false;
        }
    }
}