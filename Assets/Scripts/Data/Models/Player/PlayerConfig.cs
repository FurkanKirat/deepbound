using System.Collections.Generic;
using Data.Models.Entities;
using Systems.StatSystem;

namespace Data.Models.Player
{
    public class PlayerConfig : EntityData
    {
        public Dictionary<StatType, float> Stats { get; set; }
        public float RespawnTime { get; set; }
        public float HealingCooldown { get; set; }
        public int BlockBreakingRange { get; set; }
        public int BlockPlacingRange { get; set; }
        public WorldPosition HandOffset { get; set; }
        
        
        public int BlockBreakingRangeSqr => BlockBreakingRange * BlockBreakingRange;
        public int BlockPlacingRangeSqr => BlockPlacingRange * BlockPlacingRange;
    }
}

