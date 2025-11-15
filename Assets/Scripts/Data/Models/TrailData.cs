using Core.Context.Spawn;
using Data.Models.References;
using UnityEngine;

namespace Data.Models
{
    public class TrailData
    {
        public SpriteRef Icon { get; set; }
        public float LifeTime { get; set; }
        public float StartScale { get; set; }
        public float FinalScale { get; set; }
        public ColorRef StartColor { get; set; }
        public ColorRef FinalColor { get; set; }

        public int Count { get; set; } = 1;
        public float SpreadAngle { get; set; } = 0f;
        public float SpeedMin { get; set; } = 0f;
        public float SpeedMax { get; set; } = 0f;

        public TrailSpawnContext ToTrailSpawnContext(Vector3 position, Quaternion rotation)
        {
            return new TrailSpawnContext
            {
                LifeTime = LifeTime,
                StartScale = StartScale,
                FinalScale = FinalScale,
                StartColor = StartColor?.Load() ?? new Color(1,1,1,1),
                FinalColor = FinalColor?.Load()  ?? new Color(1,1,1,0),
                Position = position,
                Rotation = rotation,
                Sprite = Icon?.Load(),
                
                Count = Count,
                SpreadAngle = SpreadAngle,
                SpeedMin = SpeedMin,
                SpeedMax = SpeedMax,
            };
        }
        
    }
}