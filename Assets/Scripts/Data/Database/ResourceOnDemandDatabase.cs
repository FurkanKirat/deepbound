using System.Collections.Generic;
using Utils;
using Object = UnityEngine.Object;

namespace Data.Database
{
    public class ResourceOnDemandDatabase<T> :
        IMapDatabase<T> where T : Object
    {
        private readonly Dictionary<string, T> _cache = new();
        
        public T this[string path]
        {
            get
            {
                if (TryGet(path, out var value))
                    return value;
                
                return null;
            }
        }

        public bool TryGet(string path, out T value)
        {
            if (string.IsNullOrEmpty(path))
            {
                value = null;
                return false;
            }
            
            if (_cache.TryGetValue(path, out value))
                return true;
            
            var loaded = ResourcesHelper.LoadAsset<T>(path);
            if (loaded != null && _cache.TryAdd(path, loaded))
            {
                value = loaded;
                return true;
            }

            return false;
        }
    }
}