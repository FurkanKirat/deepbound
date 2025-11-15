using Visuals.UI.ErrorSystem;

namespace Core.Events
{
    public struct OpenErrorPanelRequest : IEvent
    {
        public string Text { get; }
        public string Title { get; }
        public ButtonEventData[] ButtonEvents { get; }

        public OpenErrorPanelRequest(string text, string title, ButtonEventData[] buttonEvents)
        {
            Text = text;
            Title = title;
            ButtonEvents = buttonEvents;
        }
    }
}