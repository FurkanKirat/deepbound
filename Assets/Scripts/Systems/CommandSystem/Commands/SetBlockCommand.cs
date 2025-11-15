using Core.Context;
using Data.Database;
using Data.Models;

namespace Systems.CommandSystem.Commands
{
    public class SetBlockCommand : ICommand
    {
        public string Name => "setblock";
        public string Description => "Sets the block at the given position";
        public string Usage => "/setblock <x> <y> <blockId>";
        private string _blockId;
        private int _x;
        private int _y;

        public bool TryExecute(string[] args, ClientContext ctx, out string result)
        {
            if (args.Length < 3)
            {
                result = $"Usage: {Usage}";
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
            
            _blockId = args[2];

            if (!Databases.Blocks.Exists(_blockId))
            {
                result = $"Block {_blockId} not found";
                return false;
            }

            result = $"Block {_blockId} has been placed to {_x},{_y}";
            return true;
        }

        public void OnSuccess(string result, ClientContext ctx)
        {
            var blockManager = ctx.BlockManager;
            var tilePos = new TilePosition(_x, _y);
            blockManager.PlaceBlockAt(tilePos, _blockId);
        }

        public void OnFailure(string result, ClientContext ctx) { }
    }
}