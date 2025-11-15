using Core;
using Core.Events;

namespace Visuals.UI.ChatSystem
{
    public class ChatPanelUIController : BasePanelUIController
    {
        private ChatPanel _chatPanel;
        public override bool BlocksWorldInteraction => true;
        public override bool PausesGame => false;
        public override UIPanelType PanelType => UIPanelType.Chat;
        protected override string OpenSound => null;
        protected override string CloseSound => null;


        protected override void Start()
        {
            base.Start();
            _chatPanel = GetComponentInChildren<ChatPanel>(true);
        }

        private void OnEnable()
        {
            GameEventBus.Subscribe<ChatToggleRequested>(OnToggleRequested);
            GameEventBus.Subscribe<ChatSubmitOrToggleEvent>(OnSubmitOrToggle);
        }

        private void OnDisable()
        {
            GameEventBus.Unsubscribe<ChatToggleRequested>(OnToggleRequested);
            GameEventBus.Unsubscribe<ChatSubmitOrToggleEvent>(OnSubmitOrToggle);
        }

        private void OnToggleRequested(ChatToggleRequested e) => Toggle();

        private void OnSubmitOrToggle(ChatSubmitOrToggleEvent _)
        {
            if (IsOpen)
            {
               _chatPanel.SubmitMessage();
               Close();
            }
            else
            {
                Open();
            }
        }
    }
}