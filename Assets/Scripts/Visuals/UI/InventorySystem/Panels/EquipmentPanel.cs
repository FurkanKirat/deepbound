using Systems.CombatSystem.Armor;
using Systems.InventorySystem;
using UnityEngine.UI;
using Utils;
using Visuals.UI.InventorySystem.Slots;

namespace Visuals.UI.InventorySystem.Panels
{
    public class EquipmentPanel : GridInventoryPanel<EquipmentSlotUI>
    {
        protected override int SlotCount => EnumUtils<EquipmentSlot>.AllValues.Count;
        protected override int SlotOffset => 0;
        protected override int SlotsPerRow => EnumUtils<EquipmentSlot>.AllValues.Count;
        protected override UIInventoryType UIInventoryType => UIInventoryType.Equipment;
        protected override SlotCollectionType SlotCollectionType => SlotCollectionType.Equipment;
        protected override GridLayoutGroup.Constraint Constraint => GridLayoutGroup.Constraint.FixedRowCount;
        public override string OpenSound => null;
        public override string CloseSound => null;
        public override PanelType PanelType => PanelType.Equipment;
    }
}

