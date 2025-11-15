using Systems.CombatSystem.Damage;
using Systems.EntitySystem.Interfaces;

namespace Core.Events
{
    public readonly struct PlayerDamagedEvent : IEvent
    {
        public readonly DamageInfo DamageInfo;
        public readonly float FinalDamage;

        public PlayerDamagedEvent(DamageInfo damageInfo, float finalDamage)
        {
            DamageInfo = damageInfo;
            FinalDamage = finalDamage;
        }
    }

    public readonly struct HealthChangedEvent : IEvent
    {
        public readonly float CurrentHealth;
        public readonly float MaxHealth;
        public readonly IPhysicalEntity Entity;

        public HealthChangedEvent(float currentHealth, float maxHealth, IPhysicalEntity entity)
        {
            CurrentHealth = currentHealth;
            MaxHealth = maxHealth;
            Entity = entity;
        }
    }

    public readonly struct PlayerDiedEvent : IEvent
    {
        public readonly IPlayer Player;

        public PlayerDiedEvent(IPlayer player)
        {
            Player = player;
        }
    }

    public readonly struct PlayerSpawnProgressEvent : IEvent
    {
        public readonly float RemainingTime;
        public readonly IPlayer Player;

        public PlayerSpawnProgressEvent(float remainingTime, IPlayer player)
        {
            RemainingTime = remainingTime;
            Player = player;
        }
    }
}