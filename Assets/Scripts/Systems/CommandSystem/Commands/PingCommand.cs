using Core.Context;
using Data.Models;

namespace Systems.CommandSystem.Commands
{
    public class PingCommand : ICommand
    {
        public string Name => "ping";
        public string Description => "Tests if the command system is working.";
        public string Usage => "/ping";
        public bool TryExecute(string[] args, ClientContext ctx, out string result)
        {
            result = "pong!";
            return true;
        }

        public void OnSuccess(string result, ClientContext ctx) { }

        public void OnFailure(string result, ClientContext ctx) { }
    }
}