using Data.Models.Items;
using Systems.InventorySystem;

namespace Visuals.UI.InventorySystem.Slots
{
    public interface IItemSlotUI : ISlotUI<ItemInstance>
    {
        void Init(IInventoryOwner owner, int globalSlotIndex, UIInventoryType uiInventoryType);
        void UpdateSelection(bool isSelected);
    }
}