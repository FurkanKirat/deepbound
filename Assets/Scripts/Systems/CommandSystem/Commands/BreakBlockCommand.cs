using Core.Context;
using Data.Models;

namespace Systems.CommandSystem.Commands
{
    public class BreakBlockCommand : ICommand
    {
        public string Name => "breakblock";
        public string Description => "Breaks the block at the given position";
        public string Usage => "/breakblock <x> <y>";
        
        private int _x;
        private int _y;

        public bool TryExecute(string[] args, ClientContext ctx, out string result)
        {
            if (args.Length < 2)
            {
                result = $"Usage: {Usage}";
                return false;
            }
            
            if (!int.TryParse(args[0], out _x) || _x < 0)
            {
                result = "<x> must be a positive number.";
                return false;
            }
            
            if (!int.TryParse(args[1], out _y) || _y < 0)
            {
                result = "<y> must be a positive number.";
                return false;
            }
            
            if (!ctx.BlockManager.CanBreakAt(new TilePosition(_x, _y)))
            {
                result = $"Block does not exist at {_x},{_y}.";
                return false;
            }

            result = $"Removed block at ({_x}, {_y}).";
            return true;
        }

        public void OnSuccess(string result, ClientContext ctx)
        {
            var blockManager = ctx.BlockManager;
            blockManager.BreakBlock(new TilePosition(_x,_y));
        }

        public void OnFailure(string result, ClientContext ctx) { }
    }
}