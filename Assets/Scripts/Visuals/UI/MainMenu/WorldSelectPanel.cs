using Core;
using Core.Events;
using Generated.Localization;
using Localization;
using Systems.WorldSystem;
using TMPro;
using UnityEngine;
using Utils;

namespace Visuals.UI.MainMenu
{
    public class WorldSelectPanel : MonoBehaviour, ILocalizable
    {
        [SerializeField] private TextButton createWorldButton, backButton;
        [SerializeField] private TMP_Text titleText;
        [SerializeField] private UIList worldsList;
        private int _selectedWorld = -1;
        private void Awake()
        {
            createWorldButton.Button.onClick.AddListener(OnCreateWorldClicked);
            backButton.Button.onClick.AddListener(OnBackClicked);
            UpdateList();
            Localize();
        }

        private void OnEnable()
        {
            GameEventBus.Subscribe<WorldDeletedEvent>(OnWorldDeleted);
            GameEventBus.Subscribe<WorldItemClickedEvent>(OnWorldItemClickedEvent);
        }

        private void OnDisable()
        {
            GameEventBus.Unsubscribe<WorldDeletedEvent>(OnWorldDeleted);
            GameEventBus.Unsubscribe<WorldItemClickedEvent>(OnWorldItemClickedEvent);
        }

        private void UpdateList()
        {
            int counter = 0;
            worldsList.SetItems<WorldMetaData, WorldItemUIController>(WorldPathUtils.GetWorldMetaDataList(), 
                (component, world) =>
                {
                    component.Initialize(world);
                    component.Index = counter++;
                });
        }
        private static void OnCreateWorldClicked()
        {
            GameEventBus.Publish(new OpenMenuRequest(MenuPanelType.WorldCreate));
        }

        private static void OnBackClicked()
        {
            GameEventBus.Publish(new OpenMenuRequest(MenuPanelType.MainMenu));
        }

        private void OnWorldDeleted(WorldDeletedEvent e)
        {
            UpdateList();
        }

        public void Localize()
        {
            createWorldButton.Text.text = LocalizationDatabase.Get(LocalizationKeys.UiCreate);
            backButton.Text.text = LocalizationDatabase.Get(LocalizationKeys.UiBack);
            titleText.text = LocalizationDatabase.Get(LocalizationKeys.UiWorldSelect);
        }

        private void OnWorldItemClickedEvent(WorldItemClickedEvent e)
        {
            var clicked = worldsList.Get<WorldItemUIController>(e.Index);
            clicked.Selected = !clicked.Selected;
            
            if (_selectedWorld >= 0 && e.Index != _selectedWorld)
            {
                var old = worldsList.Get<WorldItemUIController>(_selectedWorld);
                old.Selected = false;
            }
            
            _selectedWorld = e.Index;

        }
    }
}