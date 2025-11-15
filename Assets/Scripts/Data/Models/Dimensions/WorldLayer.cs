using System.Collections.Generic;
using Data.Models.References;
using Data.Serializable;
using Systems.SpawnSystem;

namespace Data.Models.Dimensions
{
    public class WorldLayer
    {
        public string Id;
        public LayerResolverType Resolver;
        public RangeInt? Height;
        public List<ParallaxLayer> ParallaxLayers;
        public string SpawnTable;
        public LootTableRef LootTable;
        public EnemySpawnTableRef EnemySpawnTable;
        public string Music;

        public bool Matches(TilePosition pos, int surfaceY)
        {
            return Resolver switch
            {
                LayerResolverType.Always => true,
                LayerResolverType.AboveSurface => pos.Y > surfaceY,
                LayerResolverType.BelowSurface => pos.Y <= surfaceY,
                LayerResolverType.Between => Height.HasValue && pos.Y >= Height.Value.Min && pos.Y <= Height.Value.Max,
                _ => false
            };
        }
    }
    public enum LayerResolverType
    {
        Always,
        AboveSurface,
        BelowSurface,
        Between,
    }


}