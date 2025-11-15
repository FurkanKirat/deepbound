using Constants.Paths;
using Core.Context.Spawn;
using Data.Models.Player;
using Systems.EntitySystem.Player;
using Systems.InventorySystem;
using Utils;

namespace Systems.StartingSystem.Players
{
    public class PlayerCreator : IPlayerProvider
    {
        private readonly PlayerSpawnContext _playerSpawnContext;
        public PlayerCreator(PlayerSpawnContext spawnContext)
        {
            _playerSpawnContext = spawnContext;
        }

        public PlayerLogic GetPlayer()
        {
            var playerLogic = new PlayerLogic(_playerSpawnContext);
            var items = ResourcesHelper.LoadJson<StartingInventory>(PlayerPaths.StartingInventoryPath);

            foreach (var item in items.Inventory)
            {
                playerLogic.InventoryManager.GetPlayerInventory().AcceptItem(item.ToItemInstance());
            }
            
            return playerLogic;
        }
    }
}