using Core.Context;

namespace Systems.CombatSystem.Behaviors
{
    public interface IAttackBehavior
    {
        bool TryAttack(AttackContext context, out string failReason);
        void OnSuccess(AttackContext context);
        void OnFail(AttackContext context, string failReason);
    }

}