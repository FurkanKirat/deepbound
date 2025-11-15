using System.Collections.Generic;
using System.IO;
using System.Linq;
using Constants.Paths;
using Editor.ClassBuilding;
using UnityEditor;
using Utils;

namespace Editor.Automation
{
    public class ResourcesDataPathGenerator : EditorWindow
    {
        public static void Run()
        {
            string[] relativeBasePaths = 
            {
                ResourcesPaths.DataRoot,
                ResourcesPaths.ResourcesRoot,
                ResourcesPaths.GenerationRoot,
            };
            var classBuilder = new ClassConfigurator($"{ProjectPaths.Generated}Paths/ResourcesDataPaths.cs");
            classBuilder.StartClass();

            foreach (var relativeBasePath in relativeBasePaths)
            {
                var absoluteBasePath = PathUtils.GetAbsolutePath(relativeBasePath);
                // Directories such as Items & Blocks & Sprites...
                foreach (var absoluteDataDirectory in Directory.GetDirectories(absoluteBasePath))
                {
                    var rootFolderName = PathUtils.GetRootFolderName(absoluteDataDirectory);
                    var subDirectories = PathUtils.GetAllSubDirectories(absoluteDataDirectory);

                    var relativeDataDirectory =
                        PathUtils.GetRelativePath(absoluteDataDirectory, PathUtils.RelativePathMode.Resources);
                    var relativeSubDirectories = subDirectories
                        .Select(dir => PathUtils.GetRelativePath(dir, PathUtils.RelativePathMode.Resources))
                        .ToArray();

                    string rootVariableName;
                    string rootArrayName;
                    if (relativeDataDirectory.Contains("Generation"))
                    {
                        rootVariableName =  $"{rootFolderName}GenerationRoot";
                        rootArrayName = $"{rootFolderName}GenerationPaths";
                    }
                    else
                    {
                        rootVariableName =  $"{rootFolderName}Root";
                        rootArrayName = $"{rootFolderName}Paths";
                    }
                    
                    var rootConfig = new FieldConfig
                    {
                        VariableName = rootVariableName,
                        VariableValue = relativeDataDirectory,
                        ValueModes = FieldConfig.ValueMode.Literal
                    };
                    
                    var fieldConfigs = new List<FieldConfig> { new FieldConfig {ValueModes = FieldConfig.ValueMode.Reference, VariableValue = rootVariableName} };
                    foreach (var subDirectory in relativeSubDirectories)
                    {
                        var config = new FieldConfig
                        {
                            VariableValue = subDirectory,
                            ClassType = KnownClassType.String
                        };
                        fieldConfigs.Add(config);
                    }
                    classBuilder.AddVariable(rootConfig);

                    var arrayConfig = new ArrayConfig
                    {
                        ArrayName = rootArrayName,
                        FieldConfigs = fieldConfigs,
                        ClassType = KnownClassType.String
                    };
                    classBuilder.AddArray(arrayConfig);
                }
                
            }
            
            classBuilder.EndClass();
            classBuilder.SaveToFile();
        }
    }
}