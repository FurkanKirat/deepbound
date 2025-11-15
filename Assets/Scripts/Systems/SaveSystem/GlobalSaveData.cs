using System.Collections.Generic;
using Systems.SaveSystem.Interfaces;
using Systems.SaveSystem.SaveData.Entity;

namespace Systems.SaveSystem
{
    public class GlobalSaveData : ISaveData
    {
        public PlayerSaveData PlayerSaveData { get; set; }
        public string CurrentDimensionId { get; set; }
        public List<string> GeneratedDimensions { get; set; }
    }

    
}