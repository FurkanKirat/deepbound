using System;
using Constants.Paths;
using Data.Database;
using Settings;
using Utils;

namespace Session
{
    public class SettingsManager
    {
        private bool _useDefaults = false;

        private GameSettings GameSettings { get; set; }
        
        public void Load()
        {
            if (!_useDefaults && FileUtils.FileExists(SaveConstants.SettingsFile))
            {
                try
                {
                    var saveData = JsonHelper.LoadRaw<SettingsSaveData>(SaveConstants.SettingsFile);
                    GameSettings = new GameSettings(Configs.SettingsConfig, saveData); 
                }
                catch (Exception e)
                {
                    GameLogger.Error("Could not load settings: "+ e.Message, nameof(SettingsManager));
                    GameSettings = new GameSettings(Configs.SettingsConfig);
                    Save();
                }
                
            }
            else
            {
                GameSettings = new GameSettings(Configs.SettingsConfig);
                Save();
            }

            AddSaveCallback(GameSettings);
            
        }
        
        private void Save()
        {
            try
            {
                JsonHelper.SaveRaw(SaveConstants.SettingsFile, GameSettings.ToSaveData());
            }
            catch (Exception e)
            {
                GameLogger.Error($"Error saving settings {e.Message}", nameof(SettingsManager));
            }
            
        }

        private void AddSaveCallback(GameSettings settings)
        {
            foreach (var setting in settings.Settings)
            {
                setting.Value.OnChanged += Save;
            }
        }

        public object Get(string key) => GameSettings.Get(key);
        public T Get<T>(string key) => GameSettings.Get<T>(key);
        public void Set(string key, object value) => GameSettings.Set(key, value);
        
    }
}
