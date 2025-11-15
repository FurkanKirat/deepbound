using Core;
using Core.Events;
using Generated.Localization;
using Localization;
using TMPro;
using UnityEngine;

namespace Visuals.UI.DeathSystem
{
    public class DeathPanel : MonoBehaviour, ILocalizable
    {
        [Header("References")]
        [SerializeField] private TMP_Text deathText;
        [SerializeField] private TMP_Text respawnText;

        private int _currentText;

        private void Awake()
        {
            Localize();
        }
        public void OnEnable()
        {
            GameEventBus.Subscribe<PlayerSpawnProgressEvent>(OnPlayerSpawnProgress);
        }

        public void OnDisable()
        {
            GameEventBus.Unsubscribe<PlayerSpawnProgressEvent>(OnPlayerSpawnProgress);
        }

        private void OnPlayerSpawnProgress(PlayerSpawnProgressEvent e)
        {
            var remaining = e.RemainingTime;
            var remainingInt = Mathf.FloorToInt(remaining);
            
            if (remainingInt != _currentText)
            {
                _currentText = remainingInt;
                respawnText.text = remainingInt.ToString();
            }
        }

        public void Localize()
        {
            deathText.text = LocalizationDatabase.Get(LocalizationKeys.UiDeath);
        }
    }
}