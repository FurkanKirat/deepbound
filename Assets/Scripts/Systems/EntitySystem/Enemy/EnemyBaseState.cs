using System;
using Core.Context;
using Interfaces;
using Systems.EntitySystem.Interfaces;
using Systems.EntitySystem.State;
using Systems.SaveSystem.SaveData.State;

namespace Systems.EntitySystem.Enemy
{
    public abstract class EnemyBaseState : IState<IEnemy>
    {
        public abstract string Id { get; }
        protected readonly IEnemy Owner;
        protected readonly StateMachine<IEnemy> StateMachine;
        protected readonly Random Random;
        
        protected EnemyBaseState(IEnemy owner, StateMachine<IEnemy> stateMachine, Random random)
        {
            Owner = owner;
            StateMachine = stateMachine;
            Random = random;
        }

        public abstract void Enter();

        public abstract void Exit();

        public abstract void Tick(float deltaTime, TickContext ctx);
        public virtual StateSaveData ToSaveData()
        {
            return new StateSaveData
            {
                Id = Id
            };
        }
    }
}