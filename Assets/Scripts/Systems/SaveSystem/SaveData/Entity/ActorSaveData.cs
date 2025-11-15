namespace Systems.SaveSystem.SaveData.Entity
{
    public class ActorSaveData : EntitySaveData
    {
        public EntityHealthSaveData Health { get; set; }

        public ActorSaveData()
        {
        }

        public ActorSaveData(EntitySaveData other) : base(other)
        {
        }
    }
}