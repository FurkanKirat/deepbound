using Data.Models;
using Data.Models.Blocks;
using Data.Models.Blocks.Subdata;

namespace Core.Context.Registry
{
    public class BlockBehaviorContext
    {
        public TilePosition Position { get; set; }
        public BlockData BlockData { get; set; }
        public string DimensionId { get; set; }
        public bool IsGenerated { get; set; }
    }
}