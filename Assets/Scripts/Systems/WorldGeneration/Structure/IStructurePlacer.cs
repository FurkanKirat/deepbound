using Core.Context;
using Data.Models.Generation;

namespace Systems.WorldGeneration.Structure
{
    public interface IStructurePlacer
    {
        bool CanPlace(int baseX, int baseY, StructureGenerationData data, MapGenerationContext context);
        void PlaceStructure(int baseX, int baseY, StructureGenerationData data, MapGenerationContext context);
    }
}