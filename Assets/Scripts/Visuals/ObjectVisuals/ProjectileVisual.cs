using Core;
using Core.Events;
using Systems.EntitySystem.Interfaces;
using Systems.EntitySystem.Projectile;
using UnityEngine;

namespace Visuals.ObjectVisuals
{
    public class ProjectileVisual : BaseEntityVisual<IProjectile>
    {
        [SerializeField] private float trailSpawnRate = 0.05f;
        private float _trailAccumulator;
        
        public override void OnSpawn()
        {
            Renderer.sprite = Data.ProjectileData.Icon.Load();
            base.OnSpawn();
        }
        public override void ClientTick(float deltaTime)
        {
            if (!IsActive)
                return;
            
            base.ClientTick(deltaTime);

            Vector2 direction = Data.Velocity;
            if (direction != Vector2.zero)
            {
                float angle = ProjectileUtils.GetAngle(direction);
                transform.localRotation = Quaternion.Euler(0f, 0f, angle);
            }
            
            _trailAccumulator += deltaTime;
            if (_trailAccumulator >= trailSpawnRate)
            {
                _trailAccumulator -= trailSpawnRate;
                SpawnTrail();
            }

        }

        public override void OnDespawn(EntityVisualizer entityVisualizer)
        {
            _trailAccumulator = 0;
            base.OnDespawn(entityVisualizer);
        }
        
        private void SpawnTrail()
        {
            var trailData = Data.ProjectileData.Trail;
            GameEventBus.Publish(new TrailSpawnRequest
            {
                SpawnContext = trailData.ToTrailSpawnContext(transform.position, transform.rotation)
            });
        }
    }
}