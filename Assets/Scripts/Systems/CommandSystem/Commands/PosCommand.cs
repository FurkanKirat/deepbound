using Core.Context;
using Data.Models;

namespace Systems.CommandSystem.Commands
{
    public class PosCommand : ICommand 
    {
        public string Name => "pos";
        public string Description => "Displays your current position (X, Y).";
        public string Usage => "/pos";
        public bool TryExecute(string[] args, ClientContext ctx, out string result)
        {
            var pos = ctx.Player.Position;
            result =  $"You are at <b>{pos}</b>";
            return true;
        }

        public void OnSuccess(string result, ClientContext ctx) { }

        public void OnFailure(string result, ClientContext ctx) { }
    }
}