using System;
using System.Collections.Generic;
using Core.Context;
using Systems.WorldGeneration.Steps;
using Utils;

namespace Systems.WorldGeneration
{
    public class MapBuilder
    {
        private readonly List<IMapGenerationStep> _steps;
        private readonly MapGenerationContext _context;

        public MapBuilder(int worldSeed, string dimensionId)
        {
            var dimensionSeed = DeterministicHash.Fnv1aHash(dimensionId, worldSeed);
            var random = new Random(dimensionSeed);
            var seedOffset = dimensionSeed % 10000;
            
            var worldGenDatabase = new WorldGenDatabase(dimensionId);
            var dimensionGenerationData = worldGenDatabase.DimensionGenerationData;
            
            var widthRange = dimensionGenerationData.Width;
            var heightRange = dimensionGenerationData.Height;
            var width= random.Next(widthRange.Min, widthRange.Max);
            var height = random.Next(heightRange.Min, heightRange.Max);
            
            _context = new MapGenerationContext(width, height, dimensionSeed, seedOffset, random, worldGenDatabase, dimensionGenerationData);
            _steps = new List<IMapGenerationStep>();
            foreach (var stepData in dimensionGenerationData.Steps)
            {
                var step = worldGenDatabase.MapGenerationStepsRegistry.Create(stepData.Type, stepData.Params);
                _steps.Add(step);
            }
        }

        public MapGenerationContext Build()
        {
            foreach (var step in _steps)
            {
                step.Apply(_context);
            }
            
            return _context;
        }
    }



}