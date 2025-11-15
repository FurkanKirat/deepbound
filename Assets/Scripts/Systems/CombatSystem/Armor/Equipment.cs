using System.Collections.Generic;
using System.Linq;
using Core;
using Core.Events;
using Data.Models.Items;
using Interfaces;
using Systems.BuffSystem;
using Systems.InventorySystem;
using Systems.SaveSystem.SaveData.Inventory;
using Systems.StatSystem;
using Utils;

namespace Systems.CombatSystem.Armor
{
    public class Equipment : 
        IItemSlotCollection, 
        IStatProvider,
        IInitializable,
        IEffectProvider
    {
        private readonly IInventoryOwner _owner;
        private readonly SlotStorage _slotStorage;
        private readonly EquipmentSlot[] _slots;
        
        public SlotCollectionType Type => SlotCollectionType.Equipment;
        
        public IEnumerable<ItemInstance> AllItems
        {
            get
            {
                for (var i = 0; i < SlotCount; i++)
                    yield return _slotStorage.Slots[i].Item;
            }
        }
        public Equipment(IInventoryOwner owner)
        {
            _owner = owner;
            _slots = EnumUtils<EquipmentSlot>.AllValues.ToArray();
            _slotStorage = new SlotStorage(_slots.Length);
        }

        public Equipment(EquipmentSaveData data, IInventoryOwner owner)
        {
            _owner = owner;
            _slots = EnumUtils<EquipmentSlot>.AllValues.ToArray();
            _slotStorage = new SlotStorage(data.SlotStorage);
        }

        public void Initialize()
        {
            GameEventBus.Publish(new InventoryChangedEvent(_owner, this));
            GameEventBus.Publish(new StatProvidersChangedEvent(_owner));
        }

        public IReadOnlyList<InventorySlot> AllEquippedItems => _slotStorage.Slots;

        public bool IsEquipped(EquipmentSlot slot)
        {
            var index = _slots.IndexOf(slot);
            if (index < 0) 
                return false;
            return !_slotStorage.IsEmptyAt(index);
        }
        
        public ItemInstance GetItem(EquipmentSlot slot)
        {
            var index = _slots.IndexOf(slot);
            return _slotStorage.GetItemAt(index);
        }

        
        public int SlotCount => _slots.Length;
        public ItemInstance GetItem(int index)
        {
            var slot = _slots[index];
            return GetItem(slot);
        }

        public void SetItemAt(int index, ItemInstance item)
        {
            var slot = GetSlotFromIndex(index);

            if (item == null)
            {
                ClearItemAt(index);
                return;
            }

            var itemSlot = item.GetEquipmentSlot();
            if (!itemSlot.HasValue || itemSlot.Value != slot)
            {
                GameLogger.Warn($"Slot mismatch: Trying to set {itemSlot} to {slot}", nameof(Equipment));
                return;
            }
            
            _slotStorage.SetItemAt(index, item);

            GameEventBus.Publish(new InventorySlotChangedEvent(_owner, this, index, _slotStorage.GetItemAt(index)));
            GameEventBus.Publish(new StatProvidersChangedEvent(_owner));
        }

        public bool IsEmptyAt(int index) => _slotStorage.IsEmptyAt(index);

        public void ClearItemAt(int index)
        {
            if (_slotStorage.ClearSlot(index))
            {
                GameEventBus.Publish(new InventorySlotChangedEvent(_owner, this, index, _slotStorage.GetItemAt(index)));
                GameEventBus.Publish(new StatProvidersChangedEvent(_owner));
            }
        }

        public bool CanSetAt(int index, ItemInstance item)
        {
            var slot = item.GetEquipmentSlot();
     
            return (slot.HasValue && _slots[index] == slot.Value) || item.IsEmpty;
        }

        public bool AcceptItem(ItemInstance item)
        {
            var slot = item.GetEquipmentSlot();
            if (!slot.HasValue)
                return false;
            
            int index = slot.Value.ToIntSafe();
            if (!IsEmptyAt(index))
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

        private EquipmentSlot GetSlotFromIndex(int index) => _slots[index];

        public SlotCollectionSaveData ToSaveData() => new EquipmentSaveData
        {
            SlotStorage = _slotStorage.ToSaveData(),
            Type = Type
        };

        public IEnumerable<StatModifier> GetStatModifiers()
        {
            foreach (var slot in _slotStorage.Slots)
            {
                var armorData = slot.Item.GetArmorData();
                
                if (armorData?.Stats == null)
                    continue;
                foreach (var stat in armorData.Stats)
                    yield return stat;
            }
        }

        public IEnumerable<EffectData> GetActiveEffects()
        {
            foreach (var slot in _slotStorage.Slots)
            {
                var armorData = slot.Item.GetArmorData();
                
                if (armorData?.Effects == null)
                    continue;
                foreach (var effect in armorData.Effects)
                    yield return effect;
            }
        }
    }
}