using Core.Context;
using Data.Models.Generation;
using Generated.Ids;
using Utils;

namespace Systems.WorldGeneration.Structure
{
    public class TreePlacer : IStructurePlacer
    {
        public bool CanPlace(int baseX, int baseY, StructureGenerationData data, MapGenerationContext context)
            => true;

        public void PlaceStructure(int baseX, int baseY, StructureGenerationData data, MapGenerationContext context)
        {
            var random = context.Random;
            var blocks = context.Blocks;
            int x = baseX;
            int y = baseY;

            var height = random.Next(4, 9);
            for (int i = 0; i < height; i++)
            {
                // Main body
                blocks.PlaceBlock(BlockIds.Wood, x, y, true);

                // Side body extension (thickness)
                if (random.NextDouble() < 0.15)
                {
                    int dir = RandomUtils.GetRandomSign(random);
                    blocks.PlaceBlock(BlockIds.Wood, x + dir, y);
                }

                // Branches after the intermediate level
                if (i > height / 2 && random.NextDouble() < 0.25)
                {
                    int dir = RandomUtils.GetRandomSign(random);
                    int branchX = x + dir;
                    int branchY = y;
                    blocks.PlaceBlock(BlockIds.Wood, branchX, branchY);
                    blocks.PlaceBlock(BlockIds.Leaf, branchX + dir, branchY);
                    blocks.PlaceBlock(BlockIds.Leaf, branchX, branchY + 1);
                }

                // Slight curl
                if (random.NextDouble() < 0.3)
                {
                    x += random.Next(-1, 2);
                }

                y++;
            }

            const double threshold = 2.2;
            const double thresholdSquared = threshold * threshold;
            
            // Cluster of peak leaves
            for (int dx = -2; dx <= 2; dx++)
            for (int dy = -2; dy <= 2; dy++)
            {
                double distSqr = dx * dx + dy * dy;
                if (distSqr <= thresholdSquared && random.NextDouble() < 0.8) // 80%
                {
                    blocks.PlaceBlock(BlockIds.Leaf, x + dx, y + dy);
                }
            }
        }
    }
}