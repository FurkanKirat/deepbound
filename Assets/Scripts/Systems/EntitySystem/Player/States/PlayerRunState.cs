using Core.Context;
using Systems.EntitySystem.Interfaces;
using Systems.EntitySystem.State;

namespace Systems.EntitySystem.Player.States
{
    public class PlayerRunState : PlayerBaseState
    {
        private const string StateId = "player.run";
        public override string Id => StateId ;
        public PlayerRunState(IPlayer owner, StateMachine<IPlayer> stateMachine) : base(owner, stateMachine)
        {
        }

        public override void Enter() { }

        public override void Exit() { }
        
        public override void Tick(float timeInterval, TickContext ctx)
        {
            Owner.Movement.ApplyHorizontalMovement(timeInterval, ctx.PlayerInput.MoveX);
        }
    }
}