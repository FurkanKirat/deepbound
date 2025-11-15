using Core.Context;

namespace Systems.WorldGeneration.Steps
{
    public interface IMapGenerationStep
    {
        void Apply(MapGenerationContext context);
    }

}