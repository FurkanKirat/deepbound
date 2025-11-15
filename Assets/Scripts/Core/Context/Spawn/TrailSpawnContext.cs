using UnityEngine;

namespace Core.Context.Spawn
{
    public class TrailSpawnContext
    {
        public Vector3 Position { get; set; }
        public Quaternion Rotation { get; set; }
        
        public Sprite Sprite { get; set; }
        
        public float StartScale { get; set; }
        public float FinalScale { get; set; }
        public Color StartColor { get; set; }
        public Color FinalColor { get; set; }
        
        public float LifeTime { get; set; }
        
        public int Count { get; set; } = 1;
        public float SpreadAngle { get; set; } = 0f;
        public float SpeedMin { get; set; } = 0f;
        public float SpeedMax { get; set; } = 0f;
        public Vector2 Velocity { get; set; }
        
    }
}