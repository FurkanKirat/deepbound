using Data.Models;

namespace Systems.EntitySystem
{
    public class CharacterState
    {
        public bool IsJumping { get; set; }
        public bool IsGrounded { get; set; }
        public bool IsDashing { get; set; }
        public bool IsWallSliding { get; set; }
        public bool IsFacingRight { get; set; }

        public void Reset()
        {
            IsJumping = false;
            IsGrounded = false;
            IsDashing = false;
            IsWallSliding = false;
        }

        public WorldPosition GetLookingOffset(float xOffset, float yOffset)
        {
            return IsFacingRight ? 
                new WorldPosition(xOffset, yOffset) : 
                new WorldPosition(-xOffset, yOffset);
        }
    }

}