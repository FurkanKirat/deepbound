using Core;
using Core.Context;
using Core.Events;
using Systems.CombatSystem.Damage;
using Systems.CombatSystem.Interfaces;
using Systems.EntitySystem.Interfaces;
using Systems.Physics;

namespace Systems.CombatSystem.Behaviors
{
    public class ArrowHitBehavior : IAttackBehavior
    {
        public bool TryAttack(AttackContext context, out string failReason)
        {
            var target = context.TargetEntity;
            if (target == null)
            {
                failReason = "Target is null";
                return false;
            }
            
            if (!context.TargetFilter(target))
            {
                failReason = "Filter did not result successfully";
                return false;
            }
            failReason = null;
            return true;
        }

        public void OnSuccess(AttackContext context)
        {
            var attacker = context.AttackingEntity;
            var target = context.TargetEntity;

            if (target == null)
                return;
            
            if (target is IDamageable damageable && attacker is IProjectile projectile)
            {
                var knockback = PhysicsUtils.GetKnockback(attacker.Position, target.Position);
                var damageInfo = new DamageInfo(
                    context.DamageContext.Amount, 
                    projectile.ProjectileData.DamageType, 
                    context.AttackingEntity,
                    knockback,
                    context.Random,
                    context.DamageContext.CritRate,
                    context.DamageContext.CritDamage
                    );
                damageable.TakeDamage(damageInfo);
            }

            GameEventBus.Publish(new EntityDestroyRequest(attacker));
        }

        public void OnFail(AttackContext context, string failReason)
        {
            
        }
    }
}