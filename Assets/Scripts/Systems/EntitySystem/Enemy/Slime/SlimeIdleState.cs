using System;
using Core.Context;
using Systems.EntitySystem.Interfaces;
using Systems.EntitySystem.State;
using Utils;

namespace Systems.EntitySystem.Enemy.Slime
{
    public class SlimeIdleState : EnemyBaseState
    {
        private const string StateId = "ai.slime.idle";
        public override string Id => StateId ;
        public float WaitTime { get; private set; }
        public SlimeIdleState(IEnemy owner, StateMachine<IEnemy> stateMachine, Random random) : 
            base(owner, stateMachine, random)
        {
        }

        public override void Enter()
        {
            WaitTime = Random.NextFloat(1f, 3f);
        }

        public override void Exit() { }

        public override void Tick(float deltaTime, TickContext ctx)
        {
            WaitTime -= deltaTime;
        }
    }
}