using System;
using Core.Context;
using Core.Events;
using Data.RegistrySystem;

namespace Core
{
    public static class ChatHandler
    {
        public static void ProcessInput(string input, ClientContext ctx)
        {
            ChatEvent evt;
            
            if (string.IsNullOrWhiteSpace(input))
                return;
            
            if (input.StartsWith("/"))
            {
                evt = new ChatEvent
                {
                    RawInput = input,
                    Type = ChatEventType.Command,
                    SenderId = $"Player_{ctx.Player.Id}",
                    Timestamp = DateTime.Now
                };
                
                GameEventBus.Publish(evt);
                CommandRegistry.Execute(input[1..], ctx);
            }
            else
            {
                evt = new ChatEvent
                {
                    RawInput = input,
                    Message = input,
                    Type = ChatEventType.PlayerMessage,
                    SenderId = $"Player_{ctx.Player.Id}",
                    Timestamp = DateTime.Now
                };
                GameEventBus.Publish(evt);
            }
        }
    }
}