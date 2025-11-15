using Constants.Paths;
using Editor.ClassBuilding;
using Newtonsoft.Json.Linq;
using Utils;

namespace Editor.Automation
{
    public static class LocalizationGenerator
    {
        public static void Run()
        {
            var text = FileUtils.Load("Assets/Resources/Localization/en.json");
            
            var json = JObject.Parse(text);
            
            var classConfigurator = new ClassConfigurator($"{ProjectPaths.Generated}Localization/LocalizationKeys.cs");
            classConfigurator.StartClass();
            
            foreach (var file in json)
            {
                var config = new FieldConfig
                {
                    VariableName = CaseUtils.ToPascalCase(file.Key),
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