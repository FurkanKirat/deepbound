using UnityEngine;

namespace Visuals.UI.MainMenu
{
    public class LoadingPanel : MonoBehaviour
    {
        [SerializeField] private ProgressBar progressBar;

        private void Awake()
        {
            progressBar?.ChangeProgress(0.5f);
        }
    }
}