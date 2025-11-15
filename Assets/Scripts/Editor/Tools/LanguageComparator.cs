using System.IO;
using System.Text;
using Data.Database;
using Editor.Utils;
using UnityEditor;
using UnityEngine;
using Utils;

namespace Editor.Tools
{
    public static class LanguageComparator
    {
        [MenuItem("KankanGames/Tools/LanguageComparator")]
        public static void Run()
        {
            Databases.LoadAll();
            const string folder = "Assets/Resources/Localization";
            const string defaultPath = "Assets/Resources/Localization/en.json";
            var defaultFields = JsonFieldExtractor.ExtractAllFields(defaultPath);
            var files = Directory.GetFiles(folder, "*.json", SearchOption.AllDirectories);
            var sb = new StringBuilder();
            foreach (var file in files)
            {
                if (FileUtils.GetSafePath(file).EndsWith(defaultPath))
                    continue;
                
                var currentFields = JsonFieldExtractor.ExtractAllFields(file);
                foreach (var (defaultKey, _) in defaultFields)
                {
                    if (!currentFields.ContainsKey(defaultKey))
                    {
                        sb.AppendLine(defaultKey);
                    }
                }
                
                Debug.Log($"Could not find localization at {file} for localization: \n{sb} ");
            }
            
        }
    }
}