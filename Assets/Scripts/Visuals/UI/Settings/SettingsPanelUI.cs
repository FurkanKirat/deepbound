using Core;
using Generated.Localization;
using Localization;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Visuals.UI.Settings
{
    public class SettingsPanelUI : MonoBehaviour, ILocalizable
    {
        [SerializeField] private TextButton exitButton;
        [SerializeField] private TextButton applyButton;

        private void Awake()
        {
            exitButton.Button.onClick.AddListener(OnExitClicked);
            Localize();
        }

        private static void OnExitClicked()
        {
            var session = GameRoot.Instance.GameSession;
            session.GameManager.TickManager.Stop();
            session.GameManager.GameSaver.SaveWorld(true);
            session.Dispose();
            GameEventBus.LogListeners(false);
            SceneManager.LoadScene("MainMenuScene");
        }

        public void Localize()
        {
            exitButton.Text.text = LocalizationDatabase.Get(LocalizationKeys.UiSaveExit);
            applyButton.Text.text = LocalizationDatabase.Get(LocalizationKeys.UiApply);
        }
    }
}