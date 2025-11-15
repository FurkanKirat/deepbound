using System;
using Core;
using Data.Models.References;
using Systems;
using Systems.SaveSystem;
using Systems.StartingSystem.GameManagers;
using Systems.WorldSystem;
using UnityEngine.SceneManagement;
using Utils;
using Visuals.UI.MainMenu;

namespace Session
{
    public class GameSession : IDisposable
    {
        public WorldMetaData CurrentWorldMeta { get; private set; }
        public string SavePath { get; private set; }
        public WorldOperation WorldOperation { get; private set; }
        public SessionStatus Status { get; private set; }
        public GameManager GameManager { get; private set; }
        private DisposeManager DisposeManager { get; } = new();
        private IGameManagerProvider GameManagerProvider { get; set; }

        public GameSession()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (scene.name == "GameScene")
                Status = SessionStatus.InProgress;
        }
        
        public void Dispose()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            DisposeManager.DisposeAll();
        }

        public void StartWorldCreation(string worldName, int seed, SpriteRef playerIcon)
        {
            Status = SessionStatus.CreatingWorld;
            WorldOperation = WorldOperation.Create;
            GameEventBus.Publish(new OpenMenuRequest { PanelType = MenuPanelType.CreatingWorld });

            var now = DateTime.UtcNow; 
            CurrentWorldMeta = WorldMetaData.Create(
                worldId: Guid.NewGuid().ToString(),
                worldName: worldName,
                createdAt: now,
                lastSavedAt: now,
                seed: seed,
                gameVersion: VersionHandler.CurrentVersion);
           
            SavePath = WorldPathUtils.GetWorldFolder(CurrentWorldMeta);
            GameManagerProvider = new GameCreator(playerIcon);
            GameManagerProvider.RequestGameManager(OnCreateGameManagerReady);
        }

        private void OnLoadGameManagerReady(GameManager gameManager)
        {
            if (Status != SessionStatus.LoadingWorld)
                return;
            GameManager = gameManager;
            SceneManager.LoadScene("GameScene", LoadSceneMode.Single);
            Status = SessionStatus.InProgress;
        }

        private void OnCreateGameManagerReady(GameManager gameManager)
        {
            if (Status != SessionStatus.CreatingWorld)
                return;
            GameManager = gameManager;
            GameManager.GameSaver.SaveWorld(true);
            SceneManager.LoadScene("GameScene", LoadSceneMode.Single);
            Status = SessionStatus.InProgress;
        }

        public void StartWorldLoad(WorldMetaData meta)
        {
            Status = SessionStatus.LoadingWorld;
            WorldOperation = WorldOperation.Load;
            GameEventBus.Publish(new OpenMenuRequest { PanelType = MenuPanelType.LoadingGame });
            CurrentWorldMeta = meta;
            SavePath = WorldPathUtils.GetWorldFolder(CurrentWorldMeta);
            GameManagerProvider = new GameLoader();
            GameManagerProvider.RequestGameManager(OnLoadGameManagerReady);
        }

        public void RegisterDisposable(IDisposable disposable)
        {
            DisposeManager.Register(disposable);
        }

        public void EndSession()
        {
            CurrentWorldMeta = null;
            SavePath = null;
            
            DisposeManager.DisposeAll();
            GameManager = null;
            
            Status = SessionStatus.Ended;
        }

        
    }
}