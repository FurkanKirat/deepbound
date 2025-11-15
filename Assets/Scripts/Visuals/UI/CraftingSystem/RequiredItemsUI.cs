using Core.Context;
using Data.Models.Items;
using Interfaces;
using Systems.EntitySystem.Interfaces;
using Systems.InventorySystem;
using Visuals.UI.InventorySystem.Slots;

namespace Visuals.UI.CraftingSystem
{
    public class RequiredItemsUI : 
        UIGrid<RequirementItemSlotUI, ItemAmount>,
        IInitializable<ClientContext>
    {
        private IPlayer _player;
        protected override void SetupAction(RequirementItemSlotUI item, ItemAmount data)
        {
            item.UpdateSlot(data);
            var inv = _player.InventoryManager.GetInventory<PlayerInventory>(SlotCollectionType.Inventory);
            item.HasItem = inv.HasItem(data.Id, data.Count);
        }

        public void Initialize(ClientContext data)
        {
            _player = data.Player;
        }
    }
}