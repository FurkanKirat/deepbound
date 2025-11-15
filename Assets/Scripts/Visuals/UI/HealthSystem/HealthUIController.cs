using System.Collections;
using Core;
using Core.Context;
using Core.Events;
using Interfaces;
using Systems.EntitySystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Visuals.UI.HealthSystem
{
    public class HealthUIController : 
        MonoBehaviour,
        IInitializable<ClientContext>
    {
        [SerializeField] private Image healthBar;
        [SerializeField] private TMP_Text healthText;
        private Coroutine _lerpRoutine;

        private void OnEnable()
        {
            GameEventBus.Subscribe<HealthChangedEvent>(OnHealthChangedEvent);
            GameEventBus.Subscribe<PlayerSpawnEvent>(OnPlayerSpawnEvent);
        }

        private void OnDisable()
        {
            GameEventBus.Unsubscribe<HealthChangedEvent>(OnHealthChangedEvent);
            GameEventBus.Unsubscribe<PlayerSpawnEvent>(OnPlayerSpawnEvent);
        }
        
        public void Initialize(ClientContext data)
        {
            var player = data.Player;
            OnHealthChanged(player.CurrentHealth, player.MaxHealth);
        }

        private void OnPlayerSpawnEvent(PlayerSpawnEvent e)
        {
            OnHealthChanged(e.Player.CurrentHealth, e.Player.MaxHealth);
        }

        private void OnHealthChangedEvent(HealthChangedEvent e)
        {
            if (e.Entity.Type != EntityType.Player) return;
            OnHealthChanged(e.CurrentHealth, e.MaxHealth);
        }

        private void OnHealthChanged(float currentHealth, float maxHealth)
        {
            float targetFill = currentHealth / maxHealth;
            
            healthText.text = $"{currentHealth}/{maxHealth}";
            if (_lerpRoutine != null)
                StopCoroutine(_lerpRoutine);
            _lerpRoutine = StartCoroutine(LerpFill(targetFill));
        }

        private IEnumerator LerpFill(float target)
        {
            float duration = 0.5f;
            float elapsed = 0f;
            float start = healthBar.fillAmount;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                healthBar.fillAmount = Mathf.Lerp(start, target, elapsed / duration);
                yield return null;
            }

            healthBar.fillAmount = target;
        }

        
    }
}