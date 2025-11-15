using Data.Models;

namespace Systems.Physics.Colliders
{
    public interface ICollider
    {
        Bounds2D Bounds { get; }
        bool Intersects(ICollider other);
        WorldPosition ClosestPoint(WorldPosition point);
    }

}