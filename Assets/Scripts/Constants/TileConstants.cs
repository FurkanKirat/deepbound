using Data.Models;

namespace Constants
{
    public static class TileConstants
    {
        public const float TileLength = 1f;
        public static readonly WorldPosition TileSize = new (TileLength, TileLength);
    }
}