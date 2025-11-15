using System;
using System.Collections.Generic;
using Core.Context;
using Data.Database;
using Data.Models;
using Data.Models.Blocks.Behaviors;
using GameLoop;
using Generated.Ids;
using Systems.WorldSystem;
using Utils;

namespace Systems.SpawnSystem
{
    public class PortalSpawnSystem : ITickable, IDisposable
    {
        private readonly World _world;
        private float _spawnCooldown = 0f;
        private readonly Dictionary<TilePosition, PortalBehavior> _spawnedPortals = new ();

        public PortalSpawnSystem(World world)
        {
            _world = world;
        }
        
        public void Dispose()
        {
            
        }
        
        public void Tick(float timeInterval, TickContext ctx)
        {
            _spawnCooldown -= timeInterval;
            var spawn = Configs.GameConfig.Spawn;
            if (_spawnCooldown > 0 || _spawnedPortals.Count >= spawn.MaxPortalCount) return;
            
            Spawn();
            _spawnCooldown = spawn.PortalSpawnInterval;
        }

        private void Spawn()
        {
            var blockManager = _world.BlockManager;
            var pos = FindPosition();
            blockManager.PlaceBlockAt(pos, BlockIds.Portal);
            var behavior = blockManager.GetBlockEntity(pos).GetBehavior<PortalBehavior>();
            behavior.DimensionId = DimensionIds.Pocket;
            _spawnedPortals.Add(pos, behavior);
            GameLogger.Log($"Portal spawned at {pos}", nameof(PortalSpawnSystem));
        }

        private TilePosition FindPosition()
        {
            var random = _world.Random;
            var x = random.Next(10, _world.BlockManager.Width - 10);
            var maxY = _world.CurrentDimension.LayerManager.GetYPerColumn(x);
            var y = random.Next(10, maxY);
            
            return new TilePosition(x, y);
        }
    }
}