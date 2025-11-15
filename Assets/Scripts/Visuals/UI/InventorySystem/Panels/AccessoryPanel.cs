using Data.Database;
using Systems.InventorySystem;
using UnityEngine.UI;
using Visuals.UI.InventorySystem.Slots;

namespace Visuals.UI.InventorySystem.Panels
{
    public class AccessoryPanel : GridInventoryPanel<AccessorySlotUI>
    {
        protected override int SlotCount => Configs.GameConfig.Inventory.Player.AccessoryCount;
        protected override int SlotOffset => 0;
        protected override int SlotsPerRow => Configs.GameConfig.Inventory.Player.AccessoryCount;
        protected override UIInventoryType UIInventoryType => UIInventoryType.Accessory;
        protected override SlotCollectionType SlotCollectionType => SlotCollectionType.Accessory;
        protected override GridLayoutGroup.Constraint Constraint => GridLayoutGroup.Constraint.FixedRowCount;
        public override string OpenSound => null;
        public override string CloseSound => null;
        public override PanelType PanelType => PanelType.Accessory;
    }
}