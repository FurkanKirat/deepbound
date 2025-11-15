using Data.Models.Blocks;
using Data.Models.Crafting;
using Data.Models.Dimensions;
using Data.Models.Entities;
using Data.Models.Items;
using Generated.Paths;
using Systems.LootSystem;
using Systems.SpawnSystem;

namespace Data.Database
{
    public static class Databases
    {
        public static MapDatabase<BlockData> Blocks { get; private set; }
        public static MapDatabase<ItemData> Items { get; private set; }
        public static CategorizedListDatabase<CraftingRecipe, CraftingStation> Recipes { get; private set; }
        
        public static MapDatabase<EnemyData> Enemies { get; private set; }
        public static MapDatabase<NpcData> Npcs { get; private set; }
        public static MapDatabase<ProjectileData> Projectiles { get; private set; }
        
        public static MapDatabase<LootTable> LootTables { get; private set; }
        public static OnDemandDatabase<EnemySpawnTable> EnemySpawnTables { get; private set; }
        
        public static MapDatabase<DimensionData> Dimensions { get; private set; }
        
        private static bool _isLoaded = false;
        
        public static void LoadAll() {
            
            if (_isLoaded)
                return;
            
            Blocks = new MapDatabase<BlockData>();
            Blocks.LoadFromDifferentFolders(ResourcesDataPaths.BlocksRoot);

            Items = new MapDatabase<ItemData>();
            Items.LoadFromDifferentFolders(ResourcesDataPaths.ItemsRoot);
            
            Recipes = new CategorizedListDatabase<CraftingRecipe, CraftingStation>();
            Recipes.LoadFromDifferentFolders(ResourcesDataPaths.CraftingsRoot);
            
            LootTables = new MapDatabase<LootTable>();
            LootTables.LoadFromDifferentFolders(ResourcesDataPaths.LootTablesRoot);

            Enemies = new MapDatabase<EnemyData>();
            Enemies.LoadFromDifferentFolders(ResourcesDataPaths.EnemiesRoot);
            
            Npcs = new MapDatabase<NpcData>();
            Npcs.LoadFromDifferentFolders(ResourcesDataPaths.NPCsRoot);
            
            Projectiles = new MapDatabase<ProjectileData>();
            Projectiles.LoadFromDifferentFolders(ResourcesDataPaths.ProjectilesRoot);
            
            EnemySpawnTables = new OnDemandDatabase<EnemySpawnTable>();
            
            Dimensions = new MapDatabase<DimensionData>();
            Dimensions.LoadFromDifferentFolders(ResourcesDataPaths.DimensionsRoot);
            
            _isLoaded = true;
        }
    }
}