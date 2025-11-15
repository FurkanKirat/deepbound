using Data.Database;

namespace Data.Models.Blocks
{
    public static class BlockExtensions
    {
        public static BlockData GetBlockData(this Block block)
            => BlockIdCache.GetBlockData(block.BlockType);
        
        public static bool IsSolid(this Block block)
            => block.GetBlockData().IsSolid;
        
        public static string Id(this Block block)
            => block.GetBlockData().Id;

        public static bool IsAir(this Block block)
            => block.BlockType == BlockIdCache.AirId;
    }
}