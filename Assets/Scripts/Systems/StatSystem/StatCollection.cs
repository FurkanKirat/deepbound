using System;
using System.Collections.Generic;
using Core;
using Core.Events;
using Systems.EntitySystem.Interfaces;
using Utils;
using Utils.Extensions;

namespace Systems.StatSystem
{
    public class StatCollection : IDisposable
    {
        private readonly Dictionary<StatType, float> _baseStats = new();
        private readonly List<IStatProvider> _providers = new();
        private readonly Dictionary<StatType, float> _cachedStats = new();

        private readonly IPhysicalEntity _owner;

        public StatCollection(IPhysicalEntity owner)
        {
            _owner = owner;
            GameEventBus.Subscribe<StatProvidersChangedEvent>(OnStatProvidersChanged);
        }

        public void Dispose()
        {
            GameEventBus.Unsubscribe<StatProvidersChangedEvent>(OnStatProvidersChanged);
        }

        public void AddProvider(IStatProvider provider) 
        {
            _providers.Add(provider);
            Recalculate();
        }

        public void AddMultipleProviders(IEnumerable<IStatProvider> providers)
        {
            _providers.AddRange(providers);
            Recalculate();
        }

        public void RemoveProvider(IStatProvider provider) 
        {
            _providers.Remove(provider);
            Recalculate();
        }

        public void SetBaseStat(StatType type, float value) 
        {
            _baseStats[type] = value;
            Recalculate();
        }

        public void SetBaseStats(Dictionary<StatType, float> baseStats)
        {
            foreach (var stat in baseStats)
            {
                _baseStats[stat.Key] = stat.Value;
            }
            Recalculate();
        }

        public float GetStat(StatType type) 
        {
            return _cachedStats.GetValueOrDefault(type, 0);
        }
        
        public float GetStatOrDefault(StatType type, float defaultValue)
            => _cachedStats.GetValueOrDefault(type, defaultValue);

        private void OnStatProvidersChanged(StatProvidersChangedEvent e)
        {
            if (_owner != e.Owner)
                return;
            Recalculate();
        }
        
        private void Recalculate()
        {
            _cachedStats.Clear();

            foreach (var kv in _baseStats)
                _cachedStats[kv.Key] = kv.Value;

            var multipliers = new Dictionary<StatType, float>();
            var overrides   = new Dictionary<StatType, float>();

            foreach (var provider in _providers)
            {
                foreach (var mod in provider.GetStatModifiers())
                {
                    switch (mod.Operation)
                    {
                        case StatOperation.Add:
                            if (_cachedStats.ContainsKey(mod.Type))
                                _cachedStats[mod.Type] += mod.Value;
                            else
                                _cachedStats[mod.Type] = mod.Value;
                            break;

                        case StatOperation.Multiply:
                            if (multipliers.ContainsKey(mod.Type))
                                multipliers[mod.Type] *= mod.Value;
                            else
                                multipliers[mod.Type] = mod.Value;
                            break;

                        case StatOperation.Override:
                            overrides[mod.Type] = mod.Value;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }
            
            foreach (var kv in multipliers)
            {
                if (_cachedStats.ContainsKey(kv.Key))
                    _cachedStats[kv.Key] *= kv.Value;
            }

            foreach (var kv in overrides)
            {
                _cachedStats[kv.Key] = kv.Value;
            }
            DebugUtils.LogObject(_cachedStats.ToDebugString(), nameof(StatCollection));
        }

    }

}
