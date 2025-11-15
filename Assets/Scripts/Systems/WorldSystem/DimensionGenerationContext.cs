using Data.Models;
using Data.Models.Blocks;

namespace Systems.WorldSystem
{
    public class DimensionGenerationContext
    {
        public string DimensionId { get; set; }
        public WorldPosition PlayerSpawn { get; set; }
        public WorldGrid<Block> Blocks { get; set; }
        public int[] SurfaceYPerX { get; set; }
        
        public World World { get; set; }
    }
}