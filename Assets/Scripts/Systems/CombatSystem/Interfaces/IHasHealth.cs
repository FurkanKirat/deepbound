namespace Systems.CombatSystem.Interfaces
{
    public interface IHasHealth : IDamageable
    {
        float CurrentHealth { get; }
        float MaxHealth { get; }
        bool IsDead { get; }
        void Heal(float amount);
    }

}