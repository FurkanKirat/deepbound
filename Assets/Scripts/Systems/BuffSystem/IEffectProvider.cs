using System.Collections.Generic;

namespace Systems.BuffSystem
{
    public interface IEffectProvider
    {
        IEnumerable<EffectData> GetActiveEffects();
    }
}