using System.Collections.Generic;
using Systems.SaveSystem.Interfaces;

namespace Systems.SaveSystem.SaveData
{
    public class EffectsSaveData : ISaveData
    {
        public List<EffectSaveData> Effects { get; set; }
    }
}