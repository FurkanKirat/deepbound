using System.Collections.Generic;
using Utils;
using UnityEngine;
using Utils.Extensions;

namespace Data.Database
{
    public class ResourceDatabase<T> :
        IMapDatabase<T> where T : Object
    {
        private readonly Dictionary<string, T> _assets = new();

        /// <summary>
        /// Loads assets from the given Resources folder path, using each asset's name as key.
        /// </summary>
        public void LoadFromFolder(string folderPath)
        {
            bool isKnown = ResourcesHelper.KnownSpriteFolders.TryGetValue(folderPath, out var ns) && !string.IsNullOrEmpty(ns);
            T[] loadedAssets = ResourcesHelper.LoadAllAssets<T>(folderPath);
            foreach (var asset in loadedAssets)
            {
                var id = isKnown ? 
                    $"{ns}:{asset.name}" : 
                    asset.name;
                if (!_assets.TryAdd(id, asset))
                    GameLogger.Warn($"Duplicate asset name '{asset.name}' with id:{id} in {folderPath}", nameof(ResourceDatabase<T>));
            }
        }

        public void LoadFromFolders(params string[] folders)
        {
            IEnumerable<string> enumerableFolders = folders;
            LoadFromFolders(enumerableFolders);
        }

        public void LoadFromFolders(IEnumerable<string> folders)
        {
            foreach (var folder in folders)
            {
                LoadFromFolder(folder);
            }
            
            GameLogger.Log($"Loaded {_assets.Count} assets from folders",nameof(ResourceDatabase<T>));
        }

        public T this[string name] => _assets.GetValueOrDefault(name);

        public bool TryGet(string key, out T value)
        {
            if (key == null)
            {
                value = null;
                return false;
            }
            return _assets.TryGetValue(key, out value);
        }

        public IEnumerable<T> GetAll() => _assets.Values;

        public override string ToString() => _assets.ToDebugString();
    }
}