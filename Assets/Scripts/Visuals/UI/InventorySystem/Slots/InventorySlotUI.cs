using Data.Models.Items;
using Systems.InventorySystem;
using UnityEngine;
using UnityEngine.UI;

namespace Visuals.UI.InventorySystem.Slots
{
    public class InventorySlotUI : CollectionSlotUI, IItemSlotUI
    {
        [SerializeField] private Image selectionOutline;
        
        private int _slotIndex;
        private IInventoryOwner _owner;
        private UIInventoryType _uiInventoryType;
        
        protected override UIInventoryType UIInventoryType => _uiInventoryType;
        protected override SlotCollectionType SlotCollectionType => SlotCollectionType.Inventory;
        protected override int Index => _slotIndex;
        protected override IInventoryOwner Owner => _owner;
        
        
        public override void Init(IInventoryOwner owner, int globalSlotIndex, UIInventoryType uiInventoryType)
        {
            _owner = owner;
            _slotIndex = globalSlotIndex;
            _uiInventoryType = uiInventoryType;
        }

        public override void UpdateSlot(ItemInstance item)
        {
            if (!item.IsEmpty)
            {
                SetItem(item, item.Count > 1 ? item.Count.ToString() : "");
            }
            else
            {
                ClearSlot();
            }
        }

        
        public override void UpdateSelection(bool isSelected)
        {
            if (selectionOutline.enabled == isSelected)
                return;
            selectionOutline.enabled = isSelected;
        }
    }

}