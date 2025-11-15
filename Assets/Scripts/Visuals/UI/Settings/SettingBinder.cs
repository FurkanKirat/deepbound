using UnityEngine;
using System;
using Core;
using Core.Events;
using Data.Serializable;
using Generated.Resource;
using Localization;
using Session;
using Settings;
using UnityEngine.EventSystems;
using Utils.Parsers;

namespace Visuals.UI.Settings
{
    public class SettingBinder : MonoBehaviour, ILocalizable, 
        IPointerEnterHandler,  IPointerExitHandler
    {
        [SerializeField] private TextButton button;
        [SerializeField] private Color normalColor = Color.white;
        [SerializeField] private Color highlightColor = Color.yellow;

        private SettingsManager _settings;
        private SettingEntry _entry;
        private string _key;
        private int _currentIndex;

        public void Initialize(SettingEntry entry, SettingsManager settings)
        {
            _entry = entry;
            _key = _entry.Key;
            _settings = settings;
            var val = _settings.Get(_key);

            switch (_entry.Type)
            {
                case SettingType.Bool:
                    _currentIndex = (bool)val ? 1 : 0;
                    break;

                case SettingType.Int:
                    if (_entry.Options != null && _entry.Options.Length > 0)
                    {
                        var options = Array.ConvertAll(_entry.Options, o => Convert.ToInt32(o));
                        var currentValue = _settings.Get<int>(_key);
                        _currentIndex = Math.Max(0, Array.IndexOf(options, currentValue));
                    }
                    break;

                case SettingType.String:
                case SettingType.Enum:
                    if (_entry.Options != null && _entry.Options.Length > 0)
                    {
                        var options = Array.ConvertAll(_entry.Options, o => o.ToString());
                        var currentValue = _settings.Get<string>(_key);
                        _currentIndex = Math.Max(0, Array.IndexOf(options, currentValue));
                    }
                    break;

                case SettingType.Float:
                    _currentIndex = 0; // typically not option-based, so 0 is fine
                    break;

                case SettingType.Int2:
                    if (_entry.Options != null && _entry.Options.Length > 0)
                    {
                        var currentValue = _settings.Get<Int2>(_key);
                        var options = Array.ConvertAll(_entry.Options, o =>
                        {
                            var str = o.ToString(); // e.g. "1920,1080"
                            return IntVectorParser.TryParse(str, out var tuple)
                                ? new Int2(tuple.x, tuple.y)
                                : Int2.Zero;
                        });

                        _currentIndex = Math.Max(0, Array.IndexOf(options, currentValue));
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            // Clamp index in case it's out of range
            if (_entry.Options != null && _entry.Options.Length > 0)
                _currentIndex = Mathf.Clamp(_currentIndex, 0, _entry.Options.Length - 1);

            UpdateDisplay();
            button.Text.color = normalColor;
            button.Button.onClick.AddListener(OnClick);
        }
        
        private void OnClick()
        {
            var val = _settings.Get(_key);

            switch (_entry.Type)
            {
                case SettingType.Bool:
                    var boolVal = (bool)val;
                    _settings.Set(_key, !boolVal);
                    _currentIndex = boolVal ? 1 : 0;
                    break;
                
                case SettingType.Int:
                    if (_entry.Options != null && _entry.Options.Length > 0)
                    {
                        var currentValue = _settings.Get<int>(_key);
                        var options = Array.ConvertAll(_entry.Options, o => Convert.ToInt32(o));

                        int index = Array.IndexOf(options, currentValue);
                        if (index < 0) index = 0;

                        index = (index + 1) % options.Length;
                        var nextValue = options[index];

                        _currentIndex = index;
                        _settings.Set(_key, nextValue);
                    }
                    break;

                case SettingType.String:
                case SettingType.Enum:
                    if (_entry.Options != null && _entry.Options.Length > 0)
                    {
                        var currentValue = _settings.Get<string>(_key);
                        var options = Array.ConvertAll(_entry.Options, o => o.ToString());

                        int index = Array.IndexOf(options, currentValue);
                        if (index < 0) index = 0;

                        index = (index + 1) % options.Length;
                        var nextValue = options[index];

                        _currentIndex = index;
                        _settings.Set(_key, nextValue);
                    }
                    break;
                case SettingType.Float:
                    float fval = (float)val;
                    fval += _entry.Step ?? 0f;
                    if (fval > _entry.Max) fval = _entry.Min ?? 0f;
                    _settings.Set(_key, fval);
                    break;
                
                case SettingType.Int2:
                    if (_entry.Options != null && _entry.Options.Length > 0)
                    {
                        var currentValue = _settings.Get<Int2>(_key);
                        
                        var options = Array.ConvertAll(_entry.Options, o =>
                        {
                            var str = o.ToString(); // "1920,1080"
                            return IntVectorParser.TryParse(str, out var tuple) 
                                ? new Int2(tuple.x, tuple.y) 
                                : Int2.Zero;
                        });
                        
                        int index = Array.IndexOf(options, currentValue);
                        if (index < 0) index = 0;
                        
                        index = (index + 1) % options.Length;
                        var nextValue = options[index];
                        
                        _currentIndex = index;
                        _settings.Set(_key, nextValue);
                    }
                    break;


                default:
                    throw new ArgumentOutOfRangeException();
            }

            UpdateDisplay();
        }

        private void UpdateDisplay()
        {
            string settingName = LocalizationDatabase.Get($"settings.{_key}");
            string settingValueStr;

            if (_entry.Type == SettingType.Bool)
            {
                settingValueStr = _settings.Get<bool>(_key) ? 
                    LocalizationDatabase.Get("settings.on") : 
                    LocalizationDatabase.Get("settings.off");
            }
            else if (_entry.Localize && _entry.OptionKeys != null && _entry.OptionKeys.Length > 0)
            {
                _currentIndex = Mathf.Clamp(_currentIndex, 0, _entry.OptionKeys.Length - 1);
                settingValueStr = LocalizationDatabase.Get(_entry.OptionKeys[_currentIndex]);
            }
            else if (_entry.OptionKeys != null && _entry.OptionKeys.Length > 0)
            {
                _currentIndex = Mathf.Clamp(_currentIndex, 0, _entry.OptionKeys.Length - 1);
                settingValueStr = _entry.OptionKeys[_currentIndex];
            }
            else
            {
                settingValueStr = _settings.Get(_key)?.ToString() ?? "—";
            }

            button.Text.text = $"{settingName}: {settingValueStr}";
        }

        public void Localize()
        {
            UpdateDisplay();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            button.Text.color = highlightColor;
            button.Text.transform.localScale = new Vector3(1.2f, 1.2f, 1);
            GameEventBus.Publish(new SfxPlayRequest(AudioIds.BlockPlacing));
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            button.Text.color = normalColor;
            button.Text.transform.localScale = Vector3.one;
        }
    }


}