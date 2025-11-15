using System;
using Core.Context;
using Systems.EntitySystem.Interfaces;
using Systems.EntitySystem.State;
using Utils;

namespace Systems.EntitySystem.Enemy.Slime
{
    public class SlimeHopState : EnemyBaseState
    {
        private const string StateId = "ai.slime.hop";
        public override string Id => StateId ;
        public const float DetectionRadius = 5f;
        private float _randomDir;
        public SlimeHopState(IEnemy owner, StateMachine<IEnemy> stateMachine, Random random) 
            : base(owner, stateMachine, random)
        {
        }

        public override void Enter()
        {
            Owner.Movement.Jump();
            _randomDir = Random.NextFloat(-1f, 1f);
        }

        public override void Exit() {}

        public override void Tick(float deltaTime, TickContext ctx)
        {
            Owner.Movement.ApplyHorizontalMovement(deltaTime, _randomDir);
        }
    }
}