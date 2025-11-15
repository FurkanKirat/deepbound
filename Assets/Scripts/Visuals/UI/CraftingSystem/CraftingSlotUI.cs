using Core;
using Core.Events;
using Data.Models.Crafting;
using Data.Models.Items;
using Systems.EntitySystem.Interfaces;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Visuals.UI.InventorySystem.Slots;

namespace Visuals.UI.CraftingSystem
{
    public class CraftingSlotUI : BaseSlotUI<CraftingRecipe>
    {
        [SerializeField] private Image blurImage;
        
        public CraftingRecipe Recipe { get; private set; }
        private IPlayer Player { get; set; }
        
        public void UpdateUser(IPlayer player) => Player = player;
        
        public bool CanCraft
        {
            get => !blurImage.gameObject.activeSelf;
            set
            {
                bool shouldBeActive = !value;
                if (blurImage.gameObject.activeSelf != shouldBeActive)
                    blurImage.gameObject.SetActive(shouldBeActive);
            }
        }

        public override void UpdateSlot(CraftingRecipe recipe)
        {
            Recipe = recipe;
            var item = recipe?.Output;
            if (item != null && !item.IsEmpty)
            {
                SetItem(item, item.FormatItemCount());
            }
            else
            {
                ClearSlot();
            }
        }

        public override void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left)
                return;
            
            GameEventBus.Publish(new CraftingSlotClickedEvent(Recipe, Player));
        }
    }
}