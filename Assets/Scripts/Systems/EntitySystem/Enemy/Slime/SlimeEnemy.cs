using Core.Context;
using Core.Context.Spawn;
using Data.Models;
using Systems.SaveSystem.SaveData.Entity;

namespace Systems.EntitySystem.Enemy.Slime
{
    public class SlimeEnemy : EnemyLogic
    {
        public SlimeEnemy(EnemySpawnContext spawnContext) : base(spawnContext)
        {
        }

        public SlimeEnemy(EnemySpawnContext spawnContext, EnemySaveData saveData) : base(spawnContext, saveData)
        {
        }

        public bool ShouldHop(TickContext ctx, SlimeIdleState idle)
        {
            return idle.WaitTime <= 0;
        }

        public bool ShouldChase(TickContext ctx)
        {
            var distanceSq = WorldPosition.SquaredDistance(Position, ctx.Player.Position);
            return distanceSq < SlimeHopState.DetectionRadius * SlimeHopState.DetectionRadius && IsGrounded;
        }

        public bool ShouldStopChasing(TickContext ctx)
        {
            var distanceSq = WorldPosition.SquaredDistance(Position, ctx.Player.Position);
            return distanceSq > 8f;
        }
    }
}