using Data.Serializable;
using Utils.Extensions;

namespace Systems.LootSystem
{
    public class LootPool
    {
        public RangeInt RollCount { get; set; }
        public LootEntry[] Entries { get; set; }
        
        public override string ToString()
        {
            return $"{nameof(RollCount)}: {RollCount}, {nameof(Entries)}: {Entries.ToDebugString()}";
        }
    }
}