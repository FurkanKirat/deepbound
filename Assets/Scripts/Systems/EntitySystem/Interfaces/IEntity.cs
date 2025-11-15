using Systems.CombatSystem.Interfaces;
using Systems.EffectSystem;

namespace Systems.EntitySystem.Interfaces
{
    public interface IEntity : ILifecycle, IEffectable
    {
        int Id { get; }
        EntityType Type { get; }

        void AddCooldown(CooldownType type, float cooldownTime);
        public float GetCooldown(CooldownType type);
        public bool HasCooldown(CooldownType type);
        
    }

    public static class EntityExtensions
    {
        public static bool IsSameEntity(this IEntity x, IEntity y)
        {
            return x!=null && y!=null && x.Id == y.Id;
        }
    }
}