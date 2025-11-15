using System.Collections.Generic;
using Systems.EffectSystem;
using Systems.SaveSystem.Interfaces;

namespace Systems.SaveSystem.SaveData
{
    public class CooldownSaveData : ISaveData
    {
        public Dictionary<CooldownType, float> Cooldowns;
    }
}