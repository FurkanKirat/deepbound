using Data.Models.Crafting;

namespace Visuals.UI.CraftingSystem
{
    public class WorkbenchPanel : BaseCraftingPanel
    {
        public override PanelType PanelType => PanelType.Workbench;
        public override CraftingStation Station => CraftingStation.Workbench;
    }
}