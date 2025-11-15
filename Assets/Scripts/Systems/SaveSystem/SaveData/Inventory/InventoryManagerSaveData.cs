using System.Collections.Generic;
using Systems.InventorySystem;
using Systems.SaveSystem.Interfaces;

namespace Systems.SaveSystem.SaveData.Inventory
{
    public class InventoryManagerSaveData : ISaveData
    {
        public Dictionary<SlotCollectionType, SlotCollectionSaveData> SlotCollections { get; set; }
    }
}