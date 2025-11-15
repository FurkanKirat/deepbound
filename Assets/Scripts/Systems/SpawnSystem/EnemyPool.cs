using Data.Serializable;

namespace Systems.SpawnSystem
{
    public class EnemySpawnPool
    {
        public RangeInt RollCount { get; set; }
        public EnemySpawnEntry[] Entries { get; set; }
    }
}