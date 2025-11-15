using System;
using System.Collections.Generic;
using Core;
using Core.Context;
using Core.Events;
using GameLoop;
using Systems.WorldSystem;

namespace Systems.EntitySystem.Player
{
    public class PlayerRespawner : ITickable, IDisposable
    {
        private readonly World _world;
        private readonly List<PlayerRespawnConfig> _playerRespawnConfigs = new();

        public PlayerRespawner(World world)
        {
            _world = world;
            GameEventBus.Subscribe<PlayerDiedEvent>(OnPlayerDied);
        }
        
        public void Dispose()
        {
            GameEventBus.Unsubscribe<PlayerDiedEvent>(OnPlayerDied);
        }

        private void OnPlayerDied(PlayerDiedEvent e)
        {
            var player = e.Player;
            _playerRespawnConfigs.Add(new PlayerRespawnConfig(player, player.Config.RespawnTime));
        }
        
        public void Tick(float timeInterval, TickContext ctx)
        {
            var copyConfigs = new List<PlayerRespawnConfig>(_playerRespawnConfigs);

            foreach (var respawnConfig in copyConfigs)
            {
                respawnConfig.RemainingTime -= timeInterval;
                var remaining = respawnConfig.RemainingTime;
                if (remaining <= 0)
                    RespawnPlayer(respawnConfig);
                
                respawnConfig.RemainingTime = remaining;
                GameEventBus.Publish(new PlayerSpawnProgressEvent(remaining, respawnConfig.Player));
            }
        }

        private void RespawnPlayer(PlayerRespawnConfig config)
        {
            var player = config.Player;
            player.Heal(player.MaxHealth);
            player.Position = _world.CurrentDimension.PlayerSpawn;
            _playerRespawnConfigs.Remove(config);
            GameEventBus.Publish(new EntitySpawnRequest(player));
        }
    }
}