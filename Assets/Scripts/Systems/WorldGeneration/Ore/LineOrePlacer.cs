using Core.Context;
using Data.Models.Generation;
using Utils;

namespace Systems.WorldGeneration.Ore
{
    public class LineOrePlacer : IOrePlacer
    {
        public void Place(int baseX, int baseY, int veinSize, MapGenerationContext context, OreGenerationData oreGenData)
        {
            var random = context.Random;
            var dir = RandomUtils.GetRandomDirection(context.Random);
            
            int x = baseX;
            int y = baseY;
            for (int i = 0; i < veinSize; i++)
            {
                context.Blocks.PlaceIfUnderSurface(oreGenData.BlockId, x, y, context.SurfaceYPerColumn);
                
                if (random.NextDouble() < 0.5)
                {
                    if (dir.xOffset == 0)
                        x += random.Next(-1, 2);
                    else
                        y += random.Next(-1, 2);
                }
                
                x += dir.xOffset;
                y += dir.yOffset;
            }
        }
    }
}