using Core;
using Generated.Localization;
using Localization;
using UnityEngine;

namespace Visuals.UI.MainMenu
{
    public class MainMenuPanel : MonoBehaviour, ILocalizable
    {
        [SerializeField] private TextButton playButton;
        [SerializeField] private TextButton settingsButton;
        [SerializeField] private TextButton exitButton;
        private void Awake()
        {
            playButton.Button.onClick.AddListener(OnPlayClicked);
            exitButton.Button.onClick.AddListener(OnExitClicked);
            settingsButton.Button.onClick.AddListener(OnSettingsClicked);
            Localize();
        }
        
        private static void OnPlayClicked()
        {
            GameEventBus.Publish(new OpenMenuRequest(MenuPanelType.WorldSelect));
        }
        private static void OnSettingsClicked()
        {
            GameEventBus.Publish(new OpenMenuRequest(MenuPanelType.Settings));
        }

        private static void OnExitClicked()
        {
            Application.Quit();
        }

        public void Localize()
        {
            playButton.Text.text = LocalizationDatabase.Get(LocalizationKeys.UiPlay);
            exitButton.Text.text = LocalizationDatabase.Get(LocalizationKeys.UiExit);
            settingsButton.Text.text = LocalizationDatabase.Get(LocalizationKeys.UiSettings);
        }
    }
}