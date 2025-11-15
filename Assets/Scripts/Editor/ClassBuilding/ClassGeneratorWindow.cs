using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

namespace Editor.ClassBuilding
{
    public class ClassGeneratorWindow : EditorWindow
    {
        private string _classPath = "Assets/Scripts/Generated/MyClass.cs";
        private string _variableName = "MyString";
        private string _variableValue = "Hello World";
        private KnownClassType _selectedType = KnownClassType.String;

        private readonly List<FieldConfig> _arrayValues = new() 
        { 
            new FieldConfig{VariableValue = "Value1"}, 
            new FieldConfig{VariableValue = "Value2" }
        };
        private string _arrayName = "MyArray";
        
        public static void ShowWindow()
        {
            GetWindow<ClassGeneratorWindow>("Class Generator");
        }

        private void OnGUI()
        {
            GUILayout.Label("Class Generation Settings", EditorStyles.boldLabel);

            _classPath = EditorGUILayout.TextField("Class Path", _classPath);
            _selectedType = (KnownClassType)EditorGUILayout.EnumPopup("Variable Type", _selectedType);
            _variableName = EditorGUILayout.TextField("Variable Name", _variableName);
            _variableValue = EditorGUILayout.TextField("Variable Value", _variableValue);

            EditorGUILayout.Space();
            GUILayout.Label("Array Settings", EditorStyles.boldLabel);
            _arrayName = EditorGUILayout.TextField("Array Name", _arrayName);

            for (int i = 0; i < _arrayValues.Count; i++)
            {
                _arrayValues[i].VariableName = EditorGUILayout.TextField($"Element {i}", _arrayValues[i].VariableName);
            }

            if (GUILayout.Button("Add Array Element"))
            {
                _arrayValues.Add(new FieldConfig(){VariableName = ""});
            }

            if (GUILayout.Button("Generate Class"))
            {
                GenerateClass();
            }
        }

        private void GenerateClass()
        {
            var configurator = new ClassConfigurator(_classPath);
            configurator.StartClass();

            var config = new FieldConfig
            {
                ValueModes = FieldConfig.ValueMode.Reference,
                VariableName = _variableName,
                VariableValue = _variableValue,
                ClassType = _selectedType
            };
            configurator.AddVariable(config);

            var arrayConfig = new ArrayConfig
            {
                ArrayName = _arrayName,
                FieldConfigs = _arrayValues,
                ClassType = _selectedType
            };
            configurator.AddArray(arrayConfig);

            configurator.EndClass();
            configurator.SaveToFile();

            Debug.Log("Class generated and saved to: " + _classPath);
        }
    }
}
