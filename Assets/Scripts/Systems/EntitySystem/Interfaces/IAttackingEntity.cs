using Core.Context;
using Systems.CombatSystem.Behaviors;
using Utils;

namespace Systems.EntitySystem.Interfaces
{
    public interface IAttackingEntity : IMovingEntity
    {
        IAttackBehavior AttackBehavior { get; }
    }

    public static class AttackingEntityExtensions
    {
        public static void AttackIfNeeded(this IAttackingEntity attackingEntity, AttackContext ctx)
        {
            var attackBehavior = attackingEntity.AttackBehavior;
            bool success = attackBehavior.TryAttack(ctx, out var failReason);
            if (success)
                attackBehavior.OnSuccess(ctx);
            
            else
                attackBehavior.OnFail(ctx, failReason);
            //GameLogger.Log($"Attack success: {success}, fail reason: {failReason}");
        }
    }
}