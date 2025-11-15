using System.Collections.Generic;
using System.Linq;
using Core;
using Core.Context;
using Core.Events;
using Data.Models.Items;
using GameLoop;
using Systems.EntitySystem.Interfaces;
using Systems.SaveSystem.Interfaces;
using Systems.SaveSystem.SaveData;
using Systems.StatSystem;
using Utils;

namespace Systems.BuffSystem
{
    public class PotionHandler : 
        ITickable, 
        IStatProvider, 
        IEffectProvider,
        ISaveable<PotionsSaveData>

    {
        private readonly Dictionary<ItemInstance, float> _activePotions = new(); // potion -> remaining time
        private readonly IPhysicalEntity _owner;

        public PotionHandler(IPhysicalEntity owner)
        {
            _owner = owner;
        }

        public PotionHandler(IPhysicalEntity owner, PotionsSaveData saveData)
        {
            _owner = owner;
            var savePotions = saveData.Potions;

            foreach (var kv in savePotions)
            {
                _activePotions.Add(ItemInstance.Load(kv.Key), kv.Value);
            }
            
        }
        
        public PotionsSaveData ToSaveData()
        {
            var saveDict = new Dictionary<ItemSaveData, float>();
            foreach (var (item, remainingTime) in _activePotions)
            {
                saveDict.Add(item.ToSaveData(), remainingTime);
            }

            return new PotionsSaveData
            {
                Potions = saveDict
            };
        }

        public void RegisterPotion(ItemInstance potion)
        {
            var potionData = potion.ItemData.PotionData;

            if (potionData.Duration.HasValue)
            {
                _activePotions[potion] = potionData.Duration.Value;
            }
            else
            {
                _activePotions[potion] = 0;
            }
            
            if (!potionData.Stats.IsNullOrEmpty())
            {
                GameEventBus.Publish(new StatProvidersChangedEvent(_owner));
            }

            if (!potionData.Effects.IsNullOrEmpty())
            {
                GameEventBus.Publish(new EffectProvidersChangedEvent(_owner, this));
            }
        }

        public void UnregisterPotion(ItemInstance potion)
        {
            if (!_activePotions.Remove(potion))
                return;
            
            var potionData = potion.ItemData.PotionData;
            if (!potionData.Stats.IsNullOrEmpty())
            {
                GameEventBus.Publish(new StatProvidersChangedEvent(_owner));
            }

            if (!potionData.Effects.IsNullOrEmpty())
            {
                GameEventBus.Publish(new EffectProvidersChangedEvent(_owner, this));
            }
        }

        public void Tick(float timeInterval, TickContext ctx)
        {
            var expired = new List<ItemInstance>();

            foreach (var kv in _activePotions.ToArray())
            {
                _activePotions[kv.Key] -= timeInterval;
                if (_activePotions[kv.Key] <= 0)
                {
                    expired.Add(kv.Key);
                }
            }

            foreach (var potion in expired)
            {
                UnregisterPotion(potion);
            }
        }

        public IEnumerable<StatModifier> GetStatModifiers()
        {
            foreach (var (potion, _) in _activePotions)
            {
                var potionData = potion.ItemData.PotionData;
                if (potionData.Effects.IsNullOrEmpty())
                    continue;
                foreach (var stat in  potion.ItemData.PotionData.Stats)
                {
                    yield return stat;
                }
            }
        }

        public IEnumerable<EffectData> GetActiveEffects()
        {
            foreach (var (potion, _) in _activePotions)
            {
                var potionData = potion.ItemData.PotionData;
                if (potionData.Effects.IsNullOrEmpty())
                    continue;
                foreach (var effect in  potion.ItemData.PotionData.Effects)
                {
                    yield return effect;
                }
            }
        }

        
    }
}