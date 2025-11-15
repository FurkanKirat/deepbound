using Data.Models.Items;
using Systems.InventorySystem;

namespace Visuals.UI.InventorySystem.Slots
{
    public class AccessorySlotUI : CollectionSlotUI, IItemSlotUI
    {
        private int _slotIndex;
        private IInventoryOwner _owner;
        

        public override void Init(IInventoryOwner owner, int globalSlotIndex, UIInventoryType uiInventoryType)
        {
            _owner = owner;
            _slotIndex = globalSlotIndex;
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

        protected override UIInventoryType UIInventoryType => UIInventoryType.Accessory;
        protected override SlotCollectionType SlotCollectionType => SlotCollectionType.Accessory;
        protected override int Index => _slotIndex;
        protected override IInventoryOwner Owner => _owner;
    }
}