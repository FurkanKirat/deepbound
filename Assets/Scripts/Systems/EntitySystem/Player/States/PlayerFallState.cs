using Core.Context;
using Systems.CombatSystem.Damage;
using Systems.EntitySystem.Interfaces;
using Systems.EntitySystem.State;
using Systems.StatSystem;
using UnityEngine;

namespace Systems.EntitySystem.Player.States
{
    public class PlayerFallState : PlayerBaseState
    {
        private const string StateId = "player.fall";
        public override string Id => StateId ;
        
        private float _fallStartHeight;
        public PlayerFallState(IPlayer owner, StateMachine<IPlayer> stateMachine) 
            : base(owner, stateMachine)
        {
        }

        public override void Enter()
        {
            _fallStartHeight = Owner.Position.y;
        }

        public override void Exit()
        {
            float fallEndHeight = Owner.Position.y;
            float fallDistance = _fallStartHeight - fallEndHeight;

            var jumpForce = Owner.StatCollection.GetStat(StatType.JumpForce);
            var safeHeight = jumpForce;
            if (fallDistance > safeHeight)
            {
                float extra = fallDistance - safeHeight;

                float damage = (extra * extra) / safeHeight * 10f;
                var damageInfo = new DamageInfo(
                    damage,
                    DamageType.Fall,
                    Owner,
                    Vector2.zero
                );
                Owner.TakeDamage(damageInfo);
            }
        }
        
        public override void Tick(float timeInterval, TickContext ctx)
        {
            Owner.Movement.ApplyHorizontalMovement(timeInterval, ctx.PlayerInput.MoveX);
        }
    }

}