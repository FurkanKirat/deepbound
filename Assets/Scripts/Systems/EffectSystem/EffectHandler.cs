using System;
using System.Collections.Generic;
using System.Linq;
using Core;
using Core.Context;
using Core.Context.Registry;
using Core.Events;
using Data.RegistrySystem;
using GameLoop;
using Systems.BuffSystem;
using Systems.EntitySystem.Interfaces;
using Systems.SaveSystem.Interfaces;
using Systems.SaveSystem.SaveData;

namespace Systems.EffectSystem
{
    public class EffectHandler : 
        ITickable, 
        IDisposable,
        ISaveable<EffectsSaveData>
    {
        private readonly Dictionary<IEffectProvider, List<IEffectBehavior>> _effectProviders = new();
        private readonly List<IEffectBehavior> _directEffects; // enemy poison, trap slow vb.
        private readonly IEntity _owner;

        public static EffectHandler Create(IEntity owner)
        {
            return new EffectHandler(owner, new List<IEffectBehavior>());
        }

        public static EffectHandler Load(IEntity owner, EffectsSaveData saveData)
        {
            var effectCtx = new EffectContext();
            var effects = new List<IEffectBehavior>();
            foreach (var saveEffect in saveData.Effects)
            {
                var effect = Registries.EffectBehaviorFactory.Load(saveEffect.EffectId, effectCtx, saveEffect);
                effects.Add(effect);
            }
            return new EffectHandler(owner, effects);
        }
        private EffectHandler(IEntity owner, List<IEffectBehavior> directEffects)
        {
            _owner = owner;
            _directEffects = directEffects;
            GameEventBus.Subscribe<EffectProvidersChangedEvent>(OnEffectProviderChange);
        }
        
        public void Dispose()
        {
            GameEventBus.Unsubscribe<EffectProvidersChangedEvent>(OnEffectProviderChange);
        }

        private void OnEffectProviderChange(EffectProvidersChangedEvent e)
        {
            if (_owner != e.Owner) return;
            SyncProviderEffects(e.Provider, e.Provider.GetActiveEffects());
        }

        private void SyncProviderEffects(IEffectProvider provider, IEnumerable<EffectData> newData)
        {
            if (!_effectProviders.TryGetValue(provider, out var current))
            {
                current = new List<IEffectBehavior>();
                _effectProviders[provider] = current;
            }

            var newDataArr = newData.ToArray();
            foreach (var data in newDataArr)
            {
                if (current.All(b => b.Id != data.Id))
                    current.Add(CreateEffectBehavior(data));
            }

            var toRemove = current.Where(b => newDataArr.All(d => d.Id != b.Id)).ToList();
            foreach (var effect in toRemove)
            {
                effect.OnRemove(_owner);
                current.Remove(effect);
            }
        }

        public void AddDirectEffect(EffectData data)
        {
            var behavior = CreateEffectBehavior(data);
            _directEffects.Add(behavior);
        }

        public void AddProvider(IEffectProvider provider)
        {
            SyncProviderEffects(provider, provider.GetActiveEffects());
        }

        public void AddProviders(IEnumerable<IEffectProvider> providers)
        {
            foreach (var provider in providers)
                AddProvider(provider);
        }

        private IEffectBehavior CreateEffectBehavior(EffectData data)
        {
            var ctx = new EffectContext { EffectData = data };
            var behavior = Registries.EffectBehaviorFactory.Create(data.Id, ctx);
            behavior.OnApply(_owner);
            return behavior;
        }

        public void Tick(float deltaTime, TickContext ctx)
        {
            var allEffects = _effectProviders.Values.SelectMany(x => x)
                .Concat(_directEffects)
                .ToList();

            foreach (var effect in allEffects.ToArray())
            {
                effect.Tick(_owner, deltaTime);
                if (effect.Duration.HasValue)
                {
                    effect.Duration -= deltaTime;
                    if (effect.Duration <= 0)
                    {
                        effect.OnRemove(_owner);
                        _directEffects.Remove(effect);
                        foreach (var providerList in _effectProviders.Values)
                            providerList.Remove(effect);
                    }
                }
            }
        }

        public EffectsSaveData ToSaveData()
        {
            var effects = new List<EffectSaveData>();
            foreach (var directEffect in _directEffects)
            {
                effects.Add(directEffect.ToSaveData());
            }

            return new EffectsSaveData
            {
                Effects = effects,
            };
        }
    }

}