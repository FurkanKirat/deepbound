using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Visuals.UI.ErrorSystem
{
    public class ErrorButton : MonoBehaviour
    {
        [SerializeField] private Button button;
        [SerializeField] private TMP_Text text;
        public Button Button => button;
        public TMP_Text Text => text;
    }
}