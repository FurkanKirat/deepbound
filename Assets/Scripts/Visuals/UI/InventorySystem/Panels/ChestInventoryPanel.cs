using Data.Database;
using Generated.Resource;
using Systems.InventorySystem;
using Visuals.UI.InventorySystem.Slots;

namespace Visuals.UI.InventorySystem.Panels
{
    public class ChestInventoryPanel : GridInventoryPanel<IItemSlotUI>
    {
        protected override int SlotCount => Configs.GameConfig.Inventory.Chest.SlotCount;
        protected override int SlotOffset => 0;
        protected override int SlotsPerRow => Configs.GameConfig.Inventory.Chest.SlotsPerRow;
        protected override UIInventoryType UIInventoryType => UIInventoryType.Chest;
        protected override SlotCollectionType SlotCollectionType => SlotCollectionType.Inventory;
        
        public override string OpenSound => AudioIds.OpeningChest;
        public override string CloseSound => AudioIds.ClosingChest;
        public override PanelType PanelType => PanelType.Chest;
    }
}