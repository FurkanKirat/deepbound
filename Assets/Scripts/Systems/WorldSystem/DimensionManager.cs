using System;
using Core;
using Core.Events;
using Systems.SaveSystem;
using Systems.SaveSystem.SaveData;
using Utils;

namespace Systems.WorldSystem
{
    public class DimensionManager : IDisposable
    {
        private readonly World _world;

        public DimensionManager(World world)
        {
            _world = world;
            GameEventBus.Subscribe<DimensionChangeRequest>(OnDimensionChangeRequest);
        }

        public void Dispose()
        {
            GameEventBus.Unsubscribe<DimensionChangeRequest>(OnDimensionChangeRequest);
        }

        private void OnDimensionChangeRequest(DimensionChangeRequest req)
        {
            var oldDim = _world.CurrentDimension;
            var player = _world.PlayerManager.Player;

            var settings = new DimensionGenerationSettings
            {
                DimensionId = req.DimensionId,
                WorldSeed = _world.Meta.Seed
            };
            // 1. Remove players from old dimension
            
            oldDim.EntityManager.Unregister(player);

            // 2. Unload the old dimension
            GameEventBus.Publish(new SaveDimensionRequest());
            oldDim.Dispose();

            // 3. New dimension create/load
            Dimension newDim;
            GameLogger.Log(settings.DimensionId, nameof(DimensionManager));
            if (_world.CreatedDimensions.Contains(settings.DimensionId))
            {
                var path = WorldPathUtils.GetDimensionFilePath(GameRoot.Instance.GameSession.SavePath, settings.DimensionId);
                var dimSave = SaveHelper.Load<DimensionSaveData>(path, SaveType.Dimension);
                newDim = Dimension.Load(_world, dimSave);
            }
            else
                newDim = DimensionGenerator.GenerateDimension(settings, _world);
            

            // 4. Spawn players to the new dimension
            player.Position = newDim.PlayerSpawn;
            newDim.EntityManager.Register(player);
            
            _world.CurrentDimension = newDim;

            // 5. Publish event
            GameEventBus.Publish(new DimensionChangedEvent(newDim.DimensionId));
        }

    }
}