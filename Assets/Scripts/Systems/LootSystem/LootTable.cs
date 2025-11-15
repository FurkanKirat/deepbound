using System;
using System.Collections.Generic;
using Core;
using Data.Models.Items;
using Systems.Randomization;
using Utils.Extensions;

namespace Systems.LootSystem
{
    public class LootTable : IIdentifiable
    {
        public string Id { get; set; }
        public LootPool[] Pools { get; set; }

        public List<ItemInstance> Roll(Random random)
        {
            var drops = new List<ItemInstance>();

            foreach (var pool in Pools)
            {
                int rollCount = random.Next(pool.RollCount.Min, pool.RollCount.Max + 1);
                for (int i = 0; i < rollCount; i++)
                {
                    var entry = WeightedPicker.PickEntry(random, pool.Entries);
                    if (entry != null && !ItemUtils.IsEmpty(entry.ItemId))
                    {
                        int count = entry.Count.Roll(random);
                        drops.Add(ItemInstance.Create(entry.ItemId, count));
                    }
                }
            }

            return drops;
        }

        public override string ToString()
        {
            return $"{nameof(Id)}: {Id}, {nameof(Pools)}: {Pools.ToDebugString()}";
        }
    }
}