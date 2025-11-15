using Data.Models;
using Data.Models.Entities;
using Systems.Physics;
using Systems.Physics.Colliders;

namespace Systems.EntitySystem.Interfaces
{
    public interface IPhysicalEntity : IEntity, ICollidable
    {
        WorldPosition Position { get; set; }
        ICollider Collider { get; }
        EntityData EntityData { get; }
        void OnCollisionWithTile(DirectionFlags directionFlags);
    }
}