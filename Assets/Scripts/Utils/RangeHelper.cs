using Data.Models;
using UnityEngine;

namespace Utils
{
    public static class RangeHelper
    {
        public static bool IsInRange(WorldPosition sourcePosition, WorldPosition targetPosition, float range)
        {
            float distanceSqr = (sourcePosition - targetPosition).SqrMagnitude;
            return distanceSqr <= range * range;
        }

        public static bool IsInRange(TilePosition sourceGrid, TilePosition targetGrid, int range)
        {
            int dx = Mathf.Abs(sourceGrid.X - targetGrid.X);
            int dy = Mathf.Abs(sourceGrid.Y - targetGrid.Y);
            return dx <= range && dy <= range;
        }
    }

}