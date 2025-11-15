using System.Linq;
using Core.Context;
using Data.Models.Blocks;
using Data.Models.Generation;
using Generated.Ids;
using Utils;
using Utils.Extensions;

namespace Systems.WorldGeneration.Structure
{
    public class CavePocketPlacer : IStructurePlacer
    {
        private int _width;
        private int _height;
        public bool CanPlace(int baseX, int baseY, StructureGenerationData data, MapGenerationContext context)
        {
            var random = context.Random;
            _width = random.Next(5, 8);
            _height = random.Next(5, 8);
            return baseX - _width >= 0 &&
                   baseY - _width >= 0 &&
                   baseX + _height < context.Width &&
                   baseY + _height < context.Height;
        }

        public void PlaceStructure(int baseX, int baseY, StructureGenerationData data, MapGenerationContext context)
        {
            var random = context.Random;
            
            var ellipsePoints = ShapeHelper.GetEllipse(baseX, baseY, _width, _height).ToArray();

            foreach (var point in ellipsePoints)
            {
                context.Blocks.PlaceIfUnderSurface(BlockIds.Air, point.x, point.y, context.SurfaceYPerColumn, true);
            }

            var possibleSpots = ellipsePoints
                .Where(p => context.Blocks.GetBlockBelowSafe(p.x, p.y).IsSolid()) 
                .ToList();

            if (possibleSpots.Count > 0 && random.NextDouble() < 0.1)
            {
                var chestSpot = possibleSpots[random.Next(possibleSpots.Count)];
                context.Blocks.PlaceIfUnderSurface(BlockIds.Chest, chestSpot.x, chestSpot.y, context.SurfaceYPerColumn, true);
            }
        }
    }
}