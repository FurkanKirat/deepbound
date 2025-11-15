using System;
using Core.Context;
using Data.Models.Generation;
using Utils;

namespace Systems.WorldGeneration.Ore
{
    public class EllipseOrePlacer : IOrePlacer
    {
        public void Place(int baseX, int baseY, int veinSize, MapGenerationContext context, OreGenerationData oreGenData)
        {
            int radiusX = (int)Math.Max(1, Math.Sqrt(veinSize));
            int radiusY = (int)Math.Max(1, veinSize / (float)radiusX);

            foreach (var point in ShapeHelper.GetEllipse(baseX, baseY, radiusX, radiusY))
            {
                context.Blocks.PlaceIfUnderSurface(oreGenData.BlockId, point.x, point.y, context.SurfaceYPerColumn);
            }
        }
    }
}