using Systems.EntitySystem.Interfaces;

namespace Systems.MovementSystem.Behaviors
{
    public class ArrowMovement : IMovementBehavior
    {
        public void TickMovement(float deltaTime, IMovingEntity owner)
        {
            var movement = owner.Movement;

            movement.ApplyGravity(deltaTime);
            var velocity = owner.Velocity;
            
            movement.TryMove(velocity * deltaTime);
        }
        
    }
}