using Core.Events;

namespace Utils
{
    public static class ChatFormatter
    {
        // Hex color codes
        private const string Gray = "#808080";
        private const string Yellow = "#FFFF00";
        private const string Red = "#FF0000";

        public static string Format(ChatEvent evt)
        {
            return evt.Type switch
            {
                ChatEventType.PlayerMessage => $"<b>{evt.SenderId}</b>: {evt.Message}",
                ChatEventType.SystemMessage => $"<color={Gray}>[System]</color> {evt.Message}",
                ChatEventType.Warning => $"<color={Yellow}>[Warning]</color> {evt.Message}",
                ChatEventType.Error => $"<color={Red}>[Error]</color> {evt.Message}",
                _ => evt.RawInput
            };
        }
    }
}