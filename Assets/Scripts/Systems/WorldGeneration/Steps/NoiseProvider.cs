using System.Collections.Generic;
using Systems.WorldSystem;

namespace Systems.WorldGeneration.Steps
{
    public class NoiseProvider
    {
        private readonly Dictionary<string, WorldGrid<float>> _cache = new();
        private readonly int _seed;

        public NoiseProvider(int seed) => _seed = seed;

        public WorldGrid<float> Get(string key, int width, int height, float scale)
        {
            if (!_cache.TryGetValue(key, out var map))
            {
                map = PerlinMapGenerator.Generate(width, height, _seed, scale);
                _cache[key] = map;
            }
            return map;
        }
    }

}