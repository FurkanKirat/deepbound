using Data.Models.References;
using Systems.SaveSystem.SaveData.Inventory;

namespace Systems.SaveSystem.SaveData.Entity
{
    public class PlayerSaveData : ActorSaveData
    {
        public InventoryManagerSaveData InventoryManager { get; set; }
        public PotionsSaveData Potions { get; set; }
        public SpriteRef Icon { get; set; }
        
        public PlayerSaveData()
        {
        }

        public PlayerSaveData(EntitySaveData other) : base(other)
        {
        }
    }
}