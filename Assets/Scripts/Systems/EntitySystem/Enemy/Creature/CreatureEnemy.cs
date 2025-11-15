using Core.Context;
using Core.Context.Registry;
using Core.Context.Spawn;
using Data.Models;
using Systems.SaveSystem.SaveData.Entity;

namespace Systems.EntitySystem.Enemy.Creature
{
    public class CreatureEnemy : EnemyLogic
    {
        private const float StartChaseRadius = 8f;
        private const float StopChaseRadius = 10f;

        private const float StartChaseSq = StartChaseRadius * StartChaseRadius;
        private const float StopChaseSq = StopChaseRadius * StopChaseRadius;
        public CreatureEnemy(EnemySpawnContext spawnContext) : base(spawnContext)
        {
        }

        public CreatureEnemy(EnemySpawnContext spawnContext, EnemySaveData saveData) : base(spawnContext, saveData)
        {
        }

        public bool ShouldWander(TickContext ctx, CreatureIdleState idle)
        {
            return idle.WaitTime <= 0;
        }
        
        public bool ShouldChase(TickContext ctx)
        {
            var distanceSq = WorldPosition.SquaredDistance(Position, ctx.Player.Position);
            return distanceSq < StartChaseSq;
        }

        public bool ShouldStopChasing(TickContext ctx)
        {
            var distanceSq = WorldPosition.SquaredDistance(Position, ctx.Player.Position);
            return distanceSq > StopChaseSq;
        }
        
    }
}