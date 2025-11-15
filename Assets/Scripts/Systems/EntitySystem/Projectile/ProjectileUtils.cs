using Data.Models;
using UnityEngine;

namespace Systems.EntitySystem.Projectile
{
    public static class ProjectileUtils
    {
        public static float GetAngle(Vector2 velocity) 
            => Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;

        public static WorldPosition GetEndpoint(ProjectileLogic projectile)
        {
            var size = projectile.ProjectileData.Size;
            var position = projectile.Position;
            var direction = projectile.Velocity;
            var normalized = direction.normalized;
            var endPoint = position + new WorldPosition(normalized.x * size.x, normalized.y * size.y);
            return endPoint;
        }
    }
}