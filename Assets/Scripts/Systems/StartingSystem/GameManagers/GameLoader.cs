using System;
using Core;
using Core.Context.Spawn;
using Core.Events;
using Generated.Localization;
using Localization;
using Systems.SaveSystem;
using Systems.SaveSystem.SaveData;
using Systems.StartingSystem.Players;
using Systems.WorldSystem;
using Utils;
using Visuals.UI.ErrorSystem;
using Visuals.UI.MainMenu;

namespace Systems.StartingSystem.GameManagers
{
    public class GameLoader : IGameManagerProvider
    {
        public void RequestGameManager(Action<GameManager> onReady)
        {
            var session = GameRoot.Instance.GameSession;
            var savePath = session.SavePath;
            var metaData = session.CurrentWorldMeta;
            GlobalSaveData globalSave;
            try
            {
                globalSave =
                    SaveHelper.Load<GlobalSaveData>(WorldPathUtils.GetGlobalFilePath(savePath), SaveType.Global);
            }
            catch(Exception e)
            {
                GameEventBus.Publish(new OpenErrorPanelRequest(
                    text: e.Message,
                    title: LocalizationDatabase.Get(LocalizationKeys.ErrorGlobalSaveNotFoundTitle),
                    buttonEvents: new ButtonEventData[]
                    {
                        new(LocalizationDatabase.Get(LocalizationKeys.UiReturnToMainMenu), () => GameEventBus.Publish(new OpenMenuRequest(MenuPanelType.MainMenu)))
                    }));
                
                GameLogger.Error(e.Message, nameof(GameLoader));
                return;
            }

            string currentDimensionId = globalSave.CurrentDimensionId;
            World world;
            
            try
            {
                var path = WorldPathUtils.GetDimensionFilePath(savePath, currentDimensionId);
                var dimensionSave = SaveHelper.Load<DimensionSaveData>(path, SaveType.Dimension);
                world = World.Load(metaData, globalSave, dimensionSave);
            }
            catch(Exception e)
            {
                GameEventBus.Publish(new OpenErrorPanelRequest(
                    text: LocalizationDatabase.Get(LocalizationKeys.ErrorDimensionSaveNotFoundText),
                    title: LocalizationDatabase.Get(LocalizationKeys.ErrorDimensionSaveNotFoundTitle),
                    buttonEvents: new ButtonEventData[]
                    {
                        new(LocalizationDatabase.Get(LocalizationKeys.UiReturnToMainMenu), () => GameEventBus.Publish(new OpenMenuRequest(MenuPanelType.MainMenu))),
                        new(LocalizationDatabase.Get(LocalizationKeys.UiCreateDimension), () =>
                        {
                            var newWorld = World.Load(metaData, globalSave);
                            var gm = CreateGameManager(newWorld, globalSave);
                            onReady?.Invoke(gm);
                        })
                    }));
                GameLogger.Error(e.Message, nameof(GameLoader));
                return;
            }
            
            var gameManager = CreateGameManager(world, globalSave);
            onReady?.Invoke(gameManager);
        }
        
        private static GameManager CreateGameManager(World world, GlobalSaveData globalSave)
        {
            var playerSaveData = globalSave.PlayerSaveData;
            var playerSpawnCtx = new PlayerSpawnContext { World = world };
            var playerLogic = new PlayerLoader(playerSpawnCtx, playerSaveData).GetPlayer();
            GameEventBus.Publish(new EntitySpawnRequest(playerLogic));

            return new GameManager(world);
        }
    }
}