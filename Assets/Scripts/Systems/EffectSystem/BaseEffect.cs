using Core.Context.Registry;
using Systems.EntitySystem.Interfaces;
using Systems.SaveSystem.SaveData;

namespace Systems.EffectSystem
{
    public abstract class BaseEffect : IEffectBehavior
    {
        public abstract string Id { get; }
        public float? Duration { get; set; }

        protected BaseEffect(EffectContext context)
        {
            Duration = context.EffectData.Duration;
        }

        protected BaseEffect(EffectContext context, EffectSaveData saveData)
        {
            Duration = saveData.Duration;
        }
        public abstract EffectSaveData ToSaveData();
        
        public virtual void OnApply(IEntity owner) { }

        public virtual void Tick(IEntity owner, float deltaTime) { }

        public virtual void OnRemove(IEntity owner) { }
    }
}