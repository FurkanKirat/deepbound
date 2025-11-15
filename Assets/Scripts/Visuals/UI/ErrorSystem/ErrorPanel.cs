using TMPro;
using UnityEngine;
using Visuals.Utils;

namespace Visuals.UI.ErrorSystem
{
    public class ErrorPanel : MonoBehaviour
    {
        [Header("UI Elements")]
        [SerializeField] private Transform buttonContainer;
        [SerializeField] private TMP_Text errorText, errorTitle;
        
        [Header("Prefabs")]
        [SerializeField] private ErrorButton buttonPrefab;
        
        private ObjectPool<ErrorButton> _buttonPool;

        public void UpdatePanel(string title, string text, ButtonEventData[] buttonsData)
        {
            errorText.text = text;
            errorTitle.text = title;

            _buttonPool ??= new ObjectPool<ErrorButton>(buttonPrefab, 3, buttonContainer);
            _buttonPool.ReleaseAll();

            foreach (var buttonData in buttonsData)
            {
                var errorButton = _buttonPool.Get();
                errorButton.Button.onClick.RemoveAllListeners();
                errorButton.Button.onClick.AddListener(() => buttonData.OnClick());
                errorButton.Button.onClick.AddListener(ClosePanel);
                errorButton.Text.text = buttonData.Text;
            }
        }
        
        private void ClosePanel() => gameObject.SetActive(false);
    }
}