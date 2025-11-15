using System.Collections.Generic;
using Core;
using Data.Models.References;
using Systems.StatSystem;

namespace Data.Models.Entities
{
    public class NpcData : EntityData, IIdentifiable
    {
        #region Identity
        public string Id { get; set; }
        public string Type { get; set; }
        #endregion
        
        #region Stats
        public Dictionary<StatType, float> BaseStats { get; set; }
        #endregion
        
        #region Visuals
        public SpriteRef Icon { get; set; }
        #endregion
        
        #region AI
        public NpcBehaviorRef AiBehavior { get; set; }
        public MovementBehaviorRef MovementBehavior { get; set; }
        #endregion
    }
}