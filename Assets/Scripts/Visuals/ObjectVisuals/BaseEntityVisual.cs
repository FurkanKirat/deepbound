using Core;
using Systems.EntitySystem.Interfaces;
using UnityEngine;
using Visuals.Interfaces;
using Visuals.UI.HealthSystem;

namespace Visuals.ObjectVisuals
{
    public abstract class BaseEntityVisual<T> : MonoBehaviour, 
        IClientTickable,
        IClientLateTickable,
        IEntityVisual
        where T : IPhysicalEntity
    {
        [SerializeField] private GameObject entity;
        public GameObject Object => entity.gameObject;
        protected Transform EntityTransform => entity.transform;
        public SpriteRenderer Renderer { get; private set; }
        private EntityHealthBar HealthBar { get; set; }

        private readonly Interpolator _interpolator = new();
        private float _accumulator;
        protected bool IsActive = false;
        public T Data { get; private set; }
        
        public virtual void ClientTick(float deltaTime)
        {
            if (!IsActive) return;
            _accumulator += deltaTime;
            
            while (_accumulator >= 0.05f)
            {
                _accumulator -= 0.05f;
                _interpolator.Tick(Data.Position.ToVector3());
            }
            
        }
        
        public void AttachHealthBar(EntityHealthBar healthBar)
        {
            HealthBar = healthBar;
        }
        public void Initialize(T data)
        {
            Renderer ??= entity.GetComponent<SpriteRenderer>();
            Data = data;
        }

        public virtual void OnSpawn()
        {
            var pos = Data.Position.ToVector3();
            EntityTransform.position = pos;
            _interpolator.Reset(pos);
            _accumulator = 0f;
            IsActive = true;
            Register();
        }

        public virtual void OnDespawn(EntityVisualizer entityVisualizer)
        {
            if (HealthBar != null)
            {
                entityVisualizer.DespawnHealthBar(HealthBar);
                HealthBar = null;
            }

            IsActive = false;
            Unregister();
            
        }

        private void Register()
        {
            ClientGameLoop.Instance.Register((IClientTickable)this);
            ClientGameLoop.Instance.Register((IClientLateTickable)this);
        }

        private void Unregister()
        {
            ClientGameLoop.Instance?.Unregister((IClientTickable)this);
            ClientGameLoop.Instance?.Unregister((IClientLateTickable)this);
        }

        public void ClientLateTick(float deltaTime)
        {
            if (!IsActive) return;

            EntityTransform.position = _interpolator.GetInterpolated();
        }
    }
}