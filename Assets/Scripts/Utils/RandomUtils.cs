using System;
using System.Collections.Generic;
using Data.Models;

namespace Utils
{
    public static class RandomUtils
    {
        public static float NextFloat(this Random random, float min, float max)
        {
            return min + (float)random.NextDouble() * (max - min);
        }
        
        public static WorldPosition RandomInsideCircle(Random random, float minRadius, float maxRadius)
        {
            float angle = random.NextFloat(0f, MathF.PI * 2f);
            float radius = random.NextFloat(minRadius, maxRadius);
            return new WorldPosition(MathF.Cos(angle), MathF.Sin(angle)) * radius;
        }

        public static void Shuffle<T>(this IList<T> list, Random random)
        {
            for (int i = 0; i < list.Count; i++)
            {
                int j = random.Next(i, list.Count);
                (list[i], list[j]) = (list[j], list[i]);
            }
        }  
        
        public static (int xOffset, int yOffset) GetRandomDirection(Random random)
        {
            return random.Next(4) switch
            {
                0 => (1, 0),
                1 => (-1, 0),
                2 => (0, 1),
                3 => (0, -1),
                _ => (0, 0)
            };
        }
        
        public static int GetRandomSign(Random random)
        {
            return random.Next(2) switch
            {
                0 => 1,
                1 => -1,
                _ => 0
            };
        }

        public static T GetRandomElement<T>(Random random, IList<T> list)
        {
            return list[random.Next(0, list.Count)];
        }

    }
}