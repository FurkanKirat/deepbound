using Core;
using Core.Context.Spawn;
using Visuals.Interfaces;
using Visuals.Utils;

namespace Visuals.Vfx
{
    using UnityEngine;

    public class Trail : MonoBehaviour, IClientTickable
    {
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private Sprite defaultSprite;

        private ObjectPool<Trail> _pool;
        private float _lifetime;
        private float _elapsed;
        private Vector3 _initialScale;
        private Vector3 _finalScale;
        private Color _startColor;
        private Color _endColor;
        private Vector2 _velocity;

        public void Initialize(
            ObjectPool<Trail> pool,
            TrailSpawnContext ctx)
        {
            _pool = pool;
            
            transform.position = ctx.Position;
            transform.rotation = ctx.Rotation;

            _lifetime = ctx.LifeTime;
            _elapsed = 0f;
            _initialScale = new Vector3(ctx.StartScale, ctx.StartScale, ctx.StartScale);
            _finalScale = new Vector3(ctx.FinalScale, ctx.FinalScale, ctx.FinalScale);
            _startColor = ctx.StartColor;
            _endColor = ctx.FinalColor;

            spriteRenderer.sprite = ctx.Sprite ?? defaultSprite;
            spriteRenderer.color = _startColor;
            transform.localScale = _initialScale;
            _velocity = ctx.Velocity;
        }

        private void OnEnable()
        {
            ClientGameLoop.Instance.Register(this);
        }

        private void OnDisable()
        {
            ClientGameLoop.Instance?.Unregister(this);
        }
        
        public void ClientTick(float deltaTime)
        {
            _elapsed += deltaTime;
            float t = Mathf.Clamp01(_elapsed / _lifetime);

            transform.position += (Vector3)(_velocity * deltaTime);
            // Fade color
            spriteRenderer.color = Color.Lerp(_startColor, _endColor, t);

            // Scale
            transform.localScale = Vector3.Lerp(_initialScale, _finalScale, t);

            if (_elapsed >= _lifetime)
            {
                _pool.Release(this);
            }
        }
    }

}