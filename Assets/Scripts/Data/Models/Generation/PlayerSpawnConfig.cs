using Systems.WorldSystem;

namespace Data.Models.Generation
{
    public class PlayerSpawnConfig
    {
        public IValueGenerator MinX { get; set; } = new NormalizedValueGenerator(0f);
        public IValueGenerator MaxX { get; set; } = new NormalizedValueGenerator(1f);
        public IValueGenerator MinY { get; set; } = new NormalizedValueGenerator(0f);
        public IValueGenerator MaxY { get; set; } = new NormalizedValueGenerator(1f);
        public bool SurfaceOnly { get; set; } = true; 
        public int MaxAttempts { get; set; } = 50;
    }
}