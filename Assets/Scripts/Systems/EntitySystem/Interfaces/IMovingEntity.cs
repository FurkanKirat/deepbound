using Systems.MovementSystem;
using Systems.StatSystem;
using UnityEngine;

namespace Systems.EntitySystem.Interfaces
{
    public interface IMovingEntity : IPhysicalEntity, ITickableEntity
    {
        StatCollection StatCollection { get; }
        CharacterState CharacterState { get; }
        
        Vector2 Velocity { get; set; }
        bool IsGrounded { get; }
        EntityMovement Movement { get; }
    }
}