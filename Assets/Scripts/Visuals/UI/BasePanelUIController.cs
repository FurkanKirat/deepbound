using Core;
using Core.Events;
using Data.Database;
using UnityEngine;

namespace Visuals.UI
{
    public abstract class BasePanelUIController : MonoBehaviour, IUIPanelController
    {
        [SerializeField] protected GameObject panelRoot;

        public bool IsOpen { get; private set; }
        public abstract bool BlocksWorldInteraction { get; }
        public abstract bool PausesGame { get; }
        public abstract UIPanelType PanelType { get; }
        protected abstract string OpenSound { get; }
        protected abstract string CloseSound { get; }
      

        protected virtual void Start()
        {
            UIPanelManager.Instance?.Register(this);
        }

        protected virtual void OnDestroy()
        {
            UIPanelManager.Instance?.Unregister(this);
        }

        public virtual void Open()
        {
            panelRoot.SetActive(true);
            IsOpen = true;
            UIPanelManager.Instance?.NotifyOpened(this);
            GameEventBus.Publish(new UIPanelOpenedEvent(PanelType, BlocksWorldInteraction, PausesGame));
            
            PlayOpenSound();
        }

        public virtual void Close()
        {
            panelRoot.SetActive(false);
            IsOpen = false;
            UIPanelManager.Instance?.NotifyClosed(this);
            GameEventBus.Publish(new UIPanelClosedEvent(PanelType, BlocksWorldInteraction, PausesGame));
            PlayCloseSound();
        }

        private void PlayOpenSound()
        {
            if (ResourceDatabases.Sounds.TryGet(OpenSound, out var sound))
                GameEventBus.Publish(new SfxPlayRequest(sound));
        }

        private void PlayCloseSound()
        {
            if (ResourceDatabases.Sounds.TryGet(CloseSound, out var sound))
                GameEventBus.Publish(new SfxPlayRequest(sound));
        }

        public void Toggle()
        {
            if (IsOpen) Close();
            else Open();
        }
        
    }

}