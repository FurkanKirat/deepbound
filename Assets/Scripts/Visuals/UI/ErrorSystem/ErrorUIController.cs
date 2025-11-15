using Core;
using Core.Events;
using UnityEngine;

namespace Visuals.UI.ErrorSystem
{
    public class ErrorUIController : MonoBehaviour
    {
        [SerializeField] private ErrorPanel errorPanel;
        private void OnEnable()
        {
           GameEventBus.Subscribe<OpenErrorPanelRequest>(OnErrorPanelRequest);
        }
        
        private void OnDisable()
        {
            GameEventBus.Unsubscribe<OpenErrorPanelRequest>(OnErrorPanelRequest);
        }
        
        private void OnErrorPanelRequest(OpenErrorPanelRequest e)
        {
            errorPanel.UpdatePanel(e.Title, e.Text, e.ButtonEvents);
            errorPanel.gameObject.SetActive(true);
        }
        
    }
}