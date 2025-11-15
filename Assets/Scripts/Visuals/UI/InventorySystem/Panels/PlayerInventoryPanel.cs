using Data.Database;
using Systems.InventorySystem;
using UnityEngine.UI;
using Visuals.UI.InventorySystem.Slots;

namespace Visuals.UI.InventorySystem.Panels
{
    public class PlayerInventoryPanel : GridInventoryPanel<IItemSlotUI>
    {
        protected override int SlotCount => Configs.GameConfig.Inventory.Player.MainInventorySlotCount;
        protected override int SlotOffset => 0;
        protected override int SlotsPerRow => Configs.GameConfig.Inventory.Player.SlotsPerRow;
        protected override GridLayoutGroup.Corner StartCorner => GridLayoutGroup.Corner.LowerLeft;
        protected override UIInventoryType UIInventoryType => UIInventoryType.Player;
        protected override SlotCollectionType SlotCollectionType => SlotCollectionType.Inventory;
        public override string OpenSound => null;
        public override string CloseSound => null;
        public override PanelType PanelType => PanelType.PlayerInventory;
    }
}