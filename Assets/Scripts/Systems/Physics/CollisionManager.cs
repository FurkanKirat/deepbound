using System.Linq;
using Core.Context;
using GameLoop;
using Systems.WorldSystem;

namespace Systems.Physics
{
    public class CollisionManager : ITickable
    {
        private readonly World _world;

        public CollisionManager(World world)
        {
            _world = world;
        }

        public void Tick(float timeInterval, TickContext ctx)
        {
            var entityManager = _world.EntityManager;
            var entities = entityManager.AllEntities;
            var blockEntitiesCopy = _world.BlockManager.BlockEntities.ToList();
            for (int i = 0; i < entities.Count; i++)
            {
                var a = entities[i];
                if (PhysicsUtils.IsCollidingWithSolidTiles(a.Collider, _world.BlockManager, DirectionFlags.All, out var flags))
                {
                    a.OnCollisionWithTile(flags);
                }
                for (int j = i + 1; j < entities.Count; j++)
                {
                    var b = entities[j];

                    if (a.Collider.Intersects(b.Collider))
                    {
                        a.OnCollisionWithEntity(b);
                        b.OnCollisionWithEntity(a);
                    }
                }
                
                foreach (var blockEntity in blockEntitiesCopy)
                {
                    if (blockEntity.Collider.Intersects(a.Collider))
                    {
                        blockEntity.OnCollisionWithEntity(a);
                    }
                }
            }
        }
    }
}