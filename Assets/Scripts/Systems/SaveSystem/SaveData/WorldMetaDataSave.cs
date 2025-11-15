using System;
using Systems.SaveSystem.Interfaces;

namespace Systems.SaveSystem.SaveData
{
    public class WorldMetaDataSave : ISaveData
    {
        public string WorldId { get; set; }
        public string WorldName { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastSavedAt { get; set; }
        public int Seed { get; set; }
        public int GameVersion { get; set; }
    }
}