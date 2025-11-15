namespace Config
{
    public class SpawnConfig
    {
        public bool EnemySpawnOn { get; set; }
        public int MaxEnemyCount { get; set; }
        public int NearestEnemySpawn { get; set; }
        public int FarthestEnemySpawn { get; set; }
        public float EnemySpawnInterval { get; set; }
        public int MaxPortalCount { get; set; }
        public float PortalSpawnInterval { get; set; }
    }
}