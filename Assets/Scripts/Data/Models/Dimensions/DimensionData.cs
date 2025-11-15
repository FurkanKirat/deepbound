using Core;

namespace Data.Models.Dimensions
{
    public class DimensionData : IIdentifiable
    {
        public string Id { get; set; }
        public WorldLayer[] Layers { get; set; }
    }
}