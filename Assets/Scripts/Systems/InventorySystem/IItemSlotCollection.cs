using System.Collections.Generic;
using Data.Models.Items;
using Systems.SaveSystem.Interfaces;
using Systems.SaveSystem.SaveData.Inventory;

namespace Systems.InventorySystem
{
    public interface IItemSlotCollection : ISaveable<SlotCollectionSaveData>
    {
        SlotCollectionType Type { get; }
        int SlotCount { get; }
        IEnumerable<ItemInstance> AllItems { get; }
        ItemInstance GetItem(int index);
        void SetItemAt(int index, ItemInstance item);
        bool IsEmptyAt(int index);
        void ClearItemAt(int index);
        bool CanSetAt(int index, ItemInstance item);
        bool AcceptItem(ItemInstance item);
        (bool accepted, int RemainingCount) AcceptAtIfSame(int index, ItemAmount amount);
        void ChangeItemCountAt(int index, int count);
    }

}