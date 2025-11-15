using System.IO;

namespace Utils
{
    using System.Collections.Generic;
    using UnityEngine;

    public static class ResourcesHelper
    {
        public static readonly Dictionary<string, string> KnownSpriteFolders = new()
        {
            { "Sprites/Blocks", "block" },
            { "Sprites/Entities", "entity"},
            { "Sprites/Items", "item"},
            { "Sprites/MapIcons", "mapicon"},
            { "Sprites/Parallax", "parallax"}
        };
        /// <summary>
        /// Loads a JSON file from the Resources folder and deserializes it into a single object.
        /// </summary>
        public static T LoadAsset<T>(string path) where T : Object
        {
            T asset = Resources.Load<T>(path);
            if (asset == null)
                GameLogger.Warn($"Asset not found at Resources/{path}", nameof(ResourcesHelper));

            return asset;
        }

        public static T[] LoadAllAssets<T>(string directory) where T : Object
        {
            return Resources.LoadAll<T>(directory);
        }

        // JSON deserialization from TextAsset
        public static T LoadJson<T>(string path)
        {
            TextAsset jsonFile = LoadAsset<TextAsset>(path);
            if (jsonFile == null)
                return default;

            return JsonHelper.Deserialize<T>(jsonFile.text);
        }
        
        public static List<TextAsset> LoadAllJsonFilesFromFolders(string[] resourceFolders)
        {
            var result = new List<TextAsset>();
            foreach (var folder in resourceFolders)
            {
                var assets = Resources.LoadAll<TextAsset>(folder);
                result.AddRange(assets);
            }
            return result;
        }


        public static List<T> LoadJsonList<T>(string path)
        {
            return LoadJson<List<T>>(path);
        }

        public static T[] LoadJsonArray<T>(string path)
        {
            return LoadJson<T[]>(path);
        }

        public static List<List<T>> LoadJson2DList<T>(string path)
        {
            return LoadJson<List<List<T>>>(path);
        }

        public static void SaveToJson<T>(string pathWithExtension, T data)
        {
            string extension = Path.GetExtension(pathWithExtension).TrimStart('.');
            string resourcePath = PathUtils.GetRelativePath(pathWithExtension, PathUtils.RelativePathMode.Resources);
            string fullPath = PathUtils.CombineWithResourcesPath(resourcePath + $".{extension}");
            
            JsonHelper.SaveRaw(fullPath, data);
        }
        
    }

}