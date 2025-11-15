using System;
using System.Collections.Generic;
using System.Linq;
using Core;
using Core.Context;
using Core.Events;
using Interfaces;
using Systems.CombatSystem.Interfaces;
using Systems.EntitySystem;
using Systems.EntitySystem.Interfaces;
using UnityEngine;
using Utils;
using Visuals.CameraScripts;
using Visuals.ObjectVisuals.Player;
using Visuals.UI.HealthSystem;
using Visuals.Utils;

namespace Visuals.ObjectVisuals
{
    public class EntityVisualizer : MonoBehaviour, IInitializable<ClientContext>
    {
        [Header("References")]
        [SerializeField] private CameraManager cameraManager;
        
        [Header("Entity Prefabs")]
        [SerializeField] private PlayerVisual playerPrefab;
        [SerializeField] private ItemEntityVisual itemEntityPrefab;
        [SerializeField] private ProjectileVisual projectilePrefab;
        [SerializeField] private EnemyVisual enemyPrefab;
        
        [Header("UI Prefabs")]
        [SerializeField] private EntityHealthBar entityHealthBarPrefab;
        
        [Header("Parents")]
        [SerializeField] private Transform healthBarParent;

        private readonly Dictionary<int, IEntityVisual> _entityById = new();
        private readonly ObjectPoolManager<EntityType> _entityPoolManager = new();
        private ObjectPool<EntityHealthBar> _entityHealthBarPool;

        private void Awake()
        {
            _entityPoolManager.RegisterPool(EntityType.Player, new ObjectPool<PlayerVisual>(playerPrefab, 1, transform));
            _entityPoolManager.RegisterPool(EntityType.Enemy, new ObjectPool<EnemyVisual>(enemyPrefab, 10, transform));
            _entityPoolManager.RegisterPool(EntityType.Item, new ObjectPool<ItemEntityVisual>(itemEntityPrefab, 5, transform));
            _entityPoolManager.RegisterPool(EntityType.Projectile, new ObjectPool<ProjectileVisual>(projectilePrefab, 10, transform));
            
            _entityHealthBarPool = new ObjectPool<EntityHealthBar>(entityHealthBarPrefab, 10, healthBarParent);
        }
        private void OnEnable()
        {
            GameEventBus.Subscribe<EntityDespawnEvent>(OnEntityDespawn);
            GameEventBus.Subscribe<EntitySpawnEvent>(OnEntitySpawn);
            GameEventBus.Subscribe<EntityDestroyedEvent>(OnEntityDestroyed);
        }
        
        private void OnDisable()
        {
            GameEventBus.Unsubscribe<EntityDespawnEvent>(OnEntityDespawn);
            GameEventBus.Unsubscribe<EntitySpawnEvent>(OnEntitySpawn);
            GameEventBus.Unsubscribe<EntityDestroyedEvent>(OnEntityDestroyed);
        }

        public void Initialize(ClientContext context)
        {
            foreach (var entity in context.World.EntityManager.AllEntities)
            {
                SpawnEntity(entity);
            }
        }
        
        private void SpawnPlayer(IPlayer player)
        {
            SpawnObject<IPlayer, PlayerVisual>(player, "Player");
        }

        private void SpawnEnemy(IEnemy enemy)
        {
            SpawnObject<IEnemy, EnemyVisual>(enemy, 
                $"{enemy.EnemyData.Id}_{enemy.Id}", true);
        }

        private void SpawnItem(IItemEntity itemEntity)
        {
            SpawnObject<IItemEntity, ItemEntityVisual>(itemEntity, 
                $"{itemEntity.ItemInstance.ItemData.Id}_{itemEntity.Id}");
        }

        private void SpawnProjectile(IProjectile projectile)
        {
            SpawnObject<IProjectile, ProjectileVisual>(projectile, 
                $"{projectile.ProjectileData.Id}_{projectile.Id}");
        }

        private void SpawnObject<TEntity, TPrefab>(
            TEntity entity,
            string objectName,
            bool spawnHealthBar = false) 
            where TEntity : IPhysicalEntity
            where TPrefab : BaseEntityVisual<TEntity>
        {
            var entityVisual = _entityPoolManager.Get<TPrefab>(entity.Type);
            #if UNITY_EDITOR
            entityVisual.name = objectName;
            #endif
            entityVisual.Initialize(entity);
            entityVisual.OnSpawn();
            
            if (spawnHealthBar)
            {
                var hasHealth = (IHasHealth)entity;
                var healthBar = SpawnHealthBar();
                healthBar.Initialize(hasHealth.CurrentHealth, hasHealth.MaxHealth, entity.Id, entityVisual.transform, cameraManager.MainCamera);
                entityVisual.AttachHealthBar(healthBar);
            }
            _entityById.Add(entity.Id, entityVisual);
            ConfigureVisual(entityVisual);
            //DebugCompareById();
        }

        private void DebugCompareById()
        {
            var tolerance = 0.05f;
            var sb = new System.Text.StringBuilder();

            var realEntities = GameRoot.Instance.GameSession.GameManager.World.EntityManager.AllEntities.ToDictionary(e => e.Id);
            var visualEntities = _entityById;

            sb.AppendLine("Checking Real -> Visual matches:");

            foreach (var real in realEntities.Values)
            {
                if (!visualEntities.TryGetValue(real.Id, out var visual))
                {
                    sb.AppendLine($"No visual found for entity {real.Id}");
                }
                else
                {
                    if (Vector3.Distance(real.Position.ToVector3(), visual.Object.transform.position) > tolerance)
                        sb.AppendLine($"Position mismatch for entity {real.Id}: Real {real.Position}, Visual {visual.Object.transform.position}");
                }
            }

            sb.AppendLine("Checking Visual -> Real matches:");

            foreach (var kvp in visualEntities)
            {
                if (!realEntities.ContainsKey(kvp.Key))
                {
                    sb.AppendLine($"Visual entity {kvp.Key} has no real entity!");
                }
            }

            GameLogger.Log(sb.ToString());
        }


        
        private void ConfigureVisual(IEntityVisual entity)
        {
            entity.Object.layer = 10;
            var entityRenderer = entity.Renderer;
            if (entityRenderer != null)
            {
                entityRenderer.sortingLayerName = "Entities";
                entityRenderer.sortingOrder = 10;
            }
        }

        private void DespawnEntity(IPhysicalEntity entity)
        {
            if (!_entityById.Remove(entity.Id, out var visual)) 
                return;
            visual.OnDespawn(this);
            switch (entity.Type)
            {
                case EntityType.Player:
                    _entityPoolManager.Release(entity.Type, (PlayerVisual)visual);
                    break;
                case EntityType.Enemy:
                    _entityPoolManager.Release(entity.Type, (EnemyVisual)visual);
                    break;
                case EntityType.Item:
                    _entityPoolManager.Release(entity.Type, (ItemEntityVisual)visual);
                    break;
                case EntityType.Projectile:
                    _entityPoolManager.Release(entity.Type, (ProjectileVisual)visual);
                    break;
                case EntityType.Npc:
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        private void OnEntitySpawn(EntitySpawnEvent e)
        {
            SpawnEntity(e.Entity);
        }

        private void SpawnEntity(IPhysicalEntity entity)
        {
            switch (entity.Type)
            {
                case EntityType.Item :
                    SpawnItem((IItemEntity)entity);
                    break;
                
                case EntityType.Enemy :
                    SpawnEnemy((IEnemy)entity);
                    break;
                
                case EntityType.Projectile :
                    SpawnProjectile((IProjectile)entity);
                    break;
                
                case EntityType.Player :
                    SpawnPlayer((IPlayer)entity);
                    break;
                
                case EntityType.Npc:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        private void OnEntityDespawn(EntityDespawnEvent e)
        {
            DespawnEntity(e.Entity);
        }

        private void OnEntityDestroyed(EntityDestroyedEvent e)
        {
            DespawnEntity(e.Entity);
        }

        private EntityHealthBar SpawnHealthBar()
        {
            return _entityHealthBarPool.Get();
        }

        public void DespawnHealthBar(EntityHealthBar healthBar)
        {
            _entityHealthBarPool.Release(healthBar);
        }
        
    }

}