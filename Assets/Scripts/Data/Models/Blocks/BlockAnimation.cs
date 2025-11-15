using Data.Models.References;

namespace Data.Models.Blocks
{
    public class BlockAnimation
    {
        public SpriteRef[] Frames { get; set; }
        public float Speed { get; set; } = 0.2f;
        public bool Loop { get; set; } = true;
    }
}