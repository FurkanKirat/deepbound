using Systems.InventorySystem;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Systems.InputSystem
{
    public static class SlotClickHelper
    {
        public static SlotClickType GetSlotClickTypeFromInput(PointerEventData eventData)
        {
            return eventData.button switch
            {
                PointerEventData.InputButton.Left => Input.GetKey(KeyCode.LeftShift)
                    ? SlotClickType.ShiftLeftClick
                    : SlotClickType.LeftClick,
                
                PointerEventData.InputButton.Right => SlotClickType.RightClick,
                _ => SlotClickType.None
            };
        }

    }
}