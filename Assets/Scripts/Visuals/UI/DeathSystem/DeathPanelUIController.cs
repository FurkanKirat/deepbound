using Core;
using Core.Events;

namespace Visuals.UI.DeathSystem
{
    public class DeathPanelUIController : BasePanelUIController
    {
        public override bool BlocksWorldInteraction => true;
        public override bool PausesGame => false;
        public override UIPanelType PanelType => UIPanelType.Death;
        protected override string OpenSound => null;
        protected override string CloseSound => null;
        private void OnEnable()
        {
            GameEventBus.Subscribe<PlayerSpawnProgressEvent>(OnPlayerSpawnProgressEvent);
        }

        private void OnDisable()
        {
            GameEventBus.Unsubscribe<PlayerSpawnProgressEvent>(OnPlayerSpawnProgressEvent);
        }
        
        private void OnPlayerSpawnProgressEvent(PlayerSpawnProgressEvent e)
        {
            if (!IsOpen && e.RemainingTime > 0)
                Open();
            else if (IsOpen && e.RemainingTime <= 0)
                Close();
            
        }
    }
}