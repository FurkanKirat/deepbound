using Constants.Paths;
using Editor.ClassBuilding;
using Editor.Utils;

namespace Editor.Automation
{
    public static class SettingsKeyGenerator
    {
        public static void Run()
        {
            const string path = "Assets/Resources/Data/settings_config.json";
            
            var json = JsonFieldExtractor.ExtractSettingKeys(path);
            
            var classConfigurator = new ClassConfigurator($"{ProjectPaths.Generated}Settings/SettingsKeys.cs");
            classConfigurator.StartClass();
            
            foreach (var file in json)
            {
                var config = new FieldConfig
                {
                    VariableName = file.Key,
                    VariableValue = file.Key,
                    ClassType = KnownClassType.String,
                    ValueModes = FieldConfig.ValueMode.Literal
                };
                classConfigurator.AddVariable(config);
            }
            classConfigurator.EndClass();
            classConfigurator.SaveToFile();
        }
    }
}