using Core;
using Core.Events;
using Data.Database;
using Systems.SaveSystem.SaveData.Inventory;

namespace Systems.InventorySystem
{
    public class PlayerInventory : Inventory
    {
        public PlayerInventory(int size, IInventoryOwner owner) : base(size, owner)
        {
            GameEventBus.Subscribe<SelectedSlotChangeRequest>(OnSelectedSlotChangeRequest);
            SelectSlot(Configs.GameConfig.Inventory.Player.HotbarStartIndex);
        }

        public PlayerInventory(InventorySaveData data, IInventoryOwner owner) : base(data, owner)
        {
            GameEventBus.Subscribe<SelectedSlotChangeRequest>(OnSelectedSlotChangeRequest);
            SelectSlot(data.SelectedSlot);
        }

        public override void Dispose()
        {
            base.Dispose();
            GameEventBus.Unsubscribe<SelectedSlotChangeRequest>(OnSelectedSlotChangeRequest);
        }
        
        private void OnSelectedSlotChangeRequest(SelectedSlotChangeRequest e)
        {
            if(e.Owner != Owner) return;
            SelectSlot(e.NewSlotIndex);
        }
    }
}