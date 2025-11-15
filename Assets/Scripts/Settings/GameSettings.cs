using System;
using System.Collections.Generic;
using Data.Serializable;
using Generated.Settings;
using Localization;
using Systems.SaveSystem.Interfaces;
using UnityEngine;
using Utils.Parsers;

namespace Settings
{
    public class GameSettings : ISaveable<SettingsSaveData>
    {
        private readonly Dictionary<string, ISettingValue> _settings = new();

        public GameSettings(SettingsConfig config, SettingsSaveData saveData = null)
        {
            foreach (var panel in config.Panels)
            {
                foreach (var s in panel.Settings)
                {
                    ISettingValue setting;

                    switch (s.Type)
                    {
                        case SettingType.Bool:
                            setting = new SettingValue<bool>(Convert.ToBoolean(s.Default));
                            break;
                        case SettingType.Int:
                            setting = new SettingValue<int>(Convert.ToInt32(s.Default));
                            break;
                        case SettingType.Float:
                            setting = new SettingValue<float>(Convert.ToSingle(s.Default));
                            break;
                        case SettingType.String:
                            setting = new SettingValue<string>(s.Default.ToString());
                            break;
                        case SettingType.Enum:
                            if (int.TryParse(s.Default.ToString(), out var intVal))
                                setting = new SettingValue<int>(intVal);
                            else
                                setting = new SettingValue<string>(s.Default.ToString());
                            break;
                        case SettingType.Int2:
                            var int2Val = s.Default.ToString();
                            setting = new SettingValue<Int2>(
                                IntVectorParser.TryParse(int2Val, out var tuple) 
                                    ? new Int2(tuple.x, tuple.y) 
                                    : Int2.Zero
                            );
                            break;
                        default:
                            setting = null;
                            break;
                    }

                    if (setting != null)
                        _settings[s.Key] = setting;

                }
            }

            if (saveData?.Settings != null)
            {
                foreach (var (key, value) in saveData.Settings)
                {
                    if (!_settings.TryGetValue(key, out var entry))
                        continue;

                    switch (entry)
                    {
                        case SettingValue<bool> boolSetting:
                            boolSetting.SetValue(Convert.ToBoolean(value));
                            break;
                        case SettingValue<int> intSetting:
                            intSetting.SetValue(Convert.ToInt32(value));
                            break;
                        case SettingValue<float> floatSetting:
                            floatSetting.SetValue(Convert.ToSingle(value));
                            break;
                        case SettingValue<string> stringSetting:
                            stringSetting.SetValue(value.ToString());
                            break;
                        case SettingValue<Int2> int2Setting:
                            if (value is string str && IntVectorParser.TryParse(str, out var tuple))
                                int2Setting.SetValue(new Int2(tuple.x, tuple.y));
                            break;
                    }
                }
            }
            RegisterCallbacks();
        }

        private void RegisterCallbacks()
        {
            var masterVolume = (SettingValue<float>)_settings[SettingsKeys.MasterVolume];
            masterVolume.OnChangedValue += val => AudioListener.volume = val;

            var fullscreen = (SettingValue<bool>)_settings[SettingsKeys.Fullscreen];
            fullscreen.OnChangedValue += val => Screen.fullScreen = val;

            var resolution = (SettingValue<Int2>)_settings[SettingsKeys.Resolution];
            resolution.OnChangedValue += val => Screen.SetResolution(val.x, val.y, Screen.fullScreen);

            var quality = (SettingValue<string>)_settings[SettingsKeys.QualityLevel];
            quality.OnChangedValue += (str) =>
            {
                var names = QualitySettings.names;
                var index = Array.IndexOf(names, str);
                QualitySettings.SetQualityLevel(index);
            };
            
            var language = (SettingValue<string>)_settings[SettingsKeys.Language];
            language.OnChangedValue += LocalizationDatabase.LoadLanguage;

        }
        
        public object Get(string key) => _settings[key].GetValue();
        public T Get<T>(string key)
        {
            var val = _settings[key].GetValue();
            if (val is T tVal)
                return tVal;

            try
            {
                return (T)Convert.ChangeType(val, typeof(T));
            }
            catch
            {
                throw new InvalidCastException($"Cannot convert setting '{key}' value '{val}' to type {typeof(T).Name}");
            }
        }

        public void Set(string key, object value) => _settings[key].SetValue(value);
        public IReadOnlyDictionary<string, ISettingValue> Settings => _settings;
        public SettingsSaveData ToSaveData()
        {
            var settings = new Dictionary<string, object>();
            foreach (var (key,setting) in _settings)
            {
                settings.Add(key, setting.GetValue());
            }
            
            return new SettingsSaveData
            {
                Settings = settings
            };
        }
    }
}