using Systems.InputSystem;

namespace Data.Models
{
    public readonly struct PlayerInput
    {
        public readonly float MoveX;
        public readonly JumpInputPhase JumpPhase;

        public PlayerInput(float moveX, JumpInputPhase jumpPhase)
        {
            MoveX = moveX;
            JumpPhase = jumpPhase;
        }

        public override string ToString()
        {
            return $"{nameof(MoveX)}: {MoveX}, {nameof(JumpPhase)}: {JumpPhase}";
        }
    }

}