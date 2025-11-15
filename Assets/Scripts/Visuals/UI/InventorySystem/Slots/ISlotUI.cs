using UnityEngine.EventSystems;

namespace Visuals.UI.InventorySystem.Slots
{
    public interface ISlotUI<in T> : IPointerClickHandler
    {
        public void UpdateSlot(T item);
    }
}