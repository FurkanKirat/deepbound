using Data.Models.Dimensions;
using Data.Serializable;
using Systems.WorldSystem;

namespace Data.Models.Generation
{
    public class StructureGenerationData
    {
        public string StructureId { get; set; }
        public IValueGenerator MinY { get; set; }
        public IValueGenerator MaxY { get; set; }
        public float Chance { get; set; }
        public LayerResolverType Layer { get; set; }
        public bool BindToSurface { get; set; }
        public RangeInt Count { get; set; }
        public int MaxAttempts { get; set; }
    }
}