using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Visuals.UI
{
    public class ProgressBar : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Image progressBar;
        
        [Header("Options")]
        [SerializeField] private Color emptyColor;
        [SerializeField] private Color fullColor;
        [SerializeField] private float fillAmount;
        
        private Coroutine _lerpRoutine;

        private void Awake()
        {
            progressBar.fillAmount = fillAmount;
            progressBar.color = Color.Lerp(emptyColor, fullColor, fillAmount);
        }

        public void ChangeProgress(float targetFill)
        {
            if (_lerpRoutine != null)
                StopCoroutine(_lerpRoutine);
            _lerpRoutine = StartCoroutine(LerpFill(targetFill));
        }
        
        private IEnumerator LerpFill(float target)
        {
            float duration = 0.5f;
            float elapsed = 0f;
            float start = progressBar.fillAmount;
            Color startColor = progressBar.color;
            Color targetColor = Color.Lerp(emptyColor, fullColor, target);


            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / duration;
                progressBar.fillAmount = Mathf.Lerp(start, target, t);
                progressBar.color = Color.Lerp(startColor, targetColor, t);
                yield return null;
            }

            progressBar.fillAmount = target;
            progressBar.color = targetColor;
        }
        
    }
}