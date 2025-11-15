using System;
using System.Collections.Generic;
using Core;
using Data.Models.References;
using Generated.Ids;
using Systems.CombatSystem.Damage;
using Systems.StatSystem;

namespace Data.Models.Entities
{
    [Serializable]
    public class EnemyData : EntityData, IIdentifiable
    {
        #region Identity
        public string Id { get; set; }
        public string Type { get; set; }
        #endregion

        #region Stats
        public Dictionary<StatType, float> BaseStats { get; set; }
        public DamageType DamageType { get; set; }
        #endregion

        #region Visuals
        public SpriteRef Icon { get; set; }
        #endregion

        #region Combat
        public AttackBehaviorRef AttackBehavior { get; set; }
        #endregion

        #region AI
        public EnemyBehaviorRef AiBehavior { get; set; }
        public MovementBehaviorRef MovementBehavior { get; set; }
        #endregion

        #region Loot
        public LootTableRef LootTable { get; set; }
        #endregion

        public void ApplyFallbacks()
        {
            LootTable ??= new LootTableRef(LootTableIds.Empty);
        }
    }
}