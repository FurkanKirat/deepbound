using System.IO;
using UnityEngine;

namespace Constants.Paths
{
    public static class SaveConstants
    {
        public static readonly string BindingsFile = Path.Combine(Application.persistentDataPath, "input_bindings.json");
        public static readonly string SettingsFile = Path.Combine(Application.persistentDataPath, "config.json");
        
        public static readonly string SavesFolder = Path.Combine(Application.persistentDataPath, Saves);
        public static readonly string ScreenshotsFolder = Path.Combine(Application.persistentDataPath, "screenshots");
        
        public const string Saves = "saves";
    }
}