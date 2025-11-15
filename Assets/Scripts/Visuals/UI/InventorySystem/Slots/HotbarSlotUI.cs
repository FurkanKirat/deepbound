using Core;
using Core.Events;
using Data.Models.Items;
using Systems.InventorySystem;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Visuals.UI.InventorySystem.Slots
{
    public class HotbarSlotUI : BaseSlotUI<ItemInstance>, IItemSlotUI
    {
        [SerializeField] private Image selectionOutline;
        
        private int _slotIndex;
        private IInventoryOwner _owner;
        
        public void Init(IInventoryOwner owner, int globalSlotIndex, UIInventoryType uiInventoryType)
        {
            _owner = owner;
            _slotIndex = globalSlotIndex;
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
        public void UpdateSelection(bool isSelected)
        {
            if (selectionOutline.enabled == isSelected)
                return;
            selectionOutline.enabled = isSelected;
        }
        
        public override void OnPointerClick(PointerEventData eventData)
        {
            GameEventBus.Publish(new SelectedSlotChangeRequest(_slotIndex, _owner));
        }

    }
}