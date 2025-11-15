using Core;
using Core.Events;
using UnityEngine;

namespace Visuals.UI
{
    public class UIPanelEscapeHandler : MonoBehaviour
    {
        private void OnEnable()
        {
            GameEventBus.Subscribe<EscapePressedEvent>(OnEscape);
        }

        private void OnDisable()
        {
            GameEventBus.Unsubscribe<EscapePressedEvent>(OnEscape);
        }

        private void OnEscape(EscapePressedEvent _)
        {
            if (UIPanelManager.Instance.IsAnyPanelOpen)
                UIPanelManager.Instance.CloseTopPanel();
            else
                GameEventBus.Publish(new SettingsToggleRequested());
        }
    }

}