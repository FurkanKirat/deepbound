using System;
using Core.Context;
using Systems.EntitySystem.Interfaces;
using Systems.EntitySystem.State;

namespace Systems.EntitySystem.Enemy.Slime
{
    public class SlimeChaseState : EnemyBaseState
    {
        private const string StateId = "ai.slime.chase";
        public override string Id => StateId ;

        public SlimeChaseState(IEnemy owner, StateMachine<IEnemy> stateMachine, Random random) 
            : base(owner, stateMachine, random)
        {
        }

        public override void Enter()
        {
            Owner.Movement.Jump();
        }

        public override void Exit() {}
        public override void Tick(float deltaTime, TickContext ctx)
        {
            if (Owner.IsGrounded)
            {
                var dir = (ctx.Player.Position.ToVector2() - Owner.Position.ToVector2()).normalized;
                Owner.Movement.Jump();
                Owner.Movement.ApplyHorizontalMovement(deltaTime, dir.x);
            }
                
        }
    }
}