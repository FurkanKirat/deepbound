using System.Collections.Generic;
using System.Text;
using Editor.Utils;
using UnityEditor;
using Utils;

namespace Editor.ClassBuilding
{
    public class ClassConfigurator
    {
        private readonly StringBuilder _builder = new();
        private readonly HashSet<string> _imports = new();
        
        public string NamespaceName { get; }

        public string ClassName { get; }

        public string ClassPath { get; }
        public ClassConfigurator(string classPath)
        {
            var (namespaceName, className) = EditorUtils.ParseNamespaceAndClassName(classPath);
            ClassName = className;
            NamespaceName = namespaceName;
            ClassPath = classPath;
        }
        
        public override string ToString() => _builder.ToString();
        
        public void StartClass()
        {
            _builder.AppendLine($"namespace {NamespaceName}");
            _builder.AppendLine("{");
            _builder.AppendLine($"{Indent(1)}public static class {ClassName} ");
            _builder.AppendLine($"{Indent(1)}{{");
        }

        public void AddVariable(FieldConfig fieldConfig)
        {
            var descriptor = ClassTypeFactory.GetDescriptor(fieldConfig.ClassType);
            AddImports(descriptor.RequiredUsings);
            
            var valueMode = fieldConfig.ValueModes;
            var variableName = fieldConfig.VariableName;
            var variableValue = fieldConfig.VariableValue;
            var finalValue = valueMode == FieldConfig.ValueMode.Literal ? descriptor.FormatValueLiteral(variableValue) : variableValue;
            
            string declaration = descriptor.CanBeConst ? "const" : "static readonly";
            _builder.AppendLine($"{Indent(2)}public {declaration} {descriptor.TypeName} {variableName} = {finalValue};");
        }

        public void AddArray(ArrayConfig arrayConfig)
        {
            var descriptor = ClassTypeFactory.GetDescriptor(arrayConfig.ClassType);
            AddImports(descriptor.RequiredUsings);
            _builder.AppendLine($"{Indent(2)}public static readonly {descriptor.TypeName}[] {arrayConfig.ArrayName} =");
            _builder.AppendLine($"{Indent(2)}{{");
            foreach (var config in arrayConfig.FieldConfigs)
            {
                var valueMode = config.ValueModes;
                var variableValue = config.VariableValue;
                var finalValue = valueMode == FieldConfig.ValueMode.Literal ? descriptor.FormatValueLiteral(variableValue) : variableValue;
                _builder.AppendLine($"{Indent(3)}{finalValue},");
            }
            _builder.AppendLine($"{Indent(2)}}};");
        }

        public void EndClass()
        {
            _builder.AppendLine($"{Indent(1)}}}");
            _builder.AppendLine("}");
        }
        
        public void SaveToFile()
        {
            FileUtils.SaveText(ClassPath, ToString());
            AssetDatabase.Refresh();
        }

        private void AddImports(string[] imports)
        {
            foreach (var import in imports)
            {
                if (!_imports.Add(import)) continue;
                _builder.Insert(0,$"using {import};\n");
            }
        }
        
        private static string Indent(int level) => new('\t', level);
    }
}