namespace Core.Tags
{
    using System.Collections.Generic;

    public class TagStore
    {
        private readonly Dictionary<object, object> _data = new();

        public void Set<T>(TagKey<T> key, T value) => _data[key] = value;

        public bool TryGet<T>(TagKey<T> key, out T value)
        {
            if (_data.TryGetValue(key, out var obj) && obj is T t)
            {
                value = t;
                return true;
            }
            value = default;
            return false;
        }

        public T GetOrDefault<T>(TagKey<T> key, T defaultValue = default)
        {
            return TryGet(key, out T value) ? value : defaultValue;
        }

        public bool Contains<T>(TagKey<T> key) => _data.ContainsKey(key);
    }

}