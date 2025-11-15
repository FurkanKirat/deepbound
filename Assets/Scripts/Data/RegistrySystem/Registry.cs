using System.Collections.Generic;
using Interfaces;

namespace Data.RegistrySystem
{
    public class Registry<T> : IReferenceSource<T>
    {
        private readonly Dictionary<string, T> _entries = new();

        public bool Register(string id, T entry) => _entries.TryAdd(id, entry);
        public void RegisterMany(params (string key, T value)[] entries)
        {
            foreach (var (key, value) in entries)
            {
                Register(key, value);
            }
        }

        public T this[string id] => _entries.GetValueOrDefault(id);
        public bool TryGet(string key, out T value)
        {
            if (key == null)
            {
                value = default;
                return false;
            }
            return _entries.TryGetValue(key, out value);
        }

        public bool Exists(string id) => _entries.ContainsKey(id);

        public void Clear() => _entries.Clear();
    }

}