using Systems.EntitySystem.Interfaces;

namespace Core.Events
{
    public struct ItemPickedUpEvent : IEvent
    {
        public IItemEntity ItemEntity { get; }
        public IPlayer Player { get; }

        public ItemPickedUpEvent(IItemEntity itemEntity, IPlayer player)
        {
            ItemEntity = itemEntity;
            Player = player;
        }
    }

    public struct EntityDestroyedEvent : IEvent
    {
        public IPhysicalEntity Entity { get; }

        public EntityDestroyedEvent(IPhysicalEntity entity)
        {
            Entity = entity;
        }
    }

    public struct EntityDestroyRequest : IEvent
    {
        public IPhysicalEntity Entity { get; }

        public EntityDestroyRequest(IPhysicalEntity entity)
        {
            Entity = entity;
        }
    }
    
    public struct EntityDespawnEvent : IEvent
    {
        public IPhysicalEntity Entity { get; }

        public EntityDespawnEvent(IPhysicalEntity entity)
        {
            Entity = entity;
        }
    }

    public struct EntityDespawnRequest : IEvent
    {
        public IPhysicalEntity Entity { get; }
        
        public EntityDespawnRequest(IPhysicalEntity entity)
        {
            Entity = entity;
        }
    }

    public struct EntitySpawnRequest : IEvent
    {
        public IPhysicalEntity Entity { get; }

        public EntitySpawnRequest(IPhysicalEntity entity)
        {
            Entity = entity;
        }
    }
    
    public struct EntitySpawnEvent : IEvent
    {
        public IPhysicalEntity Entity { get; }

        public EntitySpawnEvent(IPhysicalEntity entity)
        {
            Entity = entity;
        }
    }

}