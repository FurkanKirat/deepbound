using System;
using Core.Context;
using Systems.EntitySystem.Interfaces;
using Systems.EntitySystem.State;
using Utils;

namespace Systems.EntitySystem.Enemy.Creature
{
    public class CreatureWanderState : EnemyBaseState
    {
        private float _randomDir;
        private const string StateId = "ai.creature.wander";
        public override string Id => StateId ;
        public CreatureWanderState(IEnemy owner, StateMachine<IEnemy> stateMachine, Random random) :
            base(owner, stateMachine, random)
        {
        }

        public override void Enter()
        {
            _randomDir = Random.NextFloat(-1f, 1f);
        }

        public override void Exit()
        {
            
        }

        public override void Tick(float deltaTime, TickContext ctx)
        {
            var movement = Owner.Movement;
            movement.ApplyHorizontalMovement(deltaTime, _randomDir);

            if (MathUtils.ApproximatelyZero(Owner.Velocity.x))
            {
                _randomDir *= -1f;
            }
        }
    }
}