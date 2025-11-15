using Core.Context.Spawn;
using Systems.EntitySystem.Player;
using Systems.SaveSystem.SaveData.Entity;
using Utils;

namespace Systems.StartingSystem.Players
{
    public class PlayerLoader : IPlayerProvider
    {
        private readonly PlayerSpawnContext _playerSpawnContext;
        private readonly PlayerSaveData _playerSaveData;
        
        public PlayerLoader(PlayerSpawnContext playerSpawnContext, PlayerSaveData playerSaveData)
        {
            _playerSpawnContext = playerSpawnContext;
            _playerSaveData = playerSaveData;
            IdGenerator.RegisterId(playerSaveData.Id);
        }

        public PlayerLogic GetPlayer()
        {
            return new PlayerLogic(_playerSpawnContext, _playerSaveData);
        }
    }
}