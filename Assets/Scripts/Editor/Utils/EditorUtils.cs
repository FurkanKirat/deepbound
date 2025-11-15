using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Editor.Utils
{
    public static class EditorUtils
    {
        public static string GetIdFromJson(string path)
        {
            if (!File.Exists(path))
            {
                Debug.LogWarning($"File not found: {path}");
                return null;
            }

            string json = File.ReadAllText(path);
            var match = Regex.Match(json, @"""id""\s*:\s*""([^""]+)""");
            return match.Success ? match.Groups[1].Value : null;
        }

        /// <summary>
        /// snake_case or kebab-case → PascalCase (stone_wall → StoneWall)
        /// </summary>
        public static string ToPascalCase(string input)
        {
            if (input.Contains('.'))
                input = input.Split('.').Last();

            string[] parts = input.Split(new[] { '_', '-' });
            for (int i = 0; i < parts.Length; i++)
            {
                if (parts[i].Length > 0)
                    parts[i] = char.ToUpper(parts[i][0], CultureInfo.InvariantCulture) + parts[i][1..];
            }

            return string.Join("", parts);
        }
        
        public static (string namespaceName, string className) ParseNamespaceAndClassName(string classPath, string rootFolder = "Assets/Scripts/")
        {
            var safePath = classPath
                .Replace('\\', '/');
            var relativePath = safePath
                .Replace(rootFolder, "")
                .Replace(".cs", "");
            var lastIndex = safePath
                .LastIndexOf("/", StringComparison.InvariantCulture);
            var className = safePath[(lastIndex + 1)..]
                .Replace(".cs", "");
            var namespaceName = relativePath
                .Replace(className, "")
                .Replace("/", ".");

            if (namespaceName.EndsWith("."))
                namespaceName = namespaceName[..^1];

            return (namespaceName, className);
        }
    }

}