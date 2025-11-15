using System;
using Core.Tags;
using Data.Models;
using Data.Models.Blocks;
using Data.Models.Generation;
using Systems.WorldGeneration;
using Systems.WorldGeneration.Steps;
using Systems.WorldSystem;

namespace Core.Context
{
    public class MapGenerationContext
    {
        public readonly int Width;
        public readonly int Height;
        public readonly int DimensionSeed;
        public readonly int SeedOffset;
        public readonly Random Random;
        
        public readonly WorldGenDatabase WorldGenDatabase;
        public readonly DimensionGenerationData DimensionGenerationData;
        
        public WorldPosition PlayerSpawnPosition;
        public readonly TagStore TagStore = new();
        
        public int[] SurfaceYPerColumn;
        public WorldGrid<bool> CaveMap;
        public NoiseProvider Noise;

        public WorldGrid<Block> Blocks { get; }
        
        public MapGenerationContext(int width, int height, int dimensionSeed, int seedOffset, Random random, WorldGenDatabase worldGenDatabase, DimensionGenerationData dimensionGenerationData)
        {
            Width = width;
            Height = height;
            DimensionSeed = dimensionSeed;
            SeedOffset = seedOffset;
            Random = random;
            WorldGenDatabase = worldGenDatabase;
            DimensionGenerationData = dimensionGenerationData;
            Blocks = new WorldGrid<Block>(Width, Height);
        }

    }
    
}