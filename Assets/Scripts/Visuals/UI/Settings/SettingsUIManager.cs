using System.Collections.Generic;
using Core;
using Core.Events;
using Data.Database;
using Generated.Localization;
using Localization;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using Visuals.UI.MainMenu;

namespace Visuals.UI.Settings
{
    public class SettingsUIManager : MonoBehaviour, ILocalizable
    {
        [SerializeField] private SettingPanelUI panelUIPrefab;
        [SerializeField] private TextButton buttonPrefab;
        
        [SerializeField] private Transform panelsParent;
        [SerializeField] private Transform buttonsParent;
        [SerializeField] private bool isMainMenu;

        private readonly List<TextButton> _buttons = new();
        private readonly List<string> _buttonNames = new();
        private readonly List<SettingPanelUI> _panels = new();
        private void Start()
        {
            CreatePanels();
            Localize();
        }
        

        private void CreatePanels()
        {
            int i = 0;
            foreach (var panelData in Configs.SettingsConfig.Panels)
            {
                if ((isMainMenu && !panelData.Scope.HasFlag(SettingsScope.MainMenu)) ||
                    (!isMainMenu && !panelData.Scope.HasFlag(SettingsScope.Game)))
                    continue;
                
                var panel = Instantiate(panelUIPrefab, panelsParent);
                panel.Initialize(panelData);
                _panels.Add(panel);

                int index = i;
                AddButton(() => OnButtonClick(index), panelData.DisplayNameKey);
                panel.gameObject.SetActive(false);
                i++;
            }
            _panels[0].gameObject.SetActive(true);
            
            AddButton(OnCloseClick, LocalizationKeys.UiCloseMenu);
            if (!isMainMenu)
            {
                AddButton(OnSaveAndExitClicked, LocalizationKeys.UiSaveExit);
            }
            Localize();
        }

        private void AddButton(UnityAction onClick, string textKey)
        {
            var button = Instantiate(buttonPrefab, buttonsParent);
            button.Button.onClick.AddListener(onClick);
            _buttons.Add(button);
            _buttonNames.Add(textKey);
        }

        private void OnButtonClick(int index)
        {
            for (int i = 0; i < _panels.Count; i++)
            {
                var panel = _panels[i];
                panel.gameObject.SetActive(index == i);
            }
        }

        private void OnCloseClick()
        {
            if (isMainMenu)
                GameEventBus.Publish(new OpenMenuRequest(MenuPanelType.MainMenu));
            else
                GameEventBus.Publish(new SettingsToggleRequested());
        }

        private void OnSaveAndExitClicked()
        {
            var session = GameRoot.Instance.GameSession;
            session.GameManager.GameSaver.SaveWorld(true);
            session.EndSession();
            SceneManager.LoadScene("MainMenuScene");
        }

        public void Localize()
        {
            for (int i = 0; i < _buttons.Count; i++)
            {
                var button = _buttons[i];
                var text = LocalizationDatabase.Get(_buttonNames[i]);
                button.Text.text = text;
            }
        }
    }

}