using System.Collections.Generic;
using Systems.SaveSystem.Interfaces;

namespace Systems.SaveSystem.SaveData
{
    public class PotionsSaveData : ISaveData
    {
        public Dictionary<ItemSaveData, float> Potions { get; set; }
    }
}