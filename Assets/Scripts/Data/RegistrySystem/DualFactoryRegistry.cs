using System;
using System.Collections.Generic;

namespace Data.RegistrySystem
{
    public class DualFactoryRegistry<TContext, TLoadInput, TOutput> : 
        ICreateFactory<TContext, TOutput>
        where TContext : notnull
    {
        private readonly Dictionary<string, Func<TContext, TOutput>> _create = new();
        private readonly Dictionary<string, Func<TContext, TLoadInput, TOutput>> _load = new();

        public void Register(string id, Func<TContext, TOutput> factory, Func<TContext, TLoadInput, TOutput> load)
        {
            _create[id] = factory;
            _load[id] = load;
        }
        
        public void RegisterMany(params (string key, Func<TContext, TOutput> createFact, Func<TContext, TLoadInput, TOutput> loadFact)[] entries)
        {
            foreach (var (key, create, load) in entries)
            {
                Register(key, create, load);
            }
        }

        public TOutput Create(string id, TContext input)
        {
            if (_create.TryGetValue(id, out var func))
                return func(input);

            throw new KeyNotFoundException($"Factory for '{id}' not found.");
        }
        
        public TOutput Load(string id, TContext ctx, TLoadInput input)
        {
            if (_load.TryGetValue(id, out var func))
                return func(ctx, input);

            throw new KeyNotFoundException($"Factory for '{id}' not found.");
        }

        public bool HasFactory(string id)
        {
            return _create.ContainsKey(id);
        }
    }
}