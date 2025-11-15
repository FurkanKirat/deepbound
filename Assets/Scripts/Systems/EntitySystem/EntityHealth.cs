using Core;
using Core.Events;
using Systems.CombatSystem.Damage;
using Systems.EntitySystem.Interfaces;
using Systems.SaveSystem.Interfaces;
using Systems.SaveSystem.SaveData;
using Systems.StatSystem;
using UnityEngine;

namespace Systems.EntitySystem
{
    public class EntityHealth : ISaveable<EntityHealthSaveData>
    {
        public float Current { get; private set; }
        public float Max { get; }

        public bool IsDead => Current <= 0;
        
        private readonly IActor _entity;
        private StatCollection StatCollection => _entity.StatCollection;

        public EntityHealth(IActor entity)
        {
            Max = entity.StatCollection.GetStat(StatType.MaxHealth);
            Current = Max;
            _entity = entity;
        }

        public EntityHealth(IActor entity, EntityHealthSaveData saveData)
        {
            Max = entity.StatCollection.GetStat(StatType.MaxHealth);
            Current = saveData.CurrentHealth;
            _entity = entity;
        }
        public void TakeDamage(DamageInfo damage)
        {
            float effective = Mathf.Max(damage.Amount - StatCollection.GetStat(StatType.Defense), 0);
            var cur = Current - Mathf.RoundToInt(effective);
            Current = Mathf.Max(cur, 0);
            if (cur <= 0)
                if (_entity.Type == EntityType.Player)
                {
                    GameEventBus.Publish(new EntityDespawnRequest(_entity));
                    GameEventBus.Publish(new PlayerDiedEvent((IPlayer)_entity));
                }
                else
                    GameEventBus.Publish(new EntityDestroyRequest(_entity));
            
            GameEventBus.Publish(new HealthChangedEvent(Current, Max, _entity));
        }

        public void Heal(float amount)
        {
            Current = Mathf.Min(Current + amount, Max);
            GameEventBus.Publish(new HealthChangedEvent(Current, Max, _entity));
        }

        public EntityHealthSaveData ToSaveData()
        {
            return new EntityHealthSaveData
            {
                CurrentHealth = Current,
            };
        }
    }

}


