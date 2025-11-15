using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core;
using Core.Context;
using Core.Events;
using GameLoop;
using Generated.Settings;
using Systems.EntitySystem.Player;
using Systems.SaveSystem.SaveData;
using Systems.SaveSystem.SaveData.Entity;
using Utils;

namespace Systems.SaveSystem
{
    public class GameSaver : IDisposable, ITickable
    {
        private float _elapsedTime = 0f;
        private readonly GameManager _gameManager;

        public GameSaver(GameManager data)
        {
            _gameManager = data;
            GameEventBus.Subscribe<SaveGlobalRequest>(OnSaveGlobalRequest);
            GameEventBus.Subscribe<SaveWorldRequest>(OnSaveWorldRequest);
            GameEventBus.Subscribe<SaveDimensionRequest>(OnDimensionSaveRequest);
        }

        public void Dispose()
        {
            GameEventBus.Unsubscribe<SaveGlobalRequest>(OnSaveGlobalRequest);
            GameEventBus.Unsubscribe<SaveWorldRequest>(OnSaveWorldRequest);
            GameEventBus.Unsubscribe<SaveDimensionRequest>(OnDimensionSaveRequest);
        }
        
        public void Tick(float timeInterval, TickContext ctx)
        {
            var settings = GameRoot.Instance.SettingsManager;
            if (!settings.Get<bool>(SettingsKeys.AutoSaveEnabled))
                return;
            _elapsedTime += timeInterval;

            var saveInterval = settings.Get<float>(SettingsKeys.AutoSaveInterval);
            if (_elapsedTime >= saveInterval)
            {
                SaveWorld(false);
            }
        }
        
        private void OnSaveGlobalRequest(SaveGlobalRequest evt)
        {
            SaveGlobalAsync(true);
        }
        
        private void OnDimensionSaveRequest(SaveDimensionRequest evt)
        {
            SaveDimensionAsync();
        }

        private void OnSaveWorldRequest(SaveWorldRequest evt)
        {
            SaveWorld(evt.SaveScreenshot);
        }

        public void SaveWorld(bool saveScreenshot)
        {
            SaveGlobalAsync(saveScreenshot);
            SaveDimensionAsync();
            _elapsedTime = 0f;
        }
        
        #region Snapshot Creation

        private GlobalSaveData CreateGlobalSaveSnapshot()
        {
            var world = _gameManager.World;
            world.Meta.LastSavedAt = DateTime.UtcNow;

            return new GlobalSaveData
            {
                CurrentDimensionId = world.CurrentDimension.DimensionId,
                GeneratedDimensions = new List<string>(world.CreatedDimensions),
                PlayerSaveData = (PlayerSaveData)((PlayerLogic)world.PlayerManager.Player).ToSaveData()
            };
        }
        
        private DimensionSaveData CreateCurrentDimensionSnapshot()
        {
            return _gameManager.World.CurrentDimension.ToSaveData();
        }

        #endregion

        #region Async Saving

        private void SaveGlobalAsync(bool saveScreenshot = false)
        {
            var snapshot = CreateGlobalSaveSnapshot();
            if (snapshot == null) return;

            var metaData = _gameManager.World.Meta;
            var worldPath = GameRoot.Instance.GameSession.SavePath;
            if (saveScreenshot)
                GameEventBus.Publish(new ScreenshotRequest(WorldPathUtils.GetWorldIconPath(worldPath)));
            
            Task.Run(() =>
            {
                try
                {
                    var globalPath = WorldPathUtils.GetGlobalFilePath(worldPath);
                    
                    var metaPath = WorldPathUtils.GetMetaFilePath(worldPath);
                    SaveHelper.Save(snapshot, globalPath, SaveType.Global);
                    SaveHelper.Save(metaData, metaPath, SaveType.Metadata);
                    GameEventBus.Publish(GlobalSavedEvent.Now);
                }
                catch (Exception ex)
                {
                    GameLogger.Error($"Failed to save global game: {ex.Message}", nameof(GameSaver));
                }

                
            });
        }

        private void SaveDimensionAsync()
        {
            var snapshot = CreateCurrentDimensionSnapshot();
            string savePath = GameRoot.Instance.GameSession.SavePath;
            if (snapshot == null) return;

            Task.Run(() =>
            {
                try
                {
                    var path = WorldPathUtils.GetDimensionFilePath(savePath, snapshot.DimensionId);
                    SaveHelper.Save(snapshot, path, SaveType.Dimension);
                    GameEventBus.Publish(new DimensionSavedEvent(snapshot.DimensionId));
                }
                catch (Exception ex)
                {
                    GameLogger.Error($"Failed to save dimension '{snapshot.DimensionId}': {ex.Message}", nameof(GameSaver));
                }
            });
        }

        #endregion
        
    }
}