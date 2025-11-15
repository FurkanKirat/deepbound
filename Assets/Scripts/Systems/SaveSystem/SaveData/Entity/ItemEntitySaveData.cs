namespace Systems.SaveSystem.SaveData.Entity
{
    public class ItemEntitySaveData : EntitySaveData
    {
        public ItemSaveData ItemSaveData { get; set; }


        public ItemEntitySaveData()
        {
        }

        public ItemEntitySaveData(EntitySaveData other) : base(other)
        {
        }
    }
}