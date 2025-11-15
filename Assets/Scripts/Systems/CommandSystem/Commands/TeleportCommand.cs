using Core.Context;
using Data.Models;

namespace Systems.CommandSystem.Commands
{
    public class TeleportCommand : ICommand
    {
        public string Name => "tp";
        public string Description => "Teleports the user to the specified position";
        public string Usage => "/tp <x> <y>";

        private int _x;
        private int _y;
        
        public bool TryExecute(string[] args, ClientContext ctx, out string result)
        {
            if (args.Length < 2)
            {
                result = $"Usage: {Usage}";
                return false;
            }

            if (!int.TryParse(args[0], out _x))
            {
                result = "<x> must be a positive number";
                return false;
            }

            if (!int.TryParse(args[1], out _y))
            {
                result = "<y> must be a positive number";
                return false;
            }

            result = $"Teleporting {ctx.Player.Id} to ({_x},{_y}).";
            return true;
        }

        public void OnSuccess(string result, ClientContext ctx)
        {
            ctx.Player.Position = new WorldPosition(_x,_y);
        }

        public void OnFailure(string result, ClientContext ctx) { }
    }
}