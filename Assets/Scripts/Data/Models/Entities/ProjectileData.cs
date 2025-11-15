using System;
using System.Collections.Generic;
using Core;
using Data.Models.References;
using Systems.CombatSystem.Damage;
using Systems.StatSystem;

namespace Data.Models.Entities
{
    [Serializable]
    public class ProjectileData : EntityData, IIdentifiable
    {
        #region Identity
        public string Id { get; set; }
        #endregion

        #region References
        public SpriteRef Icon { get; set; }
        public TrailData Trail { get; set; }
        public AttackBehaviorRef AttackBehavior { get; set; }
        public MovementBehaviorRef MovementBehavior { get; set; }
        #endregion

        #region Type
        public ProjectileType ProjectileType { get; set; }
        #endregion

        #region Movement
        public Dictionary<StatType, float> BaseStats { get; set; }
        #endregion

        #region Properties
        public bool CanPierce { get; set; }
        public bool DestroyOnTileCollision { get; set; }
        #endregion

        #region Damage

        public DamageType DamageType { get; set; }

        #endregion
    }
}