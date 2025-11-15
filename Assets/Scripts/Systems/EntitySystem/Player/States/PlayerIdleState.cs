using Core.Context;
using Systems.EntitySystem.Interfaces;
using Systems.EntitySystem.State;

namespace Systems.EntitySystem.Player.States
{
    public class PlayerIdleState : PlayerBaseState
    {
        private const string StateId = "player.idle";
        public override string Id => StateId ;
        public PlayerIdleState(IPlayer owner, StateMachine<IPlayer> stateMachine) 
            : base(owner, stateMachine)
        {
        }

        public override void Enter() { }

        public override void Exit() { }
        
        public override void Tick(float timeInterval, TickContext ctx) { }
    }

}