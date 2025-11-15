using Data.Models.Items;
using Localization;
using Systems.CombatSystem.Armor;
using Systems.InventorySystem;
using TMPro;
using UnityEngine;
using Utils;

namespace Visuals.UI.InventorySystem.Slots
{
    public class EquipmentSlotUI : CollectionSlotUI, ILocalizable
    {
        [SerializeField] private TMP_Text slotLabel;

        private IInventoryOwner _owner;
        private EquipmentSlot _slot;
        
        protected override UIInventoryType UIInventoryType => UIInventoryType.Equipment;
        protected override SlotCollectionType SlotCollectionType => SlotCollectionType.Equipment;
        protected override int Index => _slot.ToIntSafe();
        protected override IInventoryOwner Owner => _owner;

        public override void Init(IInventoryOwner owner, int globalSlotIndex, UIInventoryType uiInventoryType)
        {
            _owner = owner;
            _slot = (EquipmentSlot)globalSlotIndex;
            
            Localize();
        }

        public override void UpdateSlot(ItemInstance item)
        {
            if (item != null && !item.IsEmpty)
            {
                SetItem(item);
            }
            else
            {
                ClearSlot();
            }
        }


        public void Localize()
        {
            if (slotLabel != null)
                slotLabel.text = LocalizationDatabase.Get($"equipment.{_slot.ToJsonString()}");
        }
    }
}