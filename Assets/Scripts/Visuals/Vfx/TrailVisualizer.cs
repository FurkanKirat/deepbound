using Core;
using Core.Context.Spawn;
using Core.Events;
using UnityEngine;
using Visuals.Utils;

namespace Visuals.Vfx
{
    public class TrailVisualizer : MonoBehaviour
    {
        [Header("Prefabs")]
        [SerializeField] private Trail trailPrefab;
        
        [Header("Parents")]
        [SerializeField] private Transform trailParent;
        
        private ObjectPool<Trail> _pool;

        private void Awake()
        {
            _pool = new ObjectPool<Trail>(trailPrefab, 10, trailParent);
        }

        private void OnEnable()
        {
            GameEventBus.Subscribe<TrailSpawnRequest>(OnSpawnTrailRequest);
        }

        private void OnDisable()
        {
            GameEventBus.Unsubscribe<TrailSpawnRequest>(OnSpawnTrailRequest);
        }
        
        private void OnSpawnTrailRequest(TrailSpawnRequest e)
        {
            SpawnTrail(e.SpawnContext);
        }
        private void SpawnSingleTrail(TrailSpawnContext ctx)
        {
            var trail = _pool.Get();
            trail.Initialize(_pool, ctx);
        }

        private void SpawnTrail(TrailSpawnContext ctx)
        {
            if (ctx.Count <= 1)
            {
                SpawnSingleTrail(ctx);
                return;
            }

            for (int i = 0; i < ctx.Count; i++)
            {
                float angle = Random.Range(-ctx.SpreadAngle * 0.5f, ctx.SpreadAngle * 0.5f);
                float speed = Random.Range(ctx.SpeedMin, ctx.SpeedMax);

                Vector2 dir = Quaternion.Euler(0, 0, angle) * Vector2.up;
                Vector2 velocity = dir * speed;

                ctx.Velocity = velocity;
                SpawnSingleTrail(ctx);
            }
        }
    }
}