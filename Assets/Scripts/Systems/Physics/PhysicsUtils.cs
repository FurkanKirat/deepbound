using System.Collections.Generic;
using Data.Database;
using Data.Models;
using Systems.Physics.Colliders;
using Systems.WorldSystem;
using UnityEngine;

namespace Systems.Physics
{
    public static class PhysicsUtils
    {
        public static bool IsCollidingWithSolidTiles(
            ICollider collider,
            BlockManager blockManager,
            DirectionFlags directions,
            out DirectionFlags hitDirections)
        {
            hitDirections = DirectionFlags.None;

            foreach (var (point, dir) in GetEdgePoints(collider, directions))
            {
                if (blockManager.IsSolidAt(point))
                {
                    hitDirections |= dir;
                }
            }

            return hitDirections != DirectionFlags.None;
        }
        public static Vector2 GetKnockback(WorldPosition attackerPos, WorldPosition targetPos)
        {
            var direction = (targetPos - attackerPos).ToVector2();
            direction.y += 0.5f; // bias
            direction = direction.normalized;

            var knockbackForce = 8f;
            return direction * knockbackForce;
        }

        public static Vector2 GetDirectionToCursor(WorldPosition playerPos, WorldPosition mouseWorldPos)
        {
            Vector2 direction = (mouseWorldPos - playerPos).Normalized.ToVector2();
            return direction;
        }

        public static bool IsGrounded(ICollider collider, BlockManager blockManager)
        {
            float groundTolerance = Configs.GameConfig.Physics.GroundTolerance;

            var bounds = collider.Bounds;
            float bottomY = bounds.Min.y - groundTolerance;
            
            float left = bounds.Min.x + groundTolerance;
            float right = bounds.Max.x - groundTolerance;
            
            WorldPosition probeLeft = new WorldPosition(left, bottomY);
            WorldPosition probeRight = new WorldPosition(right, bottomY);

            return blockManager.IsSolidAt(probeLeft.ToTilePosition()) ||
                   blockManager.IsSolidAt(probeRight.ToTilePosition());
        }

        
        public static IEnumerable<(TilePosition point, DirectionFlags direction)> GetEdgePoints(
            ICollider collider,
            DirectionFlags directionsFlags
            )
        {
            var bias = Configs.GameConfig.Physics.GroundTolerance;
            var bounds = collider.Bounds.ToBounds2DIntExclusive();

            if (directionsFlags.HasFlag(DirectionFlags.Down))
            {
                float y = bounds.MinY - bias;
                for (float x = bounds.MinX + bias; x <= bounds.MaxX - bias; x += 1f)
                    yield return (new WorldPosition(x, y).ToTilePosition(), DirectionFlags.Down);
            }

            if (directionsFlags.HasFlag(DirectionFlags.Up))
            {
                float y = bounds.MaxY + bias;
                for (float x = bounds.MinX + bias; x <= bounds.MaxX - bias; x += 1f)
                    yield return (new WorldPosition(x, y).ToTilePosition(), DirectionFlags.Up);
            }

            if (directionsFlags.HasFlag(DirectionFlags.Left))
            {
                float x = bounds.MinX - bias;
                for (float y = bounds.MinY + bias; y <= bounds.MaxY - bias; y += 1f)
                    yield return (new WorldPosition(x, y).ToTilePosition(), DirectionFlags.Left);
            }

            if (directionsFlags.HasFlag(DirectionFlags.Right))
            {
                float x = bounds.MaxX + bias;
                for (float y = bounds.MinY + bias; y <= bounds.MaxY - bias; y += 1f)
                    yield return (new WorldPosition(x, y).ToTilePosition(), DirectionFlags.Right);
            }
        }

    }
}