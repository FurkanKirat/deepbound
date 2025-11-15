using Core.Context;
using Data.Models;
using Utils;

namespace Systems.CommandSystem.Commands
{
    public class LogCommand : ICommand
    {
        public string Name => "log";
        public string Description => "Logs a message to the console";
        public string Usage => "/log <message>";
        private string _msg;
        
        public bool TryExecute(string[] args, ClientContext ctx, out string result)
        {
            if (args.Length == 0)
            {
                result = $"Usage: {Usage}";
                return false;
            }

            _msg = string.Join(" ", args);
            result = $"Logged to console: {_msg}";
            return true;
        }

        public void OnSuccess(string result, ClientContext ctx)
        {
            GameLogger.Log($"[{ctx.Player.Id}] {_msg}",nameof(LogCommand));
        }

        public void OnFailure(string result, ClientContext ctx) { }
    }
}