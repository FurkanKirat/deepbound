namespace Constants.Paths
{
    public static class ResourcesPaths
    {
        public const string ResourcesRoot = "Assets/Resources/";
        public const string ProjectRoot = "Assets/";
        public const string DataRoot = "Assets/Resources/Data";
        public const string GenerationRoot = "Assets/Resources/Data/Generation";
        public const string SpritesRoot = "Assets/Resources/Sprites";
        public const string PrefabsRoot = "Assets/Resources/Prefabs";
        public const string AudiosRoot = "Assets/Resources/Audio";

        public const string Data = "Data";
        public const string Items = "Data/Items";
        public const string Blocks = "Data/Blocks";
        public const string Enemies = "Data/Enemies";
        public const string Projectiles = "Data/Projectiles";
        public const string Npcs = "Data/NPCs";
        public const string BlockGenerationRules = "Data/BlockGenerationRules";
        public const string Dimensions = "Data/Dimensions";
        public const string LootTables = "Data/LootTables";

        public static string ItemsFull => $"{ResourcesRoot}{Items}";
        public static string BlocksFull => $"{ResourcesRoot}{Blocks}";
        public static string EnemiesFull => $"{ResourcesRoot}{Enemies}";
        public static string ProjectilesFull => $"{ResourcesRoot}{Projectiles}";
        public static string NpcsFull => $"{ResourcesRoot}{Npcs}";
        public static string BlockGenerationRulesFull => $"{ResourcesRoot}{BlockGenerationRules}";
        public static string DimensionFull => $"{ResourcesRoot}{Dimensions}";
        public static string LootTablesFull => $"{ResourcesRoot}{LootTables}";
    }
    
    public static class ProjectPaths
    {
        public const string Resources = "Assets/Resources/";
        public const string Scripts = "Assets/Scripts/";
        public const string Generated = "Assets/Scripts/Generated/";
    }
    
    public static class AtlasPaths
    {
        public static readonly string UvJsonResourcePath = $"Assets/Resources/{UvJsonResourcePathRelative}.json";
        public const string UvJsonResourcePathRelative = "Atlas/uv_index";
        public const string AtlasOutputPath = "Assets/Resources/Atlas/tileAtlas.png";
        public const string AtlasSourceFolder = "Assets/Resources/Sprites/Blocks/";
    }
    
}