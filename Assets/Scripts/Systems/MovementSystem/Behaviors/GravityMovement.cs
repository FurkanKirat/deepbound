using Systems.EntitySystem.Interfaces;

namespace Systems.MovementSystem.Behaviors
{
    public class GravityMovement : IMovementBehavior
    {
        public void TickMovement(float deltaTime, IMovingEntity owner)
        {
            owner.Movement.ApplyGravity(deltaTime);
            owner.Movement.ApplyFriction();
        }
    }
}