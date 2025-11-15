using Core.Events;

namespace Visuals.UI.MainMenu
{
    public struct OpenMenuRequest : IEvent
    {
        public MenuPanelType PanelType { get; set; }

        public OpenMenuRequest(MenuPanelType panelType)
        {
            PanelType = panelType;
        }
    }

    public struct WorldItemClickedEvent : IEvent
    {
        public int Index { get; }

        public WorldItemClickedEvent(int index)
        {
            Index = index;
        }
    }
    
}