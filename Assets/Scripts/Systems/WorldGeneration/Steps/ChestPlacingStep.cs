using Core.Context;
using Data.Models.Blocks;
using Generated.Ids;
using Utils;
using Utils.Extensions;

namespace Systems.WorldGeneration.Steps
{
    public class ChestPlacingStep : IMapGenerationStep
    {
        private readonly float _density;
        private readonly string[] _replace;
        private readonly int _spawnAttempts;
        public ChestPlacingStep(float density, string[] replace, int spawnAttempts)
        {
            _density = density;
            _replace = replace;
            _spawnAttempts = spawnAttempts;
        }
        public void Apply(MapGenerationContext context)
        {
            var blocks = context.Blocks;

            int count = 0;
            for (int i = 0; i < _spawnAttempts; i++)
            {
                int x = context.Random.Next(0, context.Width);
                int y = context.Random.Next(0, context.SurfaceYPerColumn[x] - 1);
                var block = blocks.GetBlock(x, y);
                    
                if (
                    context.Random.NextDouble() < _density &&
                    _replace.ContainsItem(block.Id()) && 
                    blocks.GetBlockBelowSafe(x, y).IsSolid()
                )
                {
                    context.Blocks.PlaceBlock(BlockIds.Chest, x, y, true);
                    count++;
                }
            }

            GameLogger.Log($"Placed {count} chests with attempt {_spawnAttempts} ", nameof(ChestPlacingStep));
        }
    }
}