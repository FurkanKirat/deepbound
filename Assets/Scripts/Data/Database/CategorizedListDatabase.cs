using System;
using System.Collections.Generic;
using System.Text;
using Data.Models;
using Utils;
using Utils.Extensions;

namespace Data.Database
{
    public class CategorizedListDatabase<T,TFilter> : IDatabase
        where T : ICategorizeable<TFilter>
    {
        private readonly Dictionary<TFilter, List<T>> _data = new();
        
        public IReadOnlyList<T> GetAll(TFilter category)
        {
            if (_data.TryGetValue(category, out var list))
                return list;
            return Array.Empty<T>();
        }
        
        public IEnumerable<T> All
        {
            get
            {
                foreach (var list in _data.Values)
                foreach (var item in list)
                    yield return item;
            }
        }
        
        public void Clear()
        {
            _data.Clear();
        }

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
                        var items = JsonHelper.DeserializeArray<T>(text);
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
                    GameLogger.Error($"Failed to load '{asset.name}': {e.Message}",nameof(CategorizedListDatabase<T, TFilter>));
                }
            }
        }
        
        private void Add(T item)
        {
            if (item == null || item.Category == null) return;
            if (item is IFallbackable fallbackable)
                fallbackable.ApplyFallbacks();
            if(_data.TryGetValue(item.Category, out var list))
                list.Add(item);
            else
                _data[item.Category] = new List<T> { item };
        }
        
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("{ ");
            
            foreach (var (category, list) in _data)
            {
                sb.Append($"[{category}: {list.ToDebugString()}], ");
            }
            sb.Length -= 2;
            sb.Append("}");
            return sb.ToString();
        }
    }
}