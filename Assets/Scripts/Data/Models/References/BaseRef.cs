using Interfaces;

namespace Data.Models.References
{
    public abstract class BaseRef<T> : IRef<T>
    {
        public string Key { get; }
        private T _cached;

        private readonly IReferenceSource<T> _registry;

        protected BaseRef(string key, IReferenceSource<T> registry)
        {
            Key = key;
            _registry = registry;
            _cached = default;
        }

        public T Load()
        {
            return _cached ??= _registry[Key];
        }

        public bool TryLoad(out T value)
        {
            if (_cached == null) 
                return _registry.TryGet(Key, out value);
            
            value = _cached;
            return true;
        }

        public override string ToString()
        {
            var loaded = Load();
            return $"Key: {Key}, Cached: {loaded}";
        }
    }

}