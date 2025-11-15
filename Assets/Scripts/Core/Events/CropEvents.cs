using Data.Models;
using Data.Models.Blocks.Behaviors;

namespace Core.Events
{
    public struct CropStateChangedEvent : IEvent
    {
        public TilePosition Position { get; }
        public GrowthStage NewStage { get; }
        public GrowthStage OldStage { get; }

        public CropStateChangedEvent(TilePosition position, GrowthStage oldStage, GrowthStage newStage)
        {
            Position = position;
            OldStage = oldStage;
            NewStage = newStage;
        }
    }
}