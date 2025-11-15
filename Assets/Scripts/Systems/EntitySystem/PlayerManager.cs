using System;
using Core;
using Core.Events;
using Systems.EntitySystem.Interfaces;

namespace Systems.EntitySystem
{
    public class PlayerManager: IDisposable
    {
        public IPlayer Player { get; private set; }
        
        public PlayerManager()
        {
            GameEventBus.Subscribe<EntitySpawnEvent>(OnEntitySpawned);
            GameEventBus.Subscribe<EntityDestroyedEvent>(OnEntityDestroyed);
        }
        
        public void Dispose()
        {
            GameEventBus.Unsubscribe<EntitySpawnEvent>(OnEntitySpawned);
            GameEventBus.Unsubscribe<EntityDestroyedEvent>(OnEntityDestroyed);
        }

        private void OnEntitySpawned(EntitySpawnEvent e)
        {
            if (e.Entity.Type == EntityType.Player && e.Entity is IPlayer player)
            {
                Player = player;
                
                GameEventBus.Publish(new PlayerSpawnEvent(player));
            }
        }
        

        private void OnEntityDestroyed(EntityDestroyedEvent e)
        {
            if (e.Entity.Type == EntityType.Player && e.Entity is IPlayer player && Player == player)
            {
                Player = null;
                GameEventBus.Publish(new PlayerDespawnEvent(player));
            }
        }
    }
}