namespace Constants
{
    /// <summary>
    /// Contains constants related to the tile atlas configuration.
    /// Used for UV mapping and grid-based calculations on the texture atlas.
    /// </summary>
    public static class TileAtlasConstants
    {
        /// <summary>
        /// Size of one grid cell in pixels (tile size).
        /// </summary>
        public const int GridPixelCount = 16;
        
        /// <summary>
        /// Area of one grid cell in pixels (tile size).
        /// </summary>
        public const int GridPixelArea = GridPixelCount * GridPixelCount;

        /// <summary>
        /// Size of the entire atlas texture in pixels (assumed to be square).
        /// </summary>
        public const int AtlasPixelCount = 512;

        /// <summary>
        /// Number of tiles per row in the atlas (AtlasPixelCount / GridPixelCount).
        /// </summary>
        public const int GridCountPerRow = AtlasPixelCount / GridPixelCount;

        /// <summary>
        /// Maximum number of tiles that can fit in the atlas 
        /// (total grid cells: GridCountPerRow * GridCountPerRow).
        /// </summary>
        public const int MaxTileCount = GridCountPerRow * GridCountPerRow;

        /// <summary>
        /// Normalized size of one grid cell in UV space (0.0–1.0 range).
        /// Used for texture coordinate calculations.
        /// </summary>
        public const float GridSize = (float)GridPixelCount / AtlasPixelCount;
    }
}