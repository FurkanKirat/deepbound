using System;
using System.Collections.Generic;
using Core.Context;
using Data.Models.Generation;
using Utils;

namespace Systems.WorldGeneration.Ore
{
    public class RandomWalkOrePlacer : IOrePlacer
    {
        private int _x;
        private int _y;
        public void Place(int baseX, int baseY, int veinSize, MapGenerationContext context, OreGenerationData oreGenData)
        {
            _x = baseX;
            _y = baseY;
            var blocks = context.Blocks;
            
            blocks.PlaceBlock(oreGenData.BlockId, _x, _y);
            var visited = new HashSet<(int, int)>();
            for (int i = 1; i < veinSize; i++)
            {
                const int tries = 5;
                for (int trying = 0; trying < tries; trying++)
                {
                    var dir = ChooseDirection(context.Random);

                    _x += dir.x;
                    _y += dir.y;
                    
                    if (visited.Add((_x, _y)))
                    {
                        blocks.PlaceIfUnderSurface(oreGenData.BlockId, _x, _y, context.SurfaceYPerColumn);
                        var chance = context.Random.NextDouble();
                        var dirCount = chance switch
                        {
                            < 0.02 => 3,
                            < 0.17 => 2,
                            < 0.32 => 1,
                            _ => 0
                        };

                        foreach (var extraDir in ChooseDifferentDirections(context.Random, dirCount))
                        {
                            var extraX = _x + extraDir.x;
                            var extraY = _y + extraDir.y;
                            if (!blocks.IsInBounds(extraX, extraY) || context.SurfaceYPerColumn[extraX] <= extraY)
                                continue;
                            blocks.PlaceIfUnderSurface(oreGenData.BlockId, extraX, extraY,  context.SurfaceYPerColumn);
                        }
                        break;
                    }
                }
            }
        }
        

        private static (int x, int y) ChooseDirection(Random random)
        {
            int dir = random.Next(4);
            int x = 0;
            int y = 0;
            switch (dir)
            {
                case 0:
                    x++;
                    break;
                case 1:
                    x--;
                    break;
                case 2:
                    y++;
                    break;
                case 3:
                    y--;
                    break;
            }
            return (x, y);
        }

        private static List<(int x, int y)> ChooseDifferentDirections(Random random, int count)
        {
            var dirs = new List<(int, int)>(new[] { (0, 1), (0, -1), (1, 0), (-1, 0) });
            switch (count)
            {
                case >= 4:
                    return dirs;
                case <= 0:
                    return new List<(int x, int y)>();
            }

            int removeCount = 4 - count;
            for (int i = 0; i < removeCount; i++)
            {
                dirs.RemoveAt(random.Next(dirs.Count));
            }
            return dirs;
        }
    }
}