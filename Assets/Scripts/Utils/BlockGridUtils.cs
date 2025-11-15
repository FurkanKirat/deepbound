using Data.Database;
using Data.Models.Blocks;
using Systems.WorldSystem;

namespace Utils
{
    public static class BlockGridUtils
    {
        public static Block GetBlock(this WorldGrid<Block> grid, int x, int y)
            => grid[x, y];

        public static bool TryGetBlock(this WorldGrid<Block> grid, int x, int y, out Block block)
            => grid.TryGet(x, y, out block);

        public static Block GetBlockBelowSafe(this WorldGrid<Block> grid, int x, int y)
        {
            if (grid.IsInBounds(x, y - 1))
                return grid[x, y - 1];
            return default;
        } 
        
        public static bool PlaceBlock(this WorldGrid<Block> grid, string id, int x, int y, bool replaceExisting = false)
        {
            var type = BlockIdCache.GetUshort(id);
            var blocks = Block.CreateMasterAndSlaves(type);

            foreach (var block in blocks)
            {
                int px = x + block.OffsetX;
                int py = y + block.OffsetY;

                if (!grid.IsInBounds(px,py) || (!GetBlock(grid,px, py).IsAir() && !replaceExisting))
                    return false;
            }
            
            foreach (var block in blocks)
            {
                grid[x + block.OffsetX, y + block.OffsetY] = block;
            }

            return true;
        }

        public static bool PlaceIfUnderSurface(this WorldGrid<Block> grid, string id, int x, int y,
            int[] surfaceYPerColumn, bool replaceExisting = false)
        {
            if (!grid.IsInBounds(x, y))
                return false;
            if (surfaceYPerColumn[x] <= y)
                return false;
            
            return PlaceBlock(grid, id, x, y, replaceExisting);
        }
        public static void ClearBlock(this WorldGrid<Block> grid, int x, int y) => grid[x,y] = default;
    }
}