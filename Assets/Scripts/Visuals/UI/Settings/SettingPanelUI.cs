using Core;
using Localization;
using Settings;
using TMPro;
using UnityEngine;

namespace Visuals.UI.Settings
{
    public class SettingPanelUI : MonoBehaviour, ILocalizable
    {
        [SerializeField] private TMP_Text title;
        [SerializeField] private SettingBinder settingBinderPrefab;
        [SerializeField] private Transform settingsBinderParent;

        private SettingsPanel _panelData;
        public void Initialize(SettingsPanel panelData)
        {
            name = panelData.Id;
            _panelData = panelData;
            
            foreach (var entry in panelData.Settings)
            {
                var binder = Instantiate(settingBinderPrefab, settingsBinderParent);
                binder.Initialize(entry, GameRoot.Instance.SettingsManager);
            }
            Localize();
        }

        public void Localize()
        {
            title.text = LocalizationDatabase.Get(_panelData.DisplayNameKey);
        }
    }
}