using System;
using Systems.SaveSystem.Interfaces;
using Systems.SaveSystem.SaveData;

namespace Systems.WorldSystem
{
    public class WorldMetaData : ISaveable<WorldMetaDataSave>
    {
        public string WorldId { get; }
        public string WorldName { get; }
        public DateTime CreatedAt { get; }
        public DateTime LastSavedAt { get; set; }
        public int Seed { get; }
        public int GameVersion { get; set; }

        private WorldMetaData(
            string worldId, 
            string worldName, 
            DateTime createdAt, 
            DateTime lastSavedAt, 
            int seed,
            int gameVersion)
        {
            WorldId = worldId;
            WorldName = worldName;
            CreatedAt = createdAt;
            LastSavedAt = lastSavedAt;
            Seed = seed;
            GameVersion = gameVersion;
        }

        public static WorldMetaData Create(string worldId, string worldName, DateTime createdAt, DateTime lastSavedAt, int seed, int gameVersion)
        {
            return new WorldMetaData(worldId, worldName, createdAt, lastSavedAt, seed, gameVersion);
        }

        public static WorldMetaData Load(WorldMetaDataSave saveData)
        {
            return new WorldMetaData(saveData.WorldId, saveData.WorldName, saveData.CreatedAt, saveData.LastSavedAt, saveData.Seed, saveData.GameVersion);
        }
        public WorldMetaDataSave ToSaveData()
        {
            return new WorldMetaDataSave
            {
                WorldId = WorldId,
                WorldName = WorldName,
                CreatedAt = CreatedAt,
                LastSavedAt = LastSavedAt,
                Seed = Seed,
                GameVersion = GameVersion
            };
        }
    }

}