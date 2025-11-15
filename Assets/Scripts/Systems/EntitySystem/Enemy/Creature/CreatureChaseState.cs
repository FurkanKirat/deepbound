using System.Collections.Generic;
using Core.Context;
using Systems.EntitySystem.Interfaces;
using Systems.EntitySystem.State;
using Systems.StatSystem;
using UnityEngine;
using Random = System.Random;

namespace Systems.EntitySystem.Enemy.Creature
{
    public class CreatureChaseState : EnemyBaseState, IStatProvider
    {
        private const string StateId = "ai.creature.chase";
        public override string Id => StateId ;
        
        public CreatureChaseState(IEnemy owner, StateMachine<IEnemy> stateMachine, Random random) : 
            base(owner, stateMachine, random)
        {
        }

        public override void Enter()
        {
            Owner.StatCollection.AddProvider(this);
        }

        public override void Exit()
        {
            Owner.StatCollection.RemoveProvider(this);
        }

        public override void Tick(float deltaTime, TickContext ctx)
        {
            var dir = (ctx.Player.Position.ToVector2() - Owner.Position.ToVector2()).normalized;
            var movement = Owner.Movement;
            if (dir.y > 0 && Owner.CharacterState.IsGrounded)
            {
                movement.Jump();
            }
            
            float horizontalInput = Mathf.Abs(dir.x) > 0.1f ? Mathf.Sign(dir.x) : 0f;
            movement.ApplyHorizontalMovement(deltaTime, horizontalInput);
        }

        public IEnumerable<StatModifier> GetStatModifiers()
        {
            yield return new StatModifier(StatType.Speed, 2f, StatOperation.Multiply);
        }
    }
}