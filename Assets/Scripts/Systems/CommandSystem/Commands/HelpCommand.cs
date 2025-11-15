using System;
using System.Text;
using Core;
using Core.Context;
using Core.Events;
using Data.Models;
using Data.RegistrySystem;

namespace Systems.CommandSystem.Commands
{
    public class HelpCommand : ICommand
    {
        public string Name => "help";
        public string Description => "Lists all available commands.";
        public string Usage => "/help";
        
        private string _message;
        public bool TryExecute(string[] args, ClientContext context, out string result)
        {
            result = "";
            _message = "";
            
            if (args.Length == 1)
            {
                var command = CommandRegistry.Get(args[0]);
                if (command == null)
                {
                    result = $"Unknown command: /{args[0]}";
                    return false;
                }

                _message = $"<b>/{command.Name}</b>\n{command.Description}";
                result = $"Help for /{command.Name}";
                return true;
            }
            
            var sb = new StringBuilder();
            sb.AppendLine("<b>Available Commands:</b>");
            foreach (var cmd in CommandRegistry.GetAll())
            {
                sb.AppendLine($"<b>/{cmd.Name}</b> - {cmd.Usage} - {cmd.Description}");
            }

            _message = sb.ToString();
            result = "Listed all commands.";
            return true;
        }
        
        public void OnSuccess(string result, ClientContext ctx)
        {
            GameEventBus.Publish(new ChatEvent
            {
                SenderId = "System",
                Message = _message,
                Type = ChatEventType.SystemMessage,
                Timestamp = DateTime.Now
            });
        }

        public void OnFailure(string result, ClientContext ctx) { }
    }
}