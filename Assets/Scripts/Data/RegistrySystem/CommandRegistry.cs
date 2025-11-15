using System;
using System.Collections.Generic;
using System.Linq;
using Core;
using Core.Context;
using Core.Events;
using Data.Models;
using Systems.CommandSystem;
using Utils;

namespace Data.RegistrySystem
{
    public static class CommandRegistry
    {
        private static readonly Dictionary<string, ICommand> Commands = new();

        public static void Register(ICommand command)
        {
            Commands[command.Name.ToLower()] = command;
        }
        
        public static ICommand Get(string key) => Commands[key.ToLower()];

        public static bool Execute(string input, ClientContext context)
        {
            if (string.IsNullOrWhiteSpace(input)) return false;

            var parts = input.Split(' ');
            var name = parts[0].ToLower();
            var args = parts.Skip(1).ToArray();

            if (Commands.TryGetValue(name, out var command))
            {
                if (command.TryExecute(args, context, out var result))
                {
                    command.OnSuccess(result, context);
                    CommandMessenger.SendSystemMessage(result);
                    return true;
                }
                else
                {
                    command.OnFailure(result, context);
                    CommandMessenger.SendErrorMessage(result);
                    return false;
                }
            }

            GameEventBus.Publish(new ChatEvent
            {
                SenderId = "System",
                Message = $"Unknown command: /{name}",
                Type = ChatEventType.Error,
                Timestamp = DateTime.Now
            });

            return false;
        }

        public static IEnumerable<ICommand> GetAll() => Commands.Values;
    }

}