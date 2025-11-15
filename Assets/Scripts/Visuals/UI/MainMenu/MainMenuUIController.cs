using System;
using System.Collections.Generic;
using Core;
using UnityEngine;

namespace Visuals.UI.MainMenu
{
    public class MainMenuUIController : MonoBehaviour
    {
        [SerializeField] private GameObject mainPanel;
        [SerializeField] private GameObject settingsPanel;
        [SerializeField] private GameObject worldSelectPanel;
        [SerializeField] private GameObject worldCreatePanel;
        [SerializeField] private GameObject loadingPanel;
        [SerializeField] private GameObject creatingPanel;

        private readonly List<GameObject> _panels = new();
        private void Awake()
        {
            AddPanels(
                mainPanel, 
                settingsPanel, 
                worldSelectPanel, 
                worldCreatePanel,
                loadingPanel,
                creatingPanel
                );
            ShowMain();
        }
        private void OnEnable()
        {
            GameEventBus.Subscribe<OpenMenuRequest>(OnShowPanelRequest);
        }

        private void OnDisable()
        {
            GameEventBus.Unsubscribe<OpenMenuRequest>(OnShowPanelRequest);
        }

        private void OnShowPanelRequest(OpenMenuRequest e)
        {
            switch (e.PanelType)
            {
                case MenuPanelType.MainMenu:
                    ShowMain();
                    break;
                case MenuPanelType.WorldSelect:
                    ShowWorldSelect();
                    break;
                case MenuPanelType.Settings:
                    ShowOptions();
                    break;
                case MenuPanelType.WorldCreate:
                    ShowWorldCreate();
                    break;
                case MenuPanelType.LoadingGame:
                    ShowLoadingGame();
                    break;
                case MenuPanelType.CreatingWorld:
                    ShowCreatingWorld();
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        

        public void ShowMain()
        {
            HideAll();
            mainPanel.SetActive(true);
        }

        public void ShowOptions()
        {
            HideAll();
            settingsPanel.SetActive(true);
        }

        public void ShowWorldSelect()
        {
            HideAll();
            worldSelectPanel.SetActive(true);
        }

        public void ShowWorldCreate()
        {
            HideAll();
            worldCreatePanel.SetActive(true);
        }
        
        private void ShowLoadingGame()
        {
            HideAll();
            loadingPanel.SetActive(true);
        }

        private void ShowCreatingWorld()
        {
            HideAll();
            creatingPanel.SetActive(true);
        }

        private void HideAll()
        {
            foreach (var panel in _panels)
            {
                if (panel != null)
                {
                    panel.SetActive(false);
                }
            }
        }

        private void AddPanels(params GameObject[] panels)
        {
            if (panels == null)
                return;
            foreach (var panel in panels)
            {
                if (panel == null)
                    continue;
                _panels.Add(panel);
            }
            
        }
    }
}