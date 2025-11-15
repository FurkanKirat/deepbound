using Data.Models;
using Systems.WorldSystem;

namespace Core.Context.Spawn
{
    public abstract class BaseSpawnContext
    {
        public string SubTypeId { get; set; }
        public World World { get; set; }
        public WorldPosition SpawnPosition { get; set; }
    }
}