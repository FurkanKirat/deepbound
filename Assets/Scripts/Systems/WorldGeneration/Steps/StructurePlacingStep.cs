using System.Text;
using Core.Context;
using Data.Models.Dimensions;
using Systems.WorldGeneration.Structure;
using Systems.WorldSystem;
using UnityEngine;
using Utils;

namespace Systems.WorldGeneration.Steps
{
    public class StructurePlacingStep : IMapGenerationStep
    {
        public void Apply(MapGenerationContext context)
        {
            var dimData = context.DimensionGenerationData;
            var database = context.WorldGenDatabase;
            var structureConfig = database.StructureConfig[dimData.StructureConfigFile];
            if (structureConfig == null || structureConfig.Structures.IsNullOrEmpty())
                return;
            var height = context.Height;
            var width = context.Width;
            var surfaceYPerColumn = context.SurfaceYPerColumn;
            
            
            foreach (var structureGenData in structureConfig.Structures)
            {
                int maxAttempts = structureGenData.MaxAttempts > 0
                    ? structureGenData.MaxAttempts
                    : width * height / 500;

                var targetCount = structureGenData.Count.Roll(context.Random);
                int placed = 0;
                
                int minY = structureGenData.MinY.EvaluateOrDefault(height, 0);
                int maxY = structureGenData.MaxY.EvaluateOrDefault(height, height);
                
                GameLogger.Log($"MinY={minY}, MaxY={maxY}", nameof(StructurePlacingStep));
                
                var layer = structureGenData.Layer;

                var sb = new StringBuilder();
                sb.AppendLine($"Placed structure {structureGenData.StructureId}{{");
                int trialCount = 0;
                while (placed < targetCount && trialCount < maxAttempts)
                {
                    trialCount++;
                    int x = context.Random.Next(0, width);
                    
                    if (layer == LayerResolverType.AboveSurface)
                    {
                        minY = Mathf.Max(surfaceYPerColumn[x], minY);
                    }
                    
                    if (layer == LayerResolverType.BelowSurface)
                    {
                        maxY = Mathf.Min(surfaceYPerColumn[x] - 1, maxY);
                    }
                    
                    if (minY > maxY)
                        break;

                    var y = structureGenData.BindToSurface ? 
                        surfaceYPerColumn[x] : 
                        context.Random.Next(minY, maxY + 1);
                    
                    if (context.Random.NextDouble() < structureGenData.Chance)
                    {
                        IStructurePlacer placer =
                            database.StructurePlacerRegistry[structureGenData.StructureId];
                        
                        if (placer.CanPlace(x, y, structureGenData, context))
                        {
                            placer.PlaceStructure(x, y, structureGenData, context);
                            placed++;
                            sb.AppendLine($"{x}, {y}");
                        }
                    }
                }
                sb.AppendLine("}");
                sb.AppendLine($"Count: {placed}");
                GameLogger.Log(sb.ToString(), nameof(StructurePlacingStep));
            }
        }
    }
}