namespace Systems.SaveSystem.SaveData.Entity
{
    public class NpcSaveData : ActorSaveData
    {
        public string NpcId { get; set; }
        
        public NpcSaveData()
        {
        }

        public NpcSaveData(EntitySaveData other) : base(other)
        {
        }
    }
}