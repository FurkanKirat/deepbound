using System;
using System.Collections.Generic;
using Systems.EntitySystem;
using Systems.Randomization;

namespace Systems.SpawnSystem
{
    public class EnemySpawnTable
    {
        public EnemySpawnPool[] Pools { get; set; }

        public List<EnemySpawnEntry> Roll(Random random)
        {
            var drops = new List<EnemySpawnEntry>();

            if (Pools == null)
                return drops;
            
            foreach (var pool in Pools)
            {
                int rollCount = random.Next(pool.RollCount.Min, pool.RollCount.Max + 1);
                for (int i = 0; i < rollCount; i++)
                {
                    var entry = WeightedPicker.PickEntry(random, pool.Entries);
                    if (entry != null && !EntityUtils.IsEmpty(entry.EnemyId))
                    {
                        drops.Add(entry);
                    }
                }
            }

            return drops;
        }
    }
}