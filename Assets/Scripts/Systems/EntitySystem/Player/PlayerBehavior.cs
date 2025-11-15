using Interfaces;
using Systems.EntitySystem.Interfaces;
using Systems.EntitySystem.Player.States;
using Systems.EntitySystem.State;
using Systems.InputSystem;
using Utils;

namespace Systems.EntitySystem.Player
{
    public class PlayerBehavior
    {
        public IState<IPlayer> InitialState { get; private set; }

        public void Configure(StateMachine<IPlayer> stateMachine, IPlayer player)
        {
            var idle = new PlayerIdleState(player, stateMachine);
            var run = new PlayerRunState(player, stateMachine);
            var jump = new PlayerJumpState(player, stateMachine);
            var fall = new PlayerFallState(player, stateMachine);

            // IDLE -> RUN
            stateMachine.AddTransition(idle, run, ctx => !MathUtils.ApproximatelyZero(ctx.PlayerInput.MoveX));
            // RUN -> IDLE
            stateMachine.AddTransition(run, idle, ctx => MathUtils.ApproximatelyZero(ctx.PlayerInput.MoveX));

            // Grounded -> JUMP
            stateMachine.AddTransition(idle, jump, ctx => ctx.PlayerInput.JumpPhase.ShouldJump() && player.IsGrounded);
            stateMachine.AddTransition(run, jump, ctx => ctx.PlayerInput.JumpPhase.ShouldJump() && player.IsGrounded);

            // JUMP -> FALL
            stateMachine.AddTransition(jump, fall, ctx => player.Velocity.y < 0);

            // IDLE/RUN -> FALL
            stateMachine.AddTransition(idle, fall, ctx => !player.IsGrounded);
            stateMachine.AddTransition(run, fall, ctx => !player.IsGrounded);

            // FALL -> IDLE/RUN
            stateMachine.AddTransition(fall, idle, ctx => player.IsGrounded && MathUtils.ApproximatelyZero(ctx.PlayerInput.MoveX));
            stateMachine.AddTransition(fall, run, ctx => player.IsGrounded && !MathUtils.ApproximatelyZero(ctx.PlayerInput.MoveX));

            InitialState = idle;
        }
    }
}