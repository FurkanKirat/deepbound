namespace Visuals.UI
{
    public interface IUIPanelController
    {
        bool IsOpen { get; }
        void Open();
        void Close();
        void Toggle();
        bool BlocksWorldInteraction { get; }
        bool PausesGame { get; }
        UIPanelType PanelType { get; }
    }


}