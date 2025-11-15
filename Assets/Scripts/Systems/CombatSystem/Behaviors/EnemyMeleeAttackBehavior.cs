using Core.Context;
using Systems.CombatSystem.Damage;
using Systems.EntitySystem.Interfaces;
using Systems.Physics;

namespace Systems.CombatSystem.Behaviors
{
    public class EnemyMeleeAttackBehavior : IAttackBehavior
    {
        public bool TryAttack(AttackContext context, out string failReason)
        {
            var target = context.TargetEntity;
            
            if (!context.TargetFilter(target))
            {
                failReason = "Target filtered out";
                return false;
            }
            
            failReason = null;
            return true;
        }

        public void OnSuccess(AttackContext context)
        {
            var attackingEntity = context.AttackingEntity;
            var enemy = (IEnemy)attackingEntity;
            var targetEntity = context.TargetEntity;
            var knockback = PhysicsUtils.GetKnockback(attackingEntity.Position,targetEntity.Position);
            var damageInfo = new DamageInfo(
                context.DamageContext.Amount, 
                enemy.EnemyData.DamageType,
                attackingEntity,
                knockback,
                context.Random,
                context.DamageContext.CritRate,
                context.DamageContext.CritDamage
                );
            
            targetEntity.TakeDamage(damageInfo);
        }

        public void OnFail(AttackContext context, string failReason)
        {
            
        }
    }
}