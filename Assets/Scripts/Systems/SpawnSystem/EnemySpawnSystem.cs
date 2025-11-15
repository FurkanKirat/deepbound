using System;
using System.Collections.Generic;
using Core;
using Core.Context;
using Core.Context.Spawn;
using Core.Events;
using Data.Database;
using Data.Models;
using Data.RegistrySystem;
using GameLoop;
using Systems.EntitySystem;
using Systems.EntitySystem.Interfaces;
using Systems.WorldSystem;
using Utils;

namespace Systems.SpawnSystem
{
    public class EnemySpawnSystem : 
        ITickable,
        IDisposable
    {
        private readonly List<IEnemy> _enemies = new();
        private int EnemyCount => _enemies.Count;
        private readonly World _world;

        private float _spawnCooldown = 0f;

        public EnemySpawnSystem(World world)
        {
            _world = world;
            GameEventBus.Subscribe<EntitySpawnEvent>(OnEntitySpawned);
            GameEventBus.Subscribe<EntityDespawnEvent>(OnEntityDespawned);
            GameEventBus.Subscribe<EntityDestroyedEvent>(OnEntityDestroyed);
        }
        
        public void Dispose()
        {
            GameEventBus.Unsubscribe<EntitySpawnEvent>(OnEntitySpawned);
            GameEventBus.Unsubscribe<EntityDespawnEvent>(OnEntityDespawned);
            GameEventBus.Unsubscribe<EntityDestroyedEvent>(OnEntityDestroyed);
        }

        public void Tick(float timeInterval, TickContext ctx)
        {
            if (!Configs.GameConfig.Spawn.EnemySpawnOn)
                return;
            
            _spawnCooldown -= timeInterval;
            if (_spawnCooldown > 0) return;
            
            if (EnemyCount < Configs.GameConfig.Spawn.MaxEnemyCount)
            {
                int spawnBatch = _world.Random.Next(1, 3);
                for(int i = 0; i < spawnBatch; i++)
                    SpawnEnemy();
                _spawnCooldown = Configs.GameConfig.Spawn.EnemySpawnInterval;
            }
        }

        private void SpawnEnemy()
        {
            var spawnPos = FindPosition();
            var currentLayer = _world.CurrentDimension.LayerManager.GetLayerForPosition(spawnPos.ToTilePosition());
            var spawnTable = currentLayer.EnemySpawnTable;
            
            var entities = spawnTable?.Load()?.Roll(_world.Random);
            if (entities == null)
                return;
            foreach (var entity in entities)
            {
                var enemySpawnContext = new EnemySpawnContext
                {
                    SubTypeId = entity.EnemyId,
                    World = _world,
                    SpawnPosition = spawnPos
                };
                var enemy = Registries.EnemyFactory.Create(entity.EnemyId, enemySpawnContext);
                GameEventBus.Publish(new EntitySpawnRequest(enemy));
            }
        }
        
        private WorldPosition FindPosition()
        {
            var player = _world.Player;
            var playerPos = player.Position;
            var playerTilePos = playerPos.ToTilePosition();

            var nearestEnemySpawn = Configs.GameConfig.Spawn.NearestEnemySpawn;
            var farthestEnemySpawn = Configs.GameConfig.Spawn.FarthestEnemySpawn;

            var tiles = new List<TilePosition>();
            for (int offsetX = -farthestEnemySpawn; offsetX <= farthestEnemySpawn; offsetX++)
            for(int offsetY = -farthestEnemySpawn; offsetY <= farthestEnemySpawn; offsetY++)
            {
                var pos = new TilePosition(offsetX + playerTilePos.X, offsetY + playerTilePos.Y);
                if (offsetX * offsetX + offsetY * offsetY >= nearestEnemySpawn && 
                    !_world.BlockManager.IsSolidAt(pos) &&
                    _world.BlockManager.IsSolidAt(pos.Down()))
                {
                    tiles.Add(pos);
                }
            }

            var selectedPos = RandomUtils.GetRandomElement(_world.Random, tiles);
            return selectedPos.ToWorldPosition();
        }

        private void OnEntitySpawned(EntitySpawnEvent e)
        {
            var entity = e.Entity;
            if (entity.Type == EntityType.Enemy && entity is IEnemy enemy)
                _enemies.Add(enemy);
        }

        private void OnEntityDespawned(EntityDespawnEvent e)
        {
            var entity = e.Entity;
            if (entity.Type == EntityType.Enemy && entity is IEnemy enemy)
                _enemies.Remove(enemy);
        }
        
        private void OnEntityDestroyed(EntityDestroyedEvent e)
        {
            var entity = e.Entity;
            if (entity.Type == EntityType.Enemy && entity is IEnemy enemy)
                _enemies.Remove(enemy);
        }
    }
}