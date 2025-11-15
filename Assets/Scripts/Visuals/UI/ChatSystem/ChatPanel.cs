using Core;
using Core.Context;
using Core.Events;
using Interfaces;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Utils;

namespace Visuals.UI.ChatSystem
{
    public class ChatPanel : MonoBehaviour, IInitializable<ClientContext>
    {
        [Header("References")]
        [SerializeField] private TMP_InputField inputField;
        [SerializeField] private TextMeshProUGUI chatDisplay;
        [SerializeField] private ScrollRect scrollRect;

        private ClientContext _context;
        private bool _isInitialized;

        public void Initialize(ClientContext data)
        {
            _context = data;
            _isInitialized = true;
            chatDisplay.richText = true;
        }

        private void OnEnable()
        {
            GameEventBus.Subscribe<ChatEvent>(OnChatEvent);
            EventSystem.current.SetSelectedGameObject(inputField.gameObject);
        }

        private void OnDisable()
        {
            GameEventBus.Unsubscribe<ChatEvent>(OnChatEvent);
        }

        private void OnChatEvent(ChatEvent evt)
        {
            string formatted = ChatFormatter.Format(evt);
            GameLogger.Log($"[ChatUIController] OnChatEvent → formatted = {formatted}");
            chatDisplay.text += formatted + "\n";
            
            LayoutRebuilder.ForceRebuildLayoutImmediate(chatDisplay.rectTransform);
            Canvas.ForceUpdateCanvases();
            scrollRect.verticalNormalizedPosition = 0f;
        }

        public void SubmitMessage()
        {
            if (!_isInitialized) return;

            string rawText = inputField.text;

            if (!string.IsNullOrWhiteSpace(rawText))
            {
                ChatHandler.ProcessInput(rawText, _context);
                inputField.text = "";
                inputField.ActivateInputField();
            }
        }
    }
}