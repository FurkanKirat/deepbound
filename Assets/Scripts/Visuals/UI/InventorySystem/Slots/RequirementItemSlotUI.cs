using Data.Models.Items;
using Systems.InventorySystem;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Visuals.UI.InventorySystem.Slots
{
    public class RequirementItemSlotUI : BaseSlotUI<ItemAmount>
    {
        [SerializeField] private Image blurImage;
        public ItemAmount Item { get; private set; }
        
        public bool HasItem
        {
            get => !blurImage.gameObject.activeSelf;
            set
            {
                bool shouldBeActive = !value;
                if (blurImage.gameObject.activeSelf != shouldBeActive)
                    blurImage.gameObject.SetActive(shouldBeActive);
            }
        }
        public override void UpdateSlot(ItemAmount item)
        {
            Item = item;
            if (item != null && !item.IsEmpty)
            {
                SetItem(item, item.FormatItemCount());
            }
            else
            {
                ClearSlot();
            }
        }
        
        public override void OnPointerClick(PointerEventData eventData){}
    }
}