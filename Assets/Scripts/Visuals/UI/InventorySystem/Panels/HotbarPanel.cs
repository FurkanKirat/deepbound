using Data.Database;
using Systems.InventorySystem;
using Visuals.UI.InventorySystem.Slots; 

namespace Visuals.UI.InventorySystem.Panels
{
    public class HotbarPanel : GridInventoryPanel<IItemSlotUI>
    {
        protected override int SlotCount => Configs.GameConfig.Inventory.Player.HotbarSlotCount;
        protected override int SlotOffset => Configs.GameConfig.Inventory.Player.HotbarStartIndex;
        protected override int SlotsPerRow => Configs.GameConfig.Inventory.Player.SlotsPerRow;
        protected override UIInventoryType UIInventoryType => UIInventoryType.Hotbar;
        protected override SlotCollectionType SlotCollectionType => SlotCollectionType.Inventory;
        public override string OpenSound => null;
        public override string CloseSound => null;
        public override PanelType PanelType => PanelType.Hotbar;
    }

}