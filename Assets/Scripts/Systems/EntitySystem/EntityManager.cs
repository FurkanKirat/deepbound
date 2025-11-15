using System;
using System.Collections.Generic;
using System.Linq;
using Core;
using Core.Context;
using Core.Events;
using Data.Models;
using GameLoop;
using Interfaces;
using Systems.EntitySystem.Interfaces;
using Systems.SaveSystem.SaveData.Entity;
using Systems.WorldSystem;
using Utils;

namespace Systems.EntitySystem
{
    public class EntityManager : ITickable, IInitializable, IDisposable  
    {
        private readonly Dictionary<int, IPhysicalEntity> _entityById = new();
        private readonly List<IPhysicalEntity> _entitiesList = new();
        
        public EntityManager()
        {
            GameEventBus.Subscribe<EntityDespawnRequest>(OnEntityDespawnRequested);
            GameEventBus.Subscribe<EntitySpawnRequest>(OnEntitySpawnRequested);
            GameEventBus.Subscribe<EntityDestroyRequest>(OnDestroyRequested);
        }
        
        public EntityManager(List<EntitySaveData> saveData, World world) : this()
        {
            foreach (var entitySave in saveData)
            {
                var entity = EntityLoadFactory.GetEntityFromSaveData(entitySave, world);
                Register(entity);
            }
            IdGenerator.InitializeFromSave(saveData.Select(entity => entity.Id));
        }

        public void Initialize()
        {
            for (int i = _entitiesList.Count - 1; i >= 0; i--)
            {
                var entity = _entitiesList[i];
                if (entity is IInitializable initializable)
                    initializable.Initialize();
            }
        }

        public void Dispose()
        {
            GameEventBus.Unsubscribe<EntityDespawnRequest>(OnEntityDespawnRequested);
            GameEventBus.Unsubscribe<EntitySpawnRequest>(OnEntitySpawnRequested);
            GameEventBus.Unsubscribe<EntityDestroyRequest>(OnDestroyRequested);

            foreach (var entity in _entitiesList)
                entity.Dispose();
            
            _entityById.Clear();
            _entitiesList.Clear();
        }
        
        public IReadOnlyList<IPhysicalEntity> AllEntities => _entitiesList;
        public IPhysicalEntity GetEntity(int id) => _entityById.GetValueOrDefault(id);
        
        public void Register(IPhysicalEntity entity)
        {
            if (!_entityById.TryAdd(entity.Id, entity))
            {
                GameLogger.Warn($"Could not spawn entity with id {entity.Id} it already exists!" +
                                $"\n Existing type:  {_entityById[entity.Id]}", nameof(EntityManager));
                return;
            }
                
            
            _entitiesList.Add(entity);
            entity.Spawn();
            
            GameEventBus.Publish(new EntitySpawnEvent(entity));
        }

        public void Unregister(IPhysicalEntity entity)
        {
            _entityById.Remove(entity.Id);
            _entitiesList.Remove(entity);
    
            entity.Despawn();
    
            GameEventBus.Publish(new EntityDespawnEvent(entity));
        }
        
        public void Destroy(IPhysicalEntity entity)
        {
            _entityById.Remove(entity.Id);
            _entitiesList.Remove(entity);

            entity.Dispose();
            GameEventBus.Publish(new EntityDestroyedEvent(entity));
        }
        
        public void Tick(float timeInterval, TickContext ctx)
        {
            for (int i = _entitiesList.Count - 1; i >= 0; i--)
            {
                var entity = _entitiesList[i];
                if (entity.State == EntityState.Spawned && 
                    entity is ITickable tickable)
                    tickable.Tick(timeInterval, ctx);
            }
        }

        public List<IPhysicalEntity> GetEntitiesWithinRadius(WorldPosition position, float radius)
        {
            var squaredRadius = radius * radius;
            var entities = new List<IPhysicalEntity>();
            foreach (var entity in _entitiesList)
            {
                if (entity.State == EntityState.Spawned && 
                    WorldPosition.SquaredDistance(entity.Position, position) < squaredRadius)
                {
                    entities.Add(entity);
                }
            }
            return entities;
        }
        
        private void OnEntityDespawnRequested(EntityDespawnRequest e)
        {
            Unregister(e.Entity);
        }

        private void OnEntitySpawnRequested(EntitySpawnRequest e)
        {
            Register(e.Entity);
        }
        
        private void OnDestroyRequested(EntityDestroyRequest e)
        {
            Destroy(e.Entity);
        }
        
        public List<EntitySaveData> ToSaveData()
        {
            var saveData = new List<EntitySaveData>();
            foreach (var entity in _entitiesList)
            {
                if (entity is not BaseEntity baseEntity)
                    continue;

                if (entity.State != EntityState.Spawned)
                    continue;

                if (entity.Type is EntityType.Player) 
                    continue;
                
                saveData.Add(baseEntity.ToSaveData());
            }
               
            return saveData;
        }
    }
}