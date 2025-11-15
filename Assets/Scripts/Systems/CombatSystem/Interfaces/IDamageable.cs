using Systems.CombatSystem.Damage;

namespace Systems.CombatSystem.Interfaces
{
    public interface IDamageable
    {
        void TakeDamage(DamageInfo info);
        float GetResistance(DamageType type);
    }
}