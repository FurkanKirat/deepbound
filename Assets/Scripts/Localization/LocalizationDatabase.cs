using System;
using System.Collections.Generic;
using Core;
using Core.Events;
using UnityEngine;
using Utils;
using Object = UnityEngine.Object;

namespace Localization
{
    public static class LocalizationDatabase
    {
        private static Dictionary<string, string> _language;
        private static readonly Dictionary<string, string> DefaultLanguage;
        private const string DefaultLanguageCode = "en";

        static LocalizationDatabase()
        {
            try
            {
                string path = $"Localization/{DefaultLanguageCode}";
                DefaultLanguage = ResourcesHelper.LoadJson<Dictionary<string, string>>(path);
            }
            catch (Exception e)
            {
                GameLogger.Error($"Failed to load default language '{DefaultLanguageCode}': {e.Message}", nameof(LocalizationDatabase));
                DefaultLanguage = new Dictionary<string, string>();
            }
        }
        public static void LoadLanguage(string languageCode)
        {
            if (languageCode == DefaultLanguageCode)
            {
                _language = DefaultLanguage;
                GameEventBus.Publish(new LanguageChangedEvent(languageCode));
                UpdateScene();
                return;
            }
            
            string path = $"Localization/{languageCode}";
            try
            {
                _language = ResourcesHelper.LoadJson<Dictionary<string, string>>(path);
                GameEventBus.Publish(new LanguageChangedEvent(languageCode));
                UpdateScene();
            }
            catch (Exception e)
            {
                GameLogger.Error("Failed to load default language: " + e.Message, nameof(LocalizationDatabase));
                _language = new Dictionary<string, string>();
            }
        }
        
        public static string Get(string key)
        {
            if (TryGet(key, out string value))
                return value;

            GameLogger.Warn($"Key '{key}' was not found.");
            return key;
        }

        public static bool TryGet(string key, out string value)
        {
            _language ??= DefaultLanguage;
            return _language.TryGetValue(key, out value) || 
                   DefaultLanguage.TryGetValue(key, out value);
        }

        // May be optimized later
        private static void UpdateScene()
        {
            foreach (var obj in Object.FindObjectsByType<MonoBehaviour>(FindObjectsInactive.Include, FindObjectsSortMode.None))
            {
                if (obj is ILocalizable localizable)
                {
                    localizable.Localize();
                }
            }
        }
    }
}