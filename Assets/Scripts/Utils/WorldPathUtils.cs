using System;
using System.Collections.Generic;
using System.IO;
using Constants.Paths;
using Systems.SaveSystem;
using Systems.SaveSystem.SaveData;
using Systems.WorldSystem;

namespace Utils
{
    public static class WorldPathUtils
    {
        public const string SaveExtension = ".save";
        public const string MetaFileName = "meta" + SaveExtension;
        public const string WorldIconName = "icon.png";
        
        public static readonly string WorldsFolder
            = Path.Combine(SaveConstants.SavesFolder, "Worlds");

        private static string GetWorldFolderName(WorldMetaData metaData)
            => $"[{metaData.WorldId[..8]}]{metaData.WorldName}";
        
        public static string GetWorldFolder(WorldMetaData metaData)
            => Path.Combine(WorldsFolder, GetWorldFolderName(metaData));
        
        public static string GetGlobalFilePath(string worldFolderPath)
            => Path.Combine(worldFolderPath, $"global{SaveExtension}");

        public static string GetMetaFilePath(string worldFolderPath)
            => Path.Combine(worldFolderPath, MetaFileName);
        
        public static string GetDimensionFilePath(string worldFolderPath, string dimensionName)
            => Path.Combine(GetDimensionsDirectoryPath(worldFolderPath), NamespacedIdUtils.StripNamespace(dimensionName) + SaveExtension);
        
        public static string GetDimensionsDirectoryPath(string worldFolderPath) 
            => Path.Combine(worldFolderPath, "dimensions");
        
        public static string GetDeletedWorldFolder(WorldMetaData metaData)
            => Path.Combine(SaveConstants.SavesFolder, "Deleted", GetWorldFolderName(metaData));

        private static string[] GetWorldFolders()
        {
            if (!Directory.Exists(WorldsFolder))
                return Array.Empty<string>();
            return Directory.GetDirectories(WorldsFolder, "*", SearchOption.TopDirectoryOnly);
        }
        
        public static List<WorldMetaData> GetWorldMetaDataList()
        {
            var worlds = new List<WorldMetaData>();
            var worldFolders = GetWorldFolders();
            foreach (var worldFolder in worldFolders)
            {
                string metaPath = GetMetaFilePath(worldFolder);
                if (File.Exists(metaPath))
                {
                    try
                    {
                        var saveMeta = SaveHelper.Load<WorldMetaDataSave>(metaPath, SaveType.Metadata);
                        worlds.Add(WorldMetaData.Load(saveMeta));
                    }
                    catch (Exception ex)
                    {
                        GameLogger.Error($"Failed to load metadata from {metaPath}: {ex.Message}");
                    }
                }
            }
            worlds.Sort((f, s) => s.LastSavedAt.CompareTo(f.LastSavedAt));
            return worlds;
        }

        public static string GetWorldIconPath(string worldFolderPath)
            =>  Path.Combine(worldFolderPath, WorldIconName);

        public static string GetWorldIconPath(WorldMetaData metaData)
            => GetWorldIconPath(GetWorldFolder(metaData));
    }
}