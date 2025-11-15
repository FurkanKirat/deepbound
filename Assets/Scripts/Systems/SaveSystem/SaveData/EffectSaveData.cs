using System.Collections.Generic;
using Systems.SaveSystem.Interfaces;

namespace Systems.SaveSystem.SaveData
{
    public class EffectSaveData : ISaveData
    {
        public string EffectId { get; set; }
        public float? Duration { get; set; }
        public Dictionary<string, object> Parameters { get; set; }
    }
}