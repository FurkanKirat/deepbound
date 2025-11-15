using Systems.EntitySystem.Interfaces;

namespace Systems.MovementSystem.Behaviors
{
    public interface IMovementBehavior
    {
        void TickMovement(float deltaTime, IMovingEntity owner);
    }
}