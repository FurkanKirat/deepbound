using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Visuals.UI
{
    [RequireComponent(typeof(Button))]
    public class TextButton : MonoBehaviour
    {
        [SerializeField] private TMP_Text text;
        [SerializeField] private Button button;
        
        public TMP_Text Text => text;
        public Button Button => button;
    }

}