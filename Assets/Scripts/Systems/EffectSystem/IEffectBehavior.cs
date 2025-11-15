using Systems.EntitySystem.Interfaces;
using Systems.SaveSystem.Interfaces;
using Systems.SaveSystem.SaveData;

namespace Systems.EffectSystem
{
    public interface IEffectBehavior : ISaveable<EffectSaveData>
    {
        string Id { get; }
        void OnApply(IEntity owner);

        void Tick(IEntity owner, float deltaTime);

        void OnRemove(IEntity owner);
        float? Duration { get; set; }
    }
}