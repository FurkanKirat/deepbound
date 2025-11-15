namespace Systems.SaveSystem.SaveData.Entity
{
    public class EnemySaveData : ActorSaveData
    {
        public string EnemyId { get; set; }
        public EnemySaveData()
        {
        }

        public EnemySaveData(EntitySaveData other) : base(other)
        {
        }
    }
}