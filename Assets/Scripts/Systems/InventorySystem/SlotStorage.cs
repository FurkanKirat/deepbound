using System;
using System.Collections.Generic;
using Data.Models.Items;
using Systems.SaveSystem.Interfaces;
using Systems.SaveSystem.SaveData;
using Systems.SaveSystem.SaveData.Inventory;
using Utils;

namespace Systems.InventorySystem
{
    public class SlotStorage : ISaveable<SlotStorageSaveData>
    {
        private readonly InventorySlot[] _slots;

        public SlotStorage(int size)
        {
            _slots = new InventorySlot[size];
            for (int i = 0; i < size; i++)
                _slots[i] = new InventorySlot();
        }

        public SlotStorage(SlotStorageSaveData saveData)
        {
            var count = saveData.SlotCount;
            _slots = new InventorySlot[count];
            for (int i = 0; i < count; i++)
            {
                var slot = new InventorySlot();
                var item = ItemInstance.Load(saveData.Slots[i]);
                slot.Accept(item, item?.Count ?? 0);
                _slots[i] = slot;
            }
        }
        
        public SlotStorageSaveData ToSaveData()
        {
            var saveSlots = new ItemSaveData[SlotCount];
            for (int i = 0; i < SlotCount; i++)
            {
                saveSlots[i] = _slots[i].Item?.ToSaveData();
            }
            
            return new SlotStorageSaveData
            {
                Slots = saveSlots,
                SlotCount = SlotCount,
            };
        }

        public int SlotCount => _slots.Length;
        public IReadOnlyList<InventorySlot> Slots => _slots;

        public void SetItemAt(int slotIndex, ItemInstance itemInstance)
        {
            if (!HasSlot(slotIndex)) return;
            _slots[slotIndex].Item = itemInstance;
        }

        public bool ClearSlot(int slotIndex)
        {
            if (!HasSlot(slotIndex)) return false;
            _slots[slotIndex].Clear();
            return true;
        }
        
        public ItemInstance GetItemAt(int slotIndex)
        {
            if (!HasSlot(slotIndex)) return ItemInstance.Empty;
            return _slots[slotIndex].Item;
        }
        
        public bool IsEmptyAt(int slotIndex)
        {
            if (!HasSlot(slotIndex)) return false;
            return _slots[slotIndex].IsEmpty;
        }

        public bool TryGetSlot(int slotIndex, out InventorySlot slot)
        {
            if (slotIndex < 0 || slotIndex >= _slots.Length)
            {
                slot = null;
                return false;
            }

            slot = _slots[slotIndex];
            return true;
        }
        
        public InventorySlot GetSlot(int slotIndex)
            => _slots[slotIndex];
        
        public bool HasSlot(int slotIndex) => slotIndex >= 0 && slotIndex < _slots.Length;

        public void ClearSlots() => _slots.Clear();
        
        public void Shuffle(Random random) => _slots.Shuffle(random);

        
    }

}