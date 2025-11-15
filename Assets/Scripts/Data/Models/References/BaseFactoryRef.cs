using Data.RegistrySystem;
using Interfaces;

namespace Data.Models.References
{
    public abstract class BaseFactoryRef<TContext, TResult> : IFactoryRef<TContext, TResult>, IStringConvertible
    {
        public string Key { get; }

        private readonly ICreateFactory<TContext, TResult> _registry;

        protected BaseFactoryRef(string key, ICreateFactory<TContext, TResult> registry)
        {
            Key = key;
            _registry = registry;
        }

        public TResult Create(TContext context)
        {
            return _registry.Create(Key, context);
        }

        public string ToStringValue() => Key;

        public override string ToString() => ToStringValue();
    }

}