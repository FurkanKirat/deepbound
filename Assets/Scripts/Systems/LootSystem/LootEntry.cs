using Data.Serializable;
using Systems.Randomization;

namespace Systems.LootSystem
{
    public class LootEntry : IWeightedEntry
    {
        public string ItemId { get; set; }
        public int Weight { get; set; }
        public RangeInt Count { get; set; }

        public override string ToString()
        {
            return $"{nameof(ItemId)}: {ItemId}, {nameof(Weight)}: {Weight}, {nameof(Count)}: {Count}";
        }
    }
}