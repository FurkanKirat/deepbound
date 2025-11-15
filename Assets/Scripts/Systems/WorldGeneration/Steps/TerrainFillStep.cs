using Core.Context;
using Utils;

namespace Systems.WorldGeneration.Steps
{
    public class TerrainFillStep : IMapGenerationStep
    {
        public void Apply(MapGenerationContext context)
        {
            var blocks = context.Blocks;
            for (int x = 0; x < context.Width; x++)
            {
                int surfaceY = context.SurfaceYPerColumn[x];

                for (int y = 0; y < context.Height; y++)
                {
                    if (y > surfaceY)
                    {
                        context.Blocks.ClearBlock(x, y);
                    }
                    else
                    {
                        if (y == surfaceY)
                        {
                            blocks.PlaceBlock(context.DimensionGenerationData.SurfaceBlock, x, y);
                        }
                        else if (y > surfaceY - 5)
                        {
                            blocks.PlaceBlock(context.DimensionGenerationData.BaseBlock, x, y);
                        }
                        
                        else if (context.CaveMap != null && context.CaveMap[x, y])
                        {
                            blocks.ClearBlock(x, y);
                        }
                        else
                        {
                            blocks.PlaceBlock(context.DimensionGenerationData.StoneBlock, x, y);
                        }
                    }
                }
            }
        }
    }
}