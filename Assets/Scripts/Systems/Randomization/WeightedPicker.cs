using System;
using System.Collections.Generic;

namespace Systems.Randomization
{
    public static class WeightedPicker
    {
        public static T PickEntry<T>(Random random, IList<T> entries) 
            where T : IWeightedEntry
        {
            int total = 0;
            foreach (var e in entries) total += e.Weight;
            if (total <= 0) return default;

            int roll = random.Next(total);
            int current = 0;

            foreach (var e in entries)
            {
                current += e.Weight;
                if (current > roll) return e;
            }

            return default;
        }
    }

}