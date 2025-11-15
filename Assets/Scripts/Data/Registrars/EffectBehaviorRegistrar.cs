using Data.RegistrySystem;
using Systems.EffectSystem;

namespace Data.Registrars
{
    public class EffectBehaviorRegistrar : IRegistrar
    {
        public void RegisterAll()
        {
            Registries.EffectBehaviorFactory.RegisterMany(
                (EffectIds.Poison, createCtx => new PoisonEffect(createCtx), (loadCtx, save) => new PoisonEffect(loadCtx, save) ),
                (EffectIds.Healing, createCtx => new HealingEffect(createCtx), (loadCtx, save) => new HealingEffect(loadCtx, save)),
                (EffectIds.HealingCooldown, createCtx => new HealingCooldownEffect(createCtx), (loadCtx, save) => new HealingCooldownEffect(loadCtx, save))
                
                );
        }
    }
}