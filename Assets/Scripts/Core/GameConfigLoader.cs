using Data.Database;
using Data.RegistrySystem;
using Generated.Settings;
using Localization;
using Systems.InputSystem;
using UnityEngine;

namespace Core
{
    public static class GameConfigLoader
    {
        public static bool IsLoaded { get; private set; }
        public static void Load(GameRoot root)
        {
            if (IsLoaded)
                return;
            var settings = root.SettingsManager;
            Application.targetFrameRate = 60;
            Registries.RegisterAll();
            ResourceDatabases.LoadAll();
            Databases.LoadAll();
            Configs.LoadAll();
            BlockIdCache.LoadAll();
            AtlasUVIndex.LoadUVData();
            InputBindingManager.Load();
            settings.Load();
            
            var lang = settings.Get<string>(SettingsKeys.Language);
            LocalizationDatabase.LoadLanguage(lang);
            IsLoaded = true;
        }
    }
}