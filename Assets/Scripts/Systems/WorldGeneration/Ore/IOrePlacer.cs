using Core.Context;
using Data.Models.Generation;

namespace Systems.WorldGeneration.Ore
{
    public interface IOrePlacer
    {
        public void Place(int baseX, int baseY, int veinSize, MapGenerationContext context, OreGenerationData oreGenData);
    }
}