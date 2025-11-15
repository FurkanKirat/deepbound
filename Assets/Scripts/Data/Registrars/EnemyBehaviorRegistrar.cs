using Data.RegistrySystem;
using Generated.Ids;
using Systems.EntitySystem.Enemy.Creature;
using Systems.EntitySystem.Enemy.Slime;

namespace Data.Registrars
{
    public class EnemyBehaviorRegistrar : IRegistrar
    {
        public void RegisterAll()
        {
            Registries.EnemyBehaviorFactory.Register(EnemyAIIds.Slime, (enemyCtx) =>
            {
                var enemy = enemyCtx.Enemy;
                var stateMachine = enemyCtx.StateMachine;
                
                var slime = (SlimeEnemy)enemy;
                var idle = new SlimeIdleState(slime, stateMachine, enemyCtx.Random);
                var chase = new SlimeChaseState(slime, stateMachine, enemyCtx.Random);
                var hop = new SlimeHopState(slime, stateMachine, enemyCtx.Random);

                stateMachine.AddTransition(idle, hop, (ctx) => slime.ShouldHop(ctx, idle));
                stateMachine.AddTransition(hop, chase, (ctx) => slime.ShouldChase(ctx));
                stateMachine.AddTransition(hop, idle, (ctx) => !slime.ShouldChase(ctx) && slime.IsGrounded);
                stateMachine.AddTransition(chase, idle, (ctx) => slime.ShouldStopChasing(ctx));
                return idle;
            });

            Registries.EnemyBehaviorFactory.Register(EnemyAIIds.Creature, (enemyCtx) =>
            {
                var enemy = enemyCtx.Enemy;
                var machine = enemyCtx.StateMachine;
                
                var creature = (CreatureEnemy)enemy;
                var idle = new CreatureIdleState(creature, machine, enemyCtx.Random);
                var chase = new CreatureChaseState(creature, machine, enemyCtx.Random);
                var wander = new CreatureWanderState(creature, machine, enemyCtx.Random);
                
                machine.AddTransition(idle, chase, ctx => creature.ShouldChase(ctx));
                machine.AddTransition(idle, wander, ctx => !creature.ShouldChase(ctx) && creature.ShouldWander(ctx, idle));
                
                machine.AddTransition(wander, chase, ctx => creature.ShouldChase(ctx));
                machine.AddTransition(wander, idle, ctx => !creature.ShouldChase(ctx) && !creature.ShouldWander(ctx, idle));
                
                machine.AddTransition(chase, wander, ctx => creature.ShouldStopChasing(ctx) && creature.ShouldWander(ctx, idle));
                machine.AddTransition(chase, idle, ctx => creature.ShouldStopChasing(ctx) && !creature.ShouldWander(ctx, idle));
                
                return idle;
            });

        }
    }
}