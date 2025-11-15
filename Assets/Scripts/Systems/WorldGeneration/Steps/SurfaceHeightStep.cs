using Core.Context;
using UnityEngine;

namespace Systems.WorldGeneration.Steps
{
    public class SurfaceHeightStep : IMapGenerationStep
    {
        private readonly float _scale;
        private readonly int _minHeightOffset;
        private readonly int _maxHeightOffset;

        public SurfaceHeightStep(float scale, int minOffset, int maxOffset)
        {
            _scale = scale;
            _minHeightOffset = minOffset;
            _maxHeightOffset = maxOffset;
        }
        

        public void Apply(MapGenerationContext context)
        {
            int baseSurface = context.Height / 2;
            context.SurfaceYPerColumn = new int[context.Width];

            for (int x = 0; x < context.Width; x++)
            {
                float noise = Mathf.PerlinNoise(x * _scale, context.SeedOffset * 0.001f); // noise ∈ [0, 1]
                int offset = Mathf.RoundToInt(Mathf.Lerp(_minHeightOffset, _maxHeightOffset, noise));
                context.SurfaceYPerColumn[x] = baseSurface + offset;
            }
        }
    }
}