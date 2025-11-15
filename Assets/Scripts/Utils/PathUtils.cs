using System;
using System.IO;
using UnityEngine;

namespace Utils
{
    public static class PathUtils
    {
        public static readonly string ResourcesPath = Path.Combine(Application.dataPath, "Resources");
        
        public static string CombineWithResourcesPath(string path)
        {
            return Path.Combine(ResourcesPath, path);
        }
        
        public static string GetAbsolutePath(string relativePath)
        {
            var projectRoot = Directory.GetParent(Application.dataPath)?.FullName;
            return Path.Combine(projectRoot ?? "", relativePath);
        }

        public static string GetRootFolderName(string directory)
        {
            if (string.IsNullOrEmpty(directory))
                return string.Empty;
            return Path.GetFileName(Path.GetFullPath(directory).TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar));
        }
        
        public static string GetRelativePath(string fullPath, RelativePathMode mode)
        {
            fullPath = Path.GetFullPath(fullPath).Replace('\\', '/');
            
            string basePath = mode switch
            {
                RelativePathMode.Assets => Path.GetFullPath(Application.dataPath).Replace('\\', '/') + "/",
                RelativePathMode.Resources => Path.GetFullPath(Path.Combine(Application.dataPath, "Resources")).Replace('\\', '/') + "/",
                RelativePathMode.ResourcesData => Path.GetFullPath(Path.Combine(Application.dataPath, "Resources/Data")).Replace('\\', '/') + "/",
                _ => throw new ArgumentOutOfRangeException(nameof(mode), mode, null)
            };

            if (!fullPath.StartsWith(basePath))
                throw new ArgumentException($"Path '{fullPath}' is not under base path '{basePath}'");

            string relative = fullPath[basePath.Length..];
            
            if (mode is RelativePathMode.Resources or RelativePathMode.ResourcesData)
            {
                return Path.ChangeExtension(relative, null).Replace('\\', '/');
            }

            return relative.Replace('\\', '/');
        }

        public static string[] GetAllSubDirectories(string directory, bool includeRoot = false)
        {
            var dirs = Directory.GetDirectories(directory, "*", SearchOption.AllDirectories);

            if (includeRoot)
            {
                var result = new string[dirs.Length + 1];
                result[0] = directory;
                Array.Copy(dirs, 0, result, 1, dirs.Length);
                return result;
            }

            return dirs;
        }

        public static string[] GetAllFiles(string directory)
        {
           return Directory.GetFiles(directory, "*", SearchOption.AllDirectories);
        }
        
        public enum RelativePathMode : byte
        {
            Assets,
            Resources,
            ResourcesData,
        }
    }
}