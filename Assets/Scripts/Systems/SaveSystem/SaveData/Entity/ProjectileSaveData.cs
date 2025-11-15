using Data.Models;
using Systems.CombatSystem.Damage;

namespace Systems.SaveSystem.SaveData.Entity
{
    public class ProjectileSaveData : EntitySaveData
    {
        public int OwnerId { get; set; }
        public DamageInfo DamageInfo { get; set; }
        public WorldPosition? TargetPosition { get; set; }
        
        public ProjectileSaveData()
        {
        }

        public ProjectileSaveData(EntitySaveData other) : base(other)
        {
        }
    }
}