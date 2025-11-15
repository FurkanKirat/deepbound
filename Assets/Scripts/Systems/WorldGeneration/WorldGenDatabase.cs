using System.Collections.Generic;
using Data.Database;
using Data.Models.Generation;
using Data.Registrars.WorldGen;
using Data.RegistrySystem;
using Generated.Paths;
using Systems.WorldGeneration.Steps;
using Systems.WorldGeneration.Structure;
using Utils;

namespace Systems.WorldGeneration
{
    public class WorldGenDatabase
    {
        public DimensionGenerationData DimensionGenerationData { get; }
        public FactoryRegistry<Dictionary<string, object>, IMapGenerationStep> MapGenerationStepsRegistry { get; } 
        public OnDemandDatabase<OreGenerationConfig> OreConfig { get; }
        public OnDemandDatabase<StructureGenerationConfig> StructureConfig { get; }
        public OnDemandDatabase<PlayerSpawnConfig> PlayerSpawnConfig { get; }
        public Registry<IStructurePlacer> StructurePlacerRegistry { get; }

        public WorldGenDatabase(string dimensionId)
        {
            var dimName = NamespacedIdUtils.StripNamespace(dimensionId);
            var dimensionPath = $"{ResourcesDataPaths.DimensionsGenerationRoot}/{dimName}";
            DimensionGenerationData = ResourcesHelper.LoadJson<DimensionGenerationData>(dimensionPath);
            
            MapGenerationStepsRegistry = new FactoryRegistry<Dictionary<string,object>, IMapGenerationStep>();
            new MapGenerationStepsRegistrar(MapGenerationStepsRegistry).RegisterAll();
            
            StructurePlacerRegistry = new Registry<IStructurePlacer>();
            new StructureRegistrar(StructurePlacerRegistry).RegisterAll();
            
            OreConfig = new OnDemandDatabase<OreGenerationConfig>();
            StructureConfig = new OnDemandDatabase<StructureGenerationConfig>();
            PlayerSpawnConfig = new OnDemandDatabase<PlayerSpawnConfig>();
        }
    }
}