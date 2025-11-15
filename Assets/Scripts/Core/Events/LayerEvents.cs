using Data.Models.Dimensions;

namespace Core.Events
{
    public struct WorldLayerChangedEvent : IEvent
    {
        public WorldLayer WorldLayer { get; }

        public WorldLayerChangedEvent(WorldLayer worldLayer)
        {
            WorldLayer = worldLayer;
        }
    }
}