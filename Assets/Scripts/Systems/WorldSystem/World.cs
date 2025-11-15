using System;
using System.Collections.Generic;
using Core.Context;
using GameLoop;
using Generated.Ids;
using Interfaces;
using Systems.CraftingSystem;
using Systems.DropSystem;
using Systems.EntitySystem;
using Systems.EntitySystem.Interfaces;
using Systems.EntitySystem.Player;
using Systems.Physics;
using Systems.SaveSystem;
using Systems.SaveSystem.SaveData;
using Systems.SpawnSystem;

namespace Systems.WorldSystem
{
    public class World : IInitializable, ITickable, IDisposable
    {
        public WorldMetaData Meta { get; }
        public readonly Random Random = new ();
        public Dimension CurrentDimension { get; set; }
        public PlayerManager PlayerManager { get; }
        public List<string> CreatedDimensions { get; }
        
        public BlockManager BlockManager => CurrentDimension.BlockManager;
        public EntityManager EntityManager => CurrentDimension.EntityManager;
        public IPlayer Player => PlayerManager.Player;
        
        public CraftManager CraftManager { get; }
        public ItemDropManager ItemDropManager { get; } 
        public BreakBlockManager BreakBlockManager { get; }
        public CollisionManager CollisionManager { get; }
        public DimensionManager DimensionManager { get; }
        public PlayerRespawner PlayerRespawner { get; }
        public EnemySpawnSystem EnemySpawnSystem { get; }
        public PortalSpawnSystem PortalSpawnSystem { get; }
        
        private World(WorldMetaData metaData, List<string> createdDimensions)
        {
            Meta = metaData;
            CreatedDimensions = createdDimensions;
            
            PlayerManager = new PlayerManager();
            CraftManager = new CraftManager(this);
            ItemDropManager = new ItemDropManager(this);
            BreakBlockManager = new BreakBlockManager(this);
            CollisionManager = new CollisionManager(this);
            DimensionManager = new DimensionManager(this);
            PlayerRespawner = new PlayerRespawner(this);
            EnemySpawnSystem = new EnemySpawnSystem(this);
            PortalSpawnSystem = new PortalSpawnSystem(this);
        }

        private void AttachDimension(Dimension dimension)
        {
            CurrentDimension = dimension;
        }

        public static World Create(WorldMetaData metaData)
        {
            var dimensionCtx = new DimensionGenerationSettings
            {
                DimensionId = DimensionIds.Forest,
                WorldSeed = metaData.Seed
            };
            
            var world = new World(metaData, new List<string>());
            var dimension = DimensionGenerator.GenerateDimension(dimensionCtx, world);
            world.CreatedDimensions.Add(dimension.DimensionId);
            world.AttachDimension(dimension);
            return world;
        }

        public static World Load(WorldMetaData metaData, GlobalSaveData globalSaveData, DimensionSaveData dimensionSaveData)
        {
            var world = new World(metaData, globalSaveData.GeneratedDimensions);
            var currentDimension = Dimension.Load(world, dimensionSaveData);
            world.AttachDimension(currentDimension);
            return world;
        }
        
        public static World Load(WorldMetaData metaData, GlobalSaveData globalSaveData)
        {
            var world = new World(metaData, globalSaveData.GeneratedDimensions);
            var dimensionCtx = new DimensionGenerationSettings
            {
                DimensionId = globalSaveData.CurrentDimensionId,
                WorldSeed = metaData.Seed
            };
            var dimension = DimensionGenerator.GenerateDimension(dimensionCtx, world);
            world.AttachDimension(dimension);
            return world;
        }
       
        public void Tick(float timeInterval, TickContext ctx)
        {
            if (ctx.GameStateManager.HasFlag(GameFlags.Paused))
                return;
            CurrentDimension.Tick(timeInterval, ctx);
            PlayerRespawner.Tick(timeInterval, ctx);
            CollisionManager.Tick(timeInterval, ctx);
            EnemySpawnSystem.Tick(timeInterval, ctx);
            PortalSpawnSystem.Tick(timeInterval, ctx);
            BreakBlockManager.Tick(timeInterval, ctx);
        }

        public void Initialize()
        {
            CurrentDimension.Initialize();
        }
        public void Dispose()
        {
            CurrentDimension.Dispose();
            PlayerManager.Dispose();
            CraftManager.Dispose();
            ItemDropManager.Dispose();
            BreakBlockManager.Dispose();
            DimensionManager.Dispose();
            PlayerRespawner.Dispose();
            EnemySpawnSystem.Dispose();
            PortalSpawnSystem.Dispose();
        }
    }
}