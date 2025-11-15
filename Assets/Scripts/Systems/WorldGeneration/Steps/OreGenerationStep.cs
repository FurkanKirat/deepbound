using System;
using Core.Context;
using Data.Models.Blocks;
using Data.Models.Generation;
using Systems.WorldGeneration.Ore;
using Utils;

namespace Systems.WorldGeneration.Steps
{
    public class OreGenerationStep : IMapGenerationStep
    {
        public void Apply(MapGenerationContext context)
        {
            var dimData = context.DimensionGenerationData;
            
            var oreConfig = context.WorldGenDatabase.OreConfig[dimData.OreConfigFile];
            if (oreConfig == null || oreConfig.Ores.IsNullOrEmpty())
                return;

            var surfaceYPerColumn = context.SurfaceYPerColumn;
            var height = context.Height;
            foreach (var oreGenData in oreConfig.Ores)
            {
                int spawnAttempts = context.Width * context.Height / 500;

                for (int i = 0; i < spawnAttempts; i++)
                {
                    int x = context.Random.Next(0, context.Width);
                    int minY = oreGenData.MinY.Evaluate(height);
                    int maxY = Math.Min(oreGenData.MaxY.Evaluate(height), surfaceYPerColumn[x]-1);
                    int y = context.Random.Next(minY, maxY+1);


                    var peakY = oreGenData.PeakY.Evaluate(height);
                    var targetBlock = context.Blocks.GetBlock(x, y);
                    
                    double distance = Math.Abs(y - peakY);
                    double maxDistance = Math.Max(peakY - minY, maxY - peakY);
                    double chance = oreGenData.ChanceAtPeak * (1 - distance / maxDistance) + oreGenData.ChanceAtMinMax * (distance / maxDistance);
                    
                    if ((targetBlock.Id() == dimData.BaseBlock || targetBlock.Id() == dimData.StoneBlock) &&
                        context.Random.NextDouble() < chance)
                    {
                        IOrePlacer orePlacer = oreGenData.Shape switch
                        {
                            VeinShape.RandomWalk => new RandomWalkOrePlacer(),
                            VeinShape.Ellipse => new EllipseOrePlacer(),
                            VeinShape.Line => new LineOrePlacer(),
                            _ => throw new ArgumentOutOfRangeException()
                        };
                        var veinSize = oreGenData.VeinSize.Roll(context.Random);
                        orePlacer.Place(x, y, veinSize, context, oreGenData);
                    }
                }
            }

        }
    }

}