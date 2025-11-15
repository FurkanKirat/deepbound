using Data.Models.Crafting;

namespace Visuals.UI.CraftingSystem
{
    public class FurnacePanel : BaseCraftingPanel
    {
        public override PanelType PanelType => PanelType.Furnace;
        public override CraftingStation Station => CraftingStation.Furnace;
    }
}