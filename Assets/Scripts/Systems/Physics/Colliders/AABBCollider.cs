using Data.Models;

namespace Systems.Physics.Colliders
{
    public readonly struct AABBCollider : ICollider
    {
        public WorldPosition Center { get; }
        public WorldPosition Size { get; }

        public WorldPosition Min => Center - Size / 2f;
        public WorldPosition Max => Center + Size / 2f;
        
        public Bounds2D Bounds => new()
        {
            Min = Center - Size / 2f,
            Max = Center + Size / 2f
        };

        public AABBCollider(WorldPosition center, WorldPosition size)
        {
            Center = center;
            Size = size;
        }

        public static AABBCollider FromBounds(Bounds2D bounds)
        {
            return new(bounds.Center, bounds.Size);
        }
        
        public static AABBCollider GetBlockAABB(TilePosition position, int width, int height)
        {
            var centerX = position.X + width / 2;
            var centerY = position.Y + height / 2;
            var center = new WorldPosition(centerX, centerY);
            var size = new WorldPosition(width, height);
            return new AABBCollider(center, size);
        }
        
        public bool Intersects(ICollider other)
        {
            if (other is AABBCollider aabb)
                return !(Max.x < aabb.Min.x || Min.x > aabb.Max.x ||
                         Max.y < aabb.Min.y || Min.y > aabb.Max.y);
            
            return false;
        }

        public WorldPosition ClosestPoint(WorldPosition point)
        {
            return WorldPosition.Min(WorldPosition.Max(point, Min), Max);
        }

        public bool Overlaps(AABBCollider other)
        {
            return !(Max.x < other.Min.x || Min.x > other.Max.x ||
                     Max.y < other.Min.y || Min.y > other.Max.y);
        }

        public override string ToString()
        {
            return
                $"AABBCollider: {nameof(Center)}: {Center}, {nameof(Size)}: {Size}, {nameof(Min)}: {Min}, {nameof(Max)}: {Max}, {nameof(Bounds)}: {Bounds}";
        }
    }
}