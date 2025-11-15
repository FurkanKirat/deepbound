using Data.Models.Blocks.Behaviors;
using Systems.SaveSystem.SaveData.Inventory;

namespace Systems.SaveSystem.SaveData.BlockBehavior
{
    public class ChestBehaviorSaveData : BlockBehaviorSaveData
    {
        public InventoryManagerSaveData InventoryManagerSaveData;
        public ChestBehavior.ChestState ChestState;
    }
}