using System;
using Core;
using Core.Context;
using Core.Events;
using Data.Models;

namespace Systems.CommandSystem.Commands
{
    public class SaveCommand : ICommand
    {
        public string Name => "save";
        public string Description => "Saves the game.";
        public string Usage => "/save";
        
        public bool TryExecute(string[] args, ClientContext ctx, out string result)
        {
            GameEventBus.Publish(new SaveWorldRequest(true));
            result = null;
            return true;
        }

        public void OnSuccess(string result, ClientContext ctx)
        {
            GameEventBus.Publish(new ChatEvent
            {
                SenderId = "System",
                Message = "Saved Game Successfully!",
                Type = ChatEventType.SystemMessage,
                Timestamp = DateTime.Now
            });
        }

        public void OnFailure(string result, ClientContext ctx) { }
    }
}