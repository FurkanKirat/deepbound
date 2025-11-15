using Core;

namespace Utils
{
    using Core.Events;
    using System;

    public static class CommandMessenger
    {
        public static void SendSystemMessage(string message)
        {
            GameEventBus.Publish(new ChatEvent
            {
                SenderId = "System",
                Message = message,
                Type = ChatEventType.SystemMessage,
                Timestamp = DateTime.Now
            });
        }

        public static void SendErrorMessage(string message)
        {
            GameEventBus.Publish(new ChatEvent
            {
                SenderId = "System",
                Message = message,
                Type = ChatEventType.Error,
                Timestamp = DateTime.Now
            });
        }
    }

}