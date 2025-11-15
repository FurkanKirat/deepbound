using Data.Serializable;
using Systems.WorldSystem;

namespace Data.Models.Generation
{
    public class OreGenerationData
    {
        public string BlockId { get; set; }
        public IValueGenerator MinY { get; set; }
        public IValueGenerator MaxY { get; set; }
        public IValueGenerator PeakY { get; set; }
        public float ChanceAtPeak { get; set; }
        public float ChanceAtMinMax { get; set; }
        
        // Vein info
        public RangeInt VeinSize { get; set; } = new RangeInt(10,20);
        public VeinShape Shape { get; set; } = VeinShape.RandomWalk;

        public override string ToString()
        {
            return
                $"{nameof(BlockId)}: {BlockId}, {nameof(MinY)}: {MinY}, {nameof(MaxY)}: {MaxY}, {nameof(PeakY)}: {PeakY}, {nameof(ChanceAtPeak)}: {ChanceAtPeak}, {nameof(ChanceAtMinMax)}: {ChanceAtMinMax}, {nameof(VeinSize)}: {VeinSize}, {nameof(Shape)}: {Shape}";
        }
    }

    public enum VeinShape
    {
        RandomWalk,
        Ellipse,
        Line
    }
}