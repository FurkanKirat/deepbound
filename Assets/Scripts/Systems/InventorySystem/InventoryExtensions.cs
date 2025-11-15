using Systems.CombatSystem.Armor;

namespace Systems.InventorySystem
{
    public static class InventoryExtensions
    {
        public static Equipment GetEquipment(this InventoryManager inventoryManager)
            => inventoryManager.GetInventory<Equipment>(SlotCollectionType.Equipment);
        
        public static AccessoryInventory GetAccessoryInventory(this InventoryManager inventoryManager)
            => inventoryManager.GetInventory<AccessoryInventory>(SlotCollectionType.Accessory);
        
        public static PlayerInventory GetPlayerInventory(this InventoryManager inventoryManager)
            => inventoryManager.GetInventory<PlayerInventory>(SlotCollectionType.Inventory);
    }
}