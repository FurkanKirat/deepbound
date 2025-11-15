using System.Collections;
using Core;
using Core.Events;
using UnityEngine;
using UnityEngine.UI;
using Visuals.Interfaces;

namespace Visuals.UI.HealthSystem
{
    public class EntityHealthBar : MonoBehaviour, IClientTickable
    {
        [SerializeField] private Image healthBar;
        [SerializeField] private Color healthyColor = Color.green;
        [SerializeField] private Color lowHealthColor = Color.red;
        private Coroutine _lerpRoutine;
        private int EntityId {get; set;}
        private Transform _entityTransform;
        private Camera _camera;
        public void ClientTick(float deltaTime)
        {
            FollowEntity();
        }

        public void Initialize(float currentHealth, float maxHealth, int entityId, Transform entityTransform, Camera mainCamera)
        {
            var healthRatio = currentHealth / maxHealth;
            healthBar.fillAmount = healthRatio;
            healthBar.color = Color.Lerp(lowHealthColor, healthyColor, healthRatio);
            EntityId = entityId;
            _entityTransform = entityTransform;
            _camera = mainCamera;
        }

        private void OnEnable()
        {
            ClientGameLoop.Instance.Register(this);
            GameEventBus.Subscribe<HealthChangedEvent>(OnHealthChangedEvent);
        }

        private void OnDisable()
        {
            ClientGameLoop.Instance?.Unregister(this);
            GameEventBus.Unsubscribe<HealthChangedEvent>(OnHealthChangedEvent);
        }
        
        private void OnHealthChangedEvent(HealthChangedEvent e)
        {
            if (e.Entity.Id != EntityId) return;
            OnHealthChanged(e.CurrentHealth, e.MaxHealth);
        }

        private void OnHealthChanged(float currentHealth, float maxHealth)
        {
            float targetFill = currentHealth / maxHealth;
            
            if (_lerpRoutine != null)
                StopCoroutine(_lerpRoutine);
            _lerpRoutine = StartCoroutine(LerpFill(targetFill));
        }

        private IEnumerator LerpFill(float target)
        {
            float duration = 0.5f;
            float elapsed = 0f;
            float start = healthBar.fillAmount;
            Color startColor = healthBar.color;
            Color targetColor = Color.Lerp(lowHealthColor, healthyColor, target);


            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / duration;
                healthBar.fillAmount = Mathf.Lerp(start, target, t);
                healthBar.color = Color.Lerp(startColor, targetColor, t);
                yield return null;
            }

            healthBar.fillAmount = target;
            healthBar.color = targetColor;
        }

        private void FollowEntity()
        {
            Vector3 worldPos = _entityTransform.position + new Vector3(0, 0.6f, 0);
            Vector3 screenPos = _camera.WorldToScreenPoint(worldPos);
            transform.position = screenPos;
        }
    }
}