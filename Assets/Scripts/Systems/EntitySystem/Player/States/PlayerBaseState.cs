using Core.Context;
using Interfaces;
using Systems.EntitySystem.Interfaces;
using Systems.EntitySystem.State;
using Systems.SaveSystem.SaveData.State;

namespace Systems.EntitySystem.Player.States
{
    public abstract class PlayerBaseState : IState<IPlayer>
    {
        public abstract string Id { get; }
        protected readonly IPlayer Owner;
        protected readonly StateMachine<IPlayer> StateMachine;

        protected PlayerBaseState(IPlayer owner, StateMachine<IPlayer> stateMachine)
        {
            Owner = owner;
            StateMachine = stateMachine;
        }
        public abstract void Enter();
        public abstract void Exit();
        public abstract void Tick(float timeInterval, TickContext ctx);
        public StateSaveData ToSaveData()
        {
            return new StateSaveData
            {
                Id = Id,
            };
        }
    }

}