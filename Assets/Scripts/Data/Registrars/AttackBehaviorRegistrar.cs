using Data.RegistrySystem;
using Generated.Ids;
using Systems.CombatSystem.Behaviors;

namespace Data.Registrars
{
    public class AttackBehaviorRegistrar : IRegistrar
    {
        public void RegisterAll()
        {
            Registries.AttackBehaviorFactory.RegisterMany(
                (AttackBehaviorIds.EnemyMelee, _ => new EnemyMeleeAttackBehavior()),
            (AttackBehaviorIds.Arrow, _ => new ArrowHitBehavior()));
        }
    }
}