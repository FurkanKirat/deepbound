using System;
using Core;
using Core.Context;
using Core.Events;
using Data.Database;
using Data.Models;
using Data.Models.Dimensions;
using GameLoop;
using Interfaces;
using Systems.EntitySystem;
using Systems.SaveSystem.Interfaces;
using Systems.SaveSystem.SaveData;
using Utils;
namespace Systems.WorldSystem
{
    public class Dimension :
        ISaveable<DimensionSaveData>, 
        ITickable, 
        IDisposable,
        IInitializable
    { 
        public DimensionData DimensionData { get; }
        public string DimensionId => DimensionData.Id;
        public EntityManager EntityManager { get; }
        public BlockManager BlockManager { get; }
        public LayerManager LayerManager { get; }
        public MinimapDiscovery MinimapDiscovery { get; }
        public WorldPosition PlayerSpawn { get; private set; }
        
        private World _world;

        private Dimension(string dimensionId, World world, EntityManager entityManager,
            BlockManager blockManager, LayerManager layerManager, MinimapDiscovery minimap, WorldPosition playerSpawn)
        {
            _world = world;
            DimensionData = Databases.Dimensions[dimensionId];
            EntityManager = entityManager;
            BlockManager = blockManager;
            LayerManager = layerManager;
            MinimapDiscovery = minimap;
            PlayerSpawn = playerSpawn;
            GameEventBus.Subscribe<PlayerSpawnChangedEvent>(OnPlayerSpawnChanged);
        }
        
        public static Dimension Create(DimensionGenerationContext generationContext)
        {
            var blocks = generationContext.Blocks;
            var id = generationContext.DimensionId;
            return new Dimension(
                generationContext.DimensionId, 
                generationContext.World,
                new EntityManager(), 
                new BlockManager(blocks, generationContext.World),
                LayerManager.Create(id, generationContext.SurfaceYPerX), 
                MinimapDiscovery.Create(blocks.Width, blocks.Height),
                generationContext.PlayerSpawn
                );
        }

        public static Dimension Load(World world, DimensionSaveData saveData)
        {
            MinimapDiscovery minimap;
            if (saveData.MinimapSaveData == null)
            {
                var grid = saveData.BlocksSaveData.WorldGrid;
                minimap = MinimapDiscovery.Create(grid.Width, grid.Height);
            }
            else
            {
                minimap = MinimapDiscovery.Load(saveData.MinimapSaveData);
            }

            var id = saveData.DimensionId;
            return new Dimension(
                id,
                world,
                new EntityManager(saveData.Entities, world),
                new BlockManager(saveData.BlocksSaveData, world),
                LayerManager.Load(id, saveData.LayersSaveData), 
                minimap,
                saveData.PlayerSpawn);
        }
        
        public void Initialize()
        {
            EntityManager.Initialize();
        }

        public void Dispose()
        {
            EntityManager.Dispose();
            BlockManager.Dispose();
            GameEventBus.Unsubscribe<PlayerSpawnChangedEvent>(OnPlayerSpawnChanged);
        }
        
        private void OnPlayerSpawnChanged(PlayerSpawnChangedEvent evt)
        {
            PlayerSpawn = evt.PlayerSpawn;
            GameLogger.Log($"changed player spawn to {PlayerSpawn}","Dimension");
        }
        
        public DimensionSaveData ToSaveData()
        {
            return new DimensionSaveData
            {
                DimensionId = DimensionId,
                Entities = EntityManager.ToSaveData(),
                BlocksSaveData = BlockManager.ToSaveData(),
                LayersSaveData = LayerManager.ToSaveData(),
                MinimapSaveData = MinimapDiscovery.ToSaveData(),
                PlayerSpawn = PlayerSpawn
            };
        }

        public void Tick(float timeInterval, TickContext ctx)
        {
            BlockManager.Tick(timeInterval, ctx);
            EntityManager.Tick(timeInterval, ctx);
            LayerManager.UpdatePlayerLayer(ctx.Player.Position);
        }
    }
    
}