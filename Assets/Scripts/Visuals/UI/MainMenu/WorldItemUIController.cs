using Core;
using Generated.Localization;
using Generated.Settings;
using Interfaces;
using Localization;
using Systems;
using Systems.WorldSystem;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Utils;

namespace Visuals.UI.MainMenu
{
    public class WorldItemUIController : MonoBehaviour,
        IInitializable<WorldMetaData>,
        IPointerClickHandler,
        ILocalizable
    {
        [SerializeField] private Button playButton, deleteButton;
        [SerializeField] private TMP_Text txtWorldName, txtLastPlayed, txtCreatedAt;
        [SerializeField] private Image worldIcon, selectedFrame;
        private WorldMetaData _metaData;
        private float _lastClick = 0f;
        public int Index { get; set; }

        public bool Selected
        {
            get => selectedFrame.enabled;
            set => selectedFrame.enabled = value;
        }
        
        public void Initialize(WorldMetaData data)
        {
            _metaData = data;
            txtWorldName.text = _metaData.WorldName;
            var sprite = ImageLoader.LoadSprite(WorldPathUtils.GetWorldIconPath(_metaData), pixelsPerUnit: 64f);
            worldIcon.sprite = sprite != null ? 
                sprite : 
                worldIcon.sprite;
            playButton.onClick.AddListener(OnPlayClicked);
            deleteButton.onClick.AddListener(OnDeleteClicked);
            Localize();
        }

        private void OnPlayClicked()
        {
            GameRoot.Instance.GameSession.StartWorldLoad(_metaData);
        }

        private void OnDeleteClicked()
        {
            var settingsManager = GameRoot.Instance.SettingsManager;
            var destroy = settingsManager.Get<bool>(SettingsKeys.DestroyWorldCompletely);
            WorldDeleter.DeleteWorld(_metaData, destroy);
        }

        public void Localize()
        {
            if (_metaData == null)
                return;
            
            txtCreatedAt.text = $"{LocalizationDatabase.Get(LocalizationKeys.UiCreatedAt)}:  {_metaData.CreatedAt.ToLocalTime():g}";
            txtLastPlayed.text = $"{LocalizationDatabase.Get(LocalizationKeys.UiLastPlayedAt)}: {_metaData.LastSavedAt.ToLocalTime():g}";
        }
        

        public void OnPointerClick(PointerEventData eventData)
        {
            if (Time.time - _lastClick < 0.2f)
            {
                OnPlayClicked();
            }
            else
            {
                _lastClick = Time.time;
                GameEventBus.Publish(new WorldItemClickedEvent(Index));
            }
            
        }
    }
}