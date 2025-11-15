using Core;
using Core.Context;
using Core.Context.Registry;
using Core.Context.Spawn;
using Core.Events;
using Data.Database;
using Data.Models.Blocks;
using Data.Models.Entities;
using Systems.EntitySystem.Interfaces;
using Systems.MovementSystem;
using Systems.Physics;
using Systems.SaveSystem.SaveData.Entity;
using Systems.StatSystem;
using Systems.WorldSystem;

namespace Systems.EntitySystem.Projectile
{
    public class ProjectileLogic : BaseEntity, IProjectile
    {
        public override EntityType Type => EntityType.Projectile;
        public ProjectileData ProjectileData { get; private set; }
        public IPhysicalEntity Owner { get; private set; }
        public DamageContext Damage { get; private set; }
        public override EntityData EntityData => ProjectileData;

        protected World World;

        // Will Change
        public ProjectileLogic(ProjectileSpawnContext spawnContext)
            => Initialize(spawnContext);

        public ProjectileLogic(ProjectileSpawnContext spawnContext, ProjectileSaveData saveData)
            => Initialize(spawnContext, saveData);

        private void Initialize(ProjectileSpawnContext spawnContext, ProjectileSaveData saveData = null)
        {
            InitializeBase(spawnContext, saveData);
            World = spawnContext.World;
            ProjectileData = Databases.Projectiles[spawnContext.SubTypeId];

            StatCollection.SetBaseStats(ProjectileData.BaseStats);
            
            Damage = spawnContext.DamageContext;
            Owner = spawnContext.Owner;
            
            ColliderHandler = new ColliderHandler(this, spawnContext.SpawnPosition, ProjectileData.Size, spawnContext.World);
            var movementCtx = new MovementBehaviorContext
            {
                SpawnerEntity = Owner,
                BehaviorOwner = this,
                TargetPos = spawnContext.TargetPosition ?? Position
            };
            var movementType = ProjectileData.MovementBehavior.Create(movementCtx);
            Movement = new EntityMovement(movementType, this, spawnContext.World);

            Velocity = spawnContext.Direction * ProjectileData.BaseStats[StatType.Speed];
        }
        
        
        public override void OnCollisionWithTile(DirectionFlags directionFlags)
        {
            if (ProjectileData.DestroyOnTileCollision)
            {
                var endPoint = ProjectileUtils.GetEndpoint(this);
                if (World.BlockManager.GetBlockAt(endPoint.ToTilePosition()).IsSolid())
                    GameEventBus.Publish(new EntityDestroyRequest(this));
            }
                
        }
    }
}