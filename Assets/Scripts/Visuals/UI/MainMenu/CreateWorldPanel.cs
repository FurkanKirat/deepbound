using Core;
using Data.Models.References;
using Generated.Localization;
using Localization;
using TMPro;
using UnityEngine;
using Utils;

namespace Visuals.UI.MainMenu
{
    public class CreateWorldPanel : MonoBehaviour, ILocalizable
    {
        [SerializeField] private TextButton createButton, backButton;
        [SerializeField] private TMP_InputField seedInputField, worldNameInputField;
        [SerializeField] private TMP_Text titleText;
        
        private void Awake()
        {
            createButton.Button.onClick.AddListener(OnCreateClicked);
            backButton.Button.onClick.AddListener(OnBackClicked);
            Localize();
        }

        private void OnBackClicked()
        {
            GameEventBus.Publish(new OpenMenuRequest(MenuPanelType.WorldSelect));
        }

        private void OnCreateClicked()
        {
            string worldName = worldNameInputField.text;
            if (string.IsNullOrEmpty(worldName))
            {
                worldName = "New World";
            }
            if (!int.TryParse(seedInputField.text, out int seed))
            {
                seed = DeterministicHash.Fnv1aHash(seedInputField.text);
            }
            GameRoot.Instance.GameSession.StartWorldCreation(worldName, seed, new SpriteRef("Sprites/Entities/Player/player3"));
        }

        public void Localize()
        {
            createButton.Text.text = LocalizationDatabase.Get(LocalizationKeys.UiCreate);
            backButton.Text.text = LocalizationDatabase.Get(LocalizationKeys.UiBack);
            titleText.text = LocalizationDatabase.Get(LocalizationKeys.UiCreateWorld);
        }
    }
}