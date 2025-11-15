using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Visuals.UI.InventorySystem.UIControllers;

namespace Visuals.UI
{
    public class UIPanelManager : MonoBehaviour
    {
        [SerializeField] private GameObject blurLayer;
        [SerializeField] private InventoryRootPanelController inventoryRootPanelController;
        public static UIPanelManager Instance { get; private set; }
        public InventoryRootPanelController InventoryRootPanelController => inventoryRootPanelController;

        private readonly List<IUIPanelController> _registeredPanels = new();
        private readonly List<IUIPanelController> _activePanels = new();

        private void Awake()
        {
            if (Instance != null && Instance != this)
                Destroy(gameObject);
            else
                Instance = this;
        }

        private void OnDestroy()
        {
            if (Instance == this)
                Instance = null;
        }

        public void Register(IUIPanelController panelController)
        {
            if (!_registeredPanels.Contains(panelController))
                _registeredPanels.Add(panelController);
        }

        public void Unregister(IUIPanelController panelController)
        {
            _registeredPanels.Remove(panelController);
            _activePanels.Remove(panelController);
        }
        
        public void NotifyOpened(IUIPanelController panel)
        {
            if (!_activePanels.Contains(panel))
                _activePanels.Add(panel);

            UpdateState();
        }

        public void NotifyClosed(IUIPanelController panel)
        {
            _activePanels.Remove(panel);
            UpdateState();
        }
        
        public void CloseTopPanel()
        {
            if (_activePanels.Count == 0) return;

            var top = _activePanels[^1];
            if (top.BlocksWorldInteraction)
                top.Close();
        }
        
        public void CloseAll()
        {
            foreach (var panel in _activePanels.ToArray())
                panel.Close();
        }

        public bool IsAnyBlockingPanelOpen 
            => _activePanels.Any(p => p.BlocksWorldInteraction);

        public bool IsAnyPanelOpen 
            => _activePanels.Count > 0;
        
        private void UpdateState()
        {
            blurLayer?.SetActive(IsAnyBlockingPanelOpen);
        }

    }


}