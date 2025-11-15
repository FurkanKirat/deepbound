namespace Visuals.UI.CraftingSystem
{
    public class CraftingPanelUIController : BasePanelUIController
    {
        public override bool BlocksWorldInteraction => false;
        public override bool PausesGame => true;
        public override UIPanelType PanelType => UIPanelType.Crafting;
        protected override string OpenSound => null;
        protected override string CloseSound => null;
    }
}

