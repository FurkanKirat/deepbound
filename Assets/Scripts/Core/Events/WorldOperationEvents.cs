using Systems.WorldSystem;

namespace Core.Events
{
    public struct WorldDeletedEvent : IEvent
    {
        public WorldMetaData MetaData { get; }

        public WorldDeletedEvent(WorldMetaData metaData)
        {
            MetaData = metaData;
        }
    }
}