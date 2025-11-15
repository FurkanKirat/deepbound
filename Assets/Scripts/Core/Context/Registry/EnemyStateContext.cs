using System;
using Systems.EntitySystem.Interfaces;
using Systems.EntitySystem.State;

namespace Core.Context.Registry
{
    public class EnemyStateContext
    {
        public IEnemy Enemy {get; set;}
        public StateMachine<IEnemy> StateMachine {get; set;}
        public Random Random {get; set;}
    }
}