namespace Editor.Automation
{
    using UnityEditor;
    using UnityEngine;

    public static class EditorAutomation
    {
        [MenuItem("KankanGames/Automation/Run All %#r")]
        public static void RunAll()
        {
            try
            {
                AtlasBuilderEditor.Run();
                LocalizationGenerator.Run();
                SettingsKeyGenerator.Run();
                ConstantsGenerator.Run();
                ResourcesDataPathGenerator.Run();
            
                AssetDatabase.Refresh();
                Debug.Log("✅ All automation tasks completed successfully!");
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"❌ Automation failed: {ex}");
            }
        }

        [MenuItem("KankanGames/Automation/Run/Tile Atlas")]
        public static void RunTileAtlas() => AtlasBuilderEditor.Run();
        
        [MenuItem("KankanGames/Automation/Run/Data Constants")]
        public static void RunConstants() => LocalizationGenerator.Run();

        [MenuItem("KankanGames/Automation/Run/Localization")]
        public static void RunLocalization() => LocalizationGenerator.Run();
        
        [MenuItem("KankanGames/Automation/Run/Resources Paths")]
        public static void RunResourcesPaths() => ResourcesDataPathGenerator.Run();
        
        [MenuItem("KankanGames/Automation/Run/Settings Keys")]
        public static void RunSettingsKeys() => SettingsKeyGenerator.Run();
    }

}