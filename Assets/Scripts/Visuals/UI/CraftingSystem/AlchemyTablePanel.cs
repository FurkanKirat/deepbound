using Data.Models.Crafting;

namespace Visuals.UI.CraftingSystem
{
    public class AlchemyTablePanel : BaseCraftingPanel
    {
        public override PanelType PanelType => PanelType.AlchemyTable;
        public override CraftingStation Station => CraftingStation.AlchemyTable;
    }
}