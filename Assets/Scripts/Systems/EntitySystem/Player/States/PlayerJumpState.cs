using Core.Context;
using Systems.EntitySystem.Interfaces;
using Systems.EntitySystem.State;

namespace Systems.EntitySystem.Player.States
{
    public class PlayerJumpState : PlayerBaseState
    {
        private const string StateId = "player.jump";
        public override string Id => StateId ;
        public PlayerJumpState(IPlayer owner, StateMachine<IPlayer> stateMachine) : base(owner, stateMachine)
        {
        }

        public override void Enter()
        {
            Owner.Movement.Jump();
        }

        public override void Exit()
        {
            Owner.CharacterState.IsJumping = false;
        }
        
        public override void Tick(float timeInterval, TickContext ctx)
        {
            Owner.Movement.ApplyHorizontalMovement(timeInterval, ctx.PlayerInput.MoveX);
        }
    }
}