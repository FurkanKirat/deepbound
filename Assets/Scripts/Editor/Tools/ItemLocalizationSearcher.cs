using System.Collections.Generic;
using Data.Database;
using Editor.Utils;
using UnityEditor;
using UnityEngine;

namespace Editor.Tools
{
    public static class ItemLocalizationSearcher
    {
        [MenuItem("KankanGames/Tools/ItemLocalizationSearcher")]
        public static void Run()
        {
            Databases.LoadAll();
            const string path = "Assets/Resources/Localization/en.json";
            var json = JsonFieldExtractor.ExtractAllFields(path);

            var items = Databases.Items.All;
            var itemList = new LinkedList<string>();

            foreach (var item in items)
            {
                if (!json.ContainsKey($"{item.Id}.name"))
                    itemList.AddLast($"{item.Id}");
            }
            
            var str = string.Join("\n", itemList);
            Debug.Log($"Could not find localization for items: \n{str}");
        }
    }
}