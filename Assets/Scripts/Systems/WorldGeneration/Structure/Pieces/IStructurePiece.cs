using Core.Context;
using Data.Models.Generation;

namespace Systems.WorldGeneration.Structure.Pieces
{
    public interface IStructurePiece
    {
        void Place(int baseX, int baseY, StructureGenerationData data, MapGenerationContext context);
    }
}