namespace Visuals.UI.ChatSystem
{
    public interface IChatUIState
    {
        bool IsOpen { get; }
        void Open();
        void SubmitMessage();
    }

}