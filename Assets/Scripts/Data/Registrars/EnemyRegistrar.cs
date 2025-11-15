using Data.RegistrySystem;
using Generated.Ids;
using Systems.EntitySystem.Enemy.Creature;
using Systems.EntitySystem.Enemy.Slime;

namespace Data.Registrars
{
    public class EnemyRegistrar : IRegistrar
    {
        public void RegisterAll()
        {
            Registries.EnemyFactory.Register(EnemyIds.Slime, 
                createCtx => new SlimeEnemy(createCtx),
                (loadCtx, loadData) => new SlimeEnemy(loadCtx, loadData)
                );
            Registries.EnemyFactory.Register(EnemyIds.Creature,
                (ctx) => new CreatureEnemy(ctx),
                    (loadCtx, loadData) => new CreatureEnemy(loadCtx, loadData)
                );
        }
    }
}