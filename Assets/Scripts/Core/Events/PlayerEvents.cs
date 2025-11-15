using Data.Models;
using Systems.EntitySystem.Interfaces;

namespace Core.Events
{
    
    public struct PlayerSpawnEvent : IEvent
    {
        public IPlayer Player { get; }

        public PlayerSpawnEvent(IPlayer player)
        {
            Player = player;
        }
    }
    
    public struct PlayerDespawnEvent : IEvent
    {
        public IPlayer Player { get; }

        public PlayerDespawnEvent(IPlayer player)
        {
            Player = player;
        }
    }
    
    public readonly struct PlayerSpawnChangedEvent : IEvent
    {
        public WorldPosition PlayerSpawn { get; }

        public PlayerSpawnChangedEvent(WorldPosition playerSpawn)
        {
            PlayerSpawn = playerSpawn;
        }
    }
}