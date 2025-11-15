using Data.Serializable;
using Systems.Randomization;

namespace Systems.SpawnSystem
{
    public class EnemySpawnEntry : IWeightedEntry
    {
        public string EnemyId { get; set; }  
        public int Weight { get; set; }
        public RangeInt Count { get; set; } = new RangeInt(1);
    }
}