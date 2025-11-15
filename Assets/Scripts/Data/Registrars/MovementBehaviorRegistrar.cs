using Data.RegistrySystem;
using Generated.Ids;
using Systems.MovementSystem.Behaviors;

namespace Data.Registrars
{
    public class MovementBehaviorRegistrar : IRegistrar
    {
        public void RegisterAll()
        {
            Registries.MovementBehaviorFactory.RegisterMany(
                (MovementBehaviorIds.Gravity, _ => new GravityMovement()),
                (MovementBehaviorIds.Arrow, _ => new ArrowMovement()),
                (MovementBehaviorIds.Boomerang, (ctx) => new BoomerangMovementBehavior(ctx.SpawnerEntity, ctx.BehaviorOwner, ctx.TargetPos))
                );
        }
    }
}