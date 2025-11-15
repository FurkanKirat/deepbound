using Data.RegistrySystem;
using Systems.WorldGeneration.Structure;

namespace Data.Registrars.WorldGen
{
    public class StructureRegistrar : IRegistrar
    {
        private Registry<IStructurePlacer> StructurePlacerRegistry { get; }

        public StructureRegistrar(Registry<IStructurePlacer> structurePlacerRegistry)
        {
            StructurePlacerRegistry = structurePlacerRegistry;
        }
        public void RegisterAll()
        {
            StructurePlacerRegistry.Register("cave_pocket", new CavePocketPlacer());
            StructurePlacerRegistry.Register("tree", new TreePlacer());
            StructurePlacerRegistry.Register("demon_castle", new DemonCastle());
        }
    }
}