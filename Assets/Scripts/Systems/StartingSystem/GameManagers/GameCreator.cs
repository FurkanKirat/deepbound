using System;
using Core;
using Core.Context.Spawn;
using Core.Events;
using Data.Models.References;
using Systems.StartingSystem.Players;
using Systems.WorldSystem;

namespace Systems.StartingSystem.GameManagers
{
    public class GameCreator : IGameManagerProvider
    {
        private readonly SpriteRef _icon;

        public GameCreator(SpriteRef icon)
        {
            _icon = icon;
        }
        public void RequestGameManager(Action<GameManager> onReady)
        {
            var session = GameRoot.Instance.GameSession;
            
            var world = World.Create(session.CurrentWorldMeta);
            
            var playerSpawnCtx = new PlayerSpawnContext
            {
                SpawnPosition = world.CurrentDimension.PlayerSpawn,
                World = world,
                Icon = _icon
            };

            var playerLogic = new PlayerCreator(playerSpawnCtx).GetPlayer();
   
            GameEventBus.Publish(new EntitySpawnRequest(playerLogic));
            onReady?.Invoke(new GameManager(world));
        }
    }
}