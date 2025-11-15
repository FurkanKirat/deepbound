using System.Collections.Generic;
using System.IO;
using System.Linq;
using Constants.Paths;
using Editor.ClassBuilding;
using Editor.Utils;
using Utils;

namespace Editor.Automation
{
    public static class ConstantsGenerator
    {
        public static void Run()
        {
            var generationConfigs = new List<ConstantsGenerationConfig>
            {
                // Tags
                new(new[] { ResourcesPaths.ItemsFull }, new[] { "tags" }, $"{ProjectPaths.Generated}Tags/ItemTags.cs"),
                new(new[] { ResourcesPaths.BlocksFull }, new[] { "tags" }, $"{ProjectPaths.Generated}Tags/BlockTags.cs"),
                new(new[] { ResourcesPaths.EnemiesFull }, new[] { "tags" }, $"{ProjectPaths.Generated}Tags/EnemyTags.cs"),
                new(new[] { ResourcesPaths.ProjectilesFull }, new[] { "tags" }, $"{ProjectPaths.Generated}Tags/ProjectileTags.cs"),
                new(new[] { ResourcesPaths.NpcsFull }, new[] { "tags" }, $"{ProjectPaths.Generated}Tags/NpcTags.cs"),

                // IDs
                new(new[] { ResourcesPaths.ItemsFull }, new[] { "id" }, $"{ProjectPaths.Generated}Ids/ItemIds.cs"),
                new(new[] { ResourcesPaths.BlocksFull }, new[] { "id" }, $"{ProjectPaths.Generated}Ids/BlockIds.cs"),
                new(new[] { ResourcesPaths.EnemiesFull }, new[] { "id" }, $"{ProjectPaths.Generated}Ids/EnemyIds.cs"),
                new(new[] { ResourcesPaths.ProjectilesFull }, new[] { "id" }, $"{ProjectPaths.Generated}Ids/ProjectileIds.cs"),
                new(new[] { ResourcesPaths.NpcsFull }, new[] { "id" }, $"{ProjectPaths.Generated}Ids/NpcIds.cs"),
                new(new[] { ResourcesPaths.DimensionFull }, new[] { "id" }, $"{ProjectPaths.Generated}Ids/DimensionIds.cs"),
                new(new[] { ResourcesPaths.LootTablesFull }, new[] { "id" }, $"{ProjectPaths.Generated}Ids/LootTableIds.cs"),
                //new(new[] { ResourcesPaths.ItemsFull}, new[]{ "effects" },$"{ProjectPaths.Generated}Ids/EffectIds.cs"),
                
                // Behaviors
                new(new[] { ResourcesPaths.ItemsFull }, new[] { "behavior" }, $"{ProjectPaths.Generated}Ids/ItemBehaviorIds.cs"),
                new(new[] { ResourcesPaths.BlocksFull }, new[] { "behaviors" }, $"{ProjectPaths.Generated}Ids/BlockBehaviorIds.cs"),
                new(new[] { ResourcesPaths.EnemiesFull, ResourcesPaths.ProjectilesFull, ResourcesPaths.NpcsFull }, new[] { "movementBehavior" }, $"{ProjectPaths.Generated}Ids/MovementBehaviorIds.cs"),
                new(new[] { ResourcesPaths.EnemiesFull, ResourcesPaths.ProjectilesFull }, new[] { "attackBehavior" }, $"{ProjectPaths.Generated}Ids/AttackBehaviorIds.cs"),
                new(new[] { ResourcesPaths.EnemiesFull }, new[] { "aiBehavior" }, $"{ProjectPaths.Generated}Ids/EnemyAIIds.cs"),
                new(new[] { ResourcesPaths.NpcsFull }, new[] { "aiBehavior" }, $"{ProjectPaths.Generated}Ids/NpcAIIds.cs"),
                
                new(new[] {ResourcesPaths.BlockGenerationRulesFull}, new[] {"category"},$"{ProjectPaths.Generated}Categories/BlockGenerationCategories.cs")
            };

            foreach (var config in generationConfigs)
            {
                Generate(config.Folders, config.Fields, config.OutputPath, KnownClassType.String);
            }

            var audioDir = PathUtils.GetAbsolutePath(ResourcesPaths.AudiosRoot);
            var files = PathUtils.GetAllFiles(audioDir)
                .Where(f => !f.Contains("meta"));
            var filesArr = files as  string[] ?? files.ToArray();

            var classConfigurator = new ClassConfigurator($"{ProjectPaths.Generated}Resource/AudioIds.cs");
            classConfigurator.StartClass();
            foreach (var file in filesArr)
            {
                var fileName = Path.GetFileNameWithoutExtension(file);
                var config = new FieldConfig
                {
                    VariableName = CaseUtils.ToPascalCase(fileName),
                    VariableValue = fileName,
                    ClassType = KnownClassType.String,
                    ValueModes = FieldConfig.ValueMode.Literal
                };
                classConfigurator.AddVariable(config);
            }
            classConfigurator.EndClass();
            classConfigurator.SaveToFile();
        }

        private static void Generate(string[] folders, string[] fields, string classPath, KnownClassType knownClassType)
        {
            var classConfigurator = new ClassConfigurator(classPath);
            classConfigurator.StartClass();

            foreach (var (variable, value) in JsonFieldExtractor.ExtractConstantsFromJson(folders, fields))
            {
                var config = new FieldConfig
                {
                    VariableName = variable,
                    VariableValue = value,
                    ClassType = knownClassType,
                    ValueModes = FieldConfig.ValueMode.Literal
                };
                classConfigurator.AddVariable(config);
            }
            
            classConfigurator.EndClass();
            classConfigurator.SaveToFile();
        }

    }
}
