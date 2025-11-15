using Core;
using Data.Serializable;
using Systems.WorldGeneration.Steps;

namespace Data.Models.Generation
{
    public class DimensionGenerationData : IIdentifiable
    {
        public string Id { get; set; }
        
        public RangeInt Width { get; set; }
        public RangeInt Height { get; set; }

        public string BaseBlock { get; set; }
        public string SurfaceBlock { get; set; }
        public string StoneBlock { get; set; }
        
        public StepsData[] Steps { get; set; }
        public string OreConfigFile { get; set; }
        public string StructureConfigFile { get; set; }
        public string PlayerSpawnConfigFile { get; set; }
    }
}