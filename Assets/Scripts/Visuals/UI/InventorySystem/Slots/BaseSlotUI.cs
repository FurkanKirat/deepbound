using Data.Models.Items;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Visuals.UI.InventorySystem.Slots
{
    public abstract class BaseSlotUI<T> : MonoBehaviour, ISlotUI<T>
    {
        [SerializeField] protected Image itemImage;
        [SerializeField] protected TMP_Text countText;

        public abstract void UpdateSlot(T item);

        public abstract void OnPointerClick(PointerEventData eventData);

        protected void ClearSlot()
        {
            if (itemImage != null) itemImage.enabled = false;
            if (countText != null) countText.text = "";
        }

        protected void SetItem(IItem item, string count = "")
        {
            
            if (itemImage != null && !item.IsEmpty)
            {
                var sprite = item.GetSprite();
                itemImage.sprite = sprite;
                itemImage.enabled = sprite != null;
            }
            if (countText != null)
            {
                countText.text = count;
            }
        }
    }
}