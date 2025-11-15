using System;
using System.Collections.Generic;
using Core;
using Data.Models;
using Utils;
using Utils.Extensions;

namespace Data.Database
{
    public class MapDatabase<T> : IMapDatabase<T> where T : IIdentifiable
    {
        private readonly Dictionary<string, T> _data = new();

        public void LoadFromSingleFile(string path)
        {
            var items = ResourcesHelper.LoadJsonList<T>(path);
            _data.Clear();
            
            foreach (var item in items)
                Add(item);
        }

        public void LoadFromDifferentFolders(params string[] folders)
        {
            _data.Clear();
            var assets = ResourcesHelper.LoadAllJsonFilesFromFolders(folders);

            foreach (var asset in assets)
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(asset.text))
                        continue;
                    
                    var text = asset.text;
                    if (JsonHelper.IsArray(text))
                    {
                        var items = JsonHelper.DeserializeArray<T>(asset.text);
                        if (items == null) continue;
                        foreach (var item in items)
                            Add(item);
                    }
                    else
                    {
                        var item = JsonHelper.Deserialize<T>(asset.text);
                        Add(item);
                    }
                    
                }
                catch (Exception e)
                {
                    GameLogger.Error($"Failed to load Type:{typeof(T).Name}, '{asset.name}': {e.Message}",nameof(MapDatabase<T>));
                }
            }
        }

        private void Add(T item)
        {
            if (item == null || item.Id == null) return;
            
            if (item is IFallbackable fallbackable)
                fallbackable.ApplyFallbacks();
            
            if (!_data.TryAdd(item.Id, item))
            {
                GameLogger.Warn($"Duplicate asset name '{item.Id}'", nameof(MapDatabase<T>));
            }
        }


        public T this[string id] => _data.GetValueOrDefault(id);

        public bool TryGet(string key, out T value)
        {
            if (key == null)
            {
                value = default;
                return false;
            }
            return _data.TryGetValue(key, out value);
        }

        public bool Exists(string id) => _data.ContainsKey(id);
        public bool TryGetValue(string id, out T value) => _data.TryGetValue(id, out value);

        public override string ToString() => _data.ToDebugString();

        public IEnumerable<T> All => _data.Values;
    }
}