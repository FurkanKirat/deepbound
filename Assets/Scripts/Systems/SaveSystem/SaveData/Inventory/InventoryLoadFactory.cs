using System;
using Systems.CombatSystem.Armor;
using Systems.InventorySystem;

namespace Systems.SaveSystem.SaveData.Inventory
{
    public static class InventoryLoadFactory
    {
        public static IItemSlotCollection LoadInventory(IInventoryOwner owner, SlotCollectionSaveData saveData)
        {
            return saveData.Type switch
            {
                SlotCollectionType.Inventory => owner.InventoryOwnerType switch
                {
                    InventoryOwnerType.Player => new PlayerInventory((InventorySaveData)saveData, owner),
                    _ => new InventorySystem.Inventory((InventorySaveData)saveData, owner)
                },
                SlotCollectionType.Equipment => new Equipment((EquipmentSaveData)saveData, owner),
                SlotCollectionType.Accessory => new AccessoryInventory((AccessorySaveData)saveData, owner),
                _ => throw new ArgumentOutOfRangeException(nameof(saveData.Type), saveData.Type, null)
            };
        }
    }
}