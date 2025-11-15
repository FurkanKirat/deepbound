using Core.Context.Spawn;

namespace Core.Events
{
    public struct TrailSpawnRequest : IEvent
    {
        public TrailSpawnContext SpawnContext { get; set; }
    }

}