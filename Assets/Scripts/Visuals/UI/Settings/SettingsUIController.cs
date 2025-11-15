using Core;
using Core.Events;

namespace Visuals.UI.Settings
{
    public class SettingsUIController : BasePanelUIController
    {
        public override bool BlocksWorldInteraction => true;
        public override bool PausesGame => true;
        public override UIPanelType PanelType => UIPanelType.Settings;
        protected override string OpenSound => null;
        protected override string CloseSound => null;

        private void OnEnable()
        {
            GameEventBus.Subscribe<SettingsToggleRequested>(OnSettingsToggleRequested);
        }

        private void OnDisable()
        {
            GameEventBus.Unsubscribe<SettingsToggleRequested>(OnSettingsToggleRequested);
        }

        private void OnSettingsToggleRequested(SettingsToggleRequested obj)
        {
            Toggle();
        }
    }
}