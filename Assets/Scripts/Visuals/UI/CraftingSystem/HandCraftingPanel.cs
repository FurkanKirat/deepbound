using Data.Models.Crafting;

namespace Visuals.UI.CraftingSystem
{
    public class HandCraftingPanel : BaseCraftingPanel
    {
        public override PanelType PanelType => PanelType.HandCrafting;
        public override CraftingStation Station => CraftingStation.Hand;
    }
}