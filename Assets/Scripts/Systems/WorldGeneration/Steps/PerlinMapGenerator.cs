using Systems.WorldSystem;
using UnityEngine;

namespace Systems.WorldGeneration.Steps
{
    public static class PerlinMapGenerator
    {
        public static WorldGrid<float> Generate(int width, int height, int seed, float scale)
        {
            var map = new WorldGrid<float>(width, height);

            float offsetX = seed * 31.7f;  // to create random deviation
            float offsetY = seed * 47.3f;

            for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
            {
                float sampleX = x * scale + offsetX;
                float sampleY = y * scale + offsetY;
                map[x, y] = Mathf.PerlinNoise(sampleX, sampleY);  // ∈ [0, 1]
            }

            return map;
        }
    }

}