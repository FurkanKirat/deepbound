using System;
using Core;
using Core.Context.Spawn;
using Core.Events;
using Systems.EntitySystem.Interfaces;
using Systems.EntitySystem.Item;
using Systems.WorldSystem;
using Utils;

namespace Systems.DropSystem
{
    public class ItemDropManager : IDisposable
    {
        //References
        private readonly World _world;
        
        public ItemDropManager(World world)
        {
            _world = world;
            GameEventBus.Subscribe<EntityDestroyedEvent>(OnEntityDestroyed);
        }
        
        public void Dispose()
        {
            GameEventBus.Unsubscribe<EntityDestroyedEvent>(OnEntityDestroyed);
        }

        private void OnEntityDestroyed(EntityDestroyedEvent e)
        {
            var entity = e.Entity;
            if (entity is IDropsItems dropper)
            {
                var droppedItems = dropper.GetDroppedItems(_world.Random);
                if (droppedItems == null)
                    return;
                foreach (var item in droppedItems)
                {
                    var offset = RandomUtils.RandomInsideCircle(_world.Random,0.3f, 0.7f);
                    var itemPosition = entity.Position + offset;
                    var itemEntitySpawnCtx = new ItemEntitySpawnContext
                    {
                        Item = item,
                        SpawnPosition = itemPosition,
                        World = _world
                    };
                    var itemDrop = new ItemEntityLogic(itemEntitySpawnCtx);
                    GameEventBus.Publish(new EntitySpawnRequest(itemDrop));
                }
            }
        }
    }

}

