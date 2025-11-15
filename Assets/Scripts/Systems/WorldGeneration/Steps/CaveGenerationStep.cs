using Core.Context;
using Systems.WorldSystem;
using UnityEngine;

namespace Systems.WorldGeneration.Steps
{
    public class CaveGenerationStep : IMapGenerationStep
    {
        private readonly float _scale;
        private readonly float _threshold;

        public CaveGenerationStep(float scale, float threshold)
        {
            _scale = scale;
            _threshold = threshold;
        }

        public void Apply(MapGenerationContext context)
        {
            int width = context.Width;
            int height = context.Height;

            var caveMap = new WorldGrid<bool>(width, height);

            for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
            {
                float noiseValue = Mathf.PerlinNoise(
                    (x + context.SeedOffset) * _scale,
                    (y + context.SeedOffset) * _scale);
                
                caveMap[x, y] = noiseValue > _threshold;
            }

            context.CaveMap = caveMap;
        }
    }
}