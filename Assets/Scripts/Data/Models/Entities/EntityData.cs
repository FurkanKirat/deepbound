using Data.Models.References;

namespace Data.Models.Entities
{
    public class EntityData
    {
        public WorldPosition Size { get; set; }
        public float? LifeTime { get; set; }
        public SpriteRef MapIcon { get; set; }
        public bool ShowInMap { get; set; }
    }
}