using Systems.SaveSystem.Interfaces;

namespace Systems.SaveSystem.SaveData.Inventory
{
    public class SlotStorageSaveData : ISaveData
    {
        public ItemSaveData[] Slots;
        public int SlotCount;
    }
}