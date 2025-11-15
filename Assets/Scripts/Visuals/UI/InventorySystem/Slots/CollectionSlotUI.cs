using Core;
using Core.Events;
using Data.Models.Items;
using Systems.InputSystem;
using Systems.InventorySystem;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Visuals.UI.InventorySystem.Slots
{
    public abstract class CollectionSlotUI : 
        BaseSlotUI<ItemInstance>, 
        IItemSlotUI,
        IPointerEnterHandler, 
        IPointerExitHandler
    {
        [SerializeField] private Image background;
        [SerializeField] private Image hoverOverlay;
        protected abstract UIInventoryType UIInventoryType { get; }
        protected abstract SlotCollectionType SlotCollectionType { get; }
        protected abstract int Index { get; }
        protected abstract IInventoryOwner Owner { get; }

        private bool HoverEnabled
        {
            get => hoverOverlay.enabled;
            set
            {
                if (hoverOverlay.enabled != value)
                    hoverOverlay.enabled = value;
            } 
        }
        
        private void OnDisable()
        {
            HoverEnabled = false;
        }
        
        public override void OnPointerClick(PointerEventData eventData)
        {
            var clickType = SlotClickHelper.GetSlotClickTypeFromInput(eventData);
            if (clickType == SlotClickType.None)
                return;
            GameEventBus.Publish(
                new ItemSlotClickedEvent(
                    Owner,
                    Index,
                    UIInventoryType,
                    SlotCollectionType,
                    clickType
                )
            );
        }
        
        private void OnItemSlotKeyPressed(ItemSlotKeyPressedEvent e)
        {
            GameEventBus.Publish(new SwapSlotsRequest(
                e.Owner.InventoryManager.GetInventory(e.SlotCollectionType), 
                e.Index,
                Owner.InventoryManager.GetInventory(SlotCollectionType),
                Index
                ));
        }
        
        public abstract void Init(IInventoryOwner owner, int globalSlotIndex, UIInventoryType uiInventoryType);

        public virtual void UpdateSelection(bool isSelected) {}
        
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            HoverEnabled = true;
            GameEventBus.Subscribe<ItemSlotKeyPressedEvent>(OnItemSlotKeyPressed);
            var item = Owner.InventoryManager.GetInventory(SlotCollectionType).GetItem(Index);
            if (item.IsEmpty)
                return;
            
            GameEventBus.Publish(new ItemSlotHoveredEvent(Owner, item, transform.position));
        }
        
        public void OnPointerExit(PointerEventData eventData)
        {
            HoverEnabled = false;
            GameEventBus.Publish(new ItemSlotHoverEndedEvent());
            GameEventBus.Unsubscribe<ItemSlotKeyPressedEvent>(OnItemSlotKeyPressed);
        }
    }
}