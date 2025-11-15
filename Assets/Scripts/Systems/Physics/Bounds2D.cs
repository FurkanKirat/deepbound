using Data.Models;
using UnityEngine;

namespace Systems.Physics
{
    public struct Bounds2D
    {
        public WorldPosition Min;
        public WorldPosition Max;

        public WorldPosition Center => (Min + Max) / 2f;
        public WorldPosition Size => Max - Min;

        public float Width => MaxX - MinX;
        public float Height => MaxY - MinY;

        public float MinX => Min.x;
        public float MaxX => Max.x;
        public float MinY => Min.y;
        public float MaxY => Max.y;

        public Bounds2D(WorldPosition min, WorldPosition max)
        {
            Min = min;
            Max = max;
        }
        
        public Bounds2DInt ToBounds2DIntInclusive()
        {
            return new Bounds2DInt(
                Mathf.FloorToInt(MinX),
                Mathf.FloorToInt(MinY),
                Mathf.CeilToInt(MaxX) - 1,
                Mathf.CeilToInt(MaxY) - 1
            );
        }

        public Bounds2DInt ToBounds2DIntExclusive()
        {
            return new Bounds2DInt(
                Mathf.FloorToInt(MinX),
                Mathf.FloorToInt(MinY),
                Mathf.CeilToInt(MaxX),
                Mathf.CeilToInt(MaxY)
            );
        }

        
        public bool Contains(WorldPosition p)
        {
            return p.x >= MinX && p.x <= MaxX &&
                   p.y >= MinY && p.y <= MaxY;
        }

        public WorldPosition ClosestPoint(WorldPosition p)
        {
            return WorldPosition.Clamp(p, Min, Max);
        }

        public Bounds2D Expanded(float margin)
        {
            return new Bounds2D(
                new WorldPosition(Min.x - margin, Min.y - margin),
                new WorldPosition(Max.x + margin, Max.y + margin)
            );
        }
        
        public override string ToString()
        {
            return $"[{Min.x:F2},{Min.y:F2}] → [{Max.x:F2},{Max.y:F2}]";
        }



    }


}