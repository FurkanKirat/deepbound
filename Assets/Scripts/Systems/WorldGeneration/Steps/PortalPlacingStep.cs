using Core.Context;
using Data.Models.Blocks;
using Generated.Ids;
using Utils;
using Utils.Extensions;

namespace Systems.WorldGeneration.Steps
{
    public class PortalPlacingStep : IMapGenerationStep
    {
        public void Apply(MapGenerationContext context)
        {
            for (int x = 0; x < context.Width; x++)
            {
                for (int y = 0; y < context.Height; y++)
                {
                    var blockId = context.Blocks.GetBlock(x, y).Id();
                    if (blockId is BlockIds.Air &&
                        context.Random.NextDouble() < 0.001f)
                    {
                        context.Blocks.PlaceBlock(BlockIds.Portal, x, y);
                    }
                }
            }
            
        }
    }

}