using System;
using System.Collections.Generic;

namespace Data.RegistrySystem
{
    public class FactoryRegistry<TInput, TOutput> : ICreateFactory<TInput, TOutput>
        where TInput : notnull
    {
        private readonly Dictionary<string, Func<TInput, TOutput>> _registry = new();

        public void Register(string id, Func<TInput, TOutput> factory)
        {
            _registry[id] = factory;
        }
        
        public void RegisterMany(params (string key, Func<TInput, TOutput> factory)[] entries)
        {
            foreach (var (key, value) in entries)
            {
                Register(key, value);
            }
        }

        public TOutput Create(string id, TInput input)
        {
            if (_registry.TryGetValue(id, out var func))
                return func(input);

            throw new KeyNotFoundException($"Factory for '{id}' not found.");
        }

        public bool TryCreate(string id, TInput input, out TOutput result)
        {
            if (_registry.TryGetValue(id, out var func))
            {
                result = func(input);
                return true;
            }

            result = default!;
            return false;
        }
        
        public bool HasFactory(string id)
        {
            return _registry.ContainsKey(id);
        }

        public IEnumerable<string> Keys => _registry.Keys;
    }

}