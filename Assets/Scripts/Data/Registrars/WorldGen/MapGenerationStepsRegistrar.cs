using System.Collections.Generic;
using Data.RegistrySystem;
using Generated.Ids;
using Systems.WorldGeneration.Steps;
using Utils.Extensions;

namespace Data.Registrars.WorldGen
{
    public class MapGenerationStepsRegistrar : IRegistrar
    {
        private FactoryRegistry<Dictionary<string,object>, IMapGenerationStep> MapGenerationStepsRegistry { get; }

        public MapGenerationStepsRegistrar(
            FactoryRegistry<Dictionary<string, object>, IMapGenerationStep> mapGenerationStepsRegistry)
        {
            MapGenerationStepsRegistry = mapGenerationStepsRegistry;
        }
        public void RegisterAll()
        {
            MapGenerationStepsRegistry.Register(nameof(SurfaceHeightStep), parameters =>
                new SurfaceHeightStep(
                    parameters.Get("scale", 0.05f),
                    parameters.Get("minOffset", -10),
                    parameters.Get("maxOffset", 10)
                )
            );

            MapGenerationStepsRegistry.Register(nameof(CaveGenerationStep), parameters =>
                new CaveGenerationStep(
                    parameters.Get("scale", 0.1f),
                    parameters.Get("threshold", 0.55f)
                )
            );
            
            MapGenerationStepsRegistry.Register(nameof(TerrainFillStep), _ =>
                new TerrainFillStep());
            
            MapGenerationStepsRegistry.Register(nameof(OreGenerationStep), _ =>
                new OreGenerationStep());
            
            MapGenerationStepsRegistry.Register(nameof(ChestPlacingStep), parameters =>
                new ChestPlacingStep(
                    parameters.Get("density", 70f),
                    parameters.Get("replace", new[] { BlockIds.Air } ),
                    parameters.Get("spawnAttempts", 10000)
                    )
            );

            MapGenerationStepsRegistry.Register(nameof(StructurePlacingStep), _ => 
                new StructurePlacingStep());
            
            MapGenerationStepsRegistry.Register(nameof(PlayerSpawnStep), _ =>
                new PlayerSpawnStep());
        }
    }
}