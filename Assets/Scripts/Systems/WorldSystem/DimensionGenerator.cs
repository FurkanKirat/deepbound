using Systems.WorldGeneration;

namespace Systems.WorldSystem
{
    public static class DimensionGenerator
    {
        public static Dimension GenerateDimension(DimensionGenerationSettings settings, World world)
        {
            var worldBuilder = new MapBuilder(settings.WorldSeed, settings.DimensionId);
            var mapCtx = worldBuilder.Build();
            var dimCtx = new DimensionGenerationContext
            {
                DimensionId = settings.DimensionId,
                PlayerSpawn = mapCtx.PlayerSpawnPosition,
                Blocks = mapCtx.Blocks,
                SurfaceYPerX = mapCtx.SurfaceYPerColumn,
                World = world,
            };
            var dim = Dimension.Create(dimCtx);
            return dim;
        }
    }
}