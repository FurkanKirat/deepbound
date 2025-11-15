using Systems.BuffSystem;
using Systems.EntitySystem.Interfaces;

namespace Core.Events
{
    public struct EffectProvidersChangedEvent : IEvent
    {
        public IEntity Owner { get; }
        public IEffectProvider Provider { get; }
        public EffectProvidersChangedEvent(IEntity owner, IEffectProvider provider)
        {
            Owner = owner;
            Provider = provider;
        }
    }
}