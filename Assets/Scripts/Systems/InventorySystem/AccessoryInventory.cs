using System.Collections.Generic;
using Core;
using Core.Events;
using Data.Models.Items;
using Systems.BuffSystem;
using Systems.SaveSystem.SaveData.Inventory;
using Systems.StatSystem;

namespace Systems.InventorySystem
{
    public class AccessoryInventory : 
        IItemSlotCollection, 
        IStatProvider,
        IEffectProvider
    {
        private readonly SlotStorage _slotStorage;
        private readonly IInventoryOwner _owner;
        
        public SlotCollectionType Type => SlotCollectionType.Accessory;
        
        public int SlotCount  => _slotStorage.SlotCount;
        
        public IEnumerable<ItemInstance> AllItems
        {
            get
            {
                for (var i = 0; i < SlotCount; i++)
                    yield return _slotStorage.Slots[i].Item;
            }
        }
        public ItemInstance GetItem(int index) 
            => _slotStorage.GetItemAt(index);

        public AccessoryInventory(int size, IInventoryOwner owner)
        {
            _slotStorage = new SlotStorage(size);
            _owner = owner;
        }

        public AccessoryInventory(AccessorySaveData saveData, IInventoryOwner owner)
        {
            _slotStorage = new SlotStorage(saveData.SlotStorage);
            _owner = owner;
        }
        public void SetItemAt(int index, ItemInstance item)
        {
            _slotStorage.SetItemAt(index, item);
            GameEventBus.Publish(new InventorySlotChangedEvent(_owner, this, index, item));
            GameEventBus.Publish(new StatProvidersChangedEvent(_owner));
        } 

        public bool IsEmptyAt(int index) 
            => _slotStorage.IsEmptyAt(index);

        public void ClearItemAt(int index)
            => SetItemAt(index, ItemInstance.Empty);
        
        public bool CanSetAt(int index, ItemInstance item) 
            => item.IsAccessory() || item.IsEmpty;

        public bool AcceptItem(ItemInstance item)
        {
            if (!item.IsAccessory())
                return false;

            int index = -1;
            for (int i = 0; i < _slotStorage.SlotCount; i++)
            {
                var slot = _slotStorage.Slots[i];
                if (slot.IsEmpty)
                {
                    index = i;
                    break;
                }
            }
            if (index == -1)
                return false;
            SetItemAt(index, item);
            return true;
        }

        public (bool accepted, int RemainingCount) AcceptAtIfSame(int index, ItemAmount amount)
        {
            return (false, 0);
        }

        public void ChangeItemCountAt(int index, int count)
        {
            if (count <= 0)
                ClearItemAt(index);
        }

        public SlotCollectionSaveData ToSaveData()
        {
            return new AccessorySaveData
            {
                Type = Type,
                SlotStorage = _slotStorage.ToSaveData()
            };
        }

        public IEnumerable<StatModifier> GetStatModifiers()
        {
            foreach (var slot in _slotStorage.Slots)
            {
                var accessory = slot.Item.GetAccessoryData();
                if (accessory?.Stats == null)
                    continue;
                foreach (var modifier in accessory.Stats)
                {
                    yield return modifier;
                }
            }
        }

        public IEnumerable<EffectData> GetActiveEffects()
        {
            foreach (var slot in _slotStorage.Slots)
            {
                var accessory = slot.Item.GetAccessoryData();
                if (accessory?.Stats == null)
                    continue;
                foreach (var effect in accessory.Effects)
                {
                    yield return effect;
                }
            }
        }
    }
}