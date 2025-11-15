using System.Collections;
using Core;
using Core.Events;
using UnityEngine;
using Visuals.Interfaces;

namespace Visuals.ObjectVisuals.Player
{
    public class HandAnimator : MonoBehaviour, IClientTickable
    {
        [SerializeField] private SpriteRenderer handRenderer;
        public Vector2 rightHandOffset;
        public Vector2 leftHandOffset;
        
        private bool _isPrimaryUseHeld = false;
        private bool _isAnimationPlaying = false;

        private void OnEnable()
        {
            GameEventBus.Subscribe<PrimaryUseClicked>(OnPrimaryUseClicked);
            GameEventBus.Subscribe<PrimaryUseStarted>(OnPrimaryUseStarted);
            GameEventBus.Subscribe<PrimaryUseEnded>(OnPrimaryUseEnded);
            ClientGameLoop.Instance.Register(this);
        }
        
        private void OnDisable()
        {
            GameEventBus.Unsubscribe<PrimaryUseClicked>(OnPrimaryUseClicked);
            GameEventBus.Unsubscribe<PrimaryUseStarted>(OnPrimaryUseStarted);
            GameEventBus.Unsubscribe<PrimaryUseEnded>(OnPrimaryUseEnded);
            ClientGameLoop.Instance?.Unregister(this);
        }

        public void ClientTick(float deltaTime)
        {
            if (_isPrimaryUseHeld && !_isAnimationPlaying)
            {
                StartCoroutine(UseItemSwing());
            }
        }

        private void OnPrimaryUseEnded(PrimaryUseEnded obj)
        {
            _isPrimaryUseHeld = false;
        }

        private void OnPrimaryUseStarted(PrimaryUseStarted obj)
        {
            _isPrimaryUseHeld = true;
            if (!_isAnimationPlaying)
                StartCoroutine(UseItemSwing());
        }

        private void OnPrimaryUseClicked(PrimaryUseClicked obj)
        {
            if (!_isAnimationPlaying)
                StartCoroutine(UseItemSwing());
        }

        public void SetFacing(bool facingRight)
        {
            if (facingRight)
            {
                transform.localPosition = rightHandOffset;
                handRenderer.flipX = false;
            }
            else
            {
                transform.localPosition = leftHandOffset;
                handRenderer.flipX = true;
            }
        }
        
        private IEnumerator UseItemSwing()
        {
            float time = 0;
            float duration = 0.2f;
            float startAngle = transform.localEulerAngles.z;
            float endAngle = startAngle + 90f;
            _isAnimationPlaying = true;
    
            while (time < duration)
            {
                float angle = Mathf.Lerp(startAngle, endAngle, time / duration);
                transform.localRotation = Quaternion.Euler(0, 0, angle);
                time += Time.deltaTime;
                yield return null;
            }

            time = 0;
            while (time < duration)
            {
                float angle = Mathf.Lerp(endAngle, startAngle, time / duration);
                transform.localRotation = Quaternion.Euler(0, 0, angle);
                time += Time.deltaTime;
                yield return null;
            }

            transform.localRotation = Quaternion.Euler(0, 0, startAngle);
            _isAnimationPlaying = false;
        }

    }
}