using System;
using System.Collections.Generic;
using Data.Models;
using Utils;
using Utils.Extensions;

namespace Data.Database
{
    public class ListDatabase<T> : IDatabase
    {
        private readonly List<T> _data = new();

        public void LoadFromSingleFile(string path)
        {
            var items = ResourcesHelper.LoadJsonList<T>(path);
            _data.Clear();
            _data.AddRange(items);
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
                        if (items != null)
                            foreach (var item in items)
                                Add(item);
                    }
                    else
                    {
                        var item = JsonHelper.Deserialize<T>(asset.text);
                        if (item != null)
                            Add(item);
                    }
                }
                catch (Exception e)
                {
                    GameLogger.Error($"Failed to load '{asset.name}': {e.Message}",nameof(ListDatabase<T>));
                }
            }
        }

        private void Add(T item)
        {
            if (item == null) return;
            if (item is IFallbackable fallbackable)
                fallbackable.ApplyFallbacks();
            _data.Add(item);
        }

        public override string ToString() => _data.ToDebugString();

        public IEnumerable<T> All => _data;
    }
}